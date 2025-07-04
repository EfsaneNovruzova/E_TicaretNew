using E_TicaretNew.Application.DTOs.CategoryDtos;
using FluentValidation;

namespace E_TicaretNew.Application.Validations.CategoryValidators;

public class CategoryCreateDtoValidator : AbstractValidator<CategoryCreateDto>
{
    public CategoryCreateDtoValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Name con not be null.")
            .MinimumLength(3).WithMessage("Name should be minimum character.");
    }
}
