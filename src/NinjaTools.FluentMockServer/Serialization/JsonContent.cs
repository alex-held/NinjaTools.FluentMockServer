using System.Net.Http;
using System.Text;
using JetBrains.Annotations;

namespace NinjaTools.FluentMockServer.Serialization
{
    public class JsonContent : StringContent
    {
        /// <inheritdoc />
        public JsonContent([NotNull] object content) : base(Serializer.Serialize(content), Encoding.UTF8, "application/json")
        {
        }
    }
}
