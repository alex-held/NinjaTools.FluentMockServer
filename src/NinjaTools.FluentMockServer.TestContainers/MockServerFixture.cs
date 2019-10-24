using System;
using System.Threading.Tasks;

using Xunit;
using Xunit.Abstractions;


namespace NinjaTools.FluentMockServer.TestContainers
{
    public class MockServerFixture : IClassFixture<MockServerRunner>
    {
        protected ITestOutputHelper OutputHelper { get; }
        public string MockServerEndpoint { get; }
        public Uri MockServerUri { get; }
        public ILogger Logger { get; set; }
        
        public MockServerFixture(MockServerRunner mockServerRunner, ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
            Logger = new TestConsoleLogger(outputHelper);
            MockServerEndpoint = mockServerRunner.MockServerEndpoint;
            MockServerUri = new Uri(MockServerEndpoint);
        }

        protected async Task Using(Func<MockServerClient, Task> clientResetScope)
        {
            using var client = new MockServerClient(MockServerUri);
            await clientResetScope(client);
        }
    }

    public interface ILogger
    {
        void LogInformation(string message, params object[] args);

        void LogWarning(string message, params object[] args);
        
        void LogError(string message, params object[] args);
    }

    internal class TestConsoleLogger : ILogger
    {
        private readonly ITestOutputHelper _testOutputHelper;

        internal TestConsoleLogger(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        private void Write(string message, params object[] args)
        {
            _testOutputHelper.WriteLine(message, args);
            Console.WriteLine(message, args);
            Console.ResetColor();
        }
        
        /// <inheritdoc />
        public void LogInformation(string message, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
           Write(message, args);
        }

        /// <inheritdoc />
        public void LogWarning(string message, params object[] args) 
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Write(message, args);
        }
        
        /// <inheritdoc />
        public void LogError(string message, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Write(message, args);
        }
    }
}
