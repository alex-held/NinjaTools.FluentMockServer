using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


namespace HardCoded.MockServer.Contracts.Models.ValueTypes
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