using NinjaTools.FluentMockServer.Models.ValueTypes;

namespace NinjaTools.FluentMockServer.Models.HttpEntities
{
    /// <summary>
    ///     Model used to describe what to verify.
    /// </summary>
    public partial class Verify
    {
        /// <summary>
        ///     The to be matched <see cref="HttpRequest" />.
        /// </summary>
        public HttpRequest HttpRequest { get; set; }

        /// <summary>
        ///     How many <see cref="Times" /> the request is expected to have occured.
        /// </summary>
        public VerificationTimes Times { get; set; }

        public static Verify Once(HttpRequest httpRequest = null)
        {
            return new Verify
            {
                HttpRequest = httpRequest,
                Times = VerificationTimes.Once
            };
        }
    }
}
