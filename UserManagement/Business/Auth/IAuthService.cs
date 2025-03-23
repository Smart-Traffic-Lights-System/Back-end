using System;
using UserManagement.Models;

namespace UserManagement.Business.Auth;

public interface IAuthService
{
    public RegisterUserDto Register(RegisterUserDto registerUserDto);
    public LoginUserDto Login(LoginUserDto loginUserDto);
    public void Logout(string token);
    public void HashPassword(string password, ref string hashedPassword);
    public RegisterUserDto FindUserByUsername(string username);
    public string GenerateToken(string userId, string role);
    public string RefreshToken(string token);
}
