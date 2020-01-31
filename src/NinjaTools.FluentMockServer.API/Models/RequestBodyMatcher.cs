using NinjaTools.FluentMockServer.API.Types;

namespace NinjaTools.FluentMockServer.API.Models
{
    public class RequestBodyMatcher :  IDontRenderWhenEmpty
    {
        public string? Content { get; set; }
        public RequestBodyKind?  Kind { get; set; }
        public bool? MatchExact { get; set; }

        public bool IsExactMatch => MatchExact != null && MatchExact == true;

        /// <inheritdoc />
        public bool IsEnabled() => !string.IsNullOrWhiteSpace(Content) || Kind.HasValue || MatchExact.HasValue;
    }
}
