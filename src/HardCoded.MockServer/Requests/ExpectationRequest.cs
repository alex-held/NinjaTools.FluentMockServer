using System;
using System.Collections.Generic;
using System.Net.Http;

namespace HardCoded.MockServer
{
    public class ExpectationRequest : List<Expectation>
    {
        public static implicit operator HttpRequestMessage(ExpectationRequest request)
        {
            return new HttpRequestMessage(HttpMethod.Put, new Uri("expectation", UriKind.Relative))
            {
                Content = new JsonContent(request)
            };
        }
        
    }
}