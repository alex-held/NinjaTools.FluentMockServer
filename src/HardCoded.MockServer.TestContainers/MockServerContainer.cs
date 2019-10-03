using System;
using System.Net.Http;
using System.Threading.Tasks;
using Polly;
using TestContainers.Core.Containers;

namespace HardCoded.MockServer.TestContainers
{
    public class MockServerContainer : Container
    {
        public const string ContainerImage = "jamesdbloom/mockserver";
        public const int ExposedPort = 1080;
        
        public string MockServerBaseUrl => $"http://{GetDockerHostIpAddress()}:{GetMappedPort(ExposedPort)}";


        public int GetMappedPort() => GetMappedPort(ExposedPort);
        
        /// <inheritdoc />
        protected override async Task WaitUntilContainerStarted()
        {
            await base.WaitUntilContainerStarted();
            await Task.Delay(1000);
            
            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Put, MockServerBaseUrl + "/status");
            
            var policyResult = await Policy
                .TimeoutAsync(TimeSpan.FromMinutes(2))
                .WrapAsync(Policy.Handle<HttpRequestException>().WaitAndRetryForeverAsync(
                    iteration => TimeSpan.FromSeconds(10), (exception, timespan) => { Console.WriteLine(exception.Message); }))
                .ExecuteAndCaptureAsync(() => httpClient.SendAsync(request));
            
            if (policyResult.Outcome == OutcomeType.Failure)
                throw new Exception(policyResult.FinalException.Message);
        }
    }
}