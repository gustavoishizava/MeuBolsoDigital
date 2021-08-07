using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MBD.Application.Core.Response;
using MBD.Transactions.Application.Response.Models;

namespace MBD.Transactions.Application.Queries
{
    public interface ITransactionQuery
    {
        Task<IEnumerable<TransactionModel>> GetAllAsync();
        Task<IResult<TransactionModel>> GetByIdAsync(Guid id);
    }
}