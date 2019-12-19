using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NinjaTools.FluentMockServer.Models;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.Tests.TestHelpers.Mocks
{
    public class MockHandler : HttpMessageHandler
    {
        private readonly ITestOutputHelper _outputHelper;


        public MockHandler(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }


        public int RequestCounter;
        
        public List<HttpRequestMessage> Requests { get; } = new List<HttpRequestMessage>();
        public List<Expectation> Expectations { get; } = new List<Expectation>();
        
        /// <inheritdoc />
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Requests.Add(request);
            var json = await request.Content.ReadAsStringAsync();

            try {
                var content = await request.Content.ReadAsStringAsync();
                var expectation = JsonConvert.DeserializeObject<Expectation>(content);

                json = expectation.ToString();
                Expectations.Add(expectation);
            } 
            catch (Exception e) 
            {
                Console.WriteLine(e);
            } finally {
                _outputHelper.WriteLine($"REQUEST {++RequestCounter}\n{json ?? "some error occurred"}");
            }
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
        
    }
}
