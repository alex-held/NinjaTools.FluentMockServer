using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NinjaTools.FluentMockServer.API.Models;
using Xunit;

namespace NinjaTools.FlAPIuentMockServer.API.Models
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
            var subject = CreateSubject(method);
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

         public static IEnumerable<object[]> GetHeaderTestData()
        {
            yield return new object[] { new Dictionary<string, string[]>(), new Dictionary<string, string[]>(), true };
            yield return new object[] {null, new Dictionary<string, string[]>() ,new Dictionary<string, string[]>(),  true};
            yield return new object[] { new Dictionary<string, string[]>
            {
                {
                    "Host", new []{"Mock-Server"}
                }
            }, new Dictionary<string, string[]>(), true 
            };
            yield return new object[] { new Dictionary<string, string[]>
            {
                {
                    "Host", new []{"Mock-Server"}
                }
            }, new Dictionary<string, string[]>
            {
                {
                    "Host", new []{"Mock-Server"}
                }
            }};
              yield return  new object[]
              {
                  new Dictionary<string, string[]>
                  {
                      {
                          "Some-Header", new []{true.ToString(), 123.ToString()}
                      }
                  },
                  new Dictionary<string, string[]>
                  {
                      {
                          "another.header", new []{"with some other value"}
                      }
                  } 
              };
        }


         private HttpContext CreateContext(string method = null, string path = null, Dictionary<string, string[]> headers = null)
        {
            var context = new DefaultHttpContext();
            var request = context.Request;
            request.Method = method;
            request.Path = new PathString(path);

            foreach (var header in headers ?? new Dictionary<string, string[]>())
            {
                request.Headers.Add(header.Key, header.Value);
            }
            
            return context;
        }

         private RequestMatcher CreateSubject(string method = null, string path = null, IDictionary<string, string[]> headers = null)
        {
            return new RequestMatcher
            {
                Method = method,
                Path = path,
                Headers = new Dictionary<string, string[]> (headers ?? new Dictionary<string, string[]>())
            };
        }
    }
}
