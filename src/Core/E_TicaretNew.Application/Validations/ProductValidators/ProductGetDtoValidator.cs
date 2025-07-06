namespace E_TicaretNew.Application.Validations.ProductValidators;

using E_TicaretNew.Application.DTOs.ProductDTOs;
using FluentValidation;

public class ProductGetDtoValidator : AbstractValidator<ProductGetDto>
{
    public ProductGetDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Product Id is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product Name is required.")
            .MaximumLength(100).WithMessage("Product Name cannot exceed 100 characters.");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage("Price cannot be negative.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(x => x.CategoryName)
            .NotEmpty().WithMessage("CategoryName is required.");

        RuleFor(x => x.FavoritesCount)
            .GreaterThanOrEqualTo(0).WithMessage("FavoritesCount cannot be negative.");

        RuleFor(x => x.ReviewsCount)
            .GreaterThanOrEqualTo(0).WithMessage("ReviewsCount cannot be negative.");
    }
}

