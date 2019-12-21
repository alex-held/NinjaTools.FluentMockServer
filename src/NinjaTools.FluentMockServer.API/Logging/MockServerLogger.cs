using System;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using NinjaTools.FluentMockServer.API.Configuration;
using NinjaTools.FluentMockServer.API.Infrastructure;
using NinjaTools.FluentMockServer.API.Middleware;

namespace NinjaTools.FluentMockServer.API.Logging
{
    public class MockServerLogger : IMockServerLogger
    {
        private readonly ILogger _logger;
        private readonly IScopeRepository _repository;
        private readonly Func<string, Exception, string> _errorFormatter;

        public MockServerLogger(ILogger logger, IScopeRepository repository)
        {
            _logger = logger;
            _repository = repository;
            _errorFormatter = (state, exception) =>  exception == null ? state : $"{state}, exception: {exception}";
        }

        /// <inheritdoc />
        public void LogTrace(string message)
        {
            var logMessage = PrepareLogMessage( GetRequestId(), GetPreviousRequestId(), message);
            _logger.Log(LogLevel.Trace, default, logMessage, null, _errorFormatter);
        }
        
        /// <inheritdoc />
        public void LogDebug(string message)
        {
            var logMessage = PrepareLogMessage( GetRequestId(), GetPreviousRequestId(), message);
            _logger.Log(LogLevel.Debug, default, logMessage, null, _errorFormatter);        }

        /// <inheritdoc />
        public void LogInformation(string message)
        {
            var logMessage = PrepareLogMessage( GetRequestId(), GetPreviousRequestId(), message);
            _logger.Log(LogLevel.Information, default, logMessage, null, _errorFormatter);
        }

        /// <inheritdoc />
        public void LogWarning(string message)
        {
            var logMessage = PrepareLogMessage( GetRequestId(), GetPreviousRequestId(), message);
            _logger.Log(LogLevel.Warning, default, logMessage, null, _errorFormatter);
        }

        /// <inheritdoc />
        public void LogError(string message, Exception exception)
        {
            var logMessage = PrepareLogMessage( GetRequestId(), GetPreviousRequestId(), message);
            _logger.Log(LogLevel.Critical, default, logMessage, exception, _errorFormatter);        }

        /// <inheritdoc />
        public void LogCritical(string message, Exception exception)
        {
            var logMessage = PrepareLogMessage( GetRequestId(), GetPreviousRequestId(), message);
            _logger.Log(LogLevel.Critical, default, logMessage, exception, _errorFormatter);
        }
        
        [NotNull]
        private string PrepareLogMessage(string requestId, string previousRequestId, string message)
        {
            return $"requestId: {requestId}, previousRequestId: {previousRequestId}, message: {message}";
        }

        
        [NotNull]
        private string GetRequestId()
        {
            var response = _repository.Get<string>(MockServerHttpContextKeys.RequestId);
            return response.IsError ? "no request id" : response.Data;
        }

        [NotNull]
        private string GetPreviousRequestId()
        {
            var response = _repository.Get<string>(MockServerHttpContextKeys.PreviousRequestId);
            return response.IsError ? "no previous request id" : response.Data;
        }
    }
}
