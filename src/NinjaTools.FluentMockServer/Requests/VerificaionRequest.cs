using System;
using System.Net.Http;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NinjaTools.FluentMockServer.Models.HttpEntities;
using NinjaTools.FluentMockServer.Models.ValueTypes;
using NinjaTools.FluentMockServer.Utils;

namespace NinjaTools.FluentMockServer.Requests
{
public class VerificaionRequest 
    {


        public static implicit operator HttpRequestMessage(VerificaionRequest request)
        {
            return new HttpRequestMessage(HttpMethod.Put, new Uri("verify", UriKind.Relative))
            {
                Content = new JsonContent(request)
            };
        }


        public static VerificaionRequest Once(HttpRequest request) => new VerificaionRequest(request, VerficationTimes.Once);

        public VerificaionRequest(HttpRequest request, VerficationTimes times)
        {
            HttpRequest = request;
            Times = times;
        }

        public HttpRequest HttpRequest { get; set; }

        public VerficationTimes Times { get; set; }

    }
}
