namespace Scenarios.Offline
{
  using System;
  using System.Collections.Generic;
  using System.Net;
  using System.Threading.Tasks;

  using Serilog;

  public class NavigationSimulator
  {
    private static readonly Random Random = new Random();
    private static readonly ILogger Logger = Log.ForContext<NavigationSimulator>();

    private static readonly List<HttpStatusCode> SuccessStatus =
      new List<HttpStatusCode>
      {
        HttpStatusCode.Accepted,
        HttpStatusCode.OK,
        HttpStatusCode.NotModified
      };

    private static readonly List<HttpStatusCode> FailStatus = new List<HttpStatusCode>
    {
      HttpStatusCode.NotFound,
      HttpStatusCode.InternalServerError,
      HttpStatusCode.BadRequest
    };

    public static Task<bool> TryGotoAsync(Uri uri)
    {
      return TryGotoAsync(uri, EnumUtil.OneAtRandom<NavigationSpeed>());
    }

    public static async Task<bool> TryGotoAsync(Uri uri, NavigationSpeed speed)
    {
      TimeSpan responseTime;
      float successRate = default;
      switch (speed)
      {
        case NavigationSpeed.Slow:
          responseTime = TimeSpan.FromSeconds(Random.Next(2, 10));
          successRate = 0.5f;
          break;
        case NavigationSpeed.Normal:
          responseTime = TimeSpan.FromMilliseconds(Random.Next(1000, 2000));
          successRate = 0.80f;
          break;
        case NavigationSpeed.Fast:
          successRate = 0.95f;
          responseTime = TimeSpan.FromMilliseconds(Random.Next(1000));
          break;
      }
      Logger.Debug("Requestion {siteUrl}, that has a {successRate}% change of success", uri, successRate*100);
      await Task.Delay(responseTime);
      var isSuccessful = Random.NextDouble() <= successRate;
      var statusCode = isSuccessful ? SuccessStatus.OneAtRandom() : FailStatus.OneAtRandom();
      Logger.Information("Requested {siteUrl} completed in {responseTime:s\\.f} s with status code {statusCode}", SiteUrls.LoginPage, responseTime, statusCode);
      return isSuccessful;
    }
  }

  public enum NavigationSpeed
  {
    Slow,
    Normal,
    Fast
  }
}
