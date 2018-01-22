using Serilog;
using Serilog.Events;
using Xunit;

namespace Definitions
{
    public class UnitTest1
    {
        [Fact]
        public void Should_Capture_Message_Template_In_Log_Event()
        {
            /* Setup */
            var testSink = new TestSink();
            var logger = new LoggerConfiguration()
              .WriteTo.Sink(testSink)
              .CreateLogger();

            const string messageTemplate = "This is a message template without any holes.";

            /* Test */
            logger.Information(messageTemplate);

            /* Assert */
            Assert.Equal(messageTemplate, testSink.ReceivedEvent.MessageTemplate.Text); // capture template
            Assert.Empty(testSink.ReceivedEvent.Properties); // no properties provided
        }

        [Fact]
        public void Should_Capture_Event_Data()
        {
            /* Setup */
            var testSink = new TestSink();
            var logger = new LoggerConfiguration()
                .WriteTo.Sink(testSink)
                .CreateLogger();

            const string messageTemplate = "The message template has Named Holes like this: {namedHole}";
            const string eventData = "eventData";

            /* Test */
            logger.Information(messageTemplate, eventData);

            /* Assert */
            Assert.Equal(eventData, testSink.ReceivedEvent.Properties["namedHole"].ToString());
        }

        [Fact]
        public void Should_Capture_Complex_Event_Data()
        {
            /* Setup */
            var testSink = new TestSink();
            var logger = new LoggerConfiguration()
                .WriteTo.Sink(testSink)
                .CreateLogger();

            const string eventData = "eventData";

            /* Test */
            logger.Information("This log event comes with an object {testObj}", new {Width = 200, Height = 150});

            /* Assert */
            Assert.Equal(eventData, testSink.ReceivedEvent.Properties["namedHole"].ToString());
        }
    }
}
