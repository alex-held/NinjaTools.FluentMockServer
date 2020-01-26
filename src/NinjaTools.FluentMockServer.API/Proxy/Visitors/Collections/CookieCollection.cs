using System.Collections.Generic;

namespace NinjaTools.FluentMockServer.API.Proxy.Visitors.Collections
{
    public class CookieCollection : Dictionary<string, string>,  IVisitable
    {
        /// <inheritdoc />
        public void Accept(IVisitor visitor)
        {
            if (visitor is IVisitor<CookieCollection> typed)
            {
                typed.Visit(this);
            }
        }
    }
}