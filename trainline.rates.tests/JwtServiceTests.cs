using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using trainline.rates.services;
using Xunit;

namespace trainline.rates.tests
{
    [ExcludeFromCodeCoverage]
    public class JwtServiceTests
    {
        private JwtService service;
        private Mock<IConfiguration> configMock;

        public JwtServiceTests()
        {
        }

        private void SetupConfig(string secret, string issuer)
        {
            configMock = new Mock<IConfiguration>();

            configMock.Setup(x =>
                x.GetSection(It.Is<string>(s => s.Equals("JwtConfig")))
                    .GetSection(It.Is<string>(s => s.Equals("secret"))).Value).Returns(secret);

            configMock.Setup(x =>
                x.GetSection(It.Is<string>(s => s.Equals("JwtConfig")))
                    .GetSection(It.Is<string>(s => s.Equals("Issuer"))).Value).Returns(issuer);

            service = new JwtService(configMock.Object);
        }

        // Create token sucessfully
        [Fact]
        public async Task CreateTokenSucessful()
        {
            // Arrange 
            SetupConfig("PDv7DrqznYL6nv7DrqzjnQYO9JxIsWdcjnQYL6nu0f", "localhost");

            // Act
            var token = await service.GenerateSecurityToken();

            // Assert
            Assert.True(!string.IsNullOrWhiteSpace(token));
        }

        // Failed to create token - issuer param is missing
        [Fact]
        public async Task CreateTokenFailed_MissingIssuerParam()
        {
            // Arrange
            SetupConfig("PDv7DrqznYL6nv7DrqzjnQYO9JxIsWdcjnQYL6nu0f", null);

            // Act
            var token = await service.GenerateSecurityToken();

            // Assert
            Assert.True(string.IsNullOrWhiteSpace(token));
        }

        // Failed to create token - secret param is missing
        [Fact]
        public async Task CreateTokenFailed_MissingSecretParam()
        {
            // Arrange
            SetupConfig(null, "localhost");

            // Act
            var token = await service.GenerateSecurityToken();

            // Assert
            Assert.True(string.IsNullOrWhiteSpace(token));
        }
    }
}
