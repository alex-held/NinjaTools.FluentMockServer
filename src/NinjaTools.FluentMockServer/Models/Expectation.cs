using JetBrains.Annotations;
using Newtonsoft.Json;
using NinjaTools.FluentMockServer.Models.HttpEntities;
using NinjaTools.FluentMockServer.Models.ValueTypes;
using NinjaTools.FluentMockServer.Serialization;

namespace NinjaTools.FluentMockServer.Models
{
    /// <summary>
    ///     Model to set up an Expectation on the MockServer.
    /// </summary>
    [JsonConverter(typeof(ExpectationConverter))]
    public partial class Expectation
    {
        private Expectation()
        {
        }
        
        [JsonConstructor]
        public Expectation(
            [CanBeNull] HttpRequest httpRequest,
            [CanBeNull] HttpResponse httpResponse,
            [CanBeNull] HttpTemplate httpResponseTemplate, 
            [CanBeNull] HttpForward httpForward,
            [CanBeNull] HttpTemplate httpForwardTemplate,
            [CanBeNull] HttpError httpError,
            [CanBeNull] Times times,
            [CanBeNull] LifeTime timeToLive)
        {
            HttpRequest = httpRequest;
            HttpResponse = httpResponse;
            HttpResponseTemplate = httpResponseTemplate;
            HttpForward = httpForward;
            HttpForwardTemplate = httpForwardTemplate;
            HttpError = httpError;
            Times = times;
            TimeToLive = timeToLive;
        }

        /// <summary>
        ///     The <see cref="HttpRequest" /> to match.
        /// </summary>
        public HttpRequest HttpRequest { get; private set; }

        /// <summary>
        ///     The <see cref="HttpResponse" /> to respond with.
        /// </summary>
        public HttpResponse HttpResponse { get; private set;}

        public HttpTemplate HttpResponseTemplate { get; private set;}

        /// <summary>
        ///     The Target specification to forward the matched <see cref="HttpRequest" /> to.
        /// </summary>
        public HttpForward HttpForward { get; private set;}

        public HttpTemplate HttpForwardTemplate { get; private set;}

        /// <summary>
        ///     An <see cref="HttpError" /> to respond with in case the <see cref="HttpRequest" /> has been matched.
        /// </summary>
        public HttpError HttpError { get;private set; }

        /// <summary>
        ///     How many times the MockServer should expect this setup.
        /// </summary>
        public Times Times { get;private set; }

        /// <summary>
        ///     How long the MockServer should expect this setup.
        /// </summary>
        public LifeTime TimeToLive { get; private set;}
    }
}
