using JetBrains.Annotations;

namespace NinjaTools.FluentMockServer.API.Models
{
  //  [DebuggerDisplay("{" + nameof(RequestMatcher) + "})")]
    public class Setup
    {
        [CanBeNull]
        public RequestMatcher? Matcher { get; set; }

        [CanBeNull]
        public ResponseAction? Action { get; set; }
    }
}
