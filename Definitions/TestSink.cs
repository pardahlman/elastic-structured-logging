using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;

namespace Definitions
{
  public class TestSink : ILogEventSink
  {
    private readonly ConcurrentQueue<LogEvent> _receivedEvents;

    public LogEvent ReceivedEvent => _receivedEvents.FirstOrDefault();
    public IList<LogEvent> ReceivedEvents => _receivedEvents.ToList().AsReadOnly();

    public TestSink()
    {
      _receivedEvents = new ConcurrentQueue<LogEvent>();
    }

    public void Emit(LogEvent logEvent)
    {
      _receivedEvents.Enqueue(logEvent);
    }
  }

  public static class LoggerSinkExtensions
  {
    public static LoggerConfiguration TestSink(this LoggerSinkConfiguration cfg, LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose)
    {
      return cfg.Sink(new TestSink(), restrictedToMinimumLevel);
    }
  }
}
