namespace NinjaTools.FluentMockServer.API.Proxy.Visitors.Collections
{
    public class HttpMethodWrapper : IVisitable
    {
        public HttpMethodWrapper(string methodString)
        {
            MethodString = methodString;
        }

        public HttpMethodWrapper()
        { }

        public string MethodString { get; set; }
        public static implicit operator string(HttpMethodWrapper wrapper) => wrapper.MethodString;
        public static implicit operator HttpMethodWrapper(string str) => new HttpMethodWrapper {MethodString = str};

        /// <inheritdoc />
        public void Accept(IVisitor visitor)
        {
            if (visitor is IVisitor<HttpMethodWrapper> typed)
            {
                typed.Visit(this);
            }
        }
    }
}