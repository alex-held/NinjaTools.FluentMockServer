using System.Collections.Generic;
using System.Reflection;
using Xunit.Sdk;

namespace HardCoded.MockServer.Tests.Utils
{
    public class InvertMatchAttribute : DataAttribute
    {
        /// <inheritdoc />
        public override IEnumerable<object[]> GetData(MethodInfo testMethod) =>
            new List<object[]> { new object[]{true}, new object[]{false}};
    }
}