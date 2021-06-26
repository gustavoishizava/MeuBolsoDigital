using System.Threading.Tasks;
using Bogus;
using MBD.Identity.Domain.Entities;
using MBD.Identity.Domain.Interfaces.Repositories;
using MBD.Identity.Domain.Services;
using MBD.Identity.Infrastructure.Services;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace MBD.Identity.Tests.unit_tests.Services
{
    public class CreateUserServiceTests
    {
        private readonly AutoMocker _mocker;
        private readonly Faker _faker;
        private readonly CreateUserService _createUserService;

        public CreateUserServiceTests()
        {
            _mocker = new AutoMocker();
            _faker = new Faker("pt_BR");
            _createUserService = _mocker.CreateInstance<CreateUserService>();
        }

        [Fact(DisplayName = "Criar novo usuário com e-mail ainda não cadastrado deve retornar sucesso.")]
        public async Task NewEmail_CreateUser_ReturnSuccess()
        {
            // Arrange
            _mocker.GetMock<IUserRepository>()
                .Setup(method => method.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);

            var email = _faker.Person.Email;
            var password = "Aaa123@458";
            var name = _faker.Person.FullName;

            // Act
            var createResult = await _createUserService.CreateAsync(name, email, password);

            // Assert
            Assert.True(createResult.IsSuccessful);
            Assert.Equal("Usuário criado com sucesso.", createResult.Message);

            _mocker.GetMock<IUserRepository>()
                .Verify(method => method.GetByEmailAsync(It.IsAny<string>()), Times.Once);
            
            _mocker.GetMock<IUserRepository>()
                .Verify(method => method.Add(It.IsAny<User>()), Times.Once);

            _mocker.GetMock<IUserRepository>()
                .Verify(method => method.SaveChangesAsync(), Times.Once);
        }

        [Fact(DisplayName = "Criar novo usuário com e-mail já cadastrado deve retornar falha.")]
        public async Task ExistingEmail_CreateUser_ReturnFail()
        {
            // Arrange            
            var email = "teste@gmail.com";
            var password = "Aaa123@458";
            var name = "New User";

            _mocker.GetMock<IUserRepository>()
                .Setup(method => method.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(new User(name, email, password, new HashService()));

            // Act
            var createResult = await _createUserService.CreateAsync(name, email, password);

            // Assert
            Assert.False(createResult.IsSuccessful);
            Assert.Equal($"Já existe um usuário com o e-mail '{email}'.", createResult.Message);

            _mocker.GetMock<IUserRepository>()
                .Verify(method => method.GetByEmailAsync(It.IsAny<string>()), Times.Once);
            
            _mocker.GetMock<IUserRepository>()
                .Verify(method => method.Add(It.IsAny<User>()), Times.Never);

            _mocker.GetMock<IUserRepository>()
                .Verify(method => method.SaveChangesAsync(), Times.Never);
        }
    }
}