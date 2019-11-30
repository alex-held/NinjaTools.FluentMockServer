using System;
using System.Net.Http;
using NinjaTools.FluentMockServer.Models.HttpEntities;
using NinjaTools.FluentMockServer.Models.ValueTypes;
using NinjaTools.FluentMockServer.Utils;

namespace NinjaTools.FluentMockServer.Requests
{
    public class VerificaionRequest
    {
        public VerificaionRequest(HttpRequest request, VerficationTimes times)
        {
            HttpRequest = request;
            Times = times;
        }

        public HttpRequest HttpRequest { get; set; }

        public VerficationTimes Times { get; set; }


        public static implicit operator HttpRequestMessage(VerificaionRequest request)
        {
            return new HttpRequestMessage(HttpMethod.Put, new Uri("verify", UriKind.Relative))
            {
                Content = new JsonContent(request)
            };
        }


        public static VerificaionRequest Once(HttpRequest request)
        {
            return new VerificaionRequest(request, VerficationTimes.Once);
        }
    }
}
