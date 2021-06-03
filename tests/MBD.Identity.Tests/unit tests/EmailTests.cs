using System;
using MBD.Identity.Domain.ValueObjects;
using Xunit;

namespace MBD.Identity.Tests.unit_tests
{
    public class EmailTests
    {
        private const string TRAIT_VALUE = "Value Object - Email";
        
        [Fact(DisplayName = "Criar e-mail inválido.")]
        [Trait("Email", TRAIT_VALUE)]
        public void InvalidEmail_NewEmail_ReturnArgumentExceptionOrArgumentNullException()
        {
            // Arrange
            var invalidEmail = "invalid_email@";
            var emptyEmail = string.Empty;
            var whiteSpaceEmail = " ";

            // Act && Assert
            Assert.Throws<ArgumentException>(() => new Email(invalidEmail));
            Assert.Throws<ArgumentNullException>(() => new Email(emptyEmail));
            Assert.Throws<ArgumentNullException>(() => new Email(whiteSpaceEmail));            
        }

        [Trait("Email", TRAIT_VALUE)]
        [Theory(DisplayName = "Criar e-mail válido")]
        [InlineData("gustavo@gmail.com")]
        [InlineData("GUSTAVO_ishizava@gmail.com")]
        [InlineData("gustavo_ishizava@gmail.com")]
        [InlineData("gustavo_ishizava20@gmail.com.br")]
        public void ValidEmail_NewEmail_ReturnSuccess(string validEmail)
        {
            // Assert
            var email = validEmail.ToLower();
            var normalizedEmail = email.ToUpper();

            // Act
            var emailObject = new Email(email);

            // Assert
            Assert.Equal(email, emailObject.Address);
            Assert.Equal(normalizedEmail, emailObject.NormalizedAddress);
        }
    }
}