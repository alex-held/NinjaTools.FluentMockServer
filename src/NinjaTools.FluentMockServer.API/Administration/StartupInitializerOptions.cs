using System;
using NinjaTools.FluentMockServer.API.Configuration;
using NinjaTools.FluentMockServer.API.Logging;
using NinjaTools.FluentMockServer.API.Types;

namespace NinjaTools.FluentMockServer.API.Administration
{
    public class StartupInitializerOptions
    {

        public PlatformID ExecutingPlatform => Environment.OSVersion.Platform;
        public bool IsRunningInDocker => Environment.GetEnvironmentVariable("MOCKSERVER_RUNNING_IN_CONTAINER") is {};
        public bool EnableLoggingInitializer { get; set; }
        public bool EnableConfigInitializer { get; set; }


        public bool IsEnabled(IInitializer initializer)
        {
            return initializer switch
            {
                ILoggingInitializer _ when !EnableLoggingInitializer => false,
                IConfigurationInitializer _ when !EnableConfigInitializer => false,
                { } _ => true,
                _ => false
            };
        }
    }
}
