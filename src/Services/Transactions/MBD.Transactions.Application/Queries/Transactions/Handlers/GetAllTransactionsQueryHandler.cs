using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MBD.Core.Identity;
using MBD.Transactions.Application.MongoDbSettings;
using MBD.Transactions.Application.Queries.Transactions.Queries;
using MBD.Transactions.Application.Response.Models;
using MediatR;
using MongoDB.Driver;

namespace MBD.Transactions.Application.Queries.Transactions.Handlers
{
    public class GetAllTransactionsQueryHandler : IRequestHandler<GetAllTransactionsQuery, IEnumerable<TransactionModel>>
    {
        private readonly IMongoCollection<TransactionModel> _transactions;
        private readonly IAspNetUser _aspNetUser;

        public GetAllTransactionsQueryHandler(ITransactionDatabaseSettings settings, IAspNetUser aspNetUser)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _transactions = database.GetCollection<TransactionModel>(settings.CollectionName);
            _aspNetUser = aspNetUser;
        }

        public async Task<IEnumerable<TransactionModel>> Handle(GetAllTransactionsQuery request, CancellationToken cancellationToken)
        {
            var resultTask = await _transactions.FindAsync(x => x.TenantId == _aspNetUser.UserId.ToString());
            return resultTask.ToList();
        }
    }
}