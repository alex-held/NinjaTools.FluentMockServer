using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using NinjaTools.FluentMockServer.API.Infrastructure;
using NinjaTools.FluentMockServer.API.Middleware;

namespace NinjaTools.FluentMockServer.API.Logging
{
    public class MockServerLoggerFactory : IMockServerLoggerFactory
    {
        private readonly IScopeRepository _repository;
        private readonly ILoggerFactory _loggerFactory;
        
        public MockServerLoggerFactory(IScopeRepository repository, ILoggerFactory loggerFactory)
        {
            _repository = repository;
            _loggerFactory = loggerFactory;
        }
        
        /// <inheritdoc />
        [NotNull]
        public IMockServerLogger CreateLogger<T>() => new MockServerLogger(_loggerFactory.CreateLogger<T>(), _repository);
    }
}
