using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.Abstractions;
using NinjaTools.FluentMockServer.Models.HttpEntities;


namespace NinjaTools.FluentMockServer.Models.ValueTypes
{
    /// <summary>
    /// Model to configure an optional <see cref="Delay"/> before responding with an action to a matched <see cref="HttpRequest"/>.
    /// </summary>
    [JsonObject(IsReference = true)]
    public class Delay : IBuildable
    {
        public Delay()
        {
        }

        private Delay(int delay, TimeUnit unit)
        {
            TimeUnit = unit;
            Value = delay;
        }

        public static Delay FromSeconds(int seconds) => new Delay(seconds, TimeUnit.Seconds);
        public static Delay FromMinutes(int minutes) => new Delay(minutes, TimeUnit.Minutes);
        public static Delay None => new Delay();

        /// <summary>
        /// The <see cref="TimeUnit"/> of the <see cref="Delay"/>.
        /// </summary>
        public TimeUnit TimeUnit { get; set; }

        /// <summary>
        /// The value of the <see cref="Delay"/>.
        /// </summary>
        public int Value { get; set; }


        /// <inheritdoc />
        public JObject SerializeJObject()
        {
            var self = new JObject();

            if (Value != 0 && TimeUnit != default)
            {
                self.Add("timeUnit", TimeUnit.ToString().ToUpper());
                self.Add("value", Value);
            }

            return self;
        }

    }
}
