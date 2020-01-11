using System.Diagnostics;
using JetBrains.Annotations;

namespace NinjaTools.FluentMockServer.API.Models
{
    [DebuggerDisplay("{DebuggerDisplay()}")]
    public class Setup
    {
        public string DebuggerDisplay()
        {
            return $"Matcher={Matcher?.DebuggerDisplay() ?? "*"}; Action={Action?.DebuggerDisplay() ?? "<null>"}";
        }

        [CanBeNull]
        public RequestMatcher? Matcher { get; set; }

        [CanBeNull]
        public ResponseAction? Action { get; set; }
    }
}
