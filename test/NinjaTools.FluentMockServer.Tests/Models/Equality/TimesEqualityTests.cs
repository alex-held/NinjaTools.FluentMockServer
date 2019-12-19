using NinjaTools.FluentMockServer.Models.ValueTypes;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.Tests.Models.Equality
{
    public class TimesEqualityTests : EqualityTestBase<Times>
    {
        /// <inheritdoc />
        public TimesEqualityTests(ITestOutputHelper outputHelper) : base(outputHelper)
        {
        }
    }
}