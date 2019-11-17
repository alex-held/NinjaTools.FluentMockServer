using System;
using System.Net;
using NinjaTools.FluentMockServer.Builders.Request;
using NinjaTools.FluentMockServer.FluentInterfaces;
using NinjaTools.FluentMockServer.Models.HttpEntities;
using NinjaTools.FluentMockServer.Models.ValueTypes;

namespace NinjaTools.FluentMockServer.Builders.Response
{
   
    /// <summary>
    /// 
    /// </summary>
    public interface IFluentHttpResponseBuilder  : IFluentInterface
    {
        IFluentHttpResponseBuilder WithBody<T>(T payload) where T : class;
        IFluentHttpResponseBuilder WithBody(string value);
        IFluentHttpResponseBuilder WithBody(byte[] bytes, string contentType);
        IFluentHttpResponseBuilder FileBody(byte[] bytes, string filename, string contentType);
        
        
        IFluentHttpResponseBuilder WithStatusCode(int code);  
        IFluentHttpResponseBuilder WithStatusCode(HttpStatusCode code);  
        
        
        IFluentHttpResponseBuilder WithHeader(string name, string value);
        IFluentHttpResponseBuilder AddContentType(string contentType);
        
        IFluentHttpResponseBuilder ConfigureHeaders(Action<IFluentResponseHeaderBuilder> headerFactory);
        IFluentHttpResponseBuilder ConfigureConnection(Action<IFluentConnectionOptionsBuilder> connectionOptionsFactory);
        
        IFluentHttpResponseBuilder WithDelay(int value, TimeUnit timeUnit);
        
        HttpResponse Build();
    }
    
    public interface IHeaderConfigurable<out T> : IFluentInterface where T : IHeaderConfigurable<T>
    {
        T WithHeader(string name, params string[] values);
        T WithContentDispositionHeader(string type, string name, string filename);
        T WithContentType(CommonContentType contentType);
    }


    public interface IConnectionConfigurable<out T>  : IFluentInterface where T : IConnectionConfigurable<T>
    {
        T WithDelay(int value, TimeUnit timeUnit);
        
        /// <summary>
        /// Whether the MockServer should close the socket after the connection.
        /// </summary>
        T CloseSocket();
        
        /// <summary>
        /// Overrides the ContentLength Header.
        /// </summary>
        T ContentLengthHeaderOverride(long length);

        /// <summary>
        /// Disables the ContentLengthHeadeer
        /// </summary>
        T SuppressContentLengthHeader();

        /// <summary>
        /// Whether to suppress the connection header.
        /// </summary>
        T SuppressConnectionHeader();

        /// <summary>
        /// Overrides the <see cref="HttpRequest.KeepAlive"/> setting.
        /// </summary>
        T KeepAliveOverride();
    }
}
