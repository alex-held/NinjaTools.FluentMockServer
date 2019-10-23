using System;
using System.ComponentModel;
using HardCoded.MockServer.Contracts.FluentInterfaces;
using HardCoded.MockServer.Contracts.Models.HttpEntities;
using HardCoded.MockServer.Contracts.Models.ValueTypes;
using HardCoded.MockServer.Fluent.Builder.Request;


namespace HardCoded.MockServer.Fluent.Builder.Response
{
    /// <summary>
    /// 
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IFluentHttpResponseBuilder : IFluentInterface
    {
        IFluentHttpResponseBuilder WithBody(Action<IFluentBodyBuilder> bodyFactory);
        IFluentHttpResponseBuilder WithDelay(Action<IFluentDelayBuilder> delayFactory);
        IFluentHttpResponseBuilder WithDelay(int value, TimeUnit timeUnit);
        IFluentHttpResponseBuilder WithHeader(string name, string value);
        IFluentHttpResponseBuilder WithConnectionOptions(Action<IFluentConnectionOptionsBuilder> connectionOptionsFactory);
        HttpResponse Build();
    }
}
