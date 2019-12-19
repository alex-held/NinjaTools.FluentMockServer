using NinjaTools.FluentMockServer.Models.ValueTypes;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.Tests.Models.Equality
{
    public class ConnectionOptionsEqualityTests : EqualityTestBase<ConnectionOptions>
    {
        /// <inheritdoc />
        public ConnectionOptionsEqualityTests(ITestOutputHelper outputHelper) : base(outputHelper)
        {
        }
    }
}