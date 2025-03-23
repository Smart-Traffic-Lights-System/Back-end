using System;
using System.IdentityModel.Tokens.Jwt;
using UserManagement.Models;

namespace UserManagement.Business.Auth;

public interface IAuthService
{
    public RegisterUserDto Register(RegisterUserDto registerUserDto);
    public string Login(LoginUserDto loginUserDto);
    public void Logout(string token);
    public void HashPassword(string password, ref string hashedPassword);
    public JwtSecurityToken GenerateToken(string userId, string username, int roleId, string key, string issuer, string audience);
}
