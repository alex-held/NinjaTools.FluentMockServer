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

        private HttpContext CreateContext(string method = null, string path = null)
        {
            var context = new DefaultHttpContext();
            var request = context.Request;
            request.Method = method;
            request.Path = new PathString(path);

            return context;
        }

        private RequestMatcher CreateSubject(string method = null, string path = null)
        {
            return new RequestMatcher
            {
                Method = method,
                Path = path
            };
        }
    }
}
