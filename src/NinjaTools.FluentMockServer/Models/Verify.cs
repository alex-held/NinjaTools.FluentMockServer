using System;
using NinjaTools.FluentMockServer.Models.HttpEntities;
using NinjaTools.FluentMockServer.Models.ValueTypes;

namespace NinjaTools.FluentMockServer.Models
{
    /// <summary>
    ///     Model used to describe what to verify.
    /// </summary>
    public class Verify
    {
        public Verify(HttpRequest httpRequest, VerificationTimes? times)
        {
            Times = times;
            HttpRequest = httpRequest;
        }

        /// <summary>
        ///     The to be matched <see cref="HttpRequest" />.
        /// </summary>
        public HttpRequest HttpRequest { get;  }

        /// <summary>
        ///     How many <see cref="Times" /> the request is expected to have occured.
        /// </summary>
        public VerificationTimes? Times { get; }

        public static Verify Once(HttpRequest httpRequest = null)
        {
            return new Verify(httpRequest, VerificationTimes.Once);
        }
    }
}
