using NinjaTools.FluentMockServer.Models.ValueTypes;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.Tests.Models.Equality
{
    public class LifeTimeEqualityTests : EqualityTestBase<LifeTime>
    {
        /// <inheritdoc />
        public LifeTimeEqualityTests(ITestOutputHelper outputHelper) : base(outputHelper)
        {
        }
    }
}