using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using NinjaTools.FluentMockServer.API.Models;
using NinjaTools.FluentMockServer.API.Models.ViewModels;

namespace NinjaTools.FluentMockServer.API.Infrastructure
{
    internal class RequestRepository
    {
        public RequestRepository()
        {
            MatchedRequests = new ConcurrentBag<MatchedRequest>();
            UnmatchedRequests = new ConcurrentBag<UnmatchedRequest>();
        }

        public ConcurrentBag<MatchedRequest> MatchedRequests { get; }
        public ConcurrentBag<UnmatchedRequest> UnmatchedRequests { get; }

        public IReadOnlyList<IUpstreamRequest> GetAll() => MatchedRequests.Cast<IUpstreamRequest>().Concat(UnmatchedRequests).ToList();

        public void Add(HttpContext context, Setup setup)
        {
            var matchedRequest = new MatchedRequest(context, setup);
            MatchedRequests.Add(matchedRequest);
        }

        public void Add(HttpContext context)
        {
            var unmatchedRequest = new UnmatchedRequest(context);
            UnmatchedRequests.Add(unmatchedRequest);
        }

        public void Clear()
        {
            MatchedRequests.Clear();
            UnmatchedRequests.Clear();
        }
    }
}
