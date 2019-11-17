using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.Abstractions;


namespace NinjaTools.FluentMockServer.Models.ValueTypes
{
    [JsonObject(IsReference = true)]
    public class LifeTime : IBuildable
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

        /// <inheritdoc />
        public JObject SerializeJObject()
        {
            var self = new JObject();

            // TODO: finish serialize


            return self;
        }
    }
}
