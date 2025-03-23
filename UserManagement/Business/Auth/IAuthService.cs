using System;
using System.IdentityModel.Tokens.Jwt;
using UserManagement.Models;

namespace UserManagement.Business.Auth;

public interface IAuthService
{
    public RegisterUserDto Register(RegisterUserDto registerUserDto);
    public string Login(LoginUserDto loginUserDto);
    public JwtSecurityToken GenerateToken(string userId, string username, string role, string key, string issuer, string audience);
    public bool isEmailVerified(string email);
    public void UpdateEmailVerificationDate(RegisterUserDto registerUserDto);
    public bool isPhoneVerified(string phone);
    public void UpdatePhoneVerificationDate(RegisterUserDto registerUserDto);
}
