using NinjaTools.FluentMockServer.Abstractions;


namespace NinjaTools.FluentMockServer.Models.ValueTypes
{
    public class VerficationTimes : BuildableBase
    {
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
