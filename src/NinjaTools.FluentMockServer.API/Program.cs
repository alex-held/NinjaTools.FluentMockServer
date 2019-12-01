using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Enrichers;
using Serilog.Extensions.Logging;
using Serilog.Formatting.Compact;

namespace NinjaTools.FluentMockServer.API
{
    public static class Program
    {
        private static readonly LoggerProviderCollection Providers = new LoggerProviderCollection();

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((ctx, config) => {
                    var serviceStartupTimeStamp = DateTime.Now.ToString("u");
                    var name = AppDomain.CurrentDomain.FriendlyName;
                    var logFileName = $"{name}-{serviceStartupTimeStamp}.log";
                    var rootedPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", logFileName);

                    config.ReadFrom.Configuration(ctx.Configuration)
                        .MinimumLevel.Information()
                        .Enrich.FromLogContext()
                        .Enrich.With<EnvironmentUserNameEnricher>()
                        .Enrich.With<MachineNameEnricher>()
                        .WriteTo.Providers(Providers)
                        .WriteTo.File(new RenderedCompactJsonFormatter(), rootedPath )
                        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}");
                }, writeToProviders: true)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}
