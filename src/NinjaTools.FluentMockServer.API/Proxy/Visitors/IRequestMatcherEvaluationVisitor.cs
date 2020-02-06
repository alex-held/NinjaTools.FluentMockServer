using Microsoft.AspNetCore.Http;
using NinjaTools.FluentMockServer.API.Models;

namespace NinjaTools.FluentMockServer.API.Proxy.Visitors
{
    /// <inheritdoc />
    public interface IRequestMatcherEvaluationVisitor : IVisitor
    {
        /// <inheritdoc cref="IVisitor" />
        /// <summary>
        ///     A score how many constraints the request fullfilled in relation to the <see cref="HttpContext"/>.
        /// </summary>
        /// <remarks> ✅ Entrypoint ✅ </remarks>
        /// <returns>
        ///  0 => No match
        ///  2 => 2 Constraints matched e.g. Content-Type Header + Path
        /// </returns>
        int Visit(RequestMatcher requestMatcher);
    }
}
