using System.IO;

namespace NinjaTools.FluentMockServer.API.Logging
{
    public static class MockServerPaths
    {
        private const string LogRoot = "/var/log/mock-server";

        public static class Logs
        {
            public static readonly string Setups = Path.Combine(LogRoot, "setups");
            public static readonly string Requests = Path.Combine(LogRoot, "requests");
        }

        public static string Configs { get; } = "/etc/mock-server/config/";
    }
}
