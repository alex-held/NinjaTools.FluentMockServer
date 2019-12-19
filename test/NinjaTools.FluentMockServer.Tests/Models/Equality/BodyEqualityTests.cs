using System;
using System.Collections.Generic;
using System.Reflection;
using NinjaTools.FluentMockServer.FluentAPI;
using NinjaTools.FluentMockServer.FluentAPI.Builders.ValueObjects;
using NinjaTools.FluentMockServer.Models.ValueTypes;
using Xunit;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.Tests.Models.Equality
{
    public class BodyEqualityTests : EqualityTestBase<Body>
    {
        static BodyEqualityTests()
        {
            TestDataOverride = new List<object[]>()
            {
                new object[] {Create(b => b.ContainingJson("some json value")), Create(b => b.NotContainingJson("not containing this string"))},
                new object[] {new Body(), Create(b => b.WithXmlContent("xml content"))},
                new[] {Create(b => b.WithXmlContent("xml content")), (object) null},
                new object[] {Create(b => b.MatchingXmlSchema("xml schema")), new FactAttribute()}
            };
        }

        private static Body Create(Action<IFluentBodyBuilder> factory)
        {
            var type = typeof(FluentBodyBuilder);
            var builder = new FluentBodyBuilder();
            factory(builder);
            
            var bodyProperty = type.GetProperty("Body", BindingFlags.NonPublic | BindingFlags.Instance);
            var body = bodyProperty.GetValue(builder) as Body;

            return body;
        }

        /// <inheritdoc />
        public BodyEqualityTests(ITestOutputHelper outputHelper) : base(outputHelper)
        {
        }
    }
}