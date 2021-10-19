using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MBD.Application.Core.Response;
using MBD.Core.Identity;
using MBD.Transactions.Application.MongoDbSettings;
using MBD.Transactions.Application.Queries.Transactions.Queries;
using MBD.Transactions.Application.Response.Models;
using MediatR;
using MongoDB.Driver;

namespace MBD.Transactions.Application.Queries.Transactions.Handlers
{
    public class GetTransactionByIdQueryHandler : IRequestHandler<GetTransactionByIdQuery, IResult<TransactionModel>>
    {
        private readonly IMongoCollection<TransactionModel> _transactions;
        private readonly IAspNetUser _aspNetUser;

        public GetTransactionByIdQueryHandler(ITransactionDatabaseSettings settings, IAspNetUser aspNetUser)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _transactions = database.GetCollection<TransactionModel>(settings.CollectionName);
            _aspNetUser = aspNetUser;
        }

        public async Task<IResult<TransactionModel>> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
        {
            var resultTask = await _transactions.FindAsync(x => x.Id == request.Id.ToString() && x.TenantId == _aspNetUser.UserId.ToString());
            var transaction = resultTask.FirstOrDefault();
            if (transaction == null)
                return Result<TransactionModel>.Fail("Transação inválida.");

            return Result<TransactionModel>.Success(transaction);
        }
    }
}