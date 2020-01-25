using System;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace NinjaTools.FluentMockServer.Models.ValueTypes
{
    [Serializable]
    public class Times
    {
        [JsonConstructor]
        public Times(int remainingTimes)
        {
            RemainingTimes = remainingTimes;
        }

        public int RemainingTimes { get;  }

        public bool Unlimited => RemainingTimes <= 0;

        [NotNull] public static Times Once => new Times(1);
        [NotNull] public static Times Always => new Times(0);
    }
}
