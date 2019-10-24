using System;

using NinjaTools.FluentMockServer.Abstractions;
using NinjaTools.FluentMockServer.Models.ValueTypes;


namespace NinjaTools.FluentMockServer.Models.HttpEntities
{
    /// <summary>
    /// Model to configure an Error.
    /// </summary>
    public class HttpError  : BuildableBase, IEquatable<HttpError>
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
        
        
        #region Equality Members

        /// <inheritdoc />
        public bool Equals(HttpError other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Equals(Delay, other.Delay) && DropConnection == other.DropConnection && string.Equals(ResponseBytes, other.ResponseBytes, StringComparison.InvariantCultureIgnoreCase);
        }


        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;

            return Equals(( HttpError ) obj);
        }


        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked {
                var hashCode = ( Delay != null ? Delay.GetHashCode() : 0 );
                hashCode = ( hashCode * 397 ) ^ DropConnection.GetHashCode();
                hashCode = ( hashCode * 397 ) ^ ( ResponseBytes != null ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(ResponseBytes) : 0 );

                return hashCode;
            }
        }


        public static bool operator ==(HttpError left, HttpError right) => Equals(left, right);


        public static bool operator !=(HttpError left, HttpError right) => !Equals(left, right);

        #endregion
    }
}
