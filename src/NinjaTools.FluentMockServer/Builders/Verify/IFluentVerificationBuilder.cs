using System;
using NinjaTools.FluentMockServer.Builders.Request;
using NinjaTools.FluentMockServer.FluentInterfaces;

namespace NinjaTools.FluentMockServer.Builders.Verify
{
    public interface IFluentVerificationBuilder : IFluentBuilder<Domain.Models.HttpEntities.Verify>
    {
        FluentVerificationBuilder.IWithRequest Verify(Action<IFluentHttpRequestBuilder> request);
    }
}
