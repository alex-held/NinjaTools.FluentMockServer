using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace NinjaTools.FluentMockServer.Tests.Assertions
{
    /*public class JsonAssertions : ReferenceTypeAssertions<string, JsonAssertions>
    {
        public AndWhichConstraint<JsonAssertions> ShouldContainSameContent(string other, string because, params object[] becauseArgs)
        {
            Execute
                .Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(other is null)
                .FailWith("You can't assert if both json files contain the same the content, when it is <null>")
                .Then
                .Given(() => Subject)
                .ForCondition(f  => f
                    .Trim()
                    .Replace("\n","")
                    .Replace("\t", "")
                    .Replace(" ", "") =! other)
        }

        /// <inheritdoc />
        protected override string Identifier => "JSON";
    }*/
}
