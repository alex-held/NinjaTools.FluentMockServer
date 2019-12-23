using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.Tests.TestHelpers
{
    public class XUnitTestBase
    {
        public ITestOutputHelper Output { get; }
        public ILogger Logger { get; }

        [NotNull]
        public ILogger<T> CreateLogger<T>() => Output.CreateLogger<T>();
        
        public XUnitTestBase(ITestOutputHelper output)
        {
            Output = output;
            Logger = Output.CreateLogger(GetType().Name);
        }
    }
    
    public class XUnitTestBase<T>
    {
        public ITestOutputHelper Output { get; }
        public ILogger<T> Logger { get; }

        [NotNull]
        public ILogger<TCategory> CreateLogger<TCategory>() => Output.CreateLogger<TCategory>();
        
        public XUnitTestBase(ITestOutputHelper output)
        {
            Output = output;
            Logger = Output.CreateLogger<T>();
        }
    }
}
