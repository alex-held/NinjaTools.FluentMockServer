using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using NinjaTools.FluentMockServer.API.Downstream;
using NinjaTools.FluentMockServer.API.Extensions;
using NinjaTools.FluentMockServer.API.Extensions.Responses;
using NinjaTools.FluentMockServer.Tests.TestHelpers;
using NinjaTools.FluentMockServer.Tests.TestHelpers.Data.TheoryData;
using Shouldly;
using TestStack.BDDfy;
using TestStack.BDDfy.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.API.Tests.Downstream
{
    // [ExampleData(ExampleData = typeof(EmailExampleTable))]
    [Story(TitlePrefix = "Internal", Title = "Modify scoped HttpRequest before they get routed by MVC.",
        AsA = "API user",
        IWant = "I want to configure httpRequest re-routes",
        SoThat = "I can setup up actions the mockserver should take.")]
    public class RequestMapperSpec : XUnitTestBase<RequestMapperSpec>
    {
        private readonly RequestMapper _requestMapper;
        private List<KeyValuePair<string, StringValues>> _inputHeaders;
        private HttpRequest _inputRequest;
        private Email _mail;
        private Response<HttpRequestMessage> _result;


        public RequestMapperSpec(ITestOutputHelper outputHelper) : base(outputHelper)
        {
            _inputRequest = new DefaultHttpContext().Request;
            _requestMapper = new RequestMapper();
        }

        [CanBeNull]
        public string? VerifyEquality([CanBeNull] HttpRequestMessage actual, [CanBeNull] HttpRequestMessage expected)
        {
            switch (actual)
            {
                case null when expected is null:
                    return null;
                case null:
                    return $"{nameof(expected)} is <null> but {nameof(actual)} is set to an instance.";
                case { } msg when expected is null:
                    return $"{nameof(expected)} is set to an instance but {nameof(actual)} is <null>.";
            }

            actual.Method.Method.ToUpper().Should().Be(expected.Method.Method.ToUpper());
            actual.RequestUri.Should().Be(expected.RequestUri);
            actual.Version.Should().Be(expected.Version);


            if (expected.Properties != null)
            {
                actual.Properties.Should().NotBeNull();
                actual.Properties.ToList().Count.Should().Be(expected.Properties.ToList().Count);
                if (expected.Properties.Any()) actual.Properties.ToList().Should().Contain(expected.Properties.ToList());
            }
            else
            {
                actual.Properties.Should().BeNull();
            }

            if (expected.Headers != null)
            {
                actual.Headers.Should().NotBeNull();
                actual.Headers.ToList().Count.Should().Be(expected.Headers.ToList().Count);

                if (expected.Headers.Any()) actual.Headers.ToList().Should().Contain(expected.Headers.ToList());
            }
            else
            {
                actual.Headers.Should().BeNull();
            }

            return null;
        }


        [BddfyTheory]
        [ClassData(typeof(HttpContentEmailData))]
        public void MapAsync_Maps_Content(Stream request, HttpContent content)
        {
            this.Given(x => x.GivenTheInputRequestHasBody(request))
                .And(_ => GivenInputRequestHasValidRequestUri("http://example.org/some/path"))
                .And(_ => GivenTheInputRequestHasMethod("POST"))
                .When(_ => WhenMapped())
                .And(_ => ThenNoErrorIsReturned())
                .Then(_ => ThenTheMappedContentIs(content))
                .BDDfy();
        }

        private async Task ThenTheMappedContentIs([NotNull] HttpContent expected)
        {
            // ReSharper disable once PossibleNullReferenceException
            var actualData = await _result.Data.Content.ReadAsStringAsync();
            var expectedData = await expected.ReadAsStringAsync();

            if (actualData != null) Output.WriteLine($"Actual data:\n{actualData}");

            if (expectedData != null) Output.WriteLine($"Expected data:\n{expectedData}");

            actualData.ShouldBe(expectedData);
        }

        [BddfyTheory]
        [ClassData(typeof(HttpRequestMapperData))]
        public void MapAsync_Maps_Method(HttpRequest request, HttpRequestMessage expected)
        {
            this.Given(_ => GivenInputData(request))
                .When(_ => WhenMapped())
                .Then(_ => ThenTheMappedRequestHasMethod(expected.Method))
                .BDDfy();
        }


        [BddfyTheory]
        [ClassData(typeof(HttpRequestMapperData))]
        public void MapAsync_Maps_RequestUri(HttpRequest request, HttpRequestMessage expected)
        {
            this.Given(_ => GivenInputData(request))
                .When(_ => WhenMapped())
                .Then(_ => ThenNoErrorIsReturned())
                .Then(x => ThenTheMappedHasRequestUri(expected.RequestUri))
                .BDDfy();
        }

        //
        // [BddfyTheory]
        // [ClassData(typeof(HttpRequestMapperData))]
        // public void MapAsync_Maps_RequestUri(HttpRequest request, HttpRequestMessage expected)
        // {
        //     this.Given(x => GivenTheInputRequestHasPath(request.Path))
        //         .And(_ => GivenTheInputRequestHasMethod(request.Method))
        //         .And(_ => GivenTheInputRequestHasHostWithPort(request.Host.Host, request.Host.Port))
        //         .And(_ => GivenTheInputRequestHasScheme(request.Scheme))
        //         .When(_ => WhenMapped())
        //         .Then(_ => ThenNoErrorIsReturned())
        //         .Then(x => ThenTheMappedHasRequestUri(expected.RequestUri))
        //         .BDDfy();
        // }

        private void GivenInputData(HttpRequest request)
        {
            _inputRequest = request;
        }


        [BddfyTheory]
        [ClassData(typeof(HttpRequestMapperData))]
        public void MapAsync_Does_Not_Map_Host(HttpRequest request, HttpRequestMessage expected)
        {
            this.Given(_ => GivenInputData(request))
                .When(_ => WhenMapped())
                .Then(_ => ThenHostHeaderIsNotMapped())
                .BDDfy();
        }

        [Theory]
        [ClassData(typeof(HttpRequestMapperData))]
        public async Task MapAsync(HttpRequest httpRequest, HttpRequestMessage expected)
        {
            // Act
            var response = await _requestMapper.MapAsync(httpRequest);

            // Assert
            response.IsError.Should().BeFalse(response.Errors.Dump());
            VerifyEquality(response.Data, expected);
        }

        [Theory]
        [ClassData(typeof(HttpMethodTestData))]
        public void Should_Map_HttpMethod(string input, HttpMethod expected)
        {
            this.Given(_ => GivenTheInputRequestHasMethod(input))
                .And(_ => GivenInputRequestHasValidRequestUri("http://google.de"))
                .When(_ => WhenMapped())
                .Then(_ => ThenNoErrorIsReturned())
                .Then(_ => ThenTheMappedRequestHasMethod(expected))
                .BDDfy();
        }

        [BddfyTheory]
        [ClassData(typeof(HttpRequestMapperData))]
        public void Should(HttpRequest request, HttpRequestMessage expected)
        {
            this.Given(_ => GivenInputRequestHasValidRequestUri("http://google.de"))
                .And(_ => GivenTheInputRequestHasMethod(request.Method))
                .When(_ => WhenMapped())
                .Then(_ => ThenNoErrorIsReturned())
                .Then(_ => ThenTheMappedRequestHasMethod(expected.Method))
                .BDDfy();
        }


        [BddfyTheory]
        [ClassData(typeof(HttpRequestMapperData))]
        public void MapAsync_Does_Not_Map_Null_Content(HttpRequest request, HttpRequestMessage expected)
        {
            this.Given(_ => GivenInputRequestHasValidRequestUri(expected.RequestUri.ToString()))
                .And(_ => GivenTheInputRequestHasMethod(request.Method))
                .And(_ => GivenTheInputRequestHasNullContent())
                .When(_ => WhenMapped())
                .Then(_ => ThenNoErrorIsReturned())
                .And(_ => ThenTheMappedContentIsNull())
                .BDDfy();
        }


        [BddfyFact]
        public void MapAsync_Does_Not_Map_HostHeader()
        {
            this.Given(_ => GivenInputRequestHasValidRequestUri("https://google.net:443/some/path?query=30"))
                .And(_ => GivenTheInputRequestHasMethod("GET"))
                .When(_ => WhenMapped())
                .Then(_ => ThenNoErrorIsReturned())
                .And(_ => ThenHostHeaderIsNotMapped())
                .BDDfy();
        }

        [BddfyTheory]
        [ClassData(typeof(HttpContentEmailData))]
        public void MapAsync_Maps_HttpContent(Stream body, HttpContent expected)
        {
            this.Given(_ => GivenInputRequestHasValidRequestUri("http://example.org"))
                .And(_ => GivenTheInputRequestHasMethod(HttpMethod.Post.ToString()))
                .And(_ => GivenTheInputRequestHasBody(body))
                .When(_ => WhenMapped())
                .And(_ => ThenNoErrorIsReturned())
                .Then(_ => ThenTheMappedContentIs(expected))
                .BDDfy("mapped HttpContent.");
        }


        private void ThenTheMappedHasRequestUri(Uri requestUri)
        {
            _result.Data?.RequestUri.ShouldBe(requestUri);
        }


        private void ThenTheMappedContentIsNull()
        {
            _result.Data?.Content.ShouldBeNull();
        }


        private void ThenHostHeaderIsNotMapped()
        {
            _result.Data.Headers.Host.ShouldBeNull();
        }


        private void GivenTheInputRequestHasMethod([NotNull] string method)
        {
            _inputRequest.Method = new HttpMethod(method).Method;
        }

        private void GivenTheInputRequestHasHostWithPort(string host, int? port = null)
        {
            _inputRequest.Host = port.HasValue
                ? new HostString(host, port.Value)
                : new HostString(host);
        }


        private void GivenTheInputRequestHasNoHeaders()
        {
            _inputRequest.Headers.Clear();
        }


        private void GivenTheInputRequestHasPath([CanBeNull] string path)
        {
            if (path != null) _inputRequest.Path = path;
        }

        private void GivenTheInputRequestHasQueryString([CanBeNull] string querystring)
        {
            if (querystring != null) _inputRequest.QueryString = new QueryString(querystring);
        }


        private async Task WhenMapped()
        {
            _result = await _requestMapper.MapAsync(_inputRequest);

            if (_result.Data != null && _result.Data is { } data) _mail = await data.Content.DeserializeAsync<Email>();
        }


        private void GivenInputRequestHasValidRequestUri([NotNull] string requestUri)
        {
            var uri = new Uri(requestUri);
            _inputRequest.Scheme = uri.Scheme;
            GivenTheInputRequestHasHostWithPort(uri.Authority, uri.Port);
        }

        private void GivenTheInputRequestHasBody([NotNull] Stream stream)
        {
            _inputRequest.Body = stream;
            //    _inputRequest.EnableBuffering();
        }

        private void GivenTheInputRequestHasNullContent()
        {
            _inputRequest.Body = null;
        }


        private void ThenTheMappedRequestHasMethod(HttpMethod method)
        {
            _result.Data?.Method.ShouldBe(method);
        }

        private void ThenNoErrorIsReturned()
        {
            _result.Errors.Should().BeEmpty();
            _result.IsError.ShouldBeFalse();
        }
    }
}
