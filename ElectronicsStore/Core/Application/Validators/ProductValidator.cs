using FluentValidation;
using ElectronicsStore.Core.Application.DTOs;

namespace ElectronicsStore.Core.Application.Validators
{
    public class ProductValidator : AbstractValidator<ProductDto>
    {
        public ProductValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .Length(1, 200).WithMessage("Product name must be between 1 and 200 characters.");

            RuleFor(p => p.Description)
                .MaximumLength(1000).WithMessage("Product description cannot exceed 1000 characters.");

            RuleFor(p => p.Price)
                .GreaterThan(0).WithMessage("Product price must be greater than 0.")
                .PrecisionScale(10, 2, false).WithMessage("Price must have at most 2 decimal places.");

            RuleFor(p => p.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("Stock quantity cannot be negative.");

            RuleFor(p => p.ImageUrl)
                .MaximumLength(500).WithMessage("Image URL cannot exceed 500 characters.");
        }
    }
}
