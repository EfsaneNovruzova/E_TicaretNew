using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using E_TicaretNew.Domain.Entities;
using E_TicaretNew.Application.Abstracts.Services;
using E_TicaretNew.Application.DTOs.UserDtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using E_TicaretNew.Application.Shared.Responses;
using E_TicaretNew.Application.Shared.Setting;
using E_TicaretNew.Application.Shared;

namespace E_TicaretNew.Persistence.Services;

public class UserService : IUserService
{
    private UserManager<User> _usermanager { get;  }
    private SignInManager<User> _signInManager { get; }
    private RoleManager<IdentityRole> _roleManager { get; }
    private JWTSettings _jwtSetting { get; }
    public UserService(UserManager<User> usermanager, SignInManager<User> signInManager, IOptions<JWTSettings> jwtSettings, RoleManager<IdentityRole> roleManager)
    {
        _usermanager = usermanager;
        _signInManager = signInManager;
        _jwtSetting = jwtSettings.Value;
        _roleManager = roleManager;
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
        var token =  await GenerateTokensAsync(existedUser);
        return new("Token generated", token, HttpStatusCode.OK);
    }
    private async Task<TokenResponse> GenerateTokensAsync(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtSetting.SecretKey);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier,user.Id),
            new Claim(ClaimTypes.Email,user.Email!)
        };

        var roles = await _usermanager.GetRolesAsync(user);
        foreach (var roleName in roles) 
        {
            claims.Add(new Claim(ClaimTypes.Role, roleName));

            var role = await _roleManager.FindByNameAsync(roleName);
            if (role != null)
            {
                var roleClaims = await _roleManager.GetClaimsAsync(role);
                var permissionClaims = roleClaims
                    .Where(c => c.Type == "Permission")
                    .Distinct();
                foreach (var permissionClaim in permissionClaims)
                {
                    claims.Add(new Claim("Permission", permissionClaim.Value));
                }
            }
        }
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSetting.ExpiresInMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _jwtSetting.Issuer,
            Audience = _jwtSetting.Audience
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwt = tokenHandler.WriteToken(token);

        var refreshToken = GenerateRefreshToken();
        var refreshTokenExpiryDate = DateTime.UtcNow.AddHours(2);

        user.RefreshToken = refreshToken; 
        user.ExpiryDate = refreshTokenExpiryDate;

        await _usermanager.UpdateAsync(user); 

        return new TokenResponse
        {
            Token = jwt,
            RefreshToken = refreshToken,
            ExpireDate = tokenDescriptor.Expires
        };
    }

    public async Task<BaseResponse<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request)
    {
        var principal = GetPrincipalFromExpiredToken(request.AccessToken);
        if (principal == null)
            return new BaseResponse<TokenResponse>("Invalid access token", null, HttpStatusCode.BadRequest);

        var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var user = await _usermanager.FindByIdAsync(userId!);
        if (user == null)
        {
            return new BaseResponse<TokenResponse>("User not found", null, HttpStatusCode.NotFound);
        }
        if (user.RefreshToken is null || user.RefreshToken != request.RefreshToken ||
            user.ExpiryDate<DateTime.UtcNow)
        {
            return new("invalid refresh token", null, HttpStatusCode.BadRequest);
        }
        var tokenResponse = await GenerateTokensAsync(user);
        return new("Refreshed",tokenResponse,HttpStatusCode.OK);
    }

    public async Task<BaseResponse<string>> AddRole(UserAddRoleDto dto)
    {
        var user = await _usermanager.FindByIdAsync(dto.UserId.ToString());
        if (user == null)
        {
         return new BaseResponse<string>("User not Found.",HttpStatusCode.NotFound);
        }
        var roleNames = new List<string>();

        foreach (var roleId in dto.RolesId.Distinct()) 
        { 
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (role == null)
            {
                return new BaseResponse<string>($"Role with Id '{roleId}' not found.",
                   HttpStatusCode.NotFound );
            }
            if (!await _usermanager.IsInRoleAsync(user, role.Name!))
            {
                var result = await _usermanager.AddToRoleAsync(user,role.Name!);
                if (!result.Succeeded) 
                {
                    var errors = string.Join(";", result.Errors.Select(e => e.Description));
                    return new BaseResponse<string>($"Failed to add role ;{role.Name} to user:{errors}", HttpStatusCode.BadRequest);
                }
                roleNames.Add(role.Name!);
            }
        }
        return new BaseResponse<string>
            (
            $"Successfully added roles :{string .Join(",",roleNames)} to user.", HttpStatusCode.OK
            );
    }
    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng =RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = false, 
            ValidIssuer = _jwtSetting.Issuer,
            ValidAudience = _jwtSetting.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.SecretKey))
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters,
                out var securityToken);


            if (securityToken is JwtSecurityToken jwtSecurityToken &&
                jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
            {
                return principal;
            }
        }
        catch
        {
            return null;
        }
        return null;
    }



}
