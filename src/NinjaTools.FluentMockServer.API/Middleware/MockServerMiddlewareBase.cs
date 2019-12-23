using System.Collections.Generic;
using NinjaTools.FluentMockServer.API.Configuration;
using NinjaTools.FluentMockServer.API.Logging;

namespace NinjaTools.FluentMockServer.API.Middleware
{
    public abstract class MockServerMiddlewareBase
    {
        protected MockServerMiddlewareBase(IMockServerLogger logger)
        {
            Logger = logger;
            MiddlewareName = GetType().Name;
        }

        public IMockServerLogger Logger { get; }

        public string MiddlewareName { get; }

        public void SetPipelineError(DownstreamContext context, List<Error> errors)
        {
            foreach (var error in errors)
            {
                SetPipelineError(context, error);
            }
        }

        public void SetPipelineError(DownstreamContext context, Error error)
        {
            Logger.LogWarning(error.Message);
            context.Errors.Add(error);
        }
    }
}