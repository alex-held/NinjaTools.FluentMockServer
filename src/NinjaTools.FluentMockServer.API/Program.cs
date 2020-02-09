using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace NinjaTools.FluentMockServer.API
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
                .ConfigureWebHost(b => b.ConfigureAppConfiguration((ctx, config) => config
                    .AddJsonFile("appsettings.json").AddEnvironmentVariables()))
                .ConfigureLogging((ctx, logging) => logging.AddConsole().AddDebug()
                    .AddApplicationInsights(ctx.Configuration.GetValue<string>("ApplicationInsights:InstrumentationKey")));
    }
}
