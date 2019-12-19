using JetBrains.Annotations;
using NinjaTools.FluentMockServer.Models.ValueTypes;

namespace NinjaTools.FluentMockServer.FluentAPI.Builders.ValueObjects
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
        [NotNull]
        public Delay Build()
        {
            return new Delay(_timeUnit, _value);
        }
    }
}
