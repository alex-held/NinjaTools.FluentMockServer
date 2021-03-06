using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;

namespace NinjaTools.FluentMockServer.Xunit
{
    /// <summary>
    /// The InMemory handle to the MockServer Docker Container
    /// </summary>
    internal class MockServerContainer : IDisposable
    {
        public const string Prefix = "NinjaTools-FluentMockServer";
        public const string Suffix = "netcore-xunit-background-runner";
        public static readonly string ContainerName = $"{Prefix}-{Suffix}";

        /// <summary>
        /// Gets the Port exposed to the Host.
        /// </summary>
        public int HostPort { get; }

        /// <summary>
        /// Gets the <see cref="ContainerService"/>. 
        /// </summary>
        public IContainerService ContainerService { get; }


        private static int GetAvailablePort(int startingPort)
        {
            var properties = IPGlobalProperties.GetIPGlobalProperties();

            //getting active connections
            var tcpConnectionPorts = properties.GetActiveTcpConnections()
                .Where(n => n.LocalEndPoint.Port >= startingPort)
                .Select(n => n.LocalEndPoint.Port);

            //getting active tcp listners - WCF service listening in tcp
            var tcpListenerPorts = properties.GetActiveTcpListeners()
                .Where(n => n.Port >= startingPort)
                .Select(n => n.Port);

            //getting active udp listeners
            var udpListenerPorts = properties.GetActiveUdpListeners()
                .Where(n => n.Port >= startingPort)
                .Select(n => n.Port);

            var port = Enumerable
                .Range(startingPort, ushort.MaxValue)
                .Where(i => !tcpConnectionPorts.Contains(i))
                .Where(i => !tcpListenerPorts.Contains(i))
                .FirstOrDefault(i => !udpListenerPorts.Contains(i));

            return port;
        }

        public MockServerContainer()
        {
            var hosts = new Hosts().Discover(true);
            var docker = hosts.FirstOrDefault(x => x.IsNative) ?? hosts.FirstOrDefault(x => x.Name == "default");

            if (docker?.GetRunningContainers().FirstOrDefault(c => c.Name == ContainerName) is {} container)
            {
                ContainerService = container;
                if (ContainerService.GetConfiguration().NetworkSettings.Ports.TryGetValue("1080/tcp", out var endpoints))
                {
                    HostPort = int.Parse(endpoints.First().HostPort);
                }
            }
            else
            {
                HostPort = GetAvailablePort(3000);

                ContainerService = new Builder()
                    .UseContainer()
                    .ReuseIfExists()
                    .WithName(ContainerName)
                    .UseImage(ContainerImage)
                    .ExposePort(HostPort, ContainerPort)
                    .Build();
            }
        }


        /// <summary>
        /// The MockServer Docker Image
        /// </summary>
        public const string ContainerImage = "jamesdbloom/mockserver";

        /// <summary>
        /// The Port beeing exposed to the Host. 
        /// </summary>
        public const int ContainerPort = 1080;

        /// <summary>
        /// Gets the BaseUrl of the MockServer.
        /// </summary>
        public string MockServerBaseUrl => $"http://localhost:{HostPort}";


        /// <summary>
        /// Waits until the MockServer is finished with initialization. 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception">Thrown when not being able to connect to the MockServer Container.</exception>
        private async Task WaitUntilContainerStarted()
        {
            var httpClient = new HttpClient();
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            while (stopwatch.IsRunning && stopwatch.Elapsed < TimeSpan.FromMinutes(2))
            {
                try
                {
                    var request = new HttpRequestMessage(HttpMethod.Put, MockServerBaseUrl + "/mockserver/status");
                    var response = await httpClient.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        httpClient.Dispose();
                        return;
                    }
                }
                catch (Exception)
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(100));
                }
            }

            httpClient.Dispose();
            throw new Exception($"Could not start MockServer TestContainer.");
        }

        /// <summary>
        /// Starts the MockServer.
        /// </summary>
        public Task StartAsync()
        {
            ContainerService.Start();
            return WaitUntilContainerStarted();
        }

        /// <summary>
        /// Stops the MockServer Container.
        /// </summary>
        public Task StopAsync()
        {
            ContainerService.Stop();
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public void Dispose()
        {
        }
    }
}
