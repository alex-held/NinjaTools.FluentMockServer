using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;

namespace NinjaTools.FluentMockServer.API.Configuration
{
    public class DownstreamContext
    {
        public DownstreamContext(HttpContext httpContext)
        {
            HttpContext = httpContext;
            Errors = new List<Error>();
        }
     
        public IMockServerReverseProxyConfiguration Configuration { get; set; } 
     
        [NotNull] 
        public HttpContext HttpContext { get; }
        
        [NotNull] 
        public List<Error> Errors { get; }

        public bool IsError => Errors.Count > 0;
    }

    public class Error
    {
        public Error(string message, MockServerErrorCode errorCode)
        {
            Message = message;
            ErrorCode = errorCode;
        }
        
        public string Message { get; }
        public MockServerErrorCode ErrorCode { get; }


        /// <inheritdoc />
        public override string ToString()
        {
            return Message;
        }
    }

    public enum MockServerErrorCode
    {
    }

    public interface IMockServerReverseProxyConfiguration
    {
        string RequestId { get; }
    }
}
