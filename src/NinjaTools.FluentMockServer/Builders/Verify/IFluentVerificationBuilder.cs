using System;

using NinjaTools.FluentMockServer.FluentInterfaces;


namespace NinjaTools.FluentMockServer.Builders
{
    public interface IFluentVerificationBuilder : IFluentBuilder<Models.HttpEntities.Verify>, IFluentInterface
    {
        FluentVerificationBuilder.IWithRequest Verify(Action<IFluentHttpRequestBuilder> request);
    }
}
