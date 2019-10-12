using Newtonsoft.Json;

namespace HardCoded.MockServer.Models.ValueTypes
{
    public class TimeToLive
    {
        [JsonProperty("timeToLive")]
        public int? Time { get; set; }
        
        [JsonProperty("timeUnit")]
        public TimeUnit? TimeUnit { get; set; }
        
        [JsonProperty("unlimited")]
        public bool? Unlimited { get; set; }
    }
}