using System.Collections.Generic;
using NinjaTools.FluentMockServer.Domain.Models;

namespace NinjaTools.FluentMockServer.Domain
{
    public class MockServerSetup
    {
        public List<Expectation> Expectations { get; } = new List<Expectation>();
        public string BaseUrl { get; set; }
    }
}
