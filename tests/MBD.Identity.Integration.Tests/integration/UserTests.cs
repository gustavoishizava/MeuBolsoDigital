using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Bogus;
using MBD.Identity.API;
using MBD.Identity.Application.Requests;
using MBD.Identity.Integration.Tests.integration.Settings;
using Xunit;

namespace MBD.Identity.Integration.Tests.integration
{
    [Collection(nameof(IntegrationTestsFixtureCollection))]
    public class UserTests
    {
        private readonly IntegrationTestsFixture<StartupTests> _testsFixture;
        private readonly Faker _faker;

        public UserTests(IntegrationTestsFixture<StartupTests> testsFixture)
        {
            _testsFixture = testsFixture;
            _faker = new Faker("pt_BR");
        }

        [Fact(DisplayName = "Criar novo usuário com dados válidos deve retornar sucesso.")]
        public async Task ValidData_CreateNewUser_ReturnSuccess()
        {
            // Arrange
            var createUserRequest = new CreateUserRequest
            {
                Name = _faker.Person.FullName,
                Email = _faker.Internet.Email(),
                Password = "Test#@1234",
                RepeatPassword = "Test#@1234"
            };

            // Act
            var response = await _testsFixture._client.PostAsJsonAsync("api/users", createUserRequest);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Theory(DisplayName = "Criar novo usuário com dados inválidos deve retornar erro.")]
        [InlineData("", "", "", "")]
        [InlineData("Name", "email", "1234", "1457")]
        [InlineData("Name", "email@email.com", "Test3@1234", "Test3@1")]
        public async Task InvalidData_CreateNewUser_ReturnError(string name, string email, string password, string repeatPassword)
        {
            // Arrange
            var createUserRequest = new CreateUserRequest
            {
                Name = name,
                Email = email,
                Password = password,
                RepeatPassword = repeatPassword
            };

            // Act
            var response = await _testsFixture._client.PostAsJsonAsync("api/users", createUserRequest);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}