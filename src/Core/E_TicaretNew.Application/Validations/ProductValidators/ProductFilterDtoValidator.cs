namespace E_TicaretNew.Application.Validations.ProductValidators;

using E_TicaretNew.Application.DTOs.ProductDTOs;
using FluentValidation;

public class ProductFilterDtoValidator : AbstractValidator<ProductFilterDto>
{
    public ProductFilterDtoValidator()
    {
        // MinPrice və MaxPrice məntiqi yoxlanış: MinPrice MaxPrice-dan böyük ola bilməz
        RuleFor(x => x)
            .Must(x => !x.MinPrice.HasValue || !x.MaxPrice.HasValue || x.MinPrice <= x.MaxPrice)
            .WithMessage("MinPrice cannot be greater than MaxPrice.");

        // CategoryId optional olduğu üçün yoxlanmayacaq, amma istəsən əlavə şərtlər qoya bilərsən

        // Search optional, amma maksimum uzunluq qoymaq istəyirsənsə, misal üçün:
        RuleFor(x => x.Search)
            .MaximumLength(100)
            .WithMessage("Search text cannot be longer than 100 characters.")
            .When(x => !string.IsNullOrEmpty(x.Search));
    }
}

