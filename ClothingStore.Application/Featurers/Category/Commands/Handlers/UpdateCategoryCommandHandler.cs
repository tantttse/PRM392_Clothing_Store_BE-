using AutoMapper;
using ClothingStore.Application.Features.Categories.Dtos;
using ClothingStore.Domain.Repositories;
using FluentValidation;
using Shared.Application.Abstractions.Messaging;
using Shared.Domain.Common.ResponseModel;

namespace ClothingStore.Application.Features.Categories.Commands
{

    public class UpdateCategoryRequestValidator : AbstractValidator<UpdateCategoryDto>
    {
        public UpdateCategoryRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.CategoryName)
                .NotEmpty().WithMessage("Category name is required.")
                .MaximumLength(100);
        }
    }

    public class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand, CategoryDto>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<Result<CategoryDto>> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(command.Category.Id, cancellationToken);
            if (category == null)
                return Result.Failure<CategoryDto>(new Error("CategoryNotFound", "Category not found."));

            category.Rename(command.Category.CategoryName);

            _categoryRepository.Update(category, cancellationToken);

            return Result.Success(_mapper.Map<CategoryDto>(category));
        }
    }
}
