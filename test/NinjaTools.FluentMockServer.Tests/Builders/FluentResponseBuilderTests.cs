//using System;
//using System.Collections.Generic;
//using System.Net;
//using System.Text;
//using System.Text.RegularExpressions;
//using Divergic.Logging.Xunit;
//using FluentAssertions;
//using FluentAssertions.Execution;
//using FluentAssertions.Formatting;
//using FluentAssertions.Json;
//using FluentAssertions.Primitives;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Logging.Abstractions;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
//using Newtonsoft.Json.Schema;
//using NinjaTools.FluentMockServer.Builders.Response;
//using NinjaTools.FluentMockServer.Domain.Models.HttpEntities;
//using NinjaTools.FluentMockServer.Domain.Models.ValueTypes;
//using Xunit;
//using Xunit.Abstractions;
//using static NinjaTools.FluentMockServer.Tests.Test.FileSystem;
//
//namespace NinjaTools.FluentMockServer.Tests.Builders
//{
//
//    /// <summary>
//    ///     The <see cref="ILogFormatter" />
//    ///     interface defines the members for formatting log messages.
//    /// </summary>
//    public class TestOutputFormatter : ILogFormatter
//    {
//        private TestOutputFormatter()
//        {
//        }
//
//        private static readonly Lazy<TestOutputFormatter> _lazy = new Lazy<TestOutputFormatter>(() => new TestOutputFormatter());
//        public static TestOutputFormatter Instance = _lazy.Value;
//        
//        
//        /// <inheritdoc />
//        public string Format(int scopeLevel, string name, LogLevel logLevel, EventId eventId, string message, Exception exception)
//        {
//            var builder = new StringBuilder();
//
//            if (scopeLevel > 0)
//            {
//                builder.Append(' ', scopeLevel * 2);
//            }
//
//            if (logLevel != LogLevel.Debug && logLevel != LogLevel.Trace && logLevel != LogLevel.Information )
//            {
//                builder.Append($"{logLevel} ");
//                
//                if (!string.IsNullOrEmpty(name))
//                {
//                    builder.Append($"{name} ");
//                }
//
//                if (eventId.Id != 0)
//                {
//                    builder.Append($"[{eventId.Id}]: ");
//                }
//            }
//            
//            if (!string.IsNullOrEmpty(message))
//            {
//                builder.Append("\n" + message);
//            }
//
//            if (exception != null)
//            {
//                builder.Append($"\n{exception}");
//            }
//
//            return builder.ToString();
//        }
//    }
//    
//  
//  
//    public static class BuildableBaseExtensions
//    {
//        public static HttpResponseAssertions Should(this HttpResponse response,  ILogger logger = null)
//        {
//            return new HttpResponseAssertions(response, logger);
//        }
//        
//        public static HttpRequestAssertions Should(this HttpRequest response,  ILogger logger = null)
//        {
//            return new HttpRequestAssertions(response, logger);
//        }
//    }
//
//    public class HttpRequestAssertions : BuildableBaseAssertions<HttpRequest>
//    {
//        public HttpRequestAssertions(HttpRequest subject, ILogger logger = null) : base(subject, logger)
//        {
//        }
//    }
//
//
//    public class HttpResponseAssertions : BuildableBaseAssertions<HttpResponse>
//    {
//        
//        /// <inheritdoc />
//        public HttpResponseAssertions(HttpResponse subject, ILogger logger = null) : base(subject, logger)
//        {
//        }
//        
//        
//      
//        public AndWhichConstraint<HttpResponseAssertions, JTokenAssertions> ContainsBody(JToken body, string because = null,  params object[] becauseArgs)
//        {
//            var jObject = Subject.SerializeJObject();
//            
//            Execute.Assertion
//                .ForCondition(!(jObject is null))
//                .UsingLineBreaks
//                .BecauseOf(because, becauseArgs)
//                .FailWith("Expected {context:JObject} to contain body {0}{reason}, but it was <null>.", body.ToString(Formatting.Indented));
//
//            var bodyValue = jObject?.SelectToken("body")?.Value<JToken>();
//
//            if (bodyValue is null)
//            {
//                Execute.Assertion
//                    .UsingLineBreaks
//                    .BecauseOf(because, becauseArgs)
//                    .FailWith("Expected {{context:JObject}} to contain body {0}{reason}, but is was <null>.", body.ToString(Formatting.Indented));
//            }
//
//       
//            var jTokenAssertions = new JTokenAssertions(bodyValue);
//            var andWhich = new AndWhichConstraint<HttpResponseAssertions, JTokenAssertions>(this, jTokenAssertions.BeEquivalentTo(body).And);
//            return andWhich;
//        }
//    }
//
//    public class BuildableBaseAssertions<TBuildable> : ReferenceTypeAssertions<TBuildable, BuildableBaseAssertions<TBuildable>> where TBuildable :  BuildableBase, 
//    {
//        private readonly ILogger _logger;
//        
//        /// <inheritdoc />
//        protected override string Identifier => typeof(TBuildable).Name;
//        
//        protected bool LoggedJson { get; set; }
//        protected bool LoggedExpectedJObject { get; set; }
//        protected bool LoggedActualJObject { get; set; }
//        
//        static BuildableBaseAssertions()
//        {
//            Formatter.AddFormatter(new JTokenFormatter());
//        }
//        
//        public BuildableBaseAssertions(TBuildable subject, ILogger logger = null)
//        {
//            _logger = logger ?? LogFactory.Create("Default");
//            Subject = subject;
//        }
//
//        private void Log(JToken expected = null)
//        {
//           
//                try
//                {
//                    if (Subject != null)
//                    {
//                        if (!LoggedJson)
//                        {
//                            _logger.LogResult("Serialize()", Subject?.Serialize() ?? "<null>");
//                            LoggedJson = true;
//                        }
//
//                        if (!LoggedActualJObject)
//                        {
//                            _logger.LogResult("SerializeJObject()", Subject?.SerializeJObject()?.ToString(Formatting.Indented) ?? "<null>");
//                            LoggedActualJObject = true;
//                        }
//                    }
//                    
//                    if (expected != null)
//                    {
//                        if (!LoggedExpectedJObject)
//                        {
//                            _logger.LogExpected("SerializeJObject()",expected?.ToString(Formatting.Indented) ?? "<null>");
//                            LoggedExpectedJObject = true;
//                        }
//                    }
//                }
//                catch (Exception e)
//                {
//                   // ignore
//                }
//        }
//
//        public AndConstraint<BuildableBaseAssertions<TBuildable>> BeValid(JSchema schema, string because = null, params object[] becauseArgs)
//        {
//            Log();
//            _logger.LogExpected("Schema", schema.ToString(SchemaVersion.Draft7));
//            var jObject = Subject?.SerializeJObject();
//            
//            Execute.Assertion
//                .ForCondition(!(jObject is null))
//                .UsingLineBreaks
//                .BecauseOf(because, becauseArgs)
//                .FailWith("Expected {context:string} to match JsonSchema {0}{reason}, but it was <null>.", schema.ToString(SchemaVersion.Draft7));
//
//           
//       
//            if (!jObject.IsValid(schema, out IList<string> errorMessages))
//            {
//
//                var errorMessage = $"JsonSchema Validation Error!\n\n{string.Join('\n', errorMessages)}";
//                _logger.LogCritical(errorMessage);
//
//                Execute.Assertion
//                    .UsingLineBreaks
//                    .BecauseOf(because, becauseArgs)
//                    .AddPreFormattedFailure(errorMessage);
//            }
//            
//         
//            var and = new AndConstraint<BuildableBaseAssertions<TBuildable>>(this);
//            return and;
//        }
//        
//        
//        public AndConstraint<BuildableBaseAssertions<TBuildable>> MatchingRegex(string regularExpression, string because = null, params object[] becauseArgs)
//        {
//            Log();
//            
//            var json = Subject?.ToString()
//            
//            Execute.Assertion
//                .ForCondition(!(json is null))
//                .UsingLineBreaks
//                .BecauseOf(because, becauseArgs)
//                .FailWith("Expected {context:string} to match regex {0}{reason}, but it was <null>.", regularExpression);
//
//            bool isMatch = false;
//            try
//            {
//                isMatch = Regex.IsMatch(json, regularExpression);
//            }
//            catch (ArgumentException)
//            {
//                Execute.Assertion
//                    .FailWith("Cannot match {context:string} against {0} because it is not a valid regular expression.", regularExpression);
//            }
//
//            Execute.Assertion
//                .ForCondition(isMatch)
//                .BecauseOf(because, becauseArgs)
//                .UsingLineBreaks
//                .FailWith("Expected {context:string} to match regex {0}{reason}, but {1} does not match.", regularExpression, json);
//
//            
//            return new AndConstraint<BuildableBaseAssertions<TBuildable>>(this);
//        }
//        
//        public AndWhichConstraint<BuildableBaseAssertions<TBuildable>, JTokenAssertions> HasValue(string jsonPath, JToken expected, string because = null, params object[] becauseArgs)
//        {
//            Log(expected);
//            
//            var jObject = Subject?.SerializeJObject();
//            
//            Execute.Assertion
//                .ForCondition(!(jObject is null))
//                .UsingLineBreaks
//                .BecauseOf(because, becauseArgs)
//                .FailWith("Expected {context:string} to contain JToken {0}{reason} at Path {1}, but it was <null>.", expected, jsonPath);
//
//            var jTokenAtPath = jObject?.SelectToken(jsonPath);
//            
//            if (jTokenAtPath is null)
//            {
//                Execute.Assertion
//                    .UsingLineBreaks
//                    .BecauseOf(because, becauseArgs)
//                    .FailWith("Expected {context:JObject} to contain JToken at JsonPath {0}, but it does not.", jsonPath);
//            }
//
//            var andJTokenAssertions = new JTokenAssertions(jTokenAtPath)
//                .BeEquivalentTo(expected, because, becauseArgs);
//            
//            var andWhich = new AndWhichConstraint<BuildableBaseAssertions<TBuildable>, JTokenAssertions>(this, andJTokenAssertions.And);
//
//            return andWhich;
//        }
//        
//        
//        private AndConstraint<JTokenAssertions> Contains<TValue>(JTokenAssertions subject, string path, TValue expected) where TValue : JToken
//        {
//            Log(expected);
//            
//            return subject.Subject
//                .SelectToken(path)
//                .Should().BeAssignableTo<TValue>()
//                .And.Subject
//                .Should().BeEquivalentTo(expected);
//        }
//    } 
//
//    public static class LogFactory
//    {
//        private static readonly Lazy<ILoggerFactory> _loggerFactory = new Lazy<ILoggerFactory>(() => _loggerFactoryFactory());
//        private static Func<ILoggerFactory> _loggerFactoryFactory = () => new NullLoggerFactory();
//
//
//        public static T Dump<T>(this T instance) where  T : class
//        {
//            var factory = GetLoggerFactory();
//            var log = factory.CreateLogger("Default");
//            var json = JsonConvert.SerializeObject(instance, Formatting.Indented);
//            log.LogWarning(json);
//            return instance;
//        }
//        
//        public static void LogExpected(this ILogger logger, string topic, string message, params object[] args)
//        {
//            var factory = GetLoggerFactory();
//            var log = factory.CreateLogger("Expected");
//            var sb = new StringBuilder($"=== {topic} ===");
//            sb.AppendLine(message);
//            log.Log(LogLevel.Information, sb.ToString(), args);
//        }
//        
//        public static void LogResult(this ILogger logger, string topic, string message, params object[] args)
//        {
//            var factory = GetLoggerFactory();
//            var log = factory.CreateLogger("Result");
//            var sb = new StringBuilder($"=== {topic} ===");
//            sb.AppendLine(message);
//            log.Log(LogLevel.Information, sb.ToString(), args);
//        }
//        
//        private class Config : LoggingConfig
//        {
//            public static Config Instance => new Config();
//            
//            public Config()
//            {
//                Formatter = TestOutputFormatter.Instance;
//            }   
//        }
//        
//        public static ILogger<T> Create<T>()
//        {
//            var loggerFactory = GetLoggerFactory();
//            return loggerFactory.CreateLogger<T>();
//        }
//        
//        public static ILogger Create(string name)
//        {
//            var loggerFactory = GetLoggerFactory();
//            return loggerFactory.CreateLogger(string.IsNullOrWhiteSpace(name) ? "Default" : name);
//        }
//
//        private static ILoggerFactory GetLoggerFactory(ITestOutputHelper output = null)
//        {
//            if (_loggerFactory.IsValueCreated)
//            {
//                return _loggerFactory.Value;
//            }
//
//            if (output is null)
//            {
//                return new NullLoggerFactory();
//            }
//            
//            _loggerFactoryFactory = () => LoggerFactory.Create(builder => builder.AddXunit(output, Config.Instance));
//            return _loggerFactory.Value;
//        }
//        
//        public static ILogger AsLogger(this ITestOutputHelper testOutputHelper)
//        {
//            var factory = GetLoggerFactory(testOutputHelper);
//            return factory.CreateLogger("Default");
//        }
//        
//        public static ILogger<T> AsLogger<T>(this ITestOutputHelper testOutputHelper)
//        {
//            var factory = GetLoggerFactory(testOutputHelper);
//            return factory.CreateLogger<T>();
//        }
//    }
//    
//    
//    public class FluentResponseBuilderTests
//    {
//        private readonly ILogger _logger;
//        
//        public FluentResponseBuilderTests(ITestOutputHelper outputHelper)
//        {
//            _logger = outputHelper.AsLogger();
//        }
//        
//        
//        [Fact]
//        public void Should_Add_Header()
//        {
//            // Arrange
//            var builder = new FluentHttpResponseBuilder();
//            
//            // Act
//            var response = builder
//                .ConfigureHeaders(opt => opt
//                    .AddContentType("text/xml charset=UTF-8;"))
//                .Build()
//                .ToString()
//            
//            // Assert
//            response.Should().MatchRegex(@"(?mis){.*""Content-Type"":.*\[.*""text/xml charset=UTF-8;"".*\].*}.*");
//
//        }
//        
//        [Fact]
//        public void Should_Add_Body_Literal()
//        {
//            // Arrange
//            var builder = new FluentHttpResponseBuilder();
//            
//            // Act
//            var response = builder
//                .WithBody("Hello World!")
//                .Build()
//                .SerializeJObject();
//            
//            // Assert
//            response.Value<string>("body").Should().Be("Hello World!");
//        }
//
//        
//        
//        [Fact]
//        public void WithLiteralResponse_Creates_LiteralBody()
//        {
//            // Arrange
//            var builder = new FluentHttpResponseBuilder();
//
//            // Act
//            var response = builder
//                .WithBody("Hello World!")
//                .Build()
//                .SerializeJObject();
//
//            // Assert
//            response.Value<string>("body")
//                .Should().Be("Hello World!");
//        }
//
//
//
//       
//        
//        [Fact]
//        public void Build_With_Complex_Setup_Matches_JsonSchema1()
//        {
//            // Arrange
//            var builder = new FluentHttpResponseBuilder();
//            const string contentType = "xml/plain; charset=utf-8";
//            var schema = Load("FluentResponseBuilder_Schema1");
//            var responseBody = LoadXml("WithOrderId");
//            
//            // Act
//            var response = builder
//                .WithStatusCode(HttpStatusCode.OK)
//                .WithDelay(50, TimeUnit.Milliseconds)
//                .AddContentType(contentType)
//                .WithBody(responseBody)
//                .Build()
//                .SerializeJObject();
//             
//            
//            // Assert
//            response.SelectToken("statusCode").Value<int>().Should().Be(200);
//            response.Value<string>("body").Should().BeEquivalentTo(responseBody);
//            response.Value<JObject>("delay").Should().BeEquivalentTo(new JObject
//            {
//                ["timeUnit"] = TimeUnit.Milliseconds.ToString().ToUpper(),
//                ["value"] = 50
//            });
//            response.Value<JObject>("headers").Value<JArray>("Content-Type")[0].Value<string>().Should().Be(contentType);
//            
//            var sb = new StringBuilder("ValidationErrors:\n");
//            
//            if (!response.IsValid(schema, out IList<string> errors))
//            {
//                int i = 0;
//                foreach (var error in errors)
//                {
//                    sb.AppendLine($"Error [{i}]");
//                    sb.AppendLine(error);
//                    sb.AppendLine();
//                    sb.AppendLine();
//                }
//            }
//            
//            _logger.LogInformation(sb.ToString());
//
////            response.Should().val
////                .ContainsBody(new JProperty("body", responseBody))
////                .And.BeValid(schema)
////                .And.Match(r => r.StatusCode == 200)
////                .And.HasValue("statusCode", new JValue(200))
////                .And.HasValue("delay", new JObject
////                {
////                    ["timeUnit"] = TimeUnit.Milliseconds.ToString().ToUpper(),
////                    ["value"] = 50
////                })
////                .And.HasValue("headers", new JObject
////                {
////                    ["Content-Type"] = new JArray(contentType)
////                });
//        }
//        
//        
//        
//        [Fact]
//        public void Should_Add_Multiple_Header_Values()
//        {
//            // Arrange
//            IFluentHttpResponseBuilder builder = new FluentHttpResponseBuilder();
//            
//            // Act
//            var response = builder
//                .AddContentType("text/xml charset=UTF-8;")
//                .WithHeader("Header-name 2", "true")
//                .Build()
//                .SerializeJObject();
//            
//            
//            _logger.LogResult("JSON", response.ToString(Formatting.Indented));
//            
//            
//            // Assert
//            response["headers"]["Content-Type"][0].Value<string>().Should().Be("text/xml charset=UTF-8;");
//            response["headers"]["Header-name 2"][0].Value<string>().Should().Be("true");
//        }
//    }
//}
