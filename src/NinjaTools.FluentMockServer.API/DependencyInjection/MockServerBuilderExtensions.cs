using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using NinjaTools.FluentMockServer.API.Administration;

namespace NinjaTools.FluentMockServer.API.DependencyInjection
{
    public static class MockServerBuilderExtensions
    {
        [NotNull]
        public static IMockServerBuilder AddAdminPath([NotNull] this IMockServerBuilder builder, int port = 1080)
        {
            builder.Services.Configure<AdminOptions>(opt => { opt.Port = port; });

            return builder;
        }

    }
}
