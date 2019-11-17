using System;
using NinjaTools.FluentMockServer.Builders.Request;
using NinjaTools.FluentMockServer.FluentInterfaces;

namespace NinjaTools.FluentMockServer.Builders.Verify
{
    public interface IFluentVerificationBuilder : IFluentBuilder<Models.HttpEntities.Verify>, IFluentInterface
    {
        FluentVerificationBuilder.IWithRequest Verify(Action<IFluentHttpRequestBuilder> request);
    }
}
