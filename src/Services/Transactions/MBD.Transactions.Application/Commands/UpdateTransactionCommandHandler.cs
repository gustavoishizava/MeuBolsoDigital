using System.Threading;
using System.Threading.Tasks;
using MBD.Application.Core.Response;
using MBD.Core.Data;
using MBD.Core.Identity;
using MBD.Transactions.Application.Response;
using MBD.Transactions.Domain.Interfaces.Repositories;
using MBD.Transactions.Domain.Interfaces.Services;
using MediatR;

namespace MBD.Transactions.Application.Commands
{
    public class UpdateTransactionCommandHandler : IRequestHandler<UpdateTransactionCommand, IResult>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAspNetUser _aspNetUser;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBankAccountService _bankAccountService;
        private readonly ICategoryRepository _categoryRepository;

        public UpdateTransactionCommandHandler(ITransactionRepository transactionRepository, IAspNetUser aspNetUser, IUnitOfWork unitOfWork, IBankAccountService bankAccountService, ICategoryRepository categoryRepository)
        {
            _transactionRepository = transactionRepository;
            _aspNetUser = aspNetUser;
            _unitOfWork = unitOfWork;
            _bankAccountService = bankAccountService;
            _categoryRepository = categoryRepository;
        }

        public async Task<IResult> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
        {
            var transaction = await _transactionRepository.GetByIdAsync(request.Id);
            if (transaction == null)
                return Result.Fail("Transação inválida.");

            var bankAccount = await _bankAccountService.GetByIdAsync(request.BankAccountId);
            if (bankAccount == null)
                return Result<TransactionResponse>.Fail("Conta bancária inválida.");

            var category = await _categoryRepository.GetByIdAsync(request.CategoryId);
            if (category == null)
                return Result<TransactionResponse>.Fail("Categoria inválida.");

            if (request.PaymentDate != null)
                transaction.Pay(request.PaymentDate.Value);
            else
                transaction.UndoPayment();

            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }
    }
}