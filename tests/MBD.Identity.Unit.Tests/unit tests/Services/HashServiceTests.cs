using System;
using MBD.Identity.Domain.Interfaces.Services;
using MBD.Identity.Infrastructure.Services;
using Xunit;

namespace MBD.Identity.Tests.unit_tests.Services
{
    public class HashServiceTests
    {
        private readonly IHashService _hashService;

        public HashServiceTests()
        {
            _hashService = new HashService();
        }

        [Theory(DisplayName = "Criar hash de input válido.")]
        [InlineData("aspnet")]
        [InlineData("aspnetcore")]
        [InlineData("aspnetmvc")]
        [InlineData("dotnet")]
        [InlineData("P@ssw0rd!")]
        public void VallidInput_GenerateHash_ReturnValidHash(string validInput)
        {
            // Arrange & Act
            var hashGenerated = _hashService.Create(validInput);

            // Assert
            Assert.NotNull(hashGenerated);
            Assert.NotEmpty(hashGenerated);
            Assert.True(_hashService.IsMatch(validInput, hashGenerated));
        }

        [Theory(DisplayName = "Criar hash de input inválido.")]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void InvalidInput_GenerateHash_ReturnException(string invalidInput)
        {
            // Arrange & Act && Assert
            Assert.Throws<ArgumentException>(() => _hashService.Create(invalidInput));
        }

        [Fact(DisplayName = "Verificar input e hash inválido.")]
        public void InvalidInputAndHash_VerifyMatch_ReturnException()
        {
            // Arrange & Act && Assert
            Assert.Throws<ArgumentException>(() => _hashService.IsMatch(string.Empty, "hash"));
            Assert.Throws<ArgumentException>(() => _hashService.IsMatch("input", string.Empty));
            Assert.Throws<ArgumentException>(() => _hashService.IsMatch(string.Empty, string.Empty));
        }
    }
}