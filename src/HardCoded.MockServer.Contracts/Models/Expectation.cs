using System;
using System.Runtime.Serialization;

using HardCoded.MockServer.Contracts.Abstractions;
using HardCoded.MockServer.Contracts.Models.HttpEntities;
using HardCoded.MockServer.Contracts.Models.ValueTypes;

using Newtonsoft.Json;


namespace HardCoded.MockServer.Contracts.Models
{
    /// <summary>
    /// Model to set up an Expectation on the MockServer.
    /// </summary>
    public class Expectation : BuildableBase, IEquatable<Expectation>
    {
        public static explicit operator Expectation(string json)
        {
            try {
                var expectation = JsonConvert.DeserializeObject<Expectation>(json);

                return expectation;
            } catch (Exception e) {
                Console.WriteLine(e);
                throw new SerializationException($"Deserialization to type: '{nameof(Expectation)}' failed for provided input data: '{json}'!", e);
            }
        }
        
        /// <summary>
        /// The <see cref="HttpRequest"/> to match.
        /// </summary>
        public HttpRequest HttpRequest { get; set; }
        
        /// <summary>
        /// The <see cref="HttpResponse"/> to respond with.
        /// </summary>
        public HttpResponse HttpResponse { get; set; }
        public HttpTemplate HttpResponseTemplate { get; set; }
        
        /// <summary>
        /// The Target specification to forward the matched <see cref="HttpRequest"/> to.
        /// </summary>
        public HttpForward HttpForward { get; set; }
        public HttpTemplate HttpForwardTemplate { get; set; }
        
        /// <summary>
        /// An <see cref="HttpError"/> to respond with in case the <see cref="HttpRequest"/> has been matched.
        /// </summary>
        public HttpError HttpError { get; set; }
        
        /// <summary>
        /// How many times the MockServer should expect this setup.
        /// </summary>
        public Times Times { get; set; }
        
        /// <summary>
        /// How long the MockServer should expect this setup.
        /// </summary>
        public TimeToLive TimeToLive { get; set; }
    }
}
