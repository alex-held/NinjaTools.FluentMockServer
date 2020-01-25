using System;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace NinjaTools.FluentMockServer.Models.ValueTypes
{
    public class VerificationTimes
    {
        [JsonConstructor]
        public VerificationTimes(int? atLeast, int? atMost)
        {
            AtLeast = atLeast;
            AtMost = atMost;
        }

        [NotNull]
        public static VerificationTimes Once => new VerificationTimes(1, 1);

        [NotNull]
        public static VerificationTimes Twice => new VerificationTimes(2, 2);

        public int? AtLeast { get; }
        public int? AtMost { get; }

        public static VerificationTimes Between(int atLeast, int atMost)
        {
            return new VerificationTimes(atLeast, atMost);
        }

        public static VerificationTimes MoreThan(int times)
        {
            return new VerificationTimes(times, null);
        }

        public static VerificationTimes LessThan(int times)
        {
            return new VerificationTimes(0, times);
        }
    }
}
