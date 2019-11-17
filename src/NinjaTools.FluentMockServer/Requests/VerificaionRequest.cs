using System;
using System.Net.Http;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.Abstractions;
using NinjaTools.FluentMockServer.Models.HttpEntities;
using NinjaTools.FluentMockServer.Models.ValueTypes;
using NinjaTools.FluentMockServer.Utils;

namespace NinjaTools.FluentMockServer.Requests
{
    [JsonObject(IsReference = true)]
    public class VerificaionRequest : IBuildable
    {



        public static VerificaionRequest Once(HttpRequest request) => new VerificaionRequest(request, VerficationTimes.Once);

        public VerificaionRequest(HttpRequest request, VerficationTimes times)
        {
            HttpRequest = request;
            Times = times;
        }

        [JsonProperty("httpRequest")]
        public HttpRequest HttpRequest { get; set; }

        [JsonProperty("times", NullValueHandling = NullValueHandling.Ignore)]
        public VerficationTimes Times { get; set; }

        public static implicit operator HttpRequestMessage(VerificaionRequest request)
        {
            return new HttpRequestMessage(HttpMethod.Put, new Uri("verify", UriKind.Relative))
            {
                Content = new JsonContent(request)
            };
        }

        /// <inheritdoc />
        public JObject SerializeJObject()
        {
            return JObject.FromObject(this, Serializer.Default);

            var self = new JObject();

            // TODO: finish serialize

            return self;
        }
    }
}
