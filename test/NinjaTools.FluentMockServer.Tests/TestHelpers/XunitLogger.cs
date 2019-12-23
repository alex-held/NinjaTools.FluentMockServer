using System;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.Tests.TestHelpers
{
    public static class XunitLoggerExtensions
    {
        public static void Dump([NotNull] this ITestOutputHelper outputHelper, object obj) => outputHelper.WriteLine(JsonConvert.SerializeObject(obj, Formatting.Indented));
     
        [NotNull]
        public static ILogger CreateLogger(this ITestOutputHelper outputHelper, [NotNull] string category) => new XunitLogger(outputHelper, category);
  
        [NotNull]
        public static ILogger<T> CreateLogger<T>(this ITestOutputHelper outputHelper) => new XunitLogger<T>(outputHelper);
    }
    
    public class XunitLogger<T> : XunitLogger, ILogger<T>
    {
        public XunitLogger(ITestOutputHelper output) : base(output, typeof(T).Name)
        {
        }
    }
    
    public class XunitLogger : ILogger, IDisposable
    {
        public string CategoryName { get; set; }
        protected ITestOutputHelper Output { get; }
        
        public XunitLogger(ITestOutputHelper output, string categoryName)
        {
            Output = output;
            CategoryName = categoryName;
        }
        
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var message = formatter != null && exception == null
                ? formatter(state, null)
                : $"{state}: {exception}";
            Output.WriteLine($"[{CategoryName}] {logLevel}: {message}");
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        [NotNull]
        public IDisposable BeginScope<TState>(TState state)
        {
            return this;
        }

        public void Dispose()
        {
        }
    }
}
