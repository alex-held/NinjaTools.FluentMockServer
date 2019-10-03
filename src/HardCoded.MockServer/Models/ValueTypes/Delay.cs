using HardCoded.MockServer.Models;
using Newtonsoft.Json;

namespace HardCoded.MockServer
{
    public class Delay
    {
        public Delay()
        {
            TimeUnit = TimeUnit.SECONDS;

        }
        private Delay(int delay, TimeUnit unit)
        {
            TimeUnit = unit;
            Value = delay;
        }

        public static Delay FromSeconds(int seconds) => new Delay(seconds, TimeUnit.SECONDS);
        public static Delay FromMinutes(int minutes) => new Delay(minutes, TimeUnit.MINUTES);
        
        
        [JsonProperty("timeUnit")]
        public TimeUnit TimeUnit { get; set; }

        [JsonProperty("value")]
        public int Value { get; set; }

        public static Delay None => FromSeconds(0);
    }
}