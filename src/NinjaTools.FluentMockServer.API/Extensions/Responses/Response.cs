using System.Collections.Generic;
using JetBrains.Annotations;
using NinjaTools.FluentMockServer.API.Configuration;

namespace NinjaTools.FluentMockServer.API.Extensions.Responses
{
    /// <inheritdoc />
    public abstract class Response<T> : Response
    {
        /// <inheritdoc />
        protected Response([NotNull] T data)
        {
            Data = data;
        }

        /// <inheritdoc />
        protected Response([CanBeNull] List<Error> errors) : base(errors)
        {
        }

        /// <summary>
        /// Gets the <see cref="Response"/>'s data content.
        /// </summary>
        /// <returns>[CanBeNull] when returning an <see cref="ErrorResponse"/>.</returns>
        [CanBeNull]
        public T Data { get; private set; }
    }

    /// <inheritdoc />
    public class ErrorResponse<T> : Response<T>
    {
        /// <inheritdoc />
        public ErrorResponse([NotNull] Error error) : this(new List<Error> { error})
        {
        }

        /// <inheritdoc />
        public ErrorResponse([NotNull] List<Error> errors) : base(errors)
        {
        }
    }


    public abstract class Response
    {
        protected Response()
        {
            Errors = new List<Error>();
        }

        protected Response([CanBeNull] List<Error> errors)
        {
            Errors = errors ?? new List<Error>();
        }

        /// <summary>
        /// Gets a list of <see cref="Error"/> related to this <see cref="Response"/>.
        /// </summary>
        public List<Error> Errors { get; }

        /// <summary>
        /// Whether this <see cref="Response"/> contains any <see cref="Error"/>.
        /// </summary>
        public bool IsError => Errors.Count > 0;
    }
}
