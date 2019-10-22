using System;
using System.Collections.Generic;
using System.Net.Http;

using HardCoded.MockServer.Contracts.Models;
using HardCoded.MockServer.Contracts.Serialization;


namespace HardCoded.MockServer.Requests
{
    public class ExpectationRequest : List<Expectation>
    {
        public ExpectationRequest()
        { }

        internal protected ExpectationRequest(IEnumerable<Expectation> expectations)
        {
            AddRange( expectations ?? throw new ArgumentNullException(nameof(expectations)));
        }
        
        public static ExpectationRequest Create(IEnumerable<Expectation> expectations) => new ExpectationRequest(expectations);
        
        public static implicit operator HttpRequestMessage(ExpectationRequest request)
        {
            return new HttpRequestMessage(HttpMethod.Put, new Uri("expectation", UriKind.Relative))
            {
                Content = new JsonContent(request)
            };
        }
        
    }
}