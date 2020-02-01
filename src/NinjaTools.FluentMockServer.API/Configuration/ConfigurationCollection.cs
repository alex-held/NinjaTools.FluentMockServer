using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;
using Newtonsoft.Json;
using NinjaTools.FluentMockServer.API.Models;

namespace NinjaTools.FluentMockServer.API.Configuration
{

    [JsonArray(AllowNullItems = false)]
    [DebuggerDisplay("{DebuggerDisplay()}")]
    public class ConfigurationCollection : List<Setup>
    {
        public ConfigurationCollection()
        { }

        public ConfigurationCollection(params Setup[]? setups) : this()
        {
            AddRange(setups);
        }

        [NotNull]
        private string DebuggerDisplay()
        {
            var count = Count;
            return $"Count: {count.ToString()}";
        }
    }
}
