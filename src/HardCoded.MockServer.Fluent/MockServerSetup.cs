using System.Collections.Generic;

using HardCoded.MockServer.Contracts.Models;
using HardCoded.MockServer.Fluent.Builder.Expectation;


namespace HardCoded.MockServer.Fluent
{

    public class MockServerSetup
    {
        public List<Expectation> Expectations { get; } = new List<Expectation>();
    }
}
