using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace HardCoded.MockServer.Contracts.Serialization
{
    public class JsonContent : StringContent
    {
        /// <inheritdoc />
        public JsonContent(object content) : base(JsonConvert.SerializeObject(content, Formatting.Indented), Encoding.UTF8, "application/json")
        {
        }
    }
}