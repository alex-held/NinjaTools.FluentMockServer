using System.Net.Http;
using JetBrains.Annotations;
using NinjaTools.FluentMockServer.Models.HttpEntities;

namespace NinjaTools.FluentMockServer.Exceptions
{
    /// <inheritdoc />
    /// <summary>
    /// Indicates that the MockServer did not receive an expected <see cref="T:NinjaTools.FluentMockServer.Models.HttpEntities.HttpRequest" />.
    /// </summary>
    [PublicAPI]
    public class MockServerVerificationException : MockServerException
    {
        /// <summary>
        /// The content of the <see cref="HttpResponseMessage"/> of the MockServer.
        /// </summary>
        public string ResponseMessage { get; }

        /// <summary>
        /// The expected <see cref="HttpRequest"/>.
        /// </summary>
        public HttpRequest Expected { get; }

        /// <inheritdoc />
        public MockServerVerificationException(string responseMessage, HttpRequest expectedHttpRequest)
        {
            ResponseMessage = responseMessage;
            Expected = expectedHttpRequest;
        }
    }
}
