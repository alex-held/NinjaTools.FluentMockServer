using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


namespace NinjaTools.FluentMockServer.Models.ValueTypes
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TimeUnit
    {
        SECONDS,
        MILLISECONDS,
        DAYS,
        HOURS,
        MICROSECONDS,
        MINUTES,
        NANOSECONDS
    }
}