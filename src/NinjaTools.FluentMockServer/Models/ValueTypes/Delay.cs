using JetBrains.Annotations;
using NinjaTools.FluentMockServer.Models.HttpEntities;

namespace NinjaTools.FluentMockServer.Models.ValueTypes
{
    /// <summary>
    ///     Model to configure an optional <see cref="Delay" /> before responding with an action to a matched <see cref="HttpRequest" />.
    /// </summary>
    [PublicAPI]
    public class Delay
    {
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
    }
}
