using Newtonsoft.Json;

namespace HardCoded.MockServer
{
    public class VerficationTimes
    {
        public static VerficationTimes Once => new VerficationTimes(0, 2);
        public static VerficationTimes Twice => new VerficationTimes(2, 2);
        public static VerficationTimes Between(int atLeast, int atMost) => new VerficationTimes(atLeast, atMost);
        public static VerficationTimes MoreThan(int times) => new VerficationTimes(times, int.MaxValue);
        public static VerficationTimes LessThan(int times) => new VerficationTimes(0, times);
        
        /// <inheritdoc />
        public VerficationTimes(int atLeast, int atMost)
        {
            AtLeast = atLeast;
            AtMost = atMost;
        }

        [JsonProperty("atLeast")]
        public int AtLeast { get; }
        
        [JsonProperty("atMost")]
        public int AtMost { get; }
    }
}