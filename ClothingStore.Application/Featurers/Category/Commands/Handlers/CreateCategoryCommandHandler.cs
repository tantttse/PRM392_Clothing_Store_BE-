using AutoMapper;
using ClothingStore.Application.Features.Categories.Dtos;
using ClothingStore.Domain.Entities;
using ClothingStore.Domain.Repositories;
using FluentValidation;
using Shared.Application.Abstractions.Messaging;
using Shared.Domain.Common.ResponseModel;

namespace ClothingStore.Application.Features.Categories.Commands
{
    public class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryDto>
    {
        public CreateCategoryRequestValidator()
        {
            RuleFor(x => x.CategoryName)
                .NotEmpty().WithMessage("Category name is required.")
                .MaximumLength(100);
        }
    }

    public class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand, CategoryDto>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CreateCategoryCommandHandler(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<Result<CategoryDto>> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
        {
            var category = Category.Create(command.Category.CategoryName);

            await _categoryRepository.AddAsync(category, cancellationToken);

            return Result.Success(_mapper.Map<CategoryDto>(category));
        }
    }
}
