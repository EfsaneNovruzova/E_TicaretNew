using E_TicaretNew.Application.DTOs.UserDtos;
using E_TicaretNew.Application.Shared.Setting.Responses;

namespace E_TicaretNew.Application.Abstracts.Services;
public  interface IUserService
{
    Task<BaseResponse<string>> Register(UserRegisterDto dto);
    Task<BaseResponse<TokenResponse>> Login(UserLoginDto dto);
}
