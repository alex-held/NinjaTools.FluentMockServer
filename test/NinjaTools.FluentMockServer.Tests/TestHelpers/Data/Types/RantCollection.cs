using System;
using System.Collections.Generic;

namespace NinjaTools.FluentMockServer.Tests.TestHelpers.Data.Types
{
    public class RantCollection
        {
            public string Owner { get; set; }
            public Gender Gender { get; set; }
            public List<RandRecord> Rants { get; set; } = new List<RandRecord>();
            public Guid? Id { get; set; }
        }
}
