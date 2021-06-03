using System;
using MBD.Core.DomainObjects;
using MBD.Identity.Domain.Entities;
using MBD.Identity.Domain.ValueObjects;
using Xunit;

namespace MBD.Identity.Tests.unit_tests
{
    public class UserTests
    {
        private const string TRAIT_VALUE = "Entities - Users";

        public UserTests()
        {
            
        }

        [Fact(DisplayName = "Criar usuário com e-mail inválido.")]
        [Trait("User", TRAIT_VALUE)]
        public void InvalidEmail_NewUser_ReturnDomainException()
        {
            // Arrange && Act && Assert
            Assert.Throws<ArgumentException>(() => 
                new User("User", "invalid_email", "password"));

            // Arrange && Act && Assert
            Assert.Throws<ArgumentNullException>(() => 
                new User("User", " ", "password"));
        }

        [Fact(DisplayName = "Criar usuário com e-mail válido.")]
        [Trait("User", TRAIT_VALUE)]
        public void ValidEmail_NewUser_ReturnSuccess()
        {
            // Assert
            var email = "gustavo@gmail.com".ToLower();
            var normalizedEmail = email.ToUpper();

            // Act
            var user = new User("Gustavo", email, "password");

            // Assert
            Assert.Equal(email, user.Email.Address);
            Assert.Equal(normalizedEmail, user.Email.NormalizedAddress);
        }
    }
}