using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Text;

using JetBrains.Annotations;

using Newtonsoft.Json;

namespace HardCoded.MockServer.Contracts.Serialization
{
    public class JsonContent : StringContent
    {
        /// <inheritdoc />
        public JsonContent([NotNull] object content) : base(JsonConvert.SerializeObject(content, Formatting.Indented), Encoding.UTF8, CommonContentType.Json)
        {
        }
    }
}
