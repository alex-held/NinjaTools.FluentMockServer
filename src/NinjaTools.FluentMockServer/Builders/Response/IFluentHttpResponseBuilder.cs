using System;
using System.ComponentModel;

using NinjaTools.FluentMockServer.FluentInterfaces;
using NinjaTools.FluentMockServer.Models.HttpEntities;
using NinjaTools.FluentMockServer.Models.ValueTypes;


namespace NinjaTools.FluentMockServer.Builders
{
    /// <summary>
    /// 
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IFluentHttpResponseBuilder : IFluentInterface
    {
        IFluentHttpResponseBuilder WithDelay(Action<IFluentDelayBuilder> delayFactory);
        IFluentHttpResponseBuilder WithDelay(int value, TimeUnit timeUnit);
        IFluentHttpResponseBuilder WithHeader(string name, string value);
        IFluentHttpResponseBuilder WithHeaders(Action<IFluentHeaderBuilder> headerFactory);
        IFluentHttpResponseBuilder WithConnectionOptions(Action<IFluentConnectionOptionsBuilder> connectionOptionsFactory);
        IFluentHttpResponseBuilder WithLiteralBody(string literal, string contentType = null);
        IFluentHttpResponseBuilder WithBinaryFileBody(byte[] bytes, string filename, string contentType);
        IFluentHttpResponseBuilder WithBinaryBody(string base64Bytes, string contentType);
        HttpResponse Build();
    }
}
