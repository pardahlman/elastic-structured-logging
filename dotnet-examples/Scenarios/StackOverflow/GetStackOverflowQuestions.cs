using System.Threading;
using System.Threading.Tasks;
using Scenarios.Offline;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using Serilog.Context;
using Serilog.Core.Enrichers;
using SerilogTimings.Extensions;

namespace Scenarios.StackOverflow
{
    public class GetStackOverflowQuestions : IActivity
    {
        private const int NumberOfPages = 1;
        private const int PageSize = 100;

        private readonly ILogger _logger = Log.ForContext<GetStackOverflowQuestions>();

        public async Task RunAsync(CancellationToken ct)
        {
            var serializer = new JsonSerializer
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                },
                Converters = {new EpochJsonConverter()}
            };
            var decompressingClientHandler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip
            };
            var httpClient = new HttpClient(decompressingClientHandler)
            {
                BaseAddress = new Uri("https://api.stackexchange.com/2.2/search"),
                DefaultRequestHeaders =
                {
                    Accept =
                    {
                        new
                            MediaTypeWithQualityHeaderValue(
                                "application/json")
                    }
                }
            };

            using (httpClient)
            {
                for (var pageIndex = 1; pageIndex <= NumberOfPages; pageIndex++)
                {
                    using (LogContext.PushProperty("pageIndex", pageIndex))
                    {
                        _logger.Information(
                            "Preparing to request page {pageIndex} StackOverflow"); // implicit use property from context/scope.

                        HttpResponseMessage response;
                        using (_logger.TimeOperation("Querying StackExchange API"))
                        {
                            var requestUrl =
                                $"questions?page={pageIndex}&pagesize={PageSize}&order=desc&sort=creation&site=stackoverflow";
                            response = await httpClient.GetAsync(requestUrl, ct);
                            _logger.Debug("Request to {requestUri} return response with status code {statusCode}",
                                requestUrl, response.StatusCode);
                        }

                        if (!response.IsSuccessStatusCode)
                        {
                            _logger.Warning(
                                "The request to StackExchange API was unsuccessful. Status code: {statusCode}, Reason {failReason}",
                                response.StatusCode, response.ReasonPhrase);
                        }

                        string jsonPayload;
                        using (_logger.TimeOperation("Read response content"))
                        {
                            jsonPayload = await response.Content.ReadAsStringAsync();
                        }

                        _logger.Verbose("The response is {numberOfChars} chars long", jsonPayload.Length);

                        using (var textReader = new StringReader(jsonPayload))
                        using (var jsonReader = new JsonTextReader(textReader))
                        {
                            try
                            {
                                var questions = serializer.Deserialize<PaginationResult<Question>>(jsonReader);
                                foreach (var question in questions.Items)
                                {
                                    using (LogContext.Push(new PropertyEnricher("questionId", question.QuestionId)))
                                    {
                                        _logger.Verbose("Received question {@question}", question);
                                        _logger.Debug("The question has {tagCount} tags", question.Tags.Count);
                                        foreach (var questionTag in question.Tags)
                                        {
                                            _logger.Information("Question is tagged with {questionTag}", questionTag);
                                        }
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                _logger.Error(e, "An unhandled error occured when deserializing response.");
                            }
                            finally
                            {
                                response.Dispose();
                            }
                        }
                    }
                }
            }
        }
    }
}