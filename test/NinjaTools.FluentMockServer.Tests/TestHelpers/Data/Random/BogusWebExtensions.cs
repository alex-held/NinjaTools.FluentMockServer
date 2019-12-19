using System.Collections.Generic;
using System.Net.Http;
using Bogus.DataSets;
using JetBrains.Annotations;

namespace NinjaTools.FluentMockServer.Tests.TestHelpers.Data.Random
{
    public static class BogusWebExtensions
    {
        public static readonly List<HttpMethod> HttpMethods = new List<HttpMethod>
        {
            HttpMethod.Delete, HttpMethod.Head, HttpMethod.Trace, HttpMethod.Options, HttpMethod.Patch, HttpMethod.Put, HttpMethod.Post
        };


        /// <summary>Generate a random <see cref="HttpMethod" />.</summary>
        public static HttpMethod RandomHttpMethod
            ([NotNull] this Internet internet) => internet.Random.CollectionItem(HttpMethods);
    }
}
