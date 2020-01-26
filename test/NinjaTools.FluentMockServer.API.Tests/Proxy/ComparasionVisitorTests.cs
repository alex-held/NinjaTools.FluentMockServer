using System.Collections.Generic;
using System.IO;
using System.Text;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using NinjaTools.FluentMockServer.API.Models;
using NinjaTools.FluentMockServer.API.Proxy.Visitors;
using NinjaTools.FluentMockServer.API.Proxy.Visitors.Collections;
using NinjaTools.FluentMockServer.Tests.TestHelpers;
using Xunit;
using Xunit.Abstractions;

namespace NinjaTools.FluentMockServer.API.Tests.Proxy
{
    public class ComparasionVisitorTests : XUnitTestBase<ComparasionVisitorTests>
    {
        /// <inheritdoc />
        public ComparasionVisitorTests(ITestOutputHelper output) : base(output)
        { }

        [Theory]
        [InlineData("GET")]
        [InlineData("POST")]
        [InlineData("PUT")]
        [InlineData("OPTIONS")]
        public void When_IsMatch_Returns_True_When_Method_Is_Equal(string method)
        {
            // Arrange
            var compare =  new HttpMethodWrapper(method);
            var context = CreateContext(method);
            var subject = new ComparasionVisitor(context);

            // Act& Assert
            subject.Visit(compare);
            subject.IsSuccess.Should().BeTrue();
            subject.Score.Should().Be(1);
        }

        [Theory]
        [InlineData("/some(abc/path")]
        [InlineData("/some/abc/path")]
        [InlineData("/some/abc/path?query=json")]
        [InlineData("/opt")]
        public void When_IsMatch_Returns_True_When_Path_Is_Match(string path)
        {
            // Arrange
            var compare = CreateObject(path: path);
            var context = CreateContext(path: path);
            var subject = new ComparasionVisitor(context);

            // Act& Assert
            subject.Visit(compare);
            subject.IsSuccess.Should().BeTrue();
            subject.Score.Should().BeGreaterThan(1);
        }

        [Theory]
        [MemberData(nameof(GetHeaderTestData))]
        public void When_IsMatch_Returns_True_When_Headers_Are_Equal(Dictionary<string, string[]> request, Dictionary<string, string[]> contextHeaders, bool isValid)
        {
            // Arrange
            var compare = new HeaderCollection(request);
            var context = CreateContext(headers: contextHeaders);
            var subject = new ComparasionVisitor(context);

            // Act& Assert
            subject.Visit(compare);
            subject.IsSuccess.Should().Be(isValid);
            subject.Score.Should().Be(isValid ? 1 : 0);
        }

        [ItemNotNull]
        public static IEnumerable<object[]> GetHeaderTestData()
        {
            yield return new object[]
            {
                new Dictionary<string, string[]>(),
                new Dictionary<string, string[]>(),
                true
            };
            yield return new object[]
            {
                null,
                new Dictionary<string, string[]>(),
                true
            };
            yield return new object[]
            {
                new Dictionary<string, string[]>
                {
                    {
                        "Host", new[] {"Mock-Server"}
                    }
                },
                new Dictionary<string, string[]>(),
                false
            };
            yield return new object[]
            {
                new Dictionary<string, string[]>
                {
                    {
                        "Host", new[] {"Mock-Server"}
                    }
                },
                new Dictionary<string, string[]>
                {
                    {
                        "Host", new[] {"Mock-Server"}
                    }
                },
                true
            };
            yield return new object[]
            {
                new Dictionary<string, string[]>
                {
                    {
                        "Some-Header", new[] {true.ToString(), 123.ToString()}
                    }
                },
                new Dictionary<string, string[]>
                {
                    {
                        "another.header", new[] {"with some other value"}
                    }
                },
                false
            };
        }


        private HttpContext CreateContext(string method = null, string path = null, Dictionary<string, string[]> headers = null, Stream requestBodyStream = null, QueryString? queryString = null)
        {
            var context = requestBodyStream is null
                ? new DefaultHttpContext()
                : new DefaultHttpContext
                {
                    Request =
                    {
                        Body = requestBodyStream
                    }
                };

            var request = context.Request;
            request.Method = method;
            request.Path = new PathString(path);

            if (queryString != null)
            {
                request.QueryString = queryString.Value;
            }

            foreach (var header in headers ?? new Dictionary<string, string[]>())
            {
                request.Headers.Add(header.Key, header.Value);
            }

            return context;
        }

        private RequestMatcher CreateObject(
            string method = null,
            string path = null,
            Dictionary<string, string[]> headers = null,
            RequestBodyMatcher bodyMatcher = null,
            QueryString? queryString = null)
        {
            return new RequestMatcher
            {
                Method = method,
                Path = path,
                Headers = headers,
                BodyMatcher = bodyMatcher,
                Query = queryString
            };
        }

        [Fact]
        public void IsMatch_Should_Be_False_When_QueryString_Not_Same()
        {
            // Arrange
            var context = CreateContext(queryString: new QueryString("?id=100"));
            var sut = new ComparasionVisitor(context);

            // Act & Assert
            sut.Visit(new QueryString("?color=green"));
            sut.IsSuccess.Should().BeFalse();
        }

        [Fact]
        public void IsMatch_Should_Be_False_When_RequestBodyMatcher_IsNotMatch()
        {
            // Arrange
            var requestBodyContent = "some content";
            var requestBodyStream = new MemoryStream();
            var bytes = Encoding.UTF8.GetBytes(requestBodyContent);
            requestBodyStream.Write(bytes);
            var bodyMatcher = new RequestBodyMatcher
            {
                Content = "hello world",
                Kind = RequestBodyKind.Text,
                MatchExact = true
            };

            var context = CreateContext(requestBodyStream: requestBodyStream);
            var subject = new ComparasionVisitor(context);

            // Act& Assert
            subject.Visit(bodyMatcher);
            subject.IsSuccess.Should().BeFalse();
            subject.Score.Should().Be(0);
        }


        [Fact]
        public void IsMatch_Should_Be_True_When_No_RequestBodyMatcher()
        {
            // Arrange
            var context = CreateContext();
            var subject = new ComparasionVisitor(context);

            // Act& Assert
            subject.Visit((RequestBodyMatcher) null);
            subject.IsSuccess.Should().BeTrue();
            subject.Score.Should().Be(1);
        }

        [Fact]
        public void IsMatch_Should_Be_True_When_QueryString_IsMatch()
        {
            // Arrange
            var query = new QueryString("?id=100");
            var context = CreateContext(queryString: query);
            var subject = new ComparasionVisitor(context);

            // Act& Assert
            subject.Visit(query);
            subject.IsSuccess.Should().BeTrue();
            subject.Score.Should().Be(1);
        }

        [Fact]
        public void IsMatch_Should_Be_True_When_QueryString_Not_Set()
        {
            // Arrange
            var context = CreateContext(queryString: new QueryString("?id=100"));
            var subject = new ComparasionVisitor(context);

            // Act& Assert
            subject.Visit((API.Proxy.Visitors.Collections.QueryCollection) null);
            subject.IsSuccess.Should().BeTrue();
            subject.Score.Should().Be(1);
        }


        [Fact]
        public void IsMatch_Should_Be_True_When_RequestBodyMatcher_IsMatch()
        {
            // Arrange
            var requestBodyContent = "some content";
            var requestBodyStream = new MemoryStream();
            var bytes = Encoding.UTF8.GetBytes(requestBodyContent);
            requestBodyStream.Write(bytes);
            var bodyMatcher = new RequestBodyMatcher
            {
                Content = requestBodyContent,
                Kind = RequestBodyKind.Text,
                MatchExact = true
            };

            var context = CreateContext(requestBodyStream: requestBodyStream);
            var subject = new ComparasionVisitor(context);

            // Act& Assert
            subject.Visit(bodyMatcher);
            subject.IsSuccess.Should().BeTrue();
            subject.Score.Should().Be(1);
        }
    }
}
