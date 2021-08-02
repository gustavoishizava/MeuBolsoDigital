using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MBD.Application.Core.Response;
using MBD.Transactions.Application.Request;
using MBD.Transactions.Application.Response;
using MBD.Transactions.Domain.Enumerations;

namespace MBD.Transactions.Application.Interfaces
{
    public interface ICategoryAppService
    {
        Task<IResult<CategoryResponse>> CreateAsync(CreateCategoryRequest request);
        Task<IResult> UpdateAsync(UpdateCategoryRequest request);
        Task<IResult<CategoryResponse>> GetByIdAsync(Guid id);
        Task<CategoryByTypeResponse> GetAllAsync();
        Task<IEnumerable<CategoryWithSubCategoriesResponse>> GetByTypeAsync(TransactionType type);
        Task<IResult> RemoveAsync(Guid id);
    }
}