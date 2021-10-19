using System.Collections.Generic;
using MBD.Transactions.Application.Response;
using MBD.Transactions.Domain.Enumerations;
using MediatR;

namespace MBD.Transactions.Application.Queries.Categories.Queries
{
    public class GetAllCategoriesByTypeQuery : IRequest<IEnumerable<CategoryWithSubCategoriesResponse>>
    {
        public TransactionType Type { get; init; }

        public GetAllCategoriesByTypeQuery(TransactionType type)
        {
            Type = type;
        }
    }
}