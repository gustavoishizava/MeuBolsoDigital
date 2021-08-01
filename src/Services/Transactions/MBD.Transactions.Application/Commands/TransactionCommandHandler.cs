using System.Threading;
using System.Threading.Tasks;
using MBD.Application.Core.Response;
using MBD.Core.Data;
using MBD.Core.Identity;
using MBD.Transactions.Application.Response;
using MBD.Transactions.Domain.Entities;
using MBD.Transactions.Domain.Interfaces.Repositories;
using MediatR;

namespace MBD.Transactions.Application.Commands
{
    public class TransactionCommandHandler : IRequestHandler<CreateTransactionCommand, IResult<TransactionResponse>>
    {
        private readonly ITransactionRepository _repository;
        private readonly IAspNetUser _aspNetUser;
        private readonly IUnitOfWork _unitOfWork;

        public TransactionCommandHandler(ITransactionRepository repository, IAspNetUser aspNetUser, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _aspNetUser = aspNetUser;
            _unitOfWork = unitOfWork;
        }

        public async Task<IResult<TransactionResponse>> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            var validation = request.Validate();
            if (!validation.IsValid)
                return Result<TransactionResponse>.Fail();

            var transaction = new Transaction(
                _aspNetUser.UserId,
                request.BankAccountId,
                request.CategoryId,
                request.ReferenceDate,
                request.DueDate,
                request.Value,
                request.Description
            );

            _repository.Add(transaction);
            await _unitOfWork.SaveChangesAsync();

            return Result<TransactionResponse>.Success(new TransactionResponse(transaction));
        }
    }
}