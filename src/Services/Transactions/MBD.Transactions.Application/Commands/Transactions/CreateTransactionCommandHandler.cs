using System.Threading;
using System.Threading.Tasks;
using MBD.Application.Core.Response;
using MBD.Core.Data;
using MBD.Core.Identity;
using MBD.Transactions.Application.Response;
using MBD.Transactions.Domain.Entities;
using MBD.Transactions.Domain.Interfaces.Repositories;
using MediatR;

namespace MBD.Transactions.Application.Commands.Transactions
{
    public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, IResult<TransactionResponse>>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAspNetUser _aspNetUser;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBankAccountRepository _bankAccountRepository;

        public CreateTransactionCommandHandler(ITransactionRepository transactionRepository, IAspNetUser aspNetUser, IUnitOfWork unitOfWork, ICategoryRepository categoryRepository, IBankAccountRepository bankAccountRepository)
        {
            _transactionRepository = transactionRepository;
            _aspNetUser = aspNetUser;
            _unitOfWork = unitOfWork;
            _categoryRepository = categoryRepository;
            _bankAccountRepository = bankAccountRepository;
        }

        public async Task<IResult<TransactionResponse>> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            var validation = request.Validate();
            if (!validation.IsValid)
                return Result<TransactionResponse>.Fail();

            var bankAccount = await _bankAccountRepository.GetByIdAsync(request.BankAccountId);
            if (bankAccount == null)
                return Result<TransactionResponse>.Fail("Conta bancária inválida.");

            var category = await _categoryRepository.GetByIdAsync(request.CategoryId);
            if (category == null)
                return Result<TransactionResponse>.Fail("Categoria inválida.");

            var transaction = new Transaction(
                _aspNetUser.UserId,
                bankAccount,
                category,
                request.ReferenceDate,
                request.DueDate,
                request.Value,
                request.Description,
                request.PaymentDate
            );

            _transactionRepository.Add(transaction);
            await _unitOfWork.SaveChangesAsync();

            return Result<TransactionResponse>.Success(new TransactionResponse(transaction));
        }
    }
}