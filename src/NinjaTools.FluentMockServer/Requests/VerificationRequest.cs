using System;
using System.Net.Http;
using NinjaTools.FluentMockServer.Models.HttpEntities;
using NinjaTools.FluentMockServer.Models.ValueTypes;
using NinjaTools.FluentMockServer.Utils;

namespace NinjaTools.FluentMockServer.Requests
{
    public class VerificationRequest
    {
        public VerificationRequest(HttpRequest request, VerificationTimes times)
        {
            HttpRequest = request;
            Times = times;
        }

        public HttpRequest HttpRequest { get; set; }

        public VerificationTimes Times { get; set; }


        public static implicit operator HttpRequestMessage(VerificationRequest request)
        {
            return new HttpRequestMessage(HttpMethod.Put, new Uri("verify", UriKind.Relative))
            {
                Content = new JsonContent(request)
            };
        }


        public static VerificationRequest Once(HttpRequest request)
        {
            return new VerificationRequest(request, VerificationTimes.Once);
        }
    }
}
