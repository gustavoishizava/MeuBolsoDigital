using System.Collections.Generic;
using MBD.Transactions.Application.Response.Models;
using MediatR;

namespace MBD.Transactions.Application.Queries.Transactions.Queries
{
    public class GetAllTransactionsQuery : IRequest<IEnumerable<TransactionModel>>
    {
    }
}