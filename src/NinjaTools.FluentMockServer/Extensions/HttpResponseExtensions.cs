using System.Net.Http;
using System.Runtime.CompilerServices;
using NinjaTools.FluentMockServer.Exceptions;

namespace NinjaTools.FluentMockServer.Extensions
{
    public static class HttpResponseExtensions
    {
        /// <summary>
        /// Ensures that the operation on the MockServer attempted, was successful.
        /// </summary>
        /// <exception cref="MockServerOperationFailedException"></exception>
        public static void EnsureSuccessfulMockServerOperation(this HttpResponseMessage responseMessage, [CallerMemberName] string caller = null)
        {
            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new MockServerOperationFailedException(caller);
            }
        }
    }
}
