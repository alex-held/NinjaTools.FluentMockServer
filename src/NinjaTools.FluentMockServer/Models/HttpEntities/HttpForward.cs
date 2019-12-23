using System;

namespace NinjaTools.FluentMockServer.Models.HttpEntities
{
    /// <summary>
    ///     Model to describe to which destination the <see cref="HttpRequest" /> to forward.
    /// </summary>
    public class HttpForward : IEquatable<HttpForward>
    {
        public HttpForward(string host, int? port)
        {
            Host = host;
            Port = port;
        }
        /// <summary>
        ///     Gets and sets the Hostname to forward to.
        /// </summary>
        public string Host { get;  }

        /// <summary>
        ///     Gets and sets the Port to forward to.
        /// </summary>
        public int? Port { get; }
        
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
