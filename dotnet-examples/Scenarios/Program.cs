using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Scenarios.Logging;
using Scenarios.Offline;
using Serilog;

namespace Scenarios
{
  using Scenarios.StackOverflow;

  public static class Program
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

    private static async Task MainAsync(string[] args, CancellationToken ct)
    {
      Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Verbose()
        .WriteTo.Console()
        .WriteTo.Elasticsearch()
        .Enrich.FromLogContext()
        .Enrich.WithProperty("buildNumber", 10456)
        .Enrich.WithProperty("branch", "feature/logging")
        .CreateLogger();

      var activities = new List<IActivity>
      {
        new GetStackOverflowQuestions()
      };

      var tasks = activities.Select(a => a.RunAsync(ct));
      await Task.WhenAll(tasks);
    }
  }
}
