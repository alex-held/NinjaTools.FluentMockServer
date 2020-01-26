// using System.IO;
// using System.Text;
// using FluentAssertions;
// using JetBrains.Annotations;
// using Microsoft.AspNetCore.Http;
// using NinjaTools.FluentMockServer.API.Models;
// using NinjaTools.FluentMockServer.Tests.TestHelpers;
// using Xunit;
// using Xunit.Abstractions;
//
// namespace NinjaTools.FluentMockServer.API.Tests.Models
// {
//     public class RequestBodyMatcherTests : XUnitTestBase<RequestBodyMatcherTests>
//     {
//         /// <inheritdoc />
//         public RequestBodyMatcherTests(ITestOutputHelper output) : base(output)
//         {
//         }
//
//         private HttpContext CreateContext([CanBeNull] string textContent = null, byte[] byteContent = null)
//         {
//             if (textContent != null)
//             {
//                 byteContent = Encoding.UTF8.GetBytes(textContent);
//             }
//
//             if (byteContent != null)
//             {
//                 var stream = new MemoryStream();
//                 stream.Write(byteContent);
//
//                 return new DefaultHttpContext
//                 {
//                     Request =
//                     {
//                         Body = stream,
//                         ContentLength = stream.Length
//                     }
//                 };
//             }
//
//             return new DefaultHttpContext();
//         }
//
//         [NotNull]
//         private RequestBodyMatcher CreateSubject(RequestBodyKind type, bool matchExact, string content)
//         {
//             return new RequestBodyMatcher
//             {
//                 Content = content,
//                 Type = type,
//                 MatchExact = matchExact
//             };
//         }
//
//         [Fact]
//         public void Should_Match_Exact_String_Body()
//         {
//             // Arrange
//             const string content = "{\"hello\":\"world!\"}";
//             var subject = CreateSubject(RequestBodyKind.Text, true, content);
//             var context = CreateContext(content);
//
//             // Act & Assert
//             subject.IsMatch(context.Request).Should().BeTrue();
//         }
//
//         [Fact]
//         public void Should_Match_String_Body_If_Contains_String()
//         {
//             // Arrange
//             const string content = "{\"hello\":\"world!\"}";
//             var subject = CreateSubject(RequestBodyKind.Text, false, "world!\"}");
//             var context = CreateContext(content);
//
//             // Act & Assert
//             subject.IsMatch(context.Request).Should().BeTrue();
//         }
//
//         [Fact]
//         public void Should_Not_Match_Exact_String_Body_When_Not_Exact_Same_String_Content()
//         {
//             // Arrange
//             var subject = CreateSubject(RequestBodyKind.Text, true,  "{\"hello\":\"car!\"}");
//             var context = CreateContext("{\"hello\":\"world!\"}");
//
//             // Act & Assert
//             subject.IsMatch(context.Request).Should().BeFalse();
//         }
//
//         [Fact]
//         public void Should_Not_Match_String_Body_If_Not_Contains_String()
//         {
//             // Arrange
//             const string content = "{\"hello\":\"world!\"}";
//             var subject = CreateSubject(RequestBodyKind.Text, false, "car!\"}");
//             var context = CreateContext(content);
//
//             // Act & Assert
//             subject.IsMatch(context.Request).Should().BeFalse();
//         }
//     }
// }
