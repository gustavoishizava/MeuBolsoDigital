using System;
using System.Text;
using System.Threading.Tasks;
using MBD.Identity.Domain.DTO;
using MBD.Identity.Domain.Entities;
using MBD.Identity.Domain.Interfaces.Repositories;
using MBD.Identity.Domain.Interfaces.Services;
using MBD.Identity.Domain.Services;
using MBD.Identity.Infrastructure.Services;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace MBD.Identity.Tests.unit_tests.Services
{
    public class AuthenticationServiceTests
    {
        private readonly AutoMocker _mocker;
        private readonly AuthenticationService _authenticationService;

        public AuthenticationServiceTests()
        {
            var secret = Guid.NewGuid();
            _mocker = new AutoMocker();
            _mocker.Use<IHashService>(new HashService());
            _mocker.Use<IJwtService>(new JwtService(new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret.ToString()))));
            _mocker.Use<JwtConfiguration>(new JwtConfiguration { Secret = secret.ToString(), Audience = "TEST", Issuer = "TEST", ExpiresInSeconds = 10, RefreshExpiresInSeconds = 15 });
            _authenticationService = _mocker.CreateInstance<AuthenticationService>();
        }

        [Fact(DisplayName = "Autenticar com e-mail e senha válido.")]
        public async Task ValidEmail_Authenticate_ReturnSuccess()
        {
            // Arrange
            var email = "gustavo@gmail.com";
            var password = "P@ssw0rd!";
            var existingUser = new User("Valid user", email, password, new HashService());

            _mocker.GetMock<IUserRepository>().Setup(method => method.GetByEmailAsync(email)).ReturnsAsync(existingUser);

            // Act
            var authenticationResponse = await _authenticationService.AuthenticateAsync(email, password);

            // Assert
            _mocker.GetMock<IUserRepository>().Verify(repository => repository.GetByEmailAsync(email), Times.Once);
            Assert.False(authenticationResponse.HasErrors);
            Assert.NotEmpty(authenticationResponse.AccessToken);
            Assert.NotEmpty(authenticationResponse.RefreshToken);
        }

        [Theory(DisplayName = "Autenticar com e-mail e/ou senha inválido(s) deve retornar erro.")]
        [InlineData("email@email.com", "P@ssw0rd!")]
        [InlineData("gustavo@gmail.com", "invalidpassword")]
        [InlineData("email@email.com", "invalidpassword")]
        public async Task InvalidEmailOrPassword_Authenticate_RetrunError(string email, string password)
        {
            // Arrange
            var existingUser = new User("Valid user", "gustavo@gmail.com", "P@ssw0rd!", new HashService());

            _mocker.GetMock<IUserRepository>().Setup(method => method.GetByEmailAsync("gustavo@gmail.com")).ReturnsAsync(existingUser);

            // Act
            var authenticationResponse = await _authenticationService.AuthenticateAsync(email, password);

            // Assert
            _mocker.GetMock<IUserRepository>().Verify(repository => repository.GetByEmailAsync(email), Times.Once);
            Assert.True(authenticationResponse.HasErrors);
        }
    }
}