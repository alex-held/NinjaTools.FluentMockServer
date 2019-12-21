using JetBrains.Annotations;
using NinjaTools.FluentMockServer.API.Configuration;
using NinjaTools.FluentMockServer.API.Extensions.Responses;

namespace NinjaTools.FluentMockServer.API.Infrastructure
{
    public interface IScopeRepository
    {
        [NotNull]
        Response<T> Get<T>([NotNull] string key);

        [NotNull]
        Response Add<T>([NotNull] string key, [NotNull] T value);

        [NotNull]
        Response Update<T>([NotNull] string key, [NotNull] T value);
    }

    /// <inheritdoc />
    public class NoDataFoundError : Error
    {
        /// <inheritdoc />
        public NoDataFoundError(string message) : base(message, MockServerErrorCode.NoDataFoundError)
        {
        }
    }

    /// <inheritdoc />
    public class AddDataFailedError : Error
    {
        /// <inheritdoc />
        public AddDataFailedError(string message) : base(message, MockServerErrorCode.AddDataFailedError)
        {
        }
    }
}
