using NinjaTools.FluentMockServer.Models.HttpEntities;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.Tests.Models.Equality
{
    public class HttpRequestEqualityTests : EqualityTestBase<HttpRequest>
    {
        /// <inheritdoc />
        public HttpRequestEqualityTests(ITestOutputHelper outputHelper) : base(outputHelper)
        {
        }
    }
}