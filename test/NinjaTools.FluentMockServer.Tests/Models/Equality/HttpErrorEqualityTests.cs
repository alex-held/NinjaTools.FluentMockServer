using NinjaTools.FluentMockServer.Models.HttpEntities;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.Tests.Models.Equality
{
    public class HttpErrorEqualityTests : EqualityTestBase<HttpError>
    {
        /// <inheritdoc />
        public HttpErrorEqualityTests(ITestOutputHelper outputHelper) : base(outputHelper)
        {
        }
    }
}