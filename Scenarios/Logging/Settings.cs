using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using Serilog.Sinks.Elasticsearch;

namespace Scenarios.Logging
{
  public class Settings
  {
    private static readonly string BasicAuth = Convert.ToBase64String(Encoding.UTF8.GetBytes("elastic:changeme"));

    public static readonly ElasticsearchSinkOptions ElasticOptions =
      new ElasticsearchSinkOptions(new Uri("http://elasticsearch:9200"))
      {
        IndexFormat = "scenario-{0:yyyy.MM.dd}",
        AutoRegisterTemplate = true,
        ModifyConnectionSettings =
          c => c.GlobalHeaders(new NameValueCollection {{"Authorization", $"Basic {BasicAuth}"}})
      };
  }
}
