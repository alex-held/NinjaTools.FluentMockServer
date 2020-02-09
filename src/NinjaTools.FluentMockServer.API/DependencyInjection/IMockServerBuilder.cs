using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NinjaTools.FluentMockServer.API.DependencyInjection
{
    public interface IMockServerBuilder
    {
        IServiceCollection Services { get; }

        IConfiguration Configuration { get; }

        IMvcCoreBuilder MvcCoreBuilder { get; }

        ServiceConstants Constants { get; }
    }
}
