using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace NinjaTools.FluentMockServer.Models.ValueTypes
{
    public class LifeTime 
    {

        public LifeTime()
        {
        }

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

        public TimeUnit? TimeUnit { get; set; }
        public int? TimeToLive { get; set; }
        public bool? Unlimited { get; set; }

    }
}
