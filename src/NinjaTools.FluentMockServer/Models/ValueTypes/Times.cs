using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.Abstractions;


namespace NinjaTools.FluentMockServer.Models.ValueTypes
{
    [JsonObject(IsReference = true)]
    public class Times : IBuildable
    {

        /// <inheritdoc />
        public JObject SerializeJObject()
        {
            var self = new JObject();

            // TODO: finish serialize

            return self;
        }

        public Times()
        { }

        public Times(int remainingTimes)
        {
            if (remainingTimes <= 0)
            {
                Unlimited = true;
            }
            else
            {
                RemainingTimes = remainingTimes;
                Unlimited = false;
            }
        }

        public static Times Once => new Times(1);
        public static Times Always => new Times(0);

        public int RemainingTimes { get; set; }

        public bool Unlimited { get; set; }
    }
}
