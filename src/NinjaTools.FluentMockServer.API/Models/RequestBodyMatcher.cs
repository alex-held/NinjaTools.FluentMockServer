using System;
using System.Diagnostics;
using System.IO;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using NinjaTools.FluentMockServer.API.Proxy.Evaluation.Visitors;

namespace NinjaTools.FluentMockServer.API.Models
{
    [DebuggerDisplay("{DebuggerDisplay()}")]
    public class RequestBodyMatcher : IRequestBody
    {
        public string DebuggerDisplay()
        {
            return $"Type={Type.ToString()}; MatchExact={MatchExact.ToString()}; Content={Content ?? "<null>"}";
        }

        public string? Content { get; set; }
        public RequestBodyKind  Type { get; set; }
        public bool MatchExact { get; set; }


        public bool IsMatch([NotNull] HttpRequest request)
        {
            request.EnableBuffering();
            request.Body.Position = 0;
            using var reader = new StreamReader(request.Body);
            var content = reader.ReadToEnd();

            if (MatchExact)
            {
                return Content == content;
            }

            return content.Contains(Content);
        }


        /// <inheritdoc />
        public void Accept(Func<IPartialVisitor> visitorFactory)
        {
            var visitor = visitorFactory();
            visitor.VisitBody(Content, MatchExact, Type);
        }
    }
}
