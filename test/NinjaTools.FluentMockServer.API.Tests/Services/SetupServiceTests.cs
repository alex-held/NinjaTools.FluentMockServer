using System;
using Moq;
using NinjaTools.FluentMockServer.API.Administration.setup;
using NinjaTools.FluentMockServer.API.Models;
using Xunit;

namespace NinjaTools.FluentMockServer.API.Tests.Services
{
    public class SetupServiceTests
    {
        [Fact]
        public void Should_Add_Setup()
        {
            // Arrange
            var repo = new Mock<ISetupRepository>();
            var subject  = new SetupService(repo.Object);
            var setup = new Setup(Guid.NewGuid());
            
            // Act
            subject.Add(setup);
            
            // Assert
            repo.Verify(m => m.Add(setup), Times.Once);
        }
    }
}
