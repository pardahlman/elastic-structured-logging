using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Scenarios.Logging;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace Scenarios
{
  public class Program
  {
    public static int Main(string[] args)
    {
      var cts = new CancellationTokenSource();
      Console.CancelKeyPress += (sender, cancelArgs) => cts.Cancel();
      MainAsync(args, cts.Token).GetAwaiter().GetResult();
      var tsc = new TaskCompletionSource<int>();
      cts.Token.Register(() => tsc.TrySetResult(1));
      return tsc.Task.GetAwaiter().GetResult();
    }

    public static async Task MainAsync(string[] args, CancellationToken ct)
    {
      Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Verbose()
        .WriteTo.Console()
        .WriteTo.Elasticsearch(Settings.ElasticOptions)
        .Enrich.FromLogContext()
        .Enrich.WithProperty("buildNumber", 10456)
        .Enrich.WithProperty("branch", "feature/logging")
        .CreateLogger();

      var activities = new List<IActivity>
      {
        new UserLogin()
      };
      while (true)
      {
        var tasks = activities.Select(a => a.RunAsync(ct));
        await Task.WhenAll(tasks);
      }
    }
  }
}
