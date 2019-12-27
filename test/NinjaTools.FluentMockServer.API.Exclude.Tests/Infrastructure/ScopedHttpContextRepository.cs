using System;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using NinjaTools.FluentMockServer.API.Configuration;
using NinjaTools.FluentMockServer.API.Extensions.Responses;
using NinjaTools.FluentMockServer.API.Infrastructure;
using NinjaTools.FluentMockServer.Tests.TestHelpers;
using Shouldly;
using TestStack.BDDfy;
using TestStack.BDDfy.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.API.Tests.Infrastructure
{
    public class MiddlewareTestBase : XUnitTestBase<MiddlewareTestBase>
    {
        /// <inheritdoc />
        public MiddlewareTestBase(ITestOutputHelper output) : base(output)
        {
        }
    }

    [ExampleData(ExampleData = typeof(HttpContextExamples))]
    [Story(
        Title = "Can store and retrieve data inside a scoped HttpContext",
        AsA = "As a user of the MockServer-API",
        IWant = "I want to setup up properties for a request",
        SoThat = "So that the MockServer later is setup up as expected."
    )]
    public class ScopedHttpContextRepositoryTests
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IScopeRepository _scopeRepository;
        private object _result;

        public ScopedHttpContextRepositoryTests()
        {
            _httpContext = new DefaultHttpContext();
            _contextAccessor = new HttpContextAccessor {HttpContext = _httpContext};
            _scopeRepository = new ScopedHttpContextRepository(_contextAccessor);
        }


        [BddfyTheory]
        [ClassData(typeof(HttpContextData))]
        public void Get_Returns_Correct_Value_For_Key([NotNull] string key, [CanBeNull] object value)
        {
            this.Given(x => x.GivenAHttpContextContaining(key, value))
                .When(x => x.GetIsCalledWithKey<string>(key))
                .Then(x => x.ThenTheResultIsAnOkResponse<string>(value))
                .Run<ScopedHttpContextRepositoryTests>("Retrieve values from HttpContext store.");
        }


        [BddfyTheory]
        [ClassData(typeof(HttpContextData))]
        public void Get_Returns_Error_When_No_Data_Found_For_Key([NotNull] string key, [CanBeNull] object value)
        {
            this.Given(x => x.GivenAHttpContextContaining(key, value))
                .When(x => x.GetIsCalledWithKey<string>("keyDoesNotExist"))
                .Then(x => x.ThenTheResultIsAnErrorResponse<string>("error message"))
                .Run<ScopedHttpContextRepositoryTests>("Error handling when no values in the store.");
        }

        [BddfyTheory]
        [ClassData(typeof(HttpContextData))]
        public void Update_Updates_The_Correct_Value_For_Key([NotNull] string key, [CanBeNull] object value)
        {
            const string initialValue = "initial value";
            this.Given(x => x.GivenAHttpContextContaining(key, initialValue))
                .And(x => x.UpdateIsCalledWithKey(key, value))
                .When(x => x.GetIsCalledWithKey<string>(key))
                .Then(x => x.ThenTheResultIsAnOkResponse<string>(value))
                .And(x => x.AndTheValueForTheKeyGotUpdated(key, initialValue))
                .Run<ScopedHttpContextRepositoryTests>("Update existing values in the HttpContext store.");
        }


        [BddfyTheory]
        [ClassData(typeof(HttpContextData))]
        public void Add_Sets_Correct_Values_For_The_Key([NotNull] string key, [CanBeNull] object value)
        {
            this.Given(x => x.GivenAHttpContextContaining("abc", "string"))
                .And(x => x.GetIsCalledWithKey<string>("abc"))
                .And(x => x.ThenTheResultIsAnOkResponse<string>("string"))
                .When(x => x.AddIsCalledWithKey(key, value))
                .And(x => x.GetIsCalledWithKey<string>(key))
                .Then(x => x.ThenTheResultIsAnOkResponse<string>(value))
                .Run<ScopedHttpContextRepositoryTests>("Add new values to the store.");
        }


        private void ThenTheResultIsAnErrorResponse<T>(string message)
        {
            var errorResponse = _result.ShouldBeOfType<ErrorResponse<T>>();
            errorResponse.Data.ShouldBeNull();
            errorResponse.IsError.ShouldBe(true);
            errorResponse.Errors.ShouldHaveSingleItem()
                .ShouldBeOfType<NoDataFoundError>()
                .Message.ShouldStartWith("Unable to find data for key: ");
        }

        private void AndTheValueForTheKeyGotUpdated<T>([NotNull] string key, T initialValue)
        {
            _scopeRepository.Get<T>(key).Data.ShouldNotBe(initialValue);
        }

        private void ThenTheResultIsAnOkResponse<T>(object resultValue)
        {
            _result.ShouldBeOfType<OkResponse<T>>().Data.ShouldBe(resultValue);
        }

        private void GivenAHttpContextContaining<T>([NotNull] string key, T o)
        {
            _scopeRepository.Add(key, o);
        }

        private void GetIsCalledWithKey<T>([NotNull] string key)
        {
            _result = _scopeRepository.Get<T>(key);
        }

        private void UpdateIsCalledWithKey<T>([NotNull] string key, [NotNull] T value)
        {
            _scopeRepository.Update(key, value);
        }

        private void AddIsCalledWithKey<T>([NotNull] string key, [NotNull] T value)
        {
            _scopeRepository.Add(key, value);
        }
    }
}

public class ExampleDataAttribute : Attribute
{
    [CanBeNull] public Type ExampleData { get; set; }
}

public static class ExampleExtensions
{
    public static Story Run<TStory>([NotNull] this object testObject, [NotNull] string scenarioTitle) where TStory : class
    {
        //  var context = TestContext.GetContext(testObject);
        //var type = context.TestObject.GetType();
        var type = typeof(TStory);
        Story story;

        if (type.GetCustomAttribute<ExampleDataAttribute>() is {} attribute)
            if (attribute.ExampleData != null && Activator.CreateInstance(attribute.ExampleData) is ExampleTable examples)
            {
                story = testObject.WithExamples(examples).LazyBDDfy<TStory>(scenarioTitle, null).Run();
                return story;
            }

        story = testObject.LazyBDDfy<TStory>(scenarioTitle, null).Run();
        return story;
    }
}


public class HttpContextData : TheoryData<string, object>
{
    public HttpContextData()
    {
        Add(MockServerHttpContextKeys.RequestId, Guid.NewGuid().ToString());
        Add(MockServerHttpContextKeys.PreviousRequestId, Guid.NewGuid().ToString());
    }
}


public class HttpContextExamples : ExampleTable
{
    public HttpContextExamples() : base("key", "value")
    {
        Add(MockServerHttpContextKeys.RequestId, "Request-TraceId-5678");
        Add(MockServerHttpContextKeys.PreviousRequestId, "PreviousRequest-TraceId-1234");
    }

    [NotNull] public static HttpContextExamples Examples => new HttpContextExamples();
}
