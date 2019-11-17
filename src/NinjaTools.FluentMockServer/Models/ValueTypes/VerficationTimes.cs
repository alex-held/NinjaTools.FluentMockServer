using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.Abstractions;


namespace NinjaTools.FluentMockServer.Models.ValueTypes
{
    [JsonObject(IsReference = true)]
    public class VerficationTimes : IBuildable
    {

        /// <inheritdoc />
        public JObject SerializeJObject()
        {
            var self = new JObject();

            // TODO: finish serialize

            return self;
        }

        public static VerficationTimes Once => new VerficationTimes(1, 1);
        public static VerficationTimes Twice => new VerficationTimes(2, 2);
        public static VerficationTimes Between(int atLeast, int atMost) => new VerficationTimes(atLeast, atMost);
        public static VerficationTimes MoreThan(int times) => new VerficationTimes(times, null);
        public static VerficationTimes LessThan(int times) => new VerficationTimes(0, times);

        /// <inheritdoc />
        public VerficationTimes(int? atLeast, int? atMost)
        {
            AtLeast = atLeast;
            AtMost = atMost;
        }

        public int? AtLeast { get; }

        public int? AtMost { get; }
    }
}
