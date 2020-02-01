using System.Collections.Generic;
using System.IO;
using System.Text;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using NinjaTools.FluentMockServer.API.Models;
using NinjaTools.FluentMockServer.API.Proxy.Visitors;
using NinjaTools.FluentMockServer.Tests.TestHelpers;
using Xunit;
using Xunit.Abstractions;
using Path = NinjaTools.FluentMockServer.API.Models.Path;

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
        public void When_IsMatch_Returns_1_When_Method_Is_Equal(string method)
        {
            // Arrange
            var compare =  new Method(method);
            var context = CreateContext(method);
            var subject = new RequestMatcherEvaluationVisitor(context);

            // Act& Assert
            var score = subject.Visit(compare);
            score.Should().Be(1);
        }

        [Theory]
        [InlineData("/some(abc/path")]
        [InlineData("/some/abc/path")]
        [InlineData("/some/abc/path?query=json")]
        [InlineData("/opt")]
        public void When_IsMatch_Returns_1_When_Path_Is_Match(string path)
        {
            // Arrange
            var compare = new Path(path);
            var context = CreateContext(path: path);
            var subject = new RequestMatcherEvaluationVisitor(context);

            // Act& Assert
            var score = subject.Visit(compare);
            score.Should().Be(1);
        }

        [Theory]
        [MemberData(nameof(GetHeaderTestData))]
        public void When_IsMatch_Returns_1_When_Headers_Are_Equal(Dictionary<string, string[]> request, Dictionary<string, string[]> contextHeaders, bool isValid)
        {
            // Arrange
            var compare = new Headers(request);
            var context = CreateContext(headers: contextHeaders);
            var subject = new RequestMatcherEvaluationVisitor(context);

            // Act& Assert
            var score = subject.Visit(compare);
            score.Should().Be(isValid ? 1 : 0);
        }

        [Fact]
        public void Score_Should_Be_5_When_5_Headers_Match()
        {
            // Arrange
            var headers = new Dictionary<string, string[]>
            {
                {"a", new []{"a"}},
                {"b", new []{"b"}},
                {"c", new []{"c"}},
                {"d", new []{"d"}},
                {"e", new []{"e"}},
            };

            var compare = new Headers(headers);
            var context = CreateContext(headers: headers);
            var subject = new RequestMatcherEvaluationVisitor(context);

            // Act
            var score = subject.Visit(compare);

            // Assert
            score.Should().Be(5);
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
                request.QueryString = new QueryString(queryString.Value.Value);

            if (headers != null)
            {
                foreach (var (key, value) in headers)
                    request.Headers.Add(key, value);
            }

            return context;
        }

        [Fact]
        public void IsMatch_Should_Be_False_When_QueryString_Not_Same()
        {
            // Arrange
            var context = CreateContext(queryString: new QueryString());
            var sut = new RequestMatcherEvaluationVisitor(context);

            // Act & Assert
            var score = sut.Visit(new Query(new QueryString("?id=100")));
            score.Should().Be(0);
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
            var subject = new RequestMatcherEvaluationVisitor(context);

            // Act& Assert
            var score =  subject.Visit(bodyMatcher);
            score.Should().Be(0);
        }


        [Fact]
        public void IsMatch_Should_Be_True_When_No_RequestBodyMatcher()
        {
            // Arrange
            var context = CreateContext();
            var subject = new RequestMatcherEvaluationVisitor(context);

            // Act& Assert
            var score = subject.Visit((RequestBodyMatcher) null);
            score.Should().Be(1);
        }

        [Fact]
        public void IsMatch_Should_Be_True_When_QueryString_IsMatch()
        {
            // Arrange
            var query = new QueryString("?id=100");
            var context = CreateContext(queryString: query);
            var subject = new RequestMatcherEvaluationVisitor(context);

            // Act& Assert
            var score = subject.Visit(new Query(query));
            score.Should().Be(1);
        }

        [Fact]
        public void IsMatch_Should_Be_True_When_QueryString_Not_Set()
        {
            // Arrange
            var context = CreateContext(queryString: new QueryString("?id=100"));
            var subject = new RequestMatcherEvaluationVisitor(context);

            // Act& Assert
            var score =  subject.Visit((Query) null);
            score.Should().Be(1);
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
            var subject = new RequestMatcherEvaluationVisitor(context);

            // Act& Assert
            var score = subject.Visit(bodyMatcher);
            score.Should().Be(1);
        }

          public class RequestBodyMatcherTests : XUnitTestBase<RequestBodyMatcherTests>
    {
        /// <inheritdoc />
        public RequestBodyMatcherTests(ITestOutputHelper output) : base(output)
        {
        }

        private HttpContext CreateContext([CanBeNull] string textContent = null, byte[] byteContent = null)
        {
            if (textContent != null)
            {
                byteContent = Encoding.UTF8.GetBytes(textContent);
            }

            if (byteContent != null)
            {
                var stream = new MemoryStream();
                stream.Write(byteContent);

                return new DefaultHttpContext
                {
                    Request =
                    {
                        Body = stream,
                        ContentLength = stream.Length
                    }
                };
            }

            return new DefaultHttpContext();
        }

        [NotNull]
        private RequestBodyMatcher CreateSubject(RequestBodyKind kind, bool matchExact, string content)
        {
            return new RequestBodyMatcher
            {
                Content = content,
                Kind = kind,
                MatchExact = matchExact
            };
        }

        [Fact]
        public void Should_Match_Exact_String_Body()
        {
            // Arrange
            const string content = "{\"hello\":\"world!\"}";
            var subject = CreateSubject(RequestBodyKind.Text, true, content);
            var context = CreateContext(content);
            var visitor = new RequestMatcherEvaluationVisitor(context);

            // Act
            var score = visitor.Visit(subject);

            // Assert
            score.Should().BeGreaterThan(0);
        }

        [Fact]
        public void Should_Match_String_Body_If_Contains_String()
        {
            // Arrange
            const string content = "{\"hello\":\"world!\"}";
            var subject = CreateSubject(RequestBodyKind.Text, false, "world!\"}");
            var context = CreateContext(content);
            var visitor = new RequestMatcherEvaluationVisitor(context);

            // Act
            var score = visitor.Visit(subject);

            // Assert
            score.Should().BeGreaterThan(0);
        }

        [Fact]
        public void Should_Not_Match_Exact_String_Body_When_Not_Exact_Same_String_Content()
        {
            // Arrange
            var subject = CreateSubject(RequestBodyKind.Text, true,  "{\"hello\":\"car!\"}");
            var context = CreateContext("{\"hello\":\"world!\"}");
            var visitor = new RequestMatcherEvaluationVisitor(context);

            // Act
            var score = visitor.Visit(subject);

            //  Assert
            score.Should().Be(0);
        }

        [Fact]
        public void Should_Not_Match_String_Body_If_Not_Contains_String()
        {
            // Arrange
            const string content = "{\"hello\":\"world!\"}";
            var subject = CreateSubject(RequestBodyKind.Text, false, "car!\"}");
            var context = CreateContext(content);
            var visitor = new RequestMatcherEvaluationVisitor(context);

            // Act
            var score = visitor.Visit(subject);

            // Assert
            score.Should().Be(0);
        }
    }
    }
}
