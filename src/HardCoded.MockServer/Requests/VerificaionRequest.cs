using System;
using System.Net.Http;
using HardCoded.MockServer.Models.HttpEntities;
using Newtonsoft.Json;

namespace HardCoded.MockServer
{
    public class VerificaionRequest
    {
        public static VerificaionRequest Once(HttpRequest request) => new VerificaionRequest(request, VerficationTimes.Once);

        public VerificaionRequest(HttpRequest request, VerficationTimes times)
        {
            HttpRequest = request;
            Times = times;
        }
        
        [JsonProperty("httpRequest")]
        public HttpRequest HttpRequest { get; set; }
        
        [JsonProperty("times")]
        public VerficationTimes Times { get; set; }
        
        public static implicit operator HttpRequestMessage(VerificaionRequest request)
        {
            return new HttpRequestMessage(HttpMethod.Put, new Uri("verify", UriKind.Relative))
            {
                Content = new JsonContent(request)
            };
        }
    }
}