using System.IO;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NinjaTools.FluentMockServer.API.Data;
using NinjaTools.FluentMockServer.API.Services;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Formatting.Json;
using Serilog.Sinks.SystemConsole.Themes;

[assembly: InternalsVisibleTo("NinjaTools.FluentMockServer.API.Tests.Exclude")]

namespace NinjaTools.FluentMockServer.API
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            WebHost
                .CreateDefaultBuilder(args)
                .UseSerilog((context, logger) => logger
                    .WriteTo.TextWriter(new CompactJsonFormatter(), new StringWriter())
                    .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                    .WriteTo.Debug()
                    .WriteTo.File(new JsonFormatter("\n-----\n"), context.Configuration.GetValue<string>("Logging:FilePath"))
                    .Enrich.FromLogContext()
                )
                .ConfigureServices((context, services) =>
                {
                    services.AddDbContext<ExpectationDbContext>(opt =>
                        {
                            // Build connection string
                            var builder = new SqlConnectionStringBuilder();
                            builder.DataSource = "localhost"; // update me
                            builder.UserID = "sa"; // update me
                            builder.Password = "59ab41dd721aa9dca2f6722a"; // update me
                            builder.InitialCatalog = "master";

                            var connectionString = builder.ConnectionString;
                            opt.UseSqlServer(new SqlConnection(connectionString));
                            opt.ConfigureWarnings(warnings => { warnings.Default(WarningBehavior.Log); });
                        })
                        .AddTransient<ExpectationService>()
                        .AddControllers()
                        .AddNewtonsoftJson()
                        .AddMvcOptions(opt => opt.EnableEndpointRouting = false);
                })
                .Configure((context, app) =>
                {
                    app.UseSerilogRequestLogging(options =>
                    {
                        // Customize the message template
                        options.MessageTemplate = "Handled {RequestPath}";

                        // Emit debug-level events instead of the defaults
                        options.GetLevel = (httpContext, elapsed, ex) => LogEventLevel.Debug;

                        // Attach additional properties to the request completion event
                        options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                        {
                            diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                            diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
                        };
                    });

                    app.UseHttpsRedirection();
                    app.UseAuthorization();
                    app.UsePathBase(new PathString("/mockconfig"));
                    app.UseMvc();
                    app.UseRouting();
                })
                .Build()
                .RunAsync();
        }
    }
}
