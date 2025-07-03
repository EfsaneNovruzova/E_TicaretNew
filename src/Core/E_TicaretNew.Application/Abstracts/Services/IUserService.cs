using AzBina.Application.Shared;
using E_TicaretNew.Application.DTOs.UserDtos;

namespace E_TicaretNew.Application.Abstracts.Services;
public  interface IUserService
{
    Task<BaseResponse<string>> Register(UserRegisterDto dto);
}
