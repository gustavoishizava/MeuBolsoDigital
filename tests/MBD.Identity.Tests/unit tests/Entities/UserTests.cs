using System;
using System.Threading;
using MBD.Core.DomainObjects;
using MBD.Identity.Domain.Entities;
using MBD.Identity.Domain.Interfaces.Services;
using MBD.Identity.Infrastructure.Services;
using Xunit;

namespace MBD.Identity.Tests.unit_tests.Entities
{
    public class UserTests
    {
        private readonly IHashService _hashService;

        private readonly User _validUser;

        private const string TRAIT_VALUE = "Entities - Users";

        public UserTests()
        {
            _hashService = new HashService();

            _validUser = new User("Valid user", "user@user.com", "P@ssw0rd!", _hashService);
        }

        [Fact(DisplayName = "Criar usuário com e-mail inválido.")]
        [Trait("User", TRAIT_VALUE)]
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

        [Theory(DisplayName = "Criar usuário com e-mail válido.")]
        [Trait("User", TRAIT_VALUE)]
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
        [Trait("Refresh token", TRAIT_VALUE)]
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
        [Trait("Refresh token", TRAIT_VALUE)]
        public void ValidRefreshToken_Revoke_ReturnSucess()
        {
            // Assert
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
        }

        [Fact(DisplayName = "Revogar um token já revogado não deve alterar a data de revogação novamente.")]
        [Trait("Refresh token", TRAIT_VALUE)]
        public void RevokedRefreshToken_Revoke_NothingChanges()
        {
            // Assert
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