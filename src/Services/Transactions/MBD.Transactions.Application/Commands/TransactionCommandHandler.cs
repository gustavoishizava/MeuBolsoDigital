using System.Threading;
using System.Threading.Tasks;
using MBD.Application.Core.Response;
using MBD.Core.Data;
using MBD.Core.Identity;
using MBD.Transactions.Application.Response;
using MBD.Transactions.Domain.Entities;
using MBD.Transactions.Domain.Interfaces.Repositories;
using MBD.Transactions.Domain.Interfaces.Services;
using MediatR;

namespace MBD.Transactions.Application.Commands
{
    public class TransactionCommandHandler : IRequestHandler<CreateTransactionCommand, IResult<TransactionResponse>>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAspNetUser _aspNetUser;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBankAccountService _bankAccountService;
        private readonly ICategoryRepository _categoryRepository;

        public TransactionCommandHandler(ITransactionRepository transactionRepository, IAspNetUser aspNetUser, IUnitOfWork unitOfWork, IBankAccountService bankAccountService, ICategoryRepository categoryRepository)
        {
            _transactionRepository = transactionRepository;
            _aspNetUser = aspNetUser;
            _unitOfWork = unitOfWork;
            _bankAccountService = bankAccountService;
            _categoryRepository = categoryRepository;
        }

        public async Task<IResult<TransactionResponse>> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            var validation = request.Validate();
            if (!validation.IsValid)
                return Result<TransactionResponse>.Fail();

            var bankAccount = await _bankAccountService.GetByIdAsync(request.BankAccountId);
            if (bankAccount == null)
                return Result<TransactionResponse>.Fail("Conta bancária inválida.");

            var category = await _categoryRepository.GetByIdAsync(request.CategoryId);
            if (category == null)
                return Result<TransactionResponse>.Fail("Categoria inválida.");

            var transaction = new Transaction(
                _aspNetUser.UserId,
                bankAccount.Id,
                category.Id,
                request.ReferenceDate,
                request.DueDate,
                request.Value,
                request.Description
            );

            _transactionRepository.Add(transaction);
            await _unitOfWork.SaveChangesAsync();

            return Result<TransactionResponse>.Success(new TransactionResponse(transaction));
        }
    }
}