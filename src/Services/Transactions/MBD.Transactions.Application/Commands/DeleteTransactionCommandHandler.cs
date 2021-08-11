using System.Threading;
using System.Threading.Tasks;
using MBD.Application.Core.Response;
using MBD.Core.Data;
using MBD.Transactions.Domain.Events;
using MBD.Transactions.Domain.Interfaces.Repositories;
using MediatR;

namespace MBD.Transactions.Application.Commands
{
    public class DeleteTransactionCommandHandler : IRequestHandler<DeleteTransactionCommand, IResult>
    {
        private readonly ITransactionRepository _repository;
        private readonly IUnitOfWork _unitOfwork;

        public DeleteTransactionCommandHandler(ITransactionRepository repository, IUnitOfWork unitOfwork)
        {
            _repository = repository;
            _unitOfwork = unitOfwork;
        }

        public async Task<IResult> Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
        {
            var validation = request.Validate();
            if (!validation.IsValid)
                return Result.Fail(validation.ToString());

            var transaction = await _repository.GetByIdAsync(request.Id);
            if (transaction is null)
                return Result.Fail("Transação inválida.");

            transaction.AddDomainEvent(new TransactionDeletedDomainEvent(transaction.Id));
            _repository.Remove(transaction);
            await _unitOfwork.SaveChangesAsync();

            return Result.Success();
        }
    }
}