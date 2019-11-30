using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.Domain.Models;
using NinjaTools.FluentMockServer.Domain.Models.HttpEntities;
using NinjaTools.FluentMockServer.Domain.Models.ValueTypes;
using Xunit;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.Tests
{
    
    public class SoapServiceTests
    {
        private readonly ITestOutputHelper _logger;
        
        public MockServerClient MockedServer { get; }
        
        public SoapServiceTests(ITestOutputHelper logger)
        {
            _logger = logger;

            MockedServer = new MockServerClient("http://localhost:9003/");
        }



        [Fact(Skip = "local")]
        public async Task Should_Setup_Expectation_With_Xml_Body()
        {
            // Arrange
            const string AxJournalId = "IP-100001";
            const string AxOrderId = "10001234";

            await SetupResponseForOrderPlacedRequest(AxJournalId, AxOrderId);
            
            // Act

            // Assert
        }
        
        [Fact(Skip = "local")]
        public async Task Should_Setup_Expectation_With_Xml_Body_When_Setup_Using_Predefined_Setup()
        {
            // Arrange
            const string AxJournalId = "IP-100001";
            const string AxOrderId = "10001234";

            var response = ResponseTemplate
                .Replace("{AxJournalId}", AxJournalId)
                .Replace("{AxOrderId}",   AxOrderId);

            var httpResponse = new HttpResponse
            {
                // TODO: Check Content
                Body = new JValue(response),
                Delay = new Delay
                {
                    Value = 50, TimeUnit = TimeUnit.Milliseconds
                },
                Headers = new Dictionary<string, string[]>
                {
                    {"Content-Type", new[] {$"text/xml; charset=utf-8"}}
                }
            };
            
            var expectation = new Expectation
            {
                HttpResponse = httpResponse
            };

            var responseJson = JsonConvert.SerializeObject(httpResponse);
            _logger.WriteLine($"responseJson: \n{responseJson}");

            var expectationJson = expectation.ToString();
            _logger.WriteLine($"expectationJson: \n{expectationJson}");
            
            var setup = new MockServerSetup();
            setup.Expectations.Add(expectation);

            var setupJson = JsonConvert.SerializeObject(setup, Formatting.Indented);
            _logger.WriteLine($"Setup json: \n{setupJson}");
            
            
            // Act
           await MockedServer.SetupAsync(setup);
            
            
            // Assert
        }
        
        
        public async Task SetupResponseForOrderPlacedRequest(string axJournalId, string axOrderId = null)
        {
            axOrderId = string.IsNullOrWhiteSpace(axOrderId) ? "" : axOrderId;

            var response = ResponseTemplate
                        .Replace("{AxJournalId}", axJournalId)
                        .Replace("{AxOrderId}",   axOrderId);
            
            await SetupResponseForRequestToReturnBody("/", response, HttpMethod.Post);
        }

        protected const string ResponseTemplate =
            @"<?xml version=""1.0"" encoding=""utf-8""?>
                    <soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
                        <soap:Body>
                            <createIgsAxdTmpSalesTableResponse xmlns=""http://schemas.microsoft.com/dynamics/2006/02/documents/IgsAxdTmpSalesTable"">
                                <EntityKey xmlns=""http://schemas.microsoft.com/dynamics/2006/02/documents/EntityKey"">
                                    <KeyData xmlns=""http://schemas.microsoft.com/dynamics/2006/02/documents/EntityKey"">
                                        <KeyField>
                                            <Field>SalesId</Field>
                                            <Value>{AxOrderId}</Value>
                                        </KeyField>
                                        <KeyField>
                                            <Field>ErrorMsg</Field>
                                            <Value></Value>
                                        </KeyField>
                                        <KeyField>
                                            <Field>SalesImportJourId</Field>
                                            <Value>{AxJournalId}</Value>
                                        </KeyField>
                                    </KeyData>
                                </EntityKey>
                            </createIgsAxdTmpSalesTableResponse>
                        </soap:Body>
                    </soap:Envelope>";
    
        
        /// <summary>
        ///     Setups the response for post request to return body.
        /// </summary>
        /// <param name="path">The path in the uri after the baseUri.</param>
        /// <param name="responseBody">The response body.</param>
        /// <param name="method">The method.</param>
        /// <param name="contentType">Type of the content.</param>
        protected async Task SetupResponseForRequestToReturnBody(
            string path,
            string responseBody,
            HttpMethod method,
            string contentType = "text/xml"
        )
        {
            contentType = $"{contentType}; charset=utf-8";
         
            
            await MockedServer.SetupAsync(
                builder =>
                    builder
                        .OnHandling(method, request => request.WithPath(path))
                        .RespondOnce(HttpStatusCode.OK, response =>
                            response
                                .WithBody(responseBody)
                                .WithDelay(50, TimeUnit.Milliseconds)
                                .AddContentType(contentType))
                        .Setup()
            );
        }
    }
}
