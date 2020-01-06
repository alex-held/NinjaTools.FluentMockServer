using System;
using System.Text;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.Tests.TestHelpers
{
    public class XUnitTestBase
    {
        public ITestOutputHelper Output { get; protected set; }
        public ILogger Logger { get; protected set; }
        
        public XUnitTestBase(ITestOutputHelper output)
        {
            Output = output;
            Logger = Output.CreateLogger(GetType().Name);
        }


        [NotNull]
        protected virtual string Dump<T>([CanBeNull] T? expected, [CanBeNull] T? actual) where T : class
        {
           var dump = $"{Dump(expected, header: "Expected")}\n{Dump(actual, header: "Actual")}";
           return dump;
           // Dump(actual, header: "Actual");

           // var sb = new StringBuilder();
           // sb.AppendLine($"{Dump(expected, header: "Expected", writeToConsole: false)}");
           // sb.AppendLine($"{Dump(actual, header: "Actual", writeToConsole: false)}");
           //
           // var dump = sb.ToString();
           // Output.WriteLine(dump);
           // return dump;
        }
        
        [NotNull]
        protected virtual string Dump<T>([CanBeNull] T? value, [CanBeNull] string? header = null, [CanBeNull] Func<T, string> formatter = null, bool writeToConsole = true) where T : class
        {
            formatter ??= v =>
            {
                if (value is string str)
                {
                    return str;
                }
                
                return JToken.FromObject(v).ToString(Formatting.Indented);
            };
            
            var sb = new StringBuilder();
            
            sb.AppendLine(header is null
                ? $"--- Dump of '{typeof(T).Name}' ---"
                : $"--- {header} ---");

            if (value is {} instance)
            {
                var json = formatter(instance);
                foreach (var line in json.Split('\n'))
                {
                    sb.AppendLine(line);
                }
            }
            else
            {
                sb.AppendLine("<null>");
            }
            
            sb.AppendLine();

            var dump = sb.ToString();
            
            if (writeToConsole)
            {
                Output.WriteLine(dump);
            }
            
            return dump;
        }
    }
    
    public class XUnitTestBase<T> : XUnitTestBase
    {
        [NotNull]
        public ILogger<TCategory> CreateLogger<TCategory>() => Output.CreateLogger<TCategory>();

        protected XUnitTestBase(ITestOutputHelper output) : base(output)
        {
            Logger = Output.CreateLogger<T>();
        }
    }
}
