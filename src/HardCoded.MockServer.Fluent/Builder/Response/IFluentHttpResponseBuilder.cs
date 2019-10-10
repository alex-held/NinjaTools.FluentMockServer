using System;
using System.ComponentModel;
using System.Net;
using FluentApi.Generics.Framework;
using HardCoded.MockServer.Models;
using HardCoded.MockServer.Models.HttpEntities;

namespace HardCoded.MockServer.Fluent.Builder
{
    /// <summary>
    /// 
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IFluentHttpResponseBuilder : IFluentInterface
    {
        IFluentHttpResponseBuilder WithBody(Action<IWithContent<IFluentHttpResponseBuilder>> contentFactory);
        IFluentHttpResponseBuilder WithDelay(Action<IFluentDelayBuilder> delayFactory);
        IFluentHttpResponseBuilder WithDelay(int value, TimeUnit timeUnit);
        IFluentHttpResponseBuilder WithConnectionOptions(Action<IFluentConnectionOptionsBuilder> connectionOptionsFactory);
        HttpResponse Build();
    }
}