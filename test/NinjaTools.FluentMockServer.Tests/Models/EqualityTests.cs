using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAssertions;
using Force.DeepCloner;
using NinjaTools.FluentMockServer.Builders.Request;
using NinjaTools.FluentMockServer.Models;
using NinjaTools.FluentMockServer.Models.HttpEntities;
using NinjaTools.FluentMockServer.Models.ValueTypes;
using NinjaTools.FluentMockServer.Utils;
using Xunit;

namespace NinjaTools.FluentMockServer.Tests.Models
{
    public class ExpectationTests : EqualityTests<Expectation>
    {
            
        [Fact]
        public void Equals_Should_Be_False_When_Instances_Are_Not_Equal()
        {
            // Arrange
            var a = new Expectation
            {
                Times = Times.Once
            };
            var b = new Expectation();
            
            // Assert
            a.Equals(b).Should().BeFalse();
        }
    }
    
    public class HttpRequestTests : EqualityTests<HttpRequest>
    {
    }

    public class HttpResponseTests : EqualityTests<HttpResponse>
    {
    }

    public class HttpErrorTests : EqualityTests<HttpError>
    {
    }

    public class HttpForwardTests : EqualityTests<HttpForward>
    {
    }

    public class HttpTemplateTests : EqualityTests<HttpTemplate>
    {
    }

    public class ConnectionOptionsTests : EqualityTests<ConnectionOptions>
    {
    }

    public class VerificationTimesTests : EqualityTests<VerificationTimes>
    {
        static VerificationTimesTests()
        {
            TestDataOverride = new List<object[]>
            {
                new object[]{ VerificationTimes.Once, VerificationTimes.Twice},
                new []{ VerificationTimes.Once, (object) null},
                new object[]{ VerificationTimes.Once, new TheoryAttribute() },
                new object[]{ new VerificationTimes(), VerificationTimes.Twice},
            };
        }
    }

    public class DelayTests : EqualityTests<Delay>
    {
    }

    public class LifeTimeTests : EqualityTests<LifeTime>
    {
    }

    public class TimesTests : EqualityTests<Times>
    {
    }

    public class BodyTests : EqualityTests<Body>
    {
        static BodyTests()
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
    }

    public class VerifyTests : EqualityTests<Verify>
    {
    }
}
