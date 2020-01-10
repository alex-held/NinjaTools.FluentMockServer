using System.Collections.Generic;
using System.IO;
using System.Text;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using NinjaTools.FluentMockServer.API.Models;
using Xunit;

namespace NinjaTools.FluentMockServer.API.Tests.Models
{
    public class RequestMatcherTests
    {
        [Theory]
         [InlineData("GET")]
         [InlineData("POST")]
         [InlineData("PUT")]
         [InlineData("OPTIONS")]
        public void When_IsMatch_Returns_True_When_Method_Is_Equal(string  method)
        {
            // Arrange
            var subject = CreateSubject(method: method);
            var context = CreateContext(method);
            
            // Act& Assert
            subject.IsMatch(context).Should().BeTrue();
        }

        [Theory]
         [InlineData("/some(abc/path")]
         [InlineData("/some/abc/path")]
         [InlineData("/some/abc/path?query=json")]
         [InlineData("/opt")]
        public void When_IsMatch_Returns_True_When_Path_Is_Match(string  path)
        {
            // Arrange
            var subject = CreateSubject(path: path);
            var context = CreateContext(path: path);
            
            // Act& Assert
            subject.IsMatch(context).Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(GetHeaderTestData))]
        public void When_IsMatch_Returns_True_When_Headers_Are_Equal(Dictionary<string, string[]> request, Dictionary<string, string[]> contextHeaders, bool isValid)
        {
            // Arrange
            var subject = CreateSubject(headers: request);
            var context = CreateContext(headers: contextHeaders);

            // Act& Assert
            subject.IsMatch(context).Should().Be(isValid);
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

        private RequestMatcher CreateSubject(string method = null, string path = null, IDictionary<string, string[]> headers = null, RequestBodyMatcher bodyMatcher = null, QueryString? queryString = null)
         {
             return new RequestMatcher
             {
                 Method = method,
                 Path = path,
                 Headers = new Dictionary<string, string[]> (headers ?? new Dictionary<string, string[]>()),
                 BodyMatcher = bodyMatcher,
                 QueryString = queryString?.Value
             };
         }

        [Fact]
        public void IsMatch_Should_Be_False_When_QueryString_Not_Same()
        {
            // Arrange
            var subject = CreateSubject(queryString: new QueryString("?color=green"));
            var context = CreateContext(queryString: new QueryString("?id=100"));
            
            // Act & Assert
            subject.IsMatch(context).Should().BeFalse();
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
                Type = RequestBodyType.Text,
                MatchExact = true
            };
            
            var subject = CreateSubject(bodyMatcher: bodyMatcher );
            var context = CreateContext(requestBodyStream: requestBodyStream);
            
            // Act & Assert
            subject.IsMatch(context).Should().BeFalse();
        }


        [Fact]
        public void IsMatch_Should_Be_True_When_No_RequestBodyMatcher()
        {
            // Arrange
            var subject = CreateSubject();
            var context = CreateContext();
            
            // Act & Assert
            subject.IsMatch(context).Should().BeTrue();
        }

        [Fact]
        public void IsMatch_Should_Be_True_When_QueryString_IsMatch()
        {
            // Arrange
            var query = new QueryString("?id=100");
            var subject = CreateSubject(queryString: query);
            var context = CreateContext(queryString: query);
            
            // Act & Assert
            subject.IsMatch(context).Should().BeTrue();
        }

        [Fact]
        public void IsMatch_Should_Be_True_When_QueryString_Not_Set()
        {
            // Arrange
            var query = new QueryString("?id=100");
            var subject = CreateSubject();
            var context = CreateContext(queryString: query);
            
            // Act & Assert
            subject.IsMatch(context).Should().BeTrue();
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
              Type = RequestBodyType.Text,
              MatchExact = true
            };
            
            var subject = CreateSubject(bodyMatcher: bodyMatcher );
            var context = CreateContext(requestBodyStream: requestBodyStream);
            
            // Act & Assert
            subject.IsMatch(context).Should().BeTrue();
        }
    }
}
