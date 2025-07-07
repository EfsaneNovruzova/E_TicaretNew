using E_TicaretNew.Application.DTOs.OrderProductDTOs;
using FluentValidation;

namespace E_TicaretNew.Application.Validations.OrderValidators;

public class OrderProductUpdateDtoValidator : AbstractValidator<OrderProductUpdateDto>
{
    public OrderProductUpdateDtoValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("ProductId boş ola bilməz.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Məhsul sayı 0-dan böyük olmalıdır.");
    }
}
