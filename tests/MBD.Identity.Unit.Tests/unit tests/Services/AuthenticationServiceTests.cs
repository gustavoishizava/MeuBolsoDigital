using System;
using System.Text;
using System.Threading.Tasks;
using MBD.Core.Data;
using MBD.Core.DomainObjects;
using MBD.Identity.Domain.Configuration;
using MBD.Identity.Domain.Entities;
using MBD.Identity.Domain.Interfaces.Repositories;
using MBD.Identity.Domain.Interfaces.Services;
using MBD.Identity.Domain.Services;
using MBD.Identity.Infrastructure.Services;
using Microsoft.Extensions.Options;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace MBD.Identity.Unit.Tests.unit_tests.Services
{
    public class AuthenticationServiceTests
    {
        private readonly AutoMocker _mocker;
        private readonly AuthenticationService _authenticationService;
        private readonly User _existingUser;
        private readonly JwtConfiguration _jwtConfiguration;
        private readonly string _validEmail;
        private readonly string _validPassword;

        public AuthenticationServiceTests()
        {
            var secret = Guid.NewGuid();
            _mocker = new AutoMocker();
            _mocker.Use<IHashService>(new HashService());
            _mocker.Use<IJwtService>(new JwtService(new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret.ToString()))));
            _mocker.Use<IOptions<JwtConfiguration>>(
                Options.Create(
                    _jwtConfiguration = new JwtConfiguration { Secret = secret.ToString(), Audience = "TEST", Issuer = "TEST", ExpiresInSeconds = 10, RefreshExpiresInSeconds = 15 }));
            _authenticationService = _mocker.CreateInstance<AuthenticationService>();

            _validEmail = "gustavo@gmail.com";
            _validPassword = "P@ssw0rd!";
            _existingUser = new User("Valid user", _validEmail, _validPassword, new HashService());
        }

        [Fact(DisplayName = "Autenticar com e-mail e senha válido.")]
        public async Task ValidEmail_Authenticate_ReturnSuccess()
        {
            // Arrange
            var currentDate = DateTime.Now;
            _mocker.GetMock<IUserRepository>().Setup(method => method.GetByEmailAsync(_validEmail)).ReturnsAsync(_existingUser);

            // Act
            var accessTokenResponse = await _authenticationService.AuthenticateAsync(_validEmail, _validPassword);

            // Assert
            _mocker.GetMock<IUserRepository>().Verify(repository => repository.GetByEmailAsync(_validEmail), Times.Once);
            _mocker.GetMock<IUserRepository>().Verify(repository => repository.AddRefreshToken(It.IsAny<RefreshToken>()), Times.Once);
            _mocker.GetMock<IUnitOfWork>().Verify(repository => repository.SaveChangesAsync(), Times.Once);

            Assert.False(accessTokenResponse.HasErrors);
            Assert.NotEmpty(accessTokenResponse.AccessToken);
            Assert.NotEmpty(accessTokenResponse.RefreshToken);
            Assert.True(accessTokenResponse.CreatedAt >= currentDate);
            Assert.Equal(_jwtConfiguration.ExpiresInSeconds, accessTokenResponse.ExpiresIn);
        }

        [Theory(DisplayName = "Autenticar com e-mail e/ou senha inválido(s) deve retornar erro.")]
        [InlineData("email@email.com", "P@ssw0rd!")]
        [InlineData("gustavo@gmail.com", "invalidpassword")]
        [InlineData("email@email.com", "invalidpassword")]
        [InlineData("", "")]
        public async Task InvalidEmailOrPassword_Authenticate_RetrunError(string email, string password)
        {
            // Arrange
            _mocker.GetMock<IUserRepository>().Setup(method => method.GetByEmailAsync(_validEmail)).ReturnsAsync(_existingUser);

            // Act
            var accessTokenResponse = await _authenticationService.AuthenticateAsync(email, password);

            // Assert
            _mocker.GetMock<IUserRepository>().Verify(repository => repository.GetByEmailAsync(email), string.IsNullOrEmpty(email) ? Times.Never : Times.Once);
            _mocker.GetMock<IUserRepository>().Verify(repository => repository.AddRefreshToken(It.IsAny<RefreshToken>()), Times.Never);
            _mocker.GetMock<IUnitOfWork>().Verify(repository => repository.SaveChangesAsync(), Times.Never);

            Assert.True(accessTokenResponse.HasErrors);
        }

        [Fact(DisplayName = "Renovar token com refresh token inválido.")]
        public async Task InvalidToken_RefreshToken_ReturnError()
        {
            // Arrange
            var validRefreshToken = _existingUser.CreateRefreshToken(3600);
            var expiredRefreshToken = _existingUser.CreateRefreshToken(0);
            var revokedRefreshToken = _existingUser.CreateRefreshToken(3600);
            revokedRefreshToken.Revoke();

            _mocker.GetMock<IUserRepository>().Setup(method => method.GetByIdAsync(_existingUser.Id)).ReturnsAsync(_existingUser);
            _mocker.GetMock<IUserRepository>().Setup(method => method.GetRefreshTokenByToken(validRefreshToken.Token)).ReturnsAsync(validRefreshToken);
            _mocker.GetMock<IUserRepository>().Setup(method => method.GetRefreshTokenByToken(expiredRefreshToken.Token)).ReturnsAsync(expiredRefreshToken);
            _mocker.GetMock<IUserRepository>().Setup(method => method.GetRefreshTokenByToken(revokedRefreshToken.Token)).ReturnsAsync(revokedRefreshToken);

            // Act
            var emptyGuidAuthenticationResponse = await _authenticationService.AuthenticateByRefreshTokenAsync(Guid.Empty);
            var invalidGuidAuthenticationResponse = await _authenticationService.AuthenticateByRefreshTokenAsync(Guid.NewGuid());
            var expiredRefreshTokenAuthenticationResponse = await _authenticationService.AuthenticateByRefreshTokenAsync(expiredRefreshToken.Token);
            var revokedRefreshTokenAuthenticationResponse = await _authenticationService.AuthenticateByRefreshTokenAsync(revokedRefreshToken.Token);

            // Assert
            _mocker.GetMock<IUnitOfWork>().Verify(repository => repository.SaveChangesAsync(), Times.Never);

            Assert.True(emptyGuidAuthenticationResponse.HasErrors);
            Assert.True(invalidGuidAuthenticationResponse.HasErrors);
            Assert.True(revokedRefreshTokenAuthenticationResponse.HasErrors);
            Assert.True(expiredRefreshTokenAuthenticationResponse.HasErrors);
        }

        [Fact(DisplayName = "Tentativa de gerar um token com um refresh token válido mas com usuário inexistente.")]
        public async Task InvalidUser_RefreshToken_ReturnDomainException()
        {
            // Arrange
            var validRefreshToken = _existingUser.CreateRefreshToken(3600);

            _mocker.GetMock<IUserRepository>().Setup(method => method.GetByIdAsync(_existingUser.Id)).ReturnsAsync((User)null);
            _mocker.GetMock<IUserRepository>().Setup(method => method.GetRefreshTokenByToken(validRefreshToken.Token)).ReturnsAsync(validRefreshToken);

            // Act && Assert
            await Assert.ThrowsAsync<DomainException>(() => _authenticationService.AuthenticateByRefreshTokenAsync(validRefreshToken.Token));
        }

        [Fact(DisplayName = "Renovar token com refresh token válido.")]
        public async Task ValidToken_RefreshToken_ReturnSuccess()
        {
            // Arrange
            var validRefreshToken = _existingUser.CreateRefreshToken(3600);

            _mocker.GetMock<IUserRepository>().Setup(method => method.GetByIdAsync(_existingUser.Id)).ReturnsAsync(_existingUser);
            _mocker.GetMock<IUserRepository>().Setup(method => method.GetRefreshTokenByToken(validRefreshToken.Token)).ReturnsAsync(validRefreshToken);

            // Act
            var refreshTokenAuthenticationResponse = await _authenticationService.AuthenticateByRefreshTokenAsync(validRefreshToken.Token);

            // Assert
            _mocker.GetMock<IUnitOfWork>().Verify(repository => repository.SaveChangesAsync(), Times.Once);

            Assert.False(refreshTokenAuthenticationResponse.HasErrors);
        }
    }
}