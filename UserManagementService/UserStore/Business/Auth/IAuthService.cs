using System.IdentityModel.Tokens.Jwt;
using UserStore.Models;

namespace UserStore.Business.Auth;

public interface IAuthService
{
    public RegisterUserDto Register(RegisterUserDto registerUserDto);
    public string Login(LoginUserDto loginUserDto);
    public JwtSecurityToken GenerateToken(string userId, string username, string role, string key, string issuer, string audience);
    public bool IsEmailVerified(string email);
    public void UpdateEmailVerificationDate(RegisterUserDto registerUserDto);
    public bool IsPhoneVerified(string phone);
    public void UpdatePhoneVerificationDate(RegisterUserDto registerUserDto);
}
