using System.Collections.Generic;
using JetBrains.Annotations;
using NinjaTools.FluentMockServer.API.Configuration;

namespace NinjaTools.FluentMockServer.API.Extensions.Responses
{
    /// <inheritdoc />
    public class ErrorResponse : Response
    {
        /// <inheritdoc />
        public ErrorResponse(Error error) : this(new List<Error> { error})
        {
        }

        /// <inheritdoc />
        public ErrorResponse([NotNull] List<Error> errors) : base(errors)
        {
        }
    }
}