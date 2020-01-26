using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Moq;
using NinjaTools.FluentMockServer.API.Infrastructure;
using NinjaTools.FluentMockServer.API.Models;
using NinjaTools.FluentMockServer.API.Services;
using Xunit;

namespace NinjaTools.FluentMockServer.API.Tests.Services
{
    public class SetupServiceTests
    {
        [NotNull]
        private SetupService CreateSubject([NotNull] MockSetupRepository repo)
        {
            return new SetupService(repo.Object);
        }


        [NotNull]
        private HttpContext CreateContext([CanBeNull] string path = null)
        {
            var context =  new DefaultHttpContext();
            if (path != null)
            {
                context.Request.Path = new PathString(path);
            }

            return context;
        }

        [Fact]
        public void Should_Add_Setup()
        {
            // Arrange
            var repo = new Mock<ISetupRepository>();
            var subject  = new SetupService(repo.Object);
            var setup = new Setup();
            
            // Act
            subject.Add(setup);
            
            // Assert
            repo.Verify(m => m.Add(setup), Times.Once);
        }


        [Fact]
        public void TryGetMatchingSetup_Should_Return_False_If_No_Matching_Setup_In_Repo()
        {
            // Arrange
            var repo = new MockSetupRepository()
                .SetupTryGetMatchingSetupReturnsNull();
            var subject = CreateSubject(repo);
            var context = CreateContext();
            
            // Act & Assert
            subject.TryGetMatchingSetup(context, out var _).Should().BeFalse();
        }

        [Fact]
        public void TryGetMatchingSetup_Should_Return_True_If_Matching_Setup_In_Repo()
        {
            // Arrange
            var setup = new Setup
            {
                Matcher = new RequestMatcher
                {
                    Path = new Path("/some/path")
                }
            };
            
            var repo = new MockSetupRepository()
                .SetupTryGetMatchingSetup(setup);
            var subject = CreateSubject(repo);
            var context = CreateContext();

            // Act & Assert
            subject.TryGetMatchingSetup(context, out var matchingSetup).Should().BeTrue();
            matchingSetup.Should().Be(setup);
        }
    }

    public class MockSetupRepository : Mock<ISetupRepository>
    {
        [NotNull]
        public MockSetupRepository SetupTryGetMatchingSetupReturnsNull()
        {
            Setup(m => m.TryGetMatchingSetup(It.IsAny<HttpContext>()))
                .Returns(null as Setup);
            return this;
        }

        [NotNull]
        public MockSetupRepository SetupTryGetMatchingSetup([CanBeNull] Setup setup)
        {
            Setup(m => m.TryGetMatchingSetup(It.IsAny<HttpContext>()))
                .Returns(setup);
            return this;
        }
    }
} 
