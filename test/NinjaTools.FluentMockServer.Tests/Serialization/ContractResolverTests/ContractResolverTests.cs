using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Newtonsoft.Json.Serialization;
using NinjaTools.FluentMockServer.Extensions;
using NinjaTools.FluentMockServer.Tests.TestHelpers;
using Xunit;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.Tests.Serialization.ContractResolverTests
{
    public class ContractResolverTests : XUnitTestBase<ContractResolverTests>
    {
        public ContractResolverTests(ITestOutputHelper outputHelper) : base(outputHelper)
        { }

        public class InternalMember 
        {
            public int Value { get; set; }

            public InternalMember(int value)
            {
                Value = value;
            }
        }
        
        public class Response 
        {
            public Response(int id)
            {
                Headers = new Dictionary<string, object>();
                BuildableMember = new InternalMember(id);
            }

            public string Path { get; set; } = "/some/path";
            public InternalMember BuildableMember { get; set; }
            public Dictionary<string, object> Headers { get; set; }
        }

        private static readonly MemoryTraceWriter TraceWriter = new MemoryTraceWriter();
        
        [Fact]
        public void Should_SetValueProvider_For_Each_BuildableBaseMember()
        {
            // Arrange
            var sut = new Response(100);
            sut.Headers.Add("Content-Type", "application/json");


            // Act
            var jo = sut.AsJObject();
            
            Output.Dump(jo);
            TraceWriter.GetTraceMessages().ToList().ForEach(m => Output.Dump(m));   
            
            // Assert
            jo.Should().NotBeNull();
        }
    }
}
