using System;

using HardCoded.MockServer.Contracts.Abstractions;
using HardCoded.MockServer.Contracts.Models.HttpEntities;


namespace HardCoded.MockServer.Contracts.Models.ValueTypes
{
   /// <summary>
   /// Model to configure an optional <see cref="Delay"/> before responding with an action to a matched <see cref="HttpRequest"/>.
   /// </summary>
    public class Delay  : BuildableBase, IEquatable<Delay>
   {
        public Delay()
        {
        }
        
        private Delay(int delay, TimeUnit unit)
        {
            TimeUnit = unit;
            Value = delay;
        }

        public static Delay FromSeconds(int seconds) => new Delay(seconds, TimeUnit.SECONDS);
        public static Delay FromMinutes(int minutes) => new Delay(minutes, TimeUnit.MINUTES);
        public static Delay None => new Delay();
        
        /// <summary>
        /// The <see cref="TimeUnit"/> of the <see cref="Delay"/>.
        /// </summary>
        public TimeUnit TimeUnit { get; set; }

        /// <summary>
        /// The value of the <see cref="Delay"/>.
        /// </summary>
        public int Value { get; set; }
    }
}
