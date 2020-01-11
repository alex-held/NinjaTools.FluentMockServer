using Microsoft.AspNetCore.Http;
using NinjaTools.FluentMockServer.API.Models.ViewModels;

namespace NinjaTools.FluentMockServer.API.Models.Logging
{
    public class RequestMatchedLog : LogItem<MatchedRequest>
    {
        /// <inheritdoc />
        public override LogKind Kind => LogKind.RequestMatched;

        /// <inheritdoc />
        public RequestMatchedLog(string id, (HttpContext, Setup) content) : base(id, content)
        {
        }

        /// <inheritdoc />
        protected override string FormatHeader()
        {
            var request = Content.Context.Request;
            var requestLiteral = $"{request.Method} {request.Path}";
            return string.Format(base.FormatHeader(), requestLiteral);
        }
    }
}
