using System;
using MBD.Application.Core.Response;
using MBD.Transactions.Application.Response.Models;
using MediatR;

namespace MBD.Transactions.Application.Queries.Transactions.Queries
{
    public class GetTransactionByIdQuery : IRequest<IResult<TransactionModel>>
    {
        public Guid Id { get; init; }

        public GetTransactionByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}