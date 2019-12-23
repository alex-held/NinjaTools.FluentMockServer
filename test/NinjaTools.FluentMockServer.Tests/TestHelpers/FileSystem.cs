using System.Collections.Concurrent;
using System.IO;
using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Schema;

namespace NinjaTools.FluentMockServer.Tests.TestHelpers
{
    public static class FileSystem
    {
                       
        public static void LogExpected(this ILogger logger, string topic, string message, params object[] args)
        {
            var sb = new StringBuilder($"=== {topic} ===");
            sb.AppendLine(message);
            logger.Log(LogLevel.Information, sb.ToString(), args);
        }
        
        public static void LogResult(this ILogger logger, string topic, string message, params object[] args)
        {
            var sb = new StringBuilder($"=== {topic} ===");
            sb.AppendLine(message);
            logger.Log(LogLevel.Information, sb.ToString(), args);
        }

        
        private static readonly ConcurrentDictionary<string, string> Files = new ConcurrentDictionary<string, string>();
        
        public static string LoadXml(string name) => ReadFile(name, "xml");
        public const string TestDataDirectory = "Data";
        
        public static JSchema Load(string filenameWithoutExtension)
        {
            var content = ReadFile(filenameWithoutExtension, "json");
            return JSchema.Parse(content);
        }

        private static string ReadFile(string filenameWithoutExtension, string fileExtension)
        {
            if (Files.ContainsKey(filenameWithoutExtension))
            {
                return Files.TryGetValue(filenameWithoutExtension, out var value)
                    ? value
                    : throw new FileNotFoundException($"Extension=.{fileExtension};", filenameWithoutExtension);
            }
            
            var file = File.ReadAllText($"{TestDataDirectory}/{filenameWithoutExtension}.{fileExtension}");
            Files.TryAdd(filenameWithoutExtension, file);
            return file;
        }
    }
}
