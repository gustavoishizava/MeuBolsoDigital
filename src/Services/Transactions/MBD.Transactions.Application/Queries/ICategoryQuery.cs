using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MBD.Application.Core.Response;
using MBD.Transactions.Application.Response;
using MBD.Transactions.Domain.Enumerations;

namespace MBD.Transactions.Application.Queries
{
    public interface ICategoryQuery
    {
        Task<IResult<CategoryResponse>> GetByIdAsync(Guid id);
        Task<CategoryByTypeResponse> GetAllAsync();
        Task<IEnumerable<CategoryWithSubCategoriesResponse>> GetByTypeAsync(TransactionType type);
    }
}