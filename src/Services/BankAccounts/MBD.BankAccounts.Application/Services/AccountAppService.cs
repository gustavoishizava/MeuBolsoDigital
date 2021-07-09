using System.Threading.Tasks;
using MBD.Application.Core.Response;
using MBD.BankAccounts.Application.Interfaces;
using MBD.BankAccounts.Application.Request;
using MBD.BankAccounts.Domain.Entities;
using MBD.BankAccounts.Domain.Interfaces.Repositories;
using MBD.Core.Identity;

namespace MBD.BankAccounts.Application.Services
{
    public class AccountAppService : IAccountAppService
    {
        private readonly IAspNetUser _aspNetUser;
        private readonly IAccountRepository _repository;

        public AccountAppService(IAspNetUser aspNetUser, IAccountRepository repository)
        {
            _aspNetUser = aspNetUser;
            _repository = repository;
        }

        public async Task<IResult> CreateAsync(CreateAccountRequest request)
        {
            var validation = request.Validate();
            if (!validation.IsValid)
                return Result.Fail(validation.ToString());

            var account = new Account(_aspNetUser.UserId, request.Description, request.InitialBalance, request.Type);

            _repository.Add(account);
            await _repository.SaveChangesAsync();

            return Result.Success("Conta bancária cadastrada com sucesso.");
        }
    }
}