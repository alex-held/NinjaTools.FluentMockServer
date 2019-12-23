using System;

namespace NinjaTools.FluentMockServer.API.Middleware
{
    internal class ReverseProxyRule
    {
        public Func<Uri, bool> Matcher { get; set; }
    }
}