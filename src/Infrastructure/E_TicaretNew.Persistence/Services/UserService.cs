using System.Net;
using System.Text;
using AzBina.Application.Shared;
using E_Ticaret.Domain.Entities;
using E_TicaretNew.Application.Abstracts.Services;
using E_TicaretNew.Application.DTOs.UserDtos;
using Microsoft.AspNetCore.Identity;

namespace E_TicaretNew.Persistence.Services;

public class UserService : IUserService
{
    private UserManager<User> _usermanager { get;  }
    public UserService(UserManager<User> usermanager)
    {
        _usermanager = usermanager;
    }

    public async Task<BaseResponse<string>> Register(UserRegisterDto dto)
    {
    var exostedEmail=await _usermanager.FindByEmailAsync(dto.Email);
        if (exostedEmail is not null)
        {
            return new BaseResponse<string>("This account already exist", HttpStatusCode.BadRequest);
        }
        User newUser = new()
        {
            Email = dto.Email,
            FulName = dto.FulName,
            UserName = dto.Email
        };
        IdentityResult identityResult= await _usermanager.CreateAsync(newUser,dto.Password);
        if (!identityResult.Succeeded) // ❗ burada yoxlama yanlış nəticəyə görə edilir
        {
            var errors = identityResult.Errors;
            StringBuilder errorMessage = new();
            foreach (var error in errors)
            {
                errorMessage.Append(error.Description+" ");
            }

            return new BaseResponse<string>(errorMessage.ToString(), HttpStatusCode.BadRequest);
        }
        return new("Successfully creted", HttpStatusCode.Created);
    }
}
