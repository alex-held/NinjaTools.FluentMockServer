using NinjaTools.FluentMockServer.Models;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.Tests.Models.Equality
{
    public class VerifyEqualityTests : EqualityTestBase<Verify>
    {
        /// <inheritdoc />
        public VerifyEqualityTests(ITestOutputHelper outputHelper) : base(outputHelper)
        {
        }
    }
}