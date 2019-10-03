using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using HardCoded.MockServer.Models.HttpBodies;
using HardCoded.MockServer.Models.HttpEntities;

namespace HardCoded.MockServer.Fluent
{
    /// <summary>
    /// 
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IFluentInterface
    {

        #region Hide object members

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        Type GetType();
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        int GetHashCode();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        string ToString();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        bool Equals(object obj);

        #endregion

    }


    /// <summary>
    /// 
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IBuildFluentExpectationsInitial : IFluentInterface
    {
       
    }

    /// <summary>
    /// 
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IHttpMethodSelector : IFluentInterface
    {
        
    }

    /// <summary>
    /// 
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IBuildHttpRequestsInitial : IFluentInterface
    {
        
    }


    public static class Extensions
    {
        internal static HttpRequest Build(this IFinishedBuildingHttpRequest request)
        {
            var requestBuilder = request as FluentHttpRequestBuilder;
            return requestBuilder.Build();
        }
        
        public static FluentExpectationBuilder BuildRequest(this IFinishedBuildingHttpRequest finalRequest)
        {
            var request = finalRequest.Build();
            var builder = request.Builder;
            return builder;
        }
    }





   

    public interface IBuildRequest
    {
    }

    
    
    
    
    
    
    public class FluentExpectationBuilder
    {
        protected Queue<HttpRequest> HttpRequests { get; }
        
        public FluentExpectationBuilder()
        {
            HttpRequests = new Queue<HttpRequest>();
        }

        
        public static IBuildFluentHttpRequestMethods WhenHandling => new FluentHttpRequestBuilder(new FluentExpectationBuilder());

        public FluentHttpResponseBuilder RespondWith => new FluentHttpResponseBuilder();
    }

    public class FluentHttpResponseBuilder
    {
    }

    public class FluentMock
    {
        public static FluentExpectationBuilder NewExpectation => new FluentExpectationBuilder();

        public void Bind(MockServerClient client)
        {
            // Bind stuff
        }

    }
    
}