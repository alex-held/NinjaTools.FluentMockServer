using NinjaTools.FluentMockServer.Models.HttpEntities;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.Tests.Models.Equality
{
    public class HttpResponseEqualityTests : EqualityTestBase<HttpResponse>
    {
        /// <inheritdoc />
        public HttpResponseEqualityTests(ITestOutputHelper outputHelper) : base(outputHelper)
        {
        }
    }
}