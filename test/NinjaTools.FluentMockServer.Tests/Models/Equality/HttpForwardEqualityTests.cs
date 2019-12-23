using NinjaTools.FluentMockServer.Models.HttpEntities;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.Tests.Models.Equality
{
    public class HttpForwardEqualityTests : EqualityTestBase<HttpForward>
    {
        /// <inheritdoc />
        public HttpForwardEqualityTests(ITestOutputHelper outputHelper) : base(outputHelper)
        {
        }
    }
}