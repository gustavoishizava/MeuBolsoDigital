using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MBD.Application.Core.Response;
using MBD.Core.Data;
using MBD.Core.Enumerations;
using MBD.Core.Identity;
using MBD.Transactions.Application.Interfaces;
using MBD.Transactions.Application.Request;
using MBD.Transactions.Application.Response;
using MBD.Transactions.Domain.Entities;
using MBD.Transactions.Domain.Enumerations;
using MBD.Transactions.Domain.Interfaces.Repositories;

namespace MBD.Transactions.Application.Services
{
    public class CategoryAppService : ICategoryAppService
    {
        private readonly IAspNetUser _aspNetUser;
        private readonly ICategoryRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryAppService(IAspNetUser aspNetUser, ICategoryRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _aspNetUser = aspNetUser;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IResult<CategoryResponse>> CreateAsync(CreateCategoryRequest request)
        {
            var validation = request.Validate();
            if (!validation.IsValid)
                return Result<CategoryResponse>.Fail(validation.ToString());

            var category = new Category(_aspNetUser.UserId, request.Name, request.Type);

            _repository.Add(category);
            await _unitOfWork.SaveChangesAsync();

            return Result<CategoryResponse>.Success(_mapper.Map<CategoryResponse>(category));
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

        public async Task<IResult> RemoveAsync(Guid id)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null)
                return Result<CategoryResponse>.Fail("Categoria inválida.");

            _repository.Remove(category);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<IResult> UpdateAsync(UpdateCategoryRequest request)
        {
            var validation = request.Validate();
            if (!validation.IsValid)
                return Result.Fail(validation.ToString());

            var category = await _repository.GetByIdAsync(request.Id);
            if (category == null)
                return Result.Fail("Categoria inválida.");

            category.SetName(request.Name);
            if (request.Status == Status.Active)
                category.Activate();
            else
                category.Deactivate();

            _repository.Update(category);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }
    }
}