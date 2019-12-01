  using System;

  namespace NinjaTools.FluentMockServer.Domain.Models.HttpEntities
{
    /// <summary>
    ///     Model to describe to which destination the <see cref="HttpRequest" /> to forward.
    /// </summary>
    public partial class HttpForward : IIdentifiable<HttpForward>
    {
        /// <inheritdoc />
        public int Id { get; }

        /// <inheritdoc />
        public DateTime CreatedOn { get; set; }

        /// <inheritdoc />
        public DateTime ModifiedOn{ get; set; }

        /// <inheritdoc />
        public byte[] Timestamp{ get; set; }
        
        
        /// <inheritdoc />
        public bool Equals(HttpForward other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Host == other.Host && Port == other.Port;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((HttpForward) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return ((Host != null ? Host.GetHashCode() : 0) * 397) ^ Port.GetHashCode();
            }
        }
    }
}
