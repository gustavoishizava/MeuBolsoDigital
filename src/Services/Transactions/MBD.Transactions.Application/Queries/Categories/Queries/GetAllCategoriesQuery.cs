using MBD.Transactions.Application.Response;
using MediatR;

namespace MBD.Transactions.Application.Queries.Categories.Queries
{
    public class GetAllCategoriesQuery : IRequest<CategoryByTypeResponse>
    {
    }
}