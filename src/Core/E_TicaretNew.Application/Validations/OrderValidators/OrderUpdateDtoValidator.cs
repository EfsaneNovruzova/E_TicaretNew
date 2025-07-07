using E_TicaretNew.Application.DTOs.OrderDTOs;
using FluentValidation;

namespace E_TicaretNew.Application.Validations.OrderValidators;

public class OrderUpdateDtoValidator : AbstractValidator<OrderUpdateDto>
{
    public OrderUpdateDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Order Id boş ola bilməz.");

        RuleFor(x => x.PaymentId)
            .NotEmpty().WithMessage("PaymentId boş ola bilməz.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Status düzgün formatda olmalıdır.");

        RuleFor(x => x.Products)
            .NotEmpty().WithMessage("Məhsullar siyahısı boş ola bilməz.");

    }
}
