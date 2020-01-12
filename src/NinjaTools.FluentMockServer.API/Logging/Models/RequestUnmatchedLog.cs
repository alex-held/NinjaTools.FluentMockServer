using Microsoft.AspNetCore.Http;
using NinjaTools.FluentMockServer.API.Models.ViewModels;

namespace NinjaTools.FluentMockServer.API.Logging.Models
{
    public class RequestUnmatchedLog : LogItem<HttpRequestViewModel>
    {
        /// <inheritdoc />
        public override LogKind Kind => LogKind.RequestUnmatched;

        /// <inheritdoc />
        public RequestUnmatchedLog(string id, HttpContext content) : base(id,new HttpRequestViewModel(content.Request))
        {
        }

        /// <inheritdoc />
        protected override string FormatHeader()
        {
            var request = Content;
            var requestLiteral = $"{request.Method} {request.Path}";
            return string.Format(base.FormatHeader(), requestLiteral);
        }
    }
}
