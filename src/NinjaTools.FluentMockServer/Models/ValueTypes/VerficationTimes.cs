using Newtonsoft.Json;

namespace NinjaTools.FluentMockServer.Models.ValueTypes
{
    public class VerficationTimes
    {
        /// <inheritdoc />
        [JsonConstructor]
        public VerficationTimes(int? atLeast, int? atMost)
        {
            AtLeast = atLeast;
            AtMost = atMost;
        }

        public static VerficationTimes Once => new VerficationTimes(1, 1);
        public static VerficationTimes Twice => new VerficationTimes(2, 2);

        public int? AtLeast { get; }
        public int? AtMost { get; }

        public static VerficationTimes Between(int atLeast, int atMost)
        {
            return new VerficationTimes(atLeast, atMost);
        }

        public static VerficationTimes MoreThan(int times)
        {
            return new VerficationTimes(times, null);
        }

        public static VerficationTimes LessThan(int times)
        {
            return new VerficationTimes(0, times);
        }
    }
}
