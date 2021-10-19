using System;
using MBD.Application.Core.Response;
using MBD.Transactions.Application.Response;
using MediatR;

namespace MBD.Transactions.Application.Queries.Categories.Queries
{
    public class GetCategoryByIdQuery : IRequest<IResult<CategoryResponse>>
    {
        public Guid Id { get; init; }

        public GetCategoryByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}