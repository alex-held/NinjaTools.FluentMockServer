using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NinjaTools.FluentMockServer.API.Configuration;
using NinjaTools.FluentMockServer.API.DependencyInjection;
using NinjaTools.FluentMockServer.API.Infrastructure;
using NinjaTools.FluentMockServer.API.Services;
using NinjaTools.FluentMockServer.API.Types;
using NinjaTools.FluentMockServer.Tests.TestHelpers;
using Xunit;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.API.Tests.DependencyInjection
{
    public class MockServerBuilderTests : XUnitTestBase<MockServerBuilder>
    {
        /// <inheritdoc />
        public MockServerBuilderTests(ITestOutputHelper output) : base(output)
        {
        }

        private IServiceCollection GetServiceCollection(out IConfiguration config)
        {
            var configBuilder = new ConfigurationBuilder();

            configBuilder.AddInMemoryCollection(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("MOCKSERVER_ADMIN_PORT", "1080")
            });

            config = configBuilder.Build();
            var services = new ServiceCollection();
            services.AddSingleton(config);
            services.AddSingleton<IConfiguration>(config);
            return services;
        }

        private IMockServerBuilder CreateSubject()
        {
            var services = GetServiceCollection(out var config);
            var builder  = new MockServerBuilder(services, config);
            return builder;
        }

        [Fact]
        public void Should_Return_When_State()
        {
            // Arrange
            var sut = CreateSubject().Services.BuildServiceProvider();

            // Assert
            sut.EnsureRegistered<ISetupRepository>();
            sut.EnsureRegistered<ISetupService>();
            sut.EnsureRegistered<IHttpContextAccessor>();
            sut.EnsureRegistered<IConfigurationService>();
            sut.EnsureRegistered<IConfigFileProvider>();
            sut.EnsureRegistered<ILogService>();

            sut.EnsureRegistered<IStartupInitializer>();
            sut.EnsureRegistered<IOptions<MockServerOptions>>().Value.Should().NotBeNull();
            sut.EnsureRegistered<ILogger<MockServerBuilderTests>>();
        }

        [Fact]
        public void Startup_Configure_Should_Register_All_Required_Services()
        {
            var host = Program.CreateHostBuilder(null).Build();

            // Arrange
            host.Services.EnsureServicesRegistered();
        }

    }

    public static class MockServerBuilderTestExtensions
    {
        public static T EnsureRegistered<T>(this IServiceProvider sp)
        {
            return sp.GetRequiredService<T>();
        }

        public static void EnsureServicesRegistered(this IServiceProvider sp)
        {
            sp.EnsureRegistered<ISetupRepository>();
            sp.EnsureRegistered<ISetupService>();
            sp.EnsureRegistered<IHttpContextAccessor>();
            sp.EnsureRegistered<IConfigurationService>();
            sp.EnsureRegistered<IConfigFileProvider>();
            sp.EnsureRegistered<ILogService>();

            sp.EnsureRegistered<IStartupInitializer>();
            sp.EnsureRegistered<IOptions<MockServerOptions>>().Value.Should().NotBeNull();
            sp.EnsureRegistered<ILogger<MockServerBuilderTests>>();
        }

    }
}
