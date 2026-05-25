using FluentValidation;
using ElectronicsStore.Core.Application.DTOs;

namespace ElectronicsStore.Core.Application.Validators
{
    public class CategoryValidator : AbstractValidator<CategoryDto>
    {
        public CategoryValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Category name is required.")
                .Length(1, 100).WithMessage("Category name must be between 1 and 100 characters.");
        }
    }
}
