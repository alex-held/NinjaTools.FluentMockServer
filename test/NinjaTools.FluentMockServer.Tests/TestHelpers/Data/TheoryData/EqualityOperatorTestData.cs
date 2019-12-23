using System;
using System.Reflection;
using JetBrains.Annotations;
using NinjaTools.FluentMockServer.FluentAPI;
using NinjaTools.FluentMockServer.FluentAPI.Builders;
using NinjaTools.FluentMockServer.FluentAPI.Builders.ValueObjects;
using NinjaTools.FluentMockServer.Models;
using NinjaTools.FluentMockServer.Models.HttpEntities;
using NinjaTools.FluentMockServer.Models.ValueTypes;
using NinjaTools.FluentMockServer.Tests.TestHelpers.Data.Random;
using Xunit;

namespace NinjaTools.FluentMockServer.Tests.TestHelpers.Data.TheoryData
{
    public class EqualityOperatorTestData<T> : TheoryData<T, T, bool> where T : class
    {
        private static Body CreateBody([NotNull] Action<IFluentBodyBuilder> factory)
        {
            var type = typeof(FluentBodyBuilder);
            var builder = new FluentBodyBuilder();
            factory(builder);
            
            var bodyProperty = type.GetProperty("Body", BindingFlags.NonPublic | BindingFlags.Instance);
            var body = bodyProperty?.GetValue(builder) as Body;

            return body;
        }
        
        public EqualityOperatorTestData()
        {
            var dataType = typeof(T);
            if (typeof(Body) == dataType)
                AddTestData(
                    CreateBody(b => b.ContainingJson("some json value")),
                    CreateBody(b =>  b.MatchingXmlSchema("xml schema")));
            else if (typeof(VerificationTimes) == dataType)
                AddTestData(VerificationTimes.Once, VerificationTimes.Twice);
            else if (typeof(Expectation) == dataType)
            {
                AddTestData(
                    FluentExpectationBuilder.Create(times: Times.Once, httpResponse: HttpResponse.Create(statusCode: 200)),
                    FluentExpectationBuilder.Create(HttpRequest.Create(method: "GET", path: "/test"), httpResponse: HttpResponse.Create(statusCode: 200)));
            }
            else
                AddTestData(InstanceFactoryCreator.CreateDefault<T>(), InstanceFactoryCreator.CreateRandom<T>());
        }

        private void AddTestData(object defaultInstance, object randomInstance)
        {
            Add(defaultInstance as T, defaultInstance as T, true );
            Add(defaultInstance as T , randomInstance as T, false);
        }
    }
}
