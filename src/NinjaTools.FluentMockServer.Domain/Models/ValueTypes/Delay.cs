using System;
using NinjaTools.FluentMockServer.Domain.Builders.Response;
using NinjaTools.FluentMockServer.Domain.Models.HttpEntities;

namespace NinjaTools.FluentMockServer.Domain.Models.ValueTypes
{
    /// <summary>
    ///     Model to configure an optional <see cref="Delay" /> before responding with an action to a matched <see cref="HttpRequest" />.
    /// </summary>
    public class Delay : IEquatable<Delay>
    {
        internal sealed class FluentDelayBuilder : IFluentDelayBuilder
        {
         
            private TimeUnit _timeUnit;
            private int _value;
            

            /// <inheritdoc />
            public void FromSeconds(int seconds)
            {
                _value = seconds;
                _timeUnit = TimeUnit.Seconds;
            }

            /// <inheritdoc />
            public void FromMilliSeconds(int ms)
            {
                _value = ms;
                _timeUnit = TimeUnit.Milliseconds;
            }


            /// <inheritdoc />
            public void FromMinutes(int minutes)
            {
                _value = minutes;
                _timeUnit = TimeUnit.Minutes;
            }

            /// <inheritdoc />
            public Delay Build()
            {
                return new Delay(_timeUnit, _value);
            }
        }
        
        public Delay(TimeUnit timeUnit, int value)
        {
            TimeUnit = timeUnit;
            Value = value;
        }
        
        /// <summary>
        ///     The <see cref="TimeUnit" /> of the <see cref="Delay" />.
        /// </summary>
        public TimeUnit TimeUnit { get;  }

        /// <summary>
        ///     The value of the <see cref="Delay" />.
        /// </summary>
        public int Value { get; }

        /// <inheritdoc />
        public bool Equals(Delay other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return TimeUnit == other.TimeUnit && Value == other.Value;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Delay) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return ((int) TimeUnit * 397) ^ Value;
            }
        }
    }
}
