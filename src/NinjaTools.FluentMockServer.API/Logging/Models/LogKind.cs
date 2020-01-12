using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NinjaTools.FluentMockServer.API.Logging.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum LogType
    {
        Request,
        Setup
    }


    [JsonConverter(typeof(StringEnumConverter))]
    public enum LogKind
    {
        [Description("Matched {0}")]
        RequestMatched,

        [Description("Unmatched {0}")]
        RequestUnmatched,

        [Description("Setup created!")]
        SetupCreated,

        [Description("Setup deleted!")]
        SetupDeleted
    }
}
