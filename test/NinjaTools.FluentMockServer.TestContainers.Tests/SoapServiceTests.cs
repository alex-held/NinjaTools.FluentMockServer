using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.FluentAPI.Builders;
using NinjaTools.FluentMockServer.Models.HttpEntities;
using NinjaTools.FluentMockServer.Models.ValueTypes;
using NinjaTools.FluentMockServer.Tests.TestHelpers;
using Xunit;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.TestContainers.Tests
{
    public class SoapServiceTests : XUnitTestBase<SoapServiceTests>, IClassFixture<MockServerFixture>
    {
        private readonly MockServerFixture _fixture;

        public SoapServiceTests(ITestOutputHelper output, MockServerFixture fixture) : base(output)
        {
            _fixture = fixture;
        }

        public MockServerClient MockedServer => _fixture.Client;

        [Fact]
        public async Task Should_Setup_Expectation_With_Xml_Body_When_Setup_Using_Predefined_Setup()
        {
            // Arrange
            const string id1 = "IP-100001";
            const string id2 = "10001234";

            var response = ResponseTemplate
                .Replace("{Id1}", id1)
                .Replace("{Id2}",   id2);

            var httpResponse = HttpResponse.Create(
                body: new JValue(response),
                delay: new Delay(TimeUnit.Milliseconds, 50),
                headers: new Dictionary<string, string[]>
                {
                    {"Content-Type", new[] {$"text/xml; charset=utf-8"}}
                });
            
            var expectation = FluentExpectationBuilder.Create(httpResponse: httpResponse);

            var responseJson = JsonConvert.SerializeObject(httpResponse);
            Output.WriteLine($"responseJson: \n{responseJson}");

            var expectationJson = expectation.ToString();
            Output.WriteLine($"expectationJson: \n{expectationJson}");
            
            var setup = new MockServerSetup();
            setup.Expectations.Add(expectation);

            var setupJson = JsonConvert.SerializeObject(setup, Formatting.Indented);
            Output.WriteLine($"Setup json: \n{setupJson}");
            
            
            // Act
           await MockedServer.SetupAsync(setup);
           await SetupResponseForOrderPlacedRequest(id1, id2);

           // Assert
           // TODO: verify setup
        }
        
        private async Task SetupResponseForOrderPlacedRequest(string id1, string id2 = null)
        {
            id2 = string.IsNullOrWhiteSpace(id2) ? "" : id2;

            var response = ResponseTemplate
                        .Replace("{Id1}", id1)
                        .Replace("{Id2}",   id2);
            
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
                                            <Value>{Id2}</Value>
                                        </KeyField>
                                        <KeyField>
                                            <Field>ErrorMsg</Field>
                                            <Value></Value>
                                        </KeyField>
                                        <KeyField>
                                            <Field>SalesImportJourId</Field>
                                            <Value>{Id1}</Value>
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
