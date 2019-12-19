using System.Collections.Generic;
using NinjaTools.FluentMockServer.Models.ValueTypes;
using Xunit;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.Tests.Models.Equality
{
    public class VerificationTimesEqualityTests : EqualityTestBase<VerificationTimes>
    {
        static VerificationTimesEqualityTests()
        {
            TestDataOverride = new List<object[]>
            {
                new object[]{ VerificationTimes.Once, VerificationTimes.Twice},
                new []{ VerificationTimes.Once, (object) null},
                new object[]{ VerificationTimes.Once, new TheoryAttribute() },
                new object[]{ new VerificationTimes(0, 0), VerificationTimes.Twice},
            };
        }

        /// <inheritdoc />
        public VerificationTimesEqualityTests(ITestOutputHelper outputHelper) : base(outputHelper)
        {
        }
    }
}