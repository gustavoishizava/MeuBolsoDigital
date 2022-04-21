using System;
using System.Threading;
using MBD.Core.DomainObjects;
using MBD.Identity.Domain.Entities;
using MBD.Identity.Domain.Interfaces.Services;
using MBD.Identity.Infrastructure.Services;
using Xunit;

namespace MBD.Identity.Unit.Tests.unit_tests.Entities
{
    public class UserTests
    {
        private readonly IHashService _hashService;

        private readonly User _validUser;

        public UserTests()
        {
            _hashService = new HashService();

            _validUser = new User("Valid user", "user@user.com", "P@ssw0rd!", _hashService);
        }

        [Fact(DisplayName = "Criar usuário inválido.")]
        public void InvalidUser_NewUser_ReturnArgumentExceptionDomainException()
        {
            // Arrange && Act && Assert
            Assert.Throws<ArgumentException>(() =>
                new User("User", "invalid_email", "Abc@122357#", _hashService));

            Assert.Throws<DomainException>(() =>
                new User("User", "email@email.com", "password", _hashService));

            Assert.Throws<DomainException>(() =>
                new User("", "email@email.com", "password", _hashService));
        }

        [Theory(DisplayName = "Criar usuário válido.")]
        [InlineData("Gustavo", "gustavo@gmail.com", "P@ssw0rd!")]
        [InlineData("Eduarda", "eduarda@hotmail.com", "S3nh@4#5a")]
        [InlineData("Vitória", "vitoria_email@hotmail.com.br", "Senh@4#5a")]
        public void ValidEmail_NewUser_ReturnSuccess(string name, string email, string password)
        {
            // Arrange
            var normalizedEmail = email.ToUpper();

            // Act
            var user = new User(name, email, password, _hashService);

            // Assert
            Assert.Equal(name, user.Name);
            Assert.Equal(email, user.Email.Address);
            Assert.Equal(normalizedEmail, user.Email.NormalizedAddress);
            Assert.True(_hashService.IsMatch(password, user.Password.PasswordHash));
        }

        [Fact(DisplayName = "Gerar um refresh token para um usuário válido.")]
        public void ValidUser_CreateRefreshToken_ReturnSuccess()
        {
            // Arrange
            int expiresIn = 3600;

            // Act
            var refreshToken = _validUser.CreateRefreshToken(expiresIn);

            // Assert
            Assert.Equal(refreshToken.UserId, _validUser.Id);
            Assert.Equal(refreshToken.CreatedAt.AddSeconds(expiresIn), refreshToken.ExpiresAt);
            Assert.False(refreshToken.IsExpired);
        }

        [Fact(DisplayName = "Revogar token válido.")]
        public void ValidRefreshToken_Revoke_ReturnSucess()
        {
            // Arrange
            var refreshToken = _validUser.CreateRefreshToken(3600);
            var isRevoked = refreshToken.IsRevoked;
            var revokedOn = refreshToken.RevokedOn;

            // Act
            refreshToken.Revoke();

            // Assert
            Assert.NotNull(refreshToken.RevokedOn);
            Assert.NotEqual(revokedOn, refreshToken.RevokedOn);
            Assert.True(refreshToken.IsRevoked);
            Assert.NotEqual(isRevoked, refreshToken.IsRevoked);
            Assert.False(refreshToken.IsValid);
        }

        [Fact(DisplayName = "Revogar um token já revogado não deve alterar a data de revogação novamente.")]
        public void RevokedRefreshToken_Revoke_NothingChanges()
        {
            // Arrange
            var refreshToken = _validUser.CreateRefreshToken(3600);
            refreshToken.Revoke();
            var revokedOn = refreshToken.RevokedOn;

            // Act
            Thread.Sleep(1000);
            refreshToken.Revoke();

            // Assert
            Assert.Equal(revokedOn, refreshToken.RevokedOn);
        }
    }
}