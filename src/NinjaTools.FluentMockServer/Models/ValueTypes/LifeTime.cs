using System;
using JetBrains.Annotations;

namespace NinjaTools.FluentMockServer.Models.ValueTypes
{
    [PublicAPI]
    public class LifeTime
    {
        public LifeTime(int? timeToLive = null, TimeUnit timeUnit = ValueTypes.TimeUnit.Milliseconds)
        {
            if (timeToLive.HasValue && timeToLive.Value > 0)
            {
                TimeToLive = timeToLive;
                TimeUnit = timeUnit;
                Unlimited = false;
            }
            else
            {
                Unlimited = true;
            }
        }

        public TimeUnit? TimeUnit { get; }
        public int? TimeToLive { get; }
        public bool? Unlimited { get; }
    }
}
