using System;
using System.ComponentModel;
using HardCoded.MockServer.Contracts.FluentInterfaces;
using HardCoded.MockServer.Fluent.Builder.Request;
using HardCoded.MockServer.Models.HttpEntities;
using HardCoded.MockServer.Models.ValueTypes;

namespace HardCoded.MockServer.Fluent.Builder.Response
{
    /// <summary>
    /// 
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IFluentHttpResponseBuilder : IFluentInterface
    {
        IFluentHttpResponseBuilder WithBody(Action<IFluentBodyBuilder> bodyFactory);
       // IFluentHttpResponseBuilder WithBody(Action<IWithContent<IFluentHttpResponseBuilder>> contentFactory);
        IFluentHttpResponseBuilder WithDelay(Action<IFluentDelayBuilder> delayFactory);
        IFluentHttpResponseBuilder WithDelay(int value, TimeUnit timeUnit);
        IFluentHttpResponseBuilder WithConnectionOptions(Action<IFluentConnectionOptionsBuilder> connectionOptionsFactory);
        HttpResponse Build();
    }
}