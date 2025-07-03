using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using E_Ticaret.Domain.Entities;
using E_TicaretNew.Application.Abstracts.Services;
using E_TicaretNew.Application.DTOs.UserDtos;
using E_TicaretNew.Application.Shared.Setting;
using E_TicaretNew.Application.Shared.Setting.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace E_TicaretNew.Persistence.Services;

public class UserService : IUserService
{
    private UserManager<User> _usermanager { get;  }
    private SignInManager<User> _signInManager { get; }
    private JWTSettings _jwtSetting { get; }
    public UserService(UserManager<User> usermanager, SignInManager<User> signInManager, IOptions<JWTSettings> jwtSettings)
    {
        _usermanager = usermanager;
        _signInManager = signInManager;
        _jwtSetting = jwtSettings.Value;
    }


    public async Task<BaseResponse<string>> Register(UserRegisterDto dto)
    {
    var existedEmail=await _usermanager.FindByEmailAsync(dto.Email);
        if (existedEmail is not null)
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
        if (!identityResult.Succeeded) 
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

    public  async Task<BaseResponse<TokenResponse>> Login(UserLoginDto dto)
    {
        var existedUser = await _usermanager.FindByEmailAsync(dto.Email);
        if (existedUser is null) 
        {
            return new("Emaail or Password is wrong.", null, HttpStatusCode.NotFound);
        }


        SignInResult signInResult = await _signInManager.PasswordSignInAsync
            (dto.Email,dto.Password,true,true);
      
        if (!signInResult.Succeeded)
        {
           
            return new("Emaail or Password is wrong.", null, HttpStatusCode.NotFound);
        }
        var token = GenerateJwtToken(dto.Email);
        var expires = DateTime.UtcNow.AddHours(_jwtSetting.ExpiresInMinutes);
        TokenResponse tokenResponse = new()
        { 
            Token = token,
            ExpireDate = expires
        };
        return new("Token generated",tokenResponse, HttpStatusCode.OK);
    }
    public string GenerateJwtToken(string userEmail)
    {
        if (string.IsNullOrEmpty(_jwtSetting.SecretKey))
            throw new Exception("JWT SecretKey is not configured.");

        // İndi davam et
        var claims = new[]
        {
        new Claim(ClaimTypes.Email, userEmail),
        new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
    };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSetting.Issuer,
            audience: _jwtSetting.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSetting.ExpiresInMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }


}
