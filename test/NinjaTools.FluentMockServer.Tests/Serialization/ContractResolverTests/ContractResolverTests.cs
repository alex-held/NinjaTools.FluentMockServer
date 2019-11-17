using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Json;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using NinjaTools.FluentMockServer.Abstractions;
using Xunit;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.Tests.Serialization.ContractResolverTests
{
    public class ContractResolverTests
    {
        private ILogger<ContractResolverTests> _logger;

        public ContractResolverTests(ITestOutputHelper logger)
        {
            _logger = LoggerFactory.Create(i => i.AddXunit(logger))
                          .CreateLogger< ContractResolverTests>();
        }

        public class InternalMember : IBuildable
        {
            public int Value { get; set; }

            public InternalMember(int value)
            {
                Value = value;
            }
            
            /// <inheritdoc />
            public JObject SerializeJObject()
            {
                return new JObject(new JProperty("Name", new JObject
                {
                    ["number"] = Value
                }));
            }
        }
        
        public class Response : IBuildable
        {
            public Response(int id)
            {
                Headers = new Dictionary<string, object>();
                BuildableMember = new InternalMember(id);
            }

            public string Path { get; set; } = "/some/path";
            public InternalMember BuildableMember { get; set; }
            public Dictionary<string, object> Headers { get; set; }
            
            
            
            /// <inheritdoc />
            public  JObject SerializeJObject()
            {
                var settings = new JsonSerializerSettings
                {
                  //  ContractResolver = new PropertyRenameAndIgnoreSerializerContractResolver(),
                    TypeNameHandling = TypeNameHandling.Auto,
                    StringEscapeHandling = StringEscapeHandling.EscapeHtml,
                    NullValueHandling = NullValueHandling.Ignore,
                    TraceWriter = TraceWriter,
                    MaxDepth = 100,
                    Formatting = Formatting.Indented,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

                 var jo = JObject.FromObject(this);
                 return jo;
            }
        }

        private static readonly MemoryTraceWriter TraceWriter = new MemoryTraceWriter();
        
        [Fact]
        public void Should_SetValueProvider_For_Each_BuildableBaseMember()
        {
            // Arrange
            var sut = new Response(100);
            sut.Headers.Add("Content-Type", "application/json");


            // Act
            var jo  = sut.SerializeJObject();
            var json = jo.ToString(Formatting.Indented);
            
            _logger.LogInformation(json);
            TraceWriter.GetTraceMessages().ToList().ForEach(m => _logger.LogInformation(m));   
            
            // Assert
            jo.Should().NotBeNull();
        }
    }
}
