using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NinjaTools.FluentMockServer.API.Configuration;
using NinjaTools.FluentMockServer.API.Downstream;
using NinjaTools.FluentMockServer.API.Extensions;
using Xunit;
using HttpMethod = Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http.HttpMethod;

namespace NinjaTools.FluentMockServer.API.Tests.Extensions
{
    public class ProxyExtensionTests
    {
        [Theory]
        [InlineData(HttpMethod.Get, false)]
        [InlineData(HttpMethod.Trace, false)]
        [InlineData(HttpMethod.Delete, false)]
        [InlineData(HttpMethod.Head, false)]
        [InlineData(HttpMethod.Post, true)]
        [InlineData(HttpMethod.Options, true)]
        [InlineData(HttpMethod.Custom, true)]
        [InlineData(HttpMethod.Put, true)]
        [InlineData(HttpMethod.Patch, true)]
        [InlineData(HttpMethod.Connect, true)]
        public void CanHaveContentBasedOnMethod_Should_Return_Value_Based_On_Http_Specification(HttpMethod method, bool expected)
        {
            // Arrange
            var sut = new DefaultHttpContext().Request;
            sut.Method = method.ToString();

            // Act && Assert
            sut.CouldHaveContentBasedOnMethod().Should().Be(expected);
        }
    }

    [SuppressMessage("ReSharper", "InvokeAsExtensionMethod")]
    public class HttpRequestMessageFactoryTests
    {
        private DownstreamContext CreateContext([JetBrains.Annotations.NotNull] HttpContext httpContext)
        {
            return new DownstreamContext(httpContext);
        }

        [Theory]
        [MemberData(nameof(CreateDefaultContextData))]
        public void CreateDefaultHttpRequestMessage_Should_Return_HttpRequestMessage_Copies_All_Properties_From_HttpRequest(HttpContext httpContext, HttpRequestMessage expected)
        {
            // Act
            var actual = HttpRequestMessageFactory.CreateDefaultHttpRequestMessage(httpContext.Request);

            // Assert
            VerifyEqual(actual, expected);
        }

        private static void VerifyEqual([JetBrains.Annotations.NotNull] HttpRequestMessage actual, [JetBrains.Annotations.NotNull] HttpRequestMessage expected)
        {
            actual.Method.Method.ToUpper().Should().Be(expected.Method.Method.ToUpper());
            actual.RequestUri.Should().Be(expected.RequestUri);
            actual.Headers.Should().Contain(expected.Headers);
            actual.Headers.Count().Should().Be(expected.Headers.Count());
            actual.Version.Should().Be(expected.Version);
            actual.Properties.Should().Contain(expected.Properties);
            actual.Properties.Count.Should().Be(expected.Properties.Count);
        }

        public static IEnumerable<object[]> CreateDefaultContextData()
        {
            yield return CreateData(new Uri("http://host:80/some/path"), HttpMethod.Post);


            object[] CreateData(Uri requestUri, HttpMethod method)
            {
                var httpContext = new DefaultHttpContext();
                httpContext.Request.Scheme = requestUri.Scheme;
                httpContext.Request.Host = new HostString(requestUri.Authority, requestUri.Port);
                httpContext.Request.Method = method.ToString();
                httpContext.Request.Path = new PathString(requestUri.AbsolutePath);
                httpContext.Request.QueryString = string.IsNullOrWhiteSpace(requestUri.Query)
                    ? new QueryString(requestUri.Query)
                    : QueryString.Empty;

                var requestMessage = new HttpRequestMessage(System.Net.Http.HttpMethod.Post, requestUri);
                return new object[]
                {
                    httpContext,
                    requestMessage
                };
            }
        }
    }
}
