using System;

namespace NinjaTools.FluentMockServer.API.DependencyInjection
{
    public class Paths
    {
        public string CONFIG_BASE_PATH { get; set; }
        public string CONFIG_FILE_PATH { get; set; }
        public string LOG_PATH { get; set; }
    }

    public class ServiceConstants
    {
        public const string KEY = "MOCKSERVER";

        public string APPLICATION_NAME { get; set; }
        public string APPLICATION_VERSION { get; set; }
        public string SWAGGER_PATH { get; set; }
        public string SWAGGER_VERSION{ get; set; }
        public string HEALTH_CHECK_PATH { get; set; }
        public int ADMIN_PORT{ get; set; }
        public int MOCK_PORT { get; set; }
        public string ASPNETCORE_URLS{ get; set; }
        public bool RunsInContainer => Environment.GetEnvironmentVariable("RUNNING_IN_CONTAINER") == "yes";
        public string ContactPerson { get; set; } = "Alexander Held";
        public string ContactEmail { get; set; } = "contact@alexanderheld.net";

        public Paths PATHS { get; set; }
    }
}
