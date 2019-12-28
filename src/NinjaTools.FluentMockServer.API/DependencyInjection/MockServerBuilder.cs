using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NinjaTools.FluentMockServer.API.Administration;
using NinjaTools.FluentMockServer.API.Models;

namespace NinjaTools.FluentMockServer.API.DependencyInjection
{
    public class MockServerBuilder : IMockServerBuilder
    {
        public MockServerBuilder(IServiceCollection services, IConfiguration configuration)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            Services.TryAddSingleton<ISetupRepository, SetupRepository>();
            
            Services.TryAddScoped<IAdministrationService, AdministrationService>();
            Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
            
            MvcCoreBuilder = Services.AddMvcCore()
                .AddControllersAsServices()
                .AddAuthorization()
                .AddNewtonsoftJson();

            Services.AddMiddlewareAnalysis();
            Services.AddLogging();
        }

        public IServiceProvider ServiceProvider => Services.BuildServiceProvider();


        public IServiceCollection Services { get; }
        public IConfiguration Configuration { get; }

        /// <inheritdoc />
        public IMvcCoreBuilder MvcCoreBuilder { get; }
    }
    
    // public class HttpDataRepository : IRequestScopedDataRepository
    // {
    //     private readonly IHttpContextAccessor _httpContextAccessor;
    //
    //     public HttpDataRepository(IHttpContextAccessor httpContextAccessor)
    //     {
    //         _httpContextAccessor = httpContextAccessor;
    //     }
    //
    //     public Response Add<T>(string key, T value)
    //     {
    //         try
    //         {
    //             _httpContextAccessor.HttpContext.Items.Add(key, value);
    //             return new OkResponse();
    //         }
    //         catch (Exception exception)
    //         {
    //             return new ErrorResponse(new CannotAddDataError(string.Format($"Unable to add data for key: {key}, exception: {exception.Message}")));
    //         }
    //     }
    //
    //     public Response Update<T>(string key, T value)
    //     {
    //         try
    //         {
    //             _httpContextAccessor.HttpContext.Items[key] = value;
    //             return new OkResponse();
    //         }
    //         catch (Exception exception)
    //         {
    //             return new ErrorResponse(new CannotAddDataError(string.Format($"Unable to update data for key: {key}, exception: {exception.Message}")));
    //         }
    //     }
    //
    //     public Response<T> Get<T>(string key)
    //     {
    //         object obj;
    //
    //         if (_httpContextAccessor.HttpContext == null || _httpContextAccessor.HttpContext.Items == null)
    //         {
    //             return new ErrorResponse<T>(new CannotFindDataError($"Unable to find data for key: {key} because HttpContext or HttpContext.Items is null"));
    //         }
    //
    //         if (_httpContextAccessor.HttpContext.Items.TryGetValue(key, out obj))
    //         {
    //             var data = (T)obj;
    //             return new OkResponse<T>(data);
    //         }
    //
    //         return new ErrorResponse<T>(new CannotFindDataError($"Unable to find data for key: {key}"));
    //     }
    // }
    //
    // public interface IRequestScopedDataRepository
    // {
    // }

    //
    // public class MockServerLoggerFactory
    // {
    //     private readonly ILoggerFactory _loggerFactory;
    //     // private readonly IRequestScopedDataRepository _scopedDataRepository;
    //
    //     // public AspDotNetLoggerFactory(ILoggerFactory loggerFactory, IRequestScopedDataRepository scopedDataRepository)
    //     // {
    //     //     _loggerFactory = loggerFactory;
    //     //     _scopedDataRepository = scopedDataRepository;
    //     // }
    //     //
    //     // public IOcelotLogger CreateLogger<T>()
    //     // {
    //     //     var logger = _loggerFactory.CreateLogger<T>();
    //     //     return new AspDotNetLogger(logger, _scopedDataRepository);
    //     // }
    //
    //     public IMockServerLogger CreateLogger<T>() => new MockServerLogger(_loggerFactory.CreateLogger<T>());
    // }
    //
    //
    // public interface IMockServerLogger
    // {
    //     void LogTrace(string message);
    //     void LogDebug(string message);
    //     void LogInformation(string message);
    //     void LogWarning(string message);
    //     void LogError(string message, Exception exception);
    //     void LogCritical(string message, Exception exception);
    // }
    //
    //  public class MockServerLogger : IMockServerLogger
    // {
    //     private readonly ILogger _logger;
    //     private readonly Func<string, Exception, string> _errorFormatter;
    //
    //     public MockServerLogger(ILogger logger)
    //     {
    //         _logger = logger;
    //         _errorFormatter = (state, exception) =>  exception == null ? state : $"{state}, exception: {exception}";
    //     }
    //
    //     /// <inheritdoc />
    //     public void LogTrace(string message)
    //     {
    //         var logMessage = PrepareLogMessage( GetRequestId(), GetPreviousRequestId(), message);
    //         _logger.Log(LogLevel.Trace, default, logMessage, null, _errorFormatter);
    //     }
    //     
    //     /// <inheritdoc />
    //     public void LogDebug(string message)
    //     {
    //         var logMessage = PrepareLogMessage( GetRequestId(), GetPreviousRequestId(), message);
    //         _logger.Log(LogLevel.Debug, default, logMessage, null, _errorFormatter);        }
    //
    //     /// <inheritdoc />
    //     public void LogInformation(string message)
    //     {
    //         var logMessage = PrepareLogMessage( GetRequestId(), GetPreviousRequestId(), message);
    //         _logger.Log(LogLevel.Information, default, logMessage, null, _errorFormatter);
    //     }
    //
    //     /// <inheritdoc />
    //     public void LogWarning(string message)
    //     {
    //         var logMessage = PrepareLogMessage( GetRequestId(), GetPreviousRequestId(), message);
    //         _logger.Log(LogLevel.Warning, default, logMessage, null, _errorFormatter);
    //     }
    //
    //     /// <inheritdoc />
    //     public void LogError(string message, Exception exception)
    //     {
    //         var logMessage = PrepareLogMessage( GetRequestId(), GetPreviousRequestId(), message);
    //         _logger.Log(LogLevel.Critical, default, logMessage, exception, _errorFormatter);        }
    //
    //     /// <inheritdoc />
    //     public void LogCritical(string message, Exception exception)
    //     {
    //         var logMessage = PrepareLogMessage( GetRequestId(), GetPreviousRequestId(), message);
    //         _logger.Log(LogLevel.Critical, default, logMessage, exception, _errorFormatter);
    //     }
    //     
    //     [NotNull]
    //     private string PrepareLogMessage(string requestId, string previousRequestId, string message)
    //     {
    //         return $"requestId: {requestId}, previousRequestId: {previousRequestId}, message: {message}";
    //     }
    //     //
    //     //
    //     // [NotNull]
    //     // private string GetRequestId()
    //     // {
    //     //     var response = _repository.Get<string>(MockServerHttpContextKeys.RequestId);
    //     //     return response.IsError ? "no request id" : response.Data;
    //     // }
    //     //
    //     // [NotNull]
    //     // private string GetPreviousRequestId()
    //     // {
    //     //     var response = _repository.Get<string>(MockServerHttpContextKeys.PreviousRequestId);
    //     //     return response.IsError ? "no previous request id" : response.Data;
    //     // }
    // }
    //
    //  public interface IMockServerLoggerFactory
    // {
    // }
}
