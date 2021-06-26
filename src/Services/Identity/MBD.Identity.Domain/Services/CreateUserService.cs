using System.Threading.Tasks;
using MBD.Identity.Domain.Entities;
using MBD.Identity.Domain.Interfaces.Repositories;
using MBD.Identity.Domain.Interfaces.Services;
using MBD.Identity.Domain.ValueObjects;
using static MBD.Identity.Domain.ValueObjects.CreateUserResult;

namespace MBD.Identity.Domain.Services
{
    public class CreateUserService : ICreateUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHashService _hashService;

        public CreateUserService(IUserRepository userRepository, IHashService hashService)
        {
            _userRepository = userRepository;
            _hashService = hashService;
        }

        public async Task<CreateUserResult> CreateAsync(string name, string email, string password)
        {
            var user = new User(name, email, password, _hashService);
            
            if (!await CheckIfEmailIsAvailableAsync(user.Email))
                return CreateUserResultFactory.Fail($"Já existe um usuário com o e-mail '{email}'.");

            _userRepository.Add(user);
            await _userRepository.SaveChangesAsync();

            return CreateUserResultFactory.Success("Usuário criado com sucesso.");
        }

        private async Task<bool> CheckIfEmailIsAvailableAsync(Email email)
        {
            var existingUser = await _userRepository.GetByEmailAsync(email.Address);
            return existingUser == null;
        }
    }
}