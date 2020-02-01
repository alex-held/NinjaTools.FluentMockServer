using System;
using Xunit.Sdk;

namespace NinjaTools.FluentMockServer.Xunit.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class MockServerCollectionAttribute : BeforeAfterTestAttribute
    {
        public string Id { get; }

        /// <inheritdoc />
        public MockServerCollectionAttribute(string collection)
        {
            Id = collection;
        }
    }
}
