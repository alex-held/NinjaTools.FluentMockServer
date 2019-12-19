using NinjaTools.FluentMockServer.Models.ValueTypes;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.Tests.Models.Equality
{
    public class DelayEqualityTests : EqualityTestBase<Delay>
    {
        /// <inheritdoc />
        public DelayEqualityTests(ITestOutputHelper outputHelper) : base(outputHelper)
        {
        }
    }
}