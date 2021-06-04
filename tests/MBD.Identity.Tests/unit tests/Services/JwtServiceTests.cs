using System.Collections.Generic;
using System;
using System.Text;
using MBD.Identity.Domain.Interfaces.Services;
using MBD.Identity.Infrastructure.Services;
using Microsoft.IdentityModel.Tokens;
using Xunit;
using System.Security.Claims;

namespace MBD.Identity.Tests.unit_tests.Services
{
    public class JwtServiceTests
    {
        private readonly IJwtService _jwtService;

        public JwtServiceTests()
        {
            _jwtService = new JwtService(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString())));
        }

        [Fact(DisplayName = "Gerar novo JWT.")]
        [Trait("Services", "JWT")]
        public void ValidData_NewToken_ReturnSuccess()
        {
            // Arrange
            var issuer = "Service";
            var audience = "Test";
            var dateNow = DateTime.Now;
            var expiresAt = dateNow.AddHours(1);

            // Act
            var jwtGenerated = _jwtService.Generate(issuer, audience, DateTime.Now, expiresAt, new List<Claim>());

            // Assert
            Assert.NotEmpty(jwtGenerated);
            Assert.NotNull(jwtGenerated);
        }
    }
}