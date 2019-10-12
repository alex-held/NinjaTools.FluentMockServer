using System;
using FluentAssertions;
using HardCoded.MockServer.Contracts.Abstractions;
using HardCoded.MockServer.Contracts.FluentInterfaces;
using HardCoded.MockServer.Fluent.Builder;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit.Abstractions;

namespace HardCoded.MockServer.Tests.Utils
{
    internal static class AssertionHelpers<TBuilder, TFluentBuilder, TBuildable> 
        where TBuilder : class, TFluentBuilder, new()
        where TFluentBuilder : IFluentBuilder<TBuildable>, IFluentInterface
        where TBuildable : class, IBuildable
    {
        internal static void Assert<TContainer>(ITestOutputHelper outputHelper, string expected, bool invert,
            Action<TFluentBuilder> factory, 
            Action<TFluentBuilder> invertedFactory)
        
            where TContainer : SerializationContainer<TBuilder, TFluentBuilder, TBuildable>
        {
            
            var containerType = typeof(TContainer);
            var container = invert
                ? (TContainer) Activator.CreateInstance(containerType, outputHelper, invertedFactory, invert)
                : (TContainer) Activator.CreateInstance(containerType, outputHelper, factory, invert);

            var actualToken = container.Deserialize();
            var expectedToken =  JToken.Parse(JObject.Parse(expected).ToString());
            if (invert) expectedToken = JToken.Parse(container.Invert(expected).ToString(Formatting.Indented));


            outputHelper.WriteLine($"Expected:\n");
            outputHelper.WriteLine($"{expectedToken}\n\n");

            outputHelper.WriteLine($"Actual:\n");
            outputHelper.WriteLine($"{actualToken}\n\n");
            actualToken.Should().BeEquivalentTo(expectedToken);
            actualToken.ToString(Formatting.Indented).Should().Be(expectedToken.ToString(Formatting.Indented));
        }
        
        internal static void Assert<TContainer>(ITestOutputHelper outputHelper, string expected, Action<TBuilder> factory) 
            where TContainer : SerializationContainer<TBuilder, TFluentBuilder, TBuildable>        
        {
            var containerType = typeof(TContainer);
            var container = (TContainer) Activator.CreateInstance(containerType, factory);
            
            var expectedToken = JToken.Parse(expected);
            var actualToken = container.Deserialize();

            outputHelper.WriteLine($"Expected:\n");
            outputHelper.WriteLine($"{expectedToken}\n\n");

            outputHelper.WriteLine($"Actual:\n");
            outputHelper.WriteLine($"{actualToken}\n\n");
            actualToken.ToString(Formatting.Indented).Should().Be(expectedToken.ToString(Formatting.Indented));
        }

    }


}