using System;
using System.Collections.Generic;
using System.Net.Http;
using NinjaTools.FluentMockServer.Models;
using NinjaTools.FluentMockServer.Utils;

namespace NinjaTools.FluentMockServer.Requests
{
    public class ExpectationRequest
    {
        private readonly List<Expectation> _listImplementation;

        public ExpectationRequest()
        {
            _listImplementation = new List<Expectation>();
        }

        protected internal ExpectationRequest(IEnumerable<Expectation> expectations)
        {
            _listImplementation.AddRange(expectations ?? throw new ArgumentNullException(nameof(expectations)));
        }


        public static ExpectationRequest Create(IEnumerable<Expectation> expectations)
        {
            return new ExpectationRequest(expectations);
        }

        public static implicit operator HttpRequestMessage(ExpectationRequest request)
        {
            return new HttpRequestMessage(HttpMethod.Put, new Uri("expectation", UriKind.Relative))
            {
                Content = new JsonContent(request)
            };
        }


        public static implicit operator ExpectationRequest(Expectation[] expectations)
        {
            var request = new ExpectationRequest();

            foreach (var expectation in expectations)
                if (expectation != null)
                    request._listImplementation.Add(expectation);

            return request;
        }
    }
}
