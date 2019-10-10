using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using FluentApi.Generics.Framework;
using HardCoded.MockServer.Models.HttpEntities;

namespace HardCoded.MockServer.Fluent.Builder
{
    
    /// <summary>
    /// 
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IFluentExpectationBuilder : IBlankExpectation, IWithRequest, IWithResponse
    {
    }
    
    /// <summary>
    /// 
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IBlankExpectation : IFluentInterface
    {
        IWithRequest OnHandling(HttpMethod method, Action<IFluentHttpRequestBuilder> requestFactory);
    }
    /// <summary>
    /// 
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IWithRequest: IFluentInterface
    {
        IWithResponse RespondWith(int statusCode, Action<IFluentHttpResponseBuilder> responseFactory);
        IWithResponse RespondWith(HttpStatusCode statusCode, Action<IFluentHttpResponseBuilder> responseFactory);
    }
    
    /// <summary>
    /// 
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IWithResponse : IFluentInterface
    {
        Expectation Setup();
        IFluentExpectationBuilder And { get; }
    }

    internal sealed class FluentExpectationBuilder : IFluentExpectationBuilder
    {
        private readonly List<Expectation> _expectations;
        private HttpRequest HttpRequest { get; set; }
        private HttpResponse HttpResponse { get; set; }

        public FluentExpectationBuilder(List<Expectation> expectations)
        {
            _expectations = expectations;
        }
        
        /// <inheritdoc />
        public IWithRequest OnHandling(HttpMethod method, Action<IFluentHttpRequestBuilder> requestFactory)
        {
            var builder = new FluentHttpRequestBuilder(method);
            requestFactory(builder);
            HttpRequest = builder.Build();
            return this;
        }
        
        /// <inheritdoc />
        public Expectation Setup()
        {
            return new Expectation
            {
                HttpRequest = HttpRequest, 
                HttpResponse = HttpResponse
            };
        }

        /// <inheritdoc />
        public IFluentExpectationBuilder And
        {
            get
            {
                var current = Setup();
                _expectations.Add(current);
                return new FluentExpectationBuilder(_expectations);
            }
        }


        /// <inheritdoc />
        public IWithResponse RespondWith(int statusCode, Action<IFluentHttpResponseBuilder> responseFactory)
        {
            var builder = new FluentHttpResponseBuilder(statusCode);
            responseFactory(builder);
            HttpResponse = builder.Build();
            return this;
        }

        /// <inheritdoc />
        public IWithResponse RespondWith(HttpStatusCode statusCode, Action<IFluentHttpResponseBuilder> responseFactory)
        {
            return RespondWith((int) statusCode, responseFactory);
        }

    }
}
