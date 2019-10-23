using System;

using HardCoded.MockServer.Builder.Request;
using HardCoded.MockServer.Contracts.FluentInterfaces;


namespace HardCoded.MockServer.Builder.Verify
{
    public interface IFluentVerificationBuilder : IFluentBuilder<Contracts.Models.Verify>, IFluentInterface
    {
        FluentVerificationBuilder.IWithRequest Verify(Action<IFluentHttpRequestBuilder> request);
    }
}
