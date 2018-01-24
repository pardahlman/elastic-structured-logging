namespace Scenarios.Offline
{
  using System;
  using System.Collections.Generic;
  using System.Threading;
  using System.Threading.Tasks;

  using Serilog;
  using Serilog.Context;

  public interface IActivity
  {
    Task RunAsync(CancellationToken ct = default);
  }

  public class UserLogin : IActivity
  {
    private readonly ILogger _logger = Log.ForContext<UserLogin>();
    private readonly List<IDisposable> _globalScopes;

    public UserLogin()
    {
      _globalScopes = new List<IDisposable>();
    }

    public async Task RunAsync(CancellationToken ct = default)
    {
      _globalScopes.Add(LogContext.PushProperty("sessionId", Guid.NewGuid()));
      _logger.Information("Starting session {sessionId}");
      _logger.Debug("Picking randomly user for session.");
      _logger.Verbose("Total user stock is {userCount}", Users.All.Count);
      var activeUser = Users.All.OneAtRandom();

      _logger.Information("User {@user} selected", activeUser);
      _globalScopes.Add(LogContext.PushProperty("userId", activeUser.UserId));

      while (!await NavigationSimulator.TryGotoAsync(SiteUrls.LoginPage))
      {
        _logger.Information("Navigation failed, trying again.");
      };

      var loginOutcome = EnumUtil.OneAtRandom<LoginOutcome>();

      foreach (var scope in _globalScopes)
      {
        scope.Dispose();
      }
    }
  }

  public enum LoginOutcome
  {
    Success,
    AccountNotActivated,
    PasswordIncorrect,
    SystemError
  }

  public class SiteUrls
  {
    public static readonly Uri LoginPage = new Uri("https://www.example.com/login");
  }

  public class User
  {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Guid UserId { get; set; }
    public string Email { get; set; }
    public ushort Age { get; set; }
  }
}
