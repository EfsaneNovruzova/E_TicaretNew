using E_TicaretNew.Application.DTOs.ReviewDTOs;
using FluentValidation;

namespace E_TicaretNew.Application.Validations.ReviewValidators;

public class ReviewCreateDtoValidator : AbstractValidator<ReviewCreateDto>
{
    public ReviewCreateDtoValidator()
    {
        RuleFor(x => x.Comment)
            .NotEmpty()
            .MaximumLength(1000);

        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5); 

        RuleFor(x => x.ProductId)
            .NotEmpty();
    }
}
