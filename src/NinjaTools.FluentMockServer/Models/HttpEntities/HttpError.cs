using NinjaTools.FluentMockServer.Models.ValueTypes;


namespace NinjaTools.FluentMockServer.Models.HttpEntities
{
    /// <summary>
    /// Model to configure an Error.
    /// </summary>
    public class HttpError
    {
        /// <summary>
        /// An optional <see cref="Delay"/> until the <see cref="HttpError"/> occurs.
        /// </summary>
        public Delay Delay { get; set; }

        /// <summary>
        /// Whether to drop the connection when erroring.
        /// </summary>
        public bool? DropConnection { get; set; }

        /// <summary>
        /// Base64 encoded byte response. 
        /// </summary>
        public string ResponseBytes { get; set; }

    }
}
