using System.Threading;
using System.Threading.Tasks;
using MBD.Application.Core.Response;
using MBD.Core.Data;
using MBD.Core.Identity;
using MBD.Transactions.Application.Response;
using MBD.Transactions.Domain.Interfaces.Repositories;
using MediatR;

namespace MBD.Transactions.Application.Commands.Transactions
{
    public class UpdateTransactionCommandHandler : IRequestHandler<UpdateTransactionCommand, IResult>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAspNetUser _aspNetUser;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBankAccountRepository _bankAccountRepository;

        public UpdateTransactionCommandHandler(ITransactionRepository transactionRepository, IAspNetUser aspNetUser, IUnitOfWork unitOfWork, ICategoryRepository categoryRepository, IBankAccountRepository bankAccountRepository)
        {
            _transactionRepository = transactionRepository;
            _aspNetUser = aspNetUser;
            _unitOfWork = unitOfWork;
            _categoryRepository = categoryRepository;
            _bankAccountRepository = bankAccountRepository;
        }

        public async Task<IResult> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
        {
            var transaction = await _transactionRepository.GetByIdAsync(request.Id);
            if (transaction == null)
                return Result.Fail("Transação inválida.");

            var bankAccount = await _bankAccountRepository.GetByIdAsync(request.BankAccountId);
            if (bankAccount == null)
                return Result<TransactionResponse>.Fail("Conta bancária inválida.");

            var category = await _categoryRepository.GetByIdAsync(request.CategoryId);
            if (category == null)
                return Result<TransactionResponse>.Fail("Categoria inválida.");

            transaction.Update(bankAccount, category, request.ReferenceDate, request.DueDate, request.Value, request.Description, request.PaymentDate);

            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }
    }
}