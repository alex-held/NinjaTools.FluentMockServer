using System.Diagnostics;
using JetBrains.Annotations;

namespace NinjaTools.FluentMockServer.API.Models
{
    public class Setup
    {
        [CanBeNull]
        public RequestMatcher? Matcher { get; set; }

        [CanBeNull]
        public ResponseAction? Action { get; set; }
    }
}
