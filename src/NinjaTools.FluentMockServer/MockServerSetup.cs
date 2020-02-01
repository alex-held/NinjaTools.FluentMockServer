using System.Collections.Generic;
using JetBrains.Annotations;
using NinjaTools.FluentMockServer.Models;

namespace NinjaTools.FluentMockServer
{
    /// <summary>
    /// Represents a collection of <see cref="Expectation"/> to be send to the MockServer.
    /// </summary>
    public class MockServerSetup
    {
        /// <summary>
        /// The collection of <see cref="Expectation"/> to be send to the MockServer.
        /// </summary>
        public List<Expectation> Expectations { get; } = new List<Expectation>();
    }
}
