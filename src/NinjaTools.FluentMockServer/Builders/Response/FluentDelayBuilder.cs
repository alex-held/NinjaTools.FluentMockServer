using NinjaTools.FluentMockServer.Models.ValueTypes;

namespace NinjaTools.FluentMockServer.Builders.Response
{
    internal sealed class FluentDelayBuilder : IFluentDelayBuilder
    {
        private readonly Delay _delay;

        public FluentDelayBuilder()
        {
            _delay = new Delay();
        }

        /// <inheritdoc />
        public void FromSeconds(int seconds)
        {
            _delay.Value = seconds;
            _delay.TimeUnit = TimeUnit.Seconds;
        }

        /// <inheritdoc />
        public void FromMiliSeconds(int ms)  
        {
            _delay.Value = ms;
            _delay.TimeUnit = TimeUnit.Milliseconds;
        }


        /// <inheritdoc />
        public void FromMinutes(int minutes)
        {
            _delay.Value = minutes;
            _delay.TimeUnit = TimeUnit.Minutes;
        }

        /// <inheritdoc />
        public Delay Build() =>
            _delay;
    }
}
