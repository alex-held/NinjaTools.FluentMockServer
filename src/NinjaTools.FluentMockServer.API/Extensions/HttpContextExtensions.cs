using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace NinjaTools.FluentMockServer.API.Extensions
{
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Reads and deserializes the <see cref="HttpContent"/> into <typeparamref name="T"/>.
        /// </summary>
        /// <param name="httpRequest">
        ///     The <see cref="HttpRequest"/> of which <see cref="HttpContent"/> gets deserialized.
        /// </param>
        /// <typeparam name="T">The type to deserialize to</typeparam>
        /// <returns></returns>
        public static async Task<T> ReadAsync<T>([NotNull] this HttpRequest httpRequest)
        {
            var reader = new StreamReader(httpRequest.Body);
            var json = await reader.ReadToEndAsync();
            return JObject.Parse(json).ToObject<T>();
        }
    }
}
