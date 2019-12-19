using NinjaTools.FluentMockServer.Models.HttpEntities;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.Tests.Models.Equality
{
    public class HttpTemplateEqualityTests : EqualityTestBase<HttpTemplate>
    {
        /// <inheritdoc />
        public HttpTemplateEqualityTests(ITestOutputHelper outputHelper) : base(outputHelper)
        {
        }
    }
}