using E_TicaretNew.Application.DTOs.UserDtos;
using E_TicaretNew.Application.Shared;
using E_TicaretNew.Application.Shared.Responses;

namespace E_TicaretNew.Application.Abstracts.Services;
public  interface IUserService
{
    Task<BaseResponse<string>> Register(UserRegisterDto dto);
    Task<BaseResponse<TokenResponse>> Login(UserLoginDto dto);
    public Task<BaseResponse<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request);
}
