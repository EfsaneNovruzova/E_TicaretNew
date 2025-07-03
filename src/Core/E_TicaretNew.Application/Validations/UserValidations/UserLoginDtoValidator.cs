using E_TicaretNew.Application.DTOs.UserDtos;
using FluentValidation;

namespace E_TicaretNew.Application.Validations.UserValidations;

public class UserLoginDtoValidator:AbstractValidator<UserLoginDto>
{
    public UserLoginDtoValidator()
    {

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email boş ola bilməz.")
            .EmailAddress().WithMessage("Email düzgün formatda deyil.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Şifrə boş ola bilməz.")
            .MinimumLength(8).WithMessage("Şifrə ən az 8 simvol olmalıdır.");
            
    }
}
