using System;

namespace NinjaTools.FluentMockServer.Tests.TestHelpers.Data.Types
{
    public class RandRecord
    {
        public string Person { get; set; }
        public string Rant { get; set; }
        public DateTime? Date { get; set; }
        public int RantId { get; set; }
    }
}