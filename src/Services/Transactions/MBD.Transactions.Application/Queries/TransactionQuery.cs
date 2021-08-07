using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MBD.Application.Core.Response;
using MBD.Transactions.Application.MongoDbSettings;
using MBD.Transactions.Application.Response.Models;
using MongoDB.Driver;
using System.Linq;
using MBD.Core.Identity;

namespace MBD.Transactions.Application.Queries
{
    public class TransactionQuery : ITransactionQuery
    {
        private readonly IMongoCollection<TransactionModel> _transactions;
        private readonly IAspNetUser _aspNetUser;

        public TransactionQuery(ITransactionDatabaseSettings settings, IAspNetUser aspNetUser)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _transactions = database.GetCollection<TransactionModel>(settings.CollectionName);
            _aspNetUser = aspNetUser;
        }

        public async Task<IEnumerable<TransactionModel>> GetAllAsync()
        {
            var resultTask = await _transactions.FindAsync(x => x.TenantId == _aspNetUser.UserId.ToString());
            return resultTask.ToList();
        }

        public async Task<IResult<TransactionModel>> GetByIdAsync(Guid id)
        {
            var resultTask = await _transactions.FindAsync(x => x.Id == id.ToString() && x.TenantId == _aspNetUser.UserId.ToString());
            var transaction = resultTask.FirstOrDefault();
            if(transaction == null)
                return Result<TransactionModel>.Fail("Transação inválida.");

            return Result<TransactionModel>.Success(transaction);
        }
    }
}