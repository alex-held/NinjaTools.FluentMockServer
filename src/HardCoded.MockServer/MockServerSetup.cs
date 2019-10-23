using System.Collections.Generic;

using HardCoded.MockServer.Contracts.Models;


namespace HardCoded.MockServer
{

    public class MockServerSetup
    {
        public List<Expectation> Expectations { get; } = new List<Expectation>();
    }
}
