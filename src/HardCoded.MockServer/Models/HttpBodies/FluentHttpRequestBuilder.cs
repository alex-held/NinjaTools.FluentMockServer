using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using HardCoded.MockServer.Fluent;
using HardCoded.MockServer.Models.HttpEntities;
using Newtonsoft.Json;

[assembly: InternalsVisibleTo("HardCoded.MockServer.Tests")]
namespace HardCoded.MockServer.Models.HttpBodies
{
    /// <summary>
    /// 
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IBuildFluentHttpRequestPath : IFluentInterface
    {
        IBuildFluentHttpRequestOptions WithPath(string path);
    }

    /// <summary>
    /// 
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IBuildFluentHttpRequestMethods : IFluentInterface
    {
        IBuildFluentHttpRequestPath WithMethod(HttpMethod method);
        IBuildFluentHttpRequestPath Post { get; }
        IBuildFluentHttpRequestPath Get { get; }
        IBuildFluentHttpRequestPath Put { get; }
    }
    
    /// <summary>
    /// 
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IBuildFluentHttpRequestContent : IFluentInterface
    {
        void SendXml(string content);
        void SendJson(string content, MatchType? matchType = null);
        void SendBinary(byte[] content);
        
        HttpRequest WithXml(string content);
        HttpRequest WithJson(string content, MatchType? matchType = null);
        HttpRequest WithJsonData<T>(params T[] items);
        HttpRequest WithJson<T>(T item);
        HttpRequest WithBinary(byte[] content);
    }

    /// <summary>
    /// 
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IFinishedBuildingHttpRequest : IFluentInterface
    {
    }
    
    /// <summary>
    /// 
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IBuildFluentHttpRequestOptions : IFluentInterface
    {
        IBuildFluentHttpRequestOptions UseEncryption(bool useSsl = true);
        IBuildFluentHttpRequestOptions KeepConnectionAlive(bool keepAlive = true);
        IBuildFluentHttpRequestOptions WithQueryStringParameters(params (string name, string value)[] parameters);
        IBuildFluentHttpRequestOptions WithHeaders(params (string name, object value)[] headers);
        IBuildFluentHttpRequestOptions WithCookies(params (string name, string value)[] cookies);
        IFinishedBuildingHttpRequest Build(Action<IBuildFluentHttpRequestContent> contentBuilder);
        HttpRequest BuildRequest(Func<IBuildFluentHttpRequestContent, HttpRequest> request);
        IBuildFluentHttpRequestContent Payload { get; }
    }
    
    internal sealed class FluentHttpRequestBuilder : 
        IBuildFluentHttpRequestPath,
        IBuildFluentHttpRequestMethods, 
        IBuildFluentHttpRequestContent,
        IBuildFluentHttpRequestOptions, 
        IFinishedBuildingHttpRequest
    {
        public FluentHttpRequestBuilder(FluentExpectationBuilder fluentExpectationBuilder)
        {
            HttpRequest = new HttpRequest(fluentExpectationBuilder);
        }
        
        protected HttpRequest HttpRequest { get; }
        
        public RequestBody Body { get; set; }
        public string Path { get; set; }

        public Dictionary<string, object> QueryParameters { get; set; }

        public IBuildFluentHttpRequestOptions WithPath(string path)
        {
            Path = path;
            return this;
        }

        /// <inheritdoc />
        public IBuildFluentHttpRequestOptions UseEncryption(bool useEncryption)
        {
            HttpRequest.Secure = useEncryption;
            return this;
        }

        /// <inheritdoc />
        public IBuildFluentHttpRequestOptions KeepConnectionAlive(bool keepAlive = true)
        {
            HttpRequest.KeepAlive = keepAlive;
            return this;
        }

  
    public  IBuildFluentHttpRequestOptions WithQueryStringParameters(params (string name, string value)[] parameters)
        {
           QueryParameters ??= new Dictionary<string, object>();
           foreach (var (name, value) in parameters)
               QueryParameters.TryAdd(name, value);
           return this;
        }
        
        public  IBuildFluentHttpRequestOptions WithHeaders(params (string name, object value)[] headers)
        {
            HttpRequest.Headers ??= new Dictionary<string, object>();
            foreach (var (name, value) in headers)
                HttpRequest.Headers.TryAdd(name, value);
            return this;
        }
        
        public  IBuildFluentHttpRequestOptions WithCookies(params (string name, string value)[] cookies)
        {
            HttpRequest.Cookies ??= new Dictionary<string, string>();
            foreach (var (name, value) in cookies)
                HttpRequest.Cookies.TryAdd(name, value);
            return this;
        }
        
        /// <inheritdoc />
        public HttpRequest BuildRequest(Func<IBuildFluentHttpRequestContent, HttpRequest> requestFactory)
        {
            var request = requestFactory(this);
            return request;
        }

        /// <inheritdoc />
        public IBuildFluentHttpRequestContent Payload => this;
        

        /// <inheritdoc />
        public IFinishedBuildingHttpRequest Build(Action<IBuildFluentHttpRequestContent> contentBuilder)
        {
            contentBuilder(this);
            return this;
        }

        public IBuildFluentHttpRequestPath WithMethod(HttpMethod method)
        {
            HttpRequest.Method = method.ToString().ToUpper();
            return this;
        }

        /// <inheritdoc />
        public IBuildFluentHttpRequestPath Post
        {
            get
            {
                HttpRequest.Method = HttpMethod.Post.ToString().ToUpper();
                return this;
            }
        }

        /// <inheritdoc />
        public IBuildFluentHttpRequestPath Get 
        {
            get
            {
                HttpRequest.Method = HttpMethod.Get.ToString().ToUpper();
                return this;
            }
        }

        /// <inheritdoc />
        public IBuildFluentHttpRequestPath Put 
        {
            get
            {
                HttpRequest.Method = HttpMethod.Put.ToString().ToUpper();
                return this;
            }
        }

        /// <inheritdoc />
        IBuildFluentHttpRequestPath IBuildFluentHttpRequestMethods.WithMethod(HttpMethod method) => WithMethod(method);

        
        /// <inheritdoc />
        public void SendXml(string content) => HttpRequest.Body.FromXml(content);

        /// <inheritdoc />
        public void SendJson(string content, MatchType? matchType = null) => HttpRequest.Body.FromJson(content, matchType);

        /// <inheritdoc />
        public void SendBinary(byte[] content) => HttpRequest.Body.FromBinary(content);

        /// <inheritdoc />
        public HttpRequest WithXml(string content)
        {
            HttpRequest.Body.FromXml(content);
            return HttpRequest;
        }

        /// <inheritdoc />
        public HttpRequest WithJson(string content, MatchType? matchType = null)
        {
            HttpRequest.Body.FromJson(content, matchType);
            return HttpRequest;
        }

        /// <inheritdoc />
        public HttpRequest WithJsonData<T>(params T[] items)
        {
            var json = JsonConvert.SerializeObject(items, Formatting.Indented);
            HttpRequest.Body.FromJson(json);
            return HttpRequest;
        }

        /// <inheritdoc />
        public HttpRequest WithJson<T>(T item)
        {
            var json = JsonConvert.SerializeObject(item, Formatting.Indented);
            HttpRequest.Body.FromJson(json);
            return HttpRequest;
        }

        /// <inheritdoc />
        public HttpRequest WithBinary(byte[] content)
        {
            HttpRequest.Body.FromBinary(content);
            return HttpRequest;
        }

    }
}