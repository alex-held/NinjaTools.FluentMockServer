using System;
using System.Runtime.Serialization;

using Newtonsoft.Json;

using NinjaTools.FluentMockServer.Abstractions;
using NinjaTools.FluentMockServer.Models.HttpEntities;
using NinjaTools.FluentMockServer.Models.ValueTypes;


namespace NinjaTools.FluentMockServer.Models
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
        
        #region Equality Members

        /// <inheritdoc />
        public bool Equals(Expectation other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Equals(HttpRequest, other.HttpRequest) && Equals(HttpResponse, other.HttpResponse) && Equals(HttpResponseTemplate, other.HttpResponseTemplate) && Equals(HttpForward, other.HttpForward) && Equals(HttpForwardTemplate, other.HttpForwardTemplate) && Equals(HttpError, other.HttpError) && Equals(Times, other.Times) && Equals(TimeToLive, other.TimeToLive);
        }


        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;

            return Equals(( Expectation ) obj);
        }


        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked {
                var hashCode = ( HttpRequest != null ? HttpRequest.GetHashCode() : 0 );
                hashCode = ( hashCode * 397 ) ^ ( HttpResponse         != null ? HttpResponse.GetHashCode() : 0 );
                hashCode = ( hashCode * 397 ) ^ ( HttpResponseTemplate != null ? HttpResponseTemplate.GetHashCode() : 0 );
                hashCode = ( hashCode * 397 ) ^ ( HttpForward          != null ? HttpForward.GetHashCode() : 0 );
                hashCode = ( hashCode * 397 ) ^ ( HttpForwardTemplate  != null ? HttpForwardTemplate.GetHashCode() : 0 );
                hashCode = ( hashCode * 397 ) ^ ( HttpError            != null ? HttpError.GetHashCode() : 0 );
                hashCode = ( hashCode * 397 ) ^ ( Times                != null ? Times.GetHashCode() : 0 );
                hashCode = ( hashCode * 397 ) ^ ( TimeToLive           != null ? TimeToLive.GetHashCode() : 0 );

                return hashCode;
            }
        }


        public static bool operator ==(Expectation left, Expectation right) => Equals(left, right);


        public static bool operator !=(Expectation left, Expectation right) => !Equals(left, right);

        #endregion
    }
}
