using System;
using MBD.Identity.Domain.Interfaces.Services;
using MBD.Identity.Infrastructure.Services;
using Xunit;

namespace MBD.Identity.Tests.unit_tests.Services
{
    public class HashServiceTest
    {
        private readonly IHashService _hashService;

        public HashServiceTest()
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
    }
}