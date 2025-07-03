using E_TicaretNew.Application.DTOs.UserDtos;
using FluentValidation;

namespace E_TicaretNew.Application.Validations.UserValodations;

public class UserRegisterDtoValidator : AbstractValidator<UserRegisterDto>
{
    public UserRegisterDtoValidator()
    {
        RuleFor(x => x.FulName)
            .NotEmpty().WithMessage("Ad və soyad boş ola bilməz.")
            .MinimumLength(3).WithMessage("Ad və soyad ən az 3 simvol olmalıdır.")
            .MaximumLength(50).WithMessage("Ad və soyad maksimum 50 simvol ola bilər.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email boş ola bilməz.")
            .EmailAddress().WithMessage("Email düzgün formatda deyil.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Şifrə boş ola bilməz.")
            .MinimumLength(8).WithMessage("Şifrə ən az 8 simvol olmalıdır.")
            .Matches("[A-Z]").WithMessage("Şifrə ən az 1 böyük hərf içərməlidir.")
            .Matches("[a-z]").WithMessage("Şifrə ən az 1 kiçik hərf içərməlidir.")
            .Matches("[0-9]").WithMessage("Şifrə ən az 1 rəqəm içərməlidir.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Şifrə ən az 1 xüsusi simvol içərməlidir.");
    }
}
