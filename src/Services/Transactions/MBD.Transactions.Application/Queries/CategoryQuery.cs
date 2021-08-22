using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MBD.Application.Core.Response;
using MBD.Transactions.Application.Response;
using MBD.Transactions.Domain.Enumerations;
using MBD.Transactions.Domain.Interfaces.Repositories;

namespace MBD.Transactions.Application.Queries
{
    public class CategoryQuery : ICategoryQuery
    {
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;

        public CategoryQuery(ICategoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CategoryByTypeResponse> GetAllAsync()
        {
            var allCategories = _mapper.Map<List<CategoryWithSubCategoriesResponse>>(await _repository.GetAllAsync());
            var incomeCategories = allCategories.Where(x => x.Type == TransactionType.Income).ToList();
            var expenseCategories = allCategories.Where(x => x.Type == TransactionType.Expense).ToList();

            return new CategoryByTypeResponse(incomeCategories, expenseCategories);
        }

        public async Task<IResult<CategoryResponse>> GetByIdAsync(Guid id)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null)
                return Result<CategoryResponse>.Fail("Categoria inválida.");

            return Result<CategoryResponse>.Success(_mapper.Map<CategoryResponse>(category));
        }

        public async Task<IEnumerable<CategoryWithSubCategoriesResponse>> GetByTypeAsync(TransactionType type)
        {
            return _mapper.Map<List<CategoryWithSubCategoriesResponse>>(await _repository.GetByTypeAsync(type));
        }
    }
}