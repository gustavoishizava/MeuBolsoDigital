using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MBD.Application.Core.Response;
using MBD.Core.Data;
using MBD.Core.Identity;
using MBD.Transactions.Application.Response;
using MBD.Transactions.Domain.Entities;
using MBD.Transactions.Domain.Interfaces.Repositories;
using MediatR;

namespace MBD.Transactions.Application.Commands
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, IResult<CategoryResponse>>
    {
        private readonly ICategoryRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAspNetUser _aspNetUser;
        private readonly IMapper _mapper;

        public CreateCategoryCommandHandler(ICategoryRepository repository, IUnitOfWork unitOfWork, IAspNetUser aspNetUser, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _aspNetUser = aspNetUser;
            _mapper = mapper;
        }

        public async Task<IResult<CategoryResponse>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var validation = request.Validate();
            if (!validation.IsValid)
                return Result<CategoryResponse>.Fail(validation.ToString());

            Category category = null;
            if (request.ParentCategoryId != null)
            {
                var parentCategory = await _repository.GetByIdAsync(request.ParentCategoryId.Value);
                if (parentCategory == null)
                    return Result<CategoryResponse>.Fail("Categoria pai inválida.");

                if (!parentCategory.CanHaveSubCategories())
                    return Result<CategoryResponse>.Fail("Não é permitido adicionar uma subcategoria à uma categoria filha.");

                category = parentCategory.AddSubCategory(request.Name);
            }
            else
            {
                category = new Category(_aspNetUser.UserId, request.Name, request.Type);
                _repository.Add(category);
            }

            await _unitOfWork.SaveChangesAsync();

            return Result<CategoryResponse>.Success(_mapper.Map<CategoryResponse>(category));
        }
    }
}