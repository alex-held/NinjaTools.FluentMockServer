using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.Models.HttpEntities;


namespace NinjaTools.FluentMockServer.Models.ValueTypes
{
    /// <summary>
    /// Model to configure an optional <see cref="Delay"/> before responding with an action to a matched <see cref="HttpRequest"/>.
    /// </summary>
        public class Delay 
    {
    

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
