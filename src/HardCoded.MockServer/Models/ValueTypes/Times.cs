using Newtonsoft.Json;

namespace HardCoded.MockServer
{
    public class Times
    {
        private Times(long remainingTimes)
        {
            if (remainingTimes < 0)
                Unlimited = true;
            else
                RemainingTimes = remainingTimes;
        }
        
        public static Times Once => new Times(1);
        public static Times Always => new Times(-1);
        
        [JsonProperty("remainingTimes")]
        public long RemainingTimes { get; set; }

        [JsonProperty("unlimited")]
        public bool Unlimited { get; set; }
    }
}