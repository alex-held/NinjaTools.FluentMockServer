using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NinjaTools.FluentMockServer.Models;

namespace NinjaTools.FluentMockServer.API.Configuration
{
    public class DownstreamContext
    {
        public DownstreamContext([NotNull] HttpContext httpContext)
        {
            HttpContext = httpContext;
            Errors = new List<Error>();
        }

        [NotNull] public IMockServerReverseProxyConfiguration Configuration { get; set; }

        [NotNull] public IServiceProvider Services => HttpContext.RequestServices;

        [NotNull] public HttpContext HttpContext { get; }

        [NotNull] public List<Error> Errors { get; }

        public bool IsError => Errors.Count > 0;
        public IExpectationRepository ExpectationRepository => Services.GetRequiredService<IExpectationRepository>(); 
    }

    public interface IExpectationRepository
    {
        Expectation? GetExpectation(HttpContext context);
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
        NoDataFoundError = 100,
        AddDataFailedError = 101,
        RequestMappingError = 200,
    }

    public interface IMockServerReverseProxyConfiguration
    {
        string RequestId { get; }
    }
}
