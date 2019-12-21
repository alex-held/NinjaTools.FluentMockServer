using JetBrains.Annotations;

namespace NinjaTools.FluentMockServer.API.Extensions.Responses
{
    /// <inheritdoc />
    public class OkResponse<T> : Response<T>
    {
        /// <inheritdoc />
        public OkResponse([NotNull] T data) : base(data)
        {
        }
    }
    
    
    /// <inheritdoc />
    public class OkResponse : Response
    {
        /// <inheritdoc />
        public OkResponse()
        {
        }
    }
}
