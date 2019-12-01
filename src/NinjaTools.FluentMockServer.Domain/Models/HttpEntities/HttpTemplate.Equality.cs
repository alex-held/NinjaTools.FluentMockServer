using System;

namespace NinjaTools.FluentMockServer.Domain.Models.HttpEntities
{
    public partial class HttpTemplate : IEquatable<HttpTemplate>
    {
        /// <inheritdoc />
        public bool Equals(HttpTemplate other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Template == other.Template && Equals(Delay, other.Delay);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((HttpTemplate) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return ((Template != null ? Template.GetHashCode() : 0) * 397) ^ (Delay != null ? Delay.GetHashCode() : 0);
            }
        }
    }
}
