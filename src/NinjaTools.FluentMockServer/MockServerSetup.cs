using System.Collections.Generic;
using NinjaTools.FluentMockServer.Models;

namespace NinjaTools.FluentMockServer
{
    public class MockServerSetup
    {
        public List<Expectation> Expectations { get; } = new List<Expectation>();
        public string BaseUrl { get; set; }
    }
}
