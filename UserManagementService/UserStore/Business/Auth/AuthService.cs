using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using UserData;
using UserData.Entities;
using UserStore.Business.Users;
using UserStore.Models;

namespace UserStore.Business.Auth;

public class AuthService : IAuthService
{
    private readonly UserDbContext _context;
    private readonly IMapper _mapper;
    
    public AuthService(UserDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public RegisterUserDto Register(RegisterUserDto registerUserDto)
    {
        // Business Logic for registering a user

        string firstName = registerUserDto.FirstName;
        UserBusinessLogic.DefineNameBL(firstName, "First Name");
        string lastName = registerUserDto.LastName;
        UserBusinessLogic.DefineNameBL(lastName, "Last Name");
        string email = registerUserDto.Email;
        UserBusinessLogic.DefineEmailBL(email);
        string phoneNumber = registerUserDto.PhoneNumber;
        UserBusinessLogic.DefinePhoneNumberBL(phoneNumber);
        string username = registerUserDto.Username;
        UserBusinessLogic.DefineUsernameBL(username);
        string password = registerUserDto.Password;
        UserBusinessLogic.DefinePasswordBL(password);

        string hashedPassword = string.Empty;
        HashPassword(password, ref hashedPassword);
        registerUserDto.Password = hashedPassword;
        registerUserDto.ConfirmPassword = hashedPassword;

        User user = _mapper.Map<User>(registerUserDto);
        user.PasswordHash = hashedPassword;
        user.CreatedAt = DateTime.Now;
        user.Role = "User";
        
        _context.User.Add(user);
        _context.SaveChanges();

        return _mapper.Map<RegisterUserDto>(user);
    }
    
    public string Login(LoginUserDto loginUserDto)
    {
        var userDto = _context.User.FirstOrDefault(u => u.Username == loginUserDto.Username);
        if (userDto == null)
        {
            return "User not found";
        }

        User user = _mapper.Map<User>(userDto);

        string hashedPassword = string.Empty;
        HashPassword(loginUserDto.Password, ref hashedPassword);

        if (user.PasswordHash != hashedPassword)
        {
            return "Invalid password";
        }
        else
        {
            return "Login successful";
        }
    }

    public JwtSecurityToken GenerateToken(string userId, string username, string role, string key, string issuer, string audience)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            expires: DateTime.Now.AddHours(1),
            claims: new[]
            {
                new Claim("UserId", userId.ToString()),
                new Claim("Username", username),
                new Claim("Role", role)
            },
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return token;
    }

    public bool IsEmailVerified(string email)
    {
        var user = _context.User.FirstOrDefault(u => u.Email == email);

       if (!user.IsEmailVerified)
        {
            return false;
        }

        return true;
    }

    public void UpdateEmailVerificationDate(RegisterUserDto registerUserDto)
    {
        var user = _context.User.Find(registerUserDto.UserId);

        user.EmailVerifiedAt = DateTime.Now;
        user.IsEmailVerified = true;
        _context.SaveChanges();
    }

    public bool IsPhoneVerified(string phone)
    {
        var user = _context.User.FirstOrDefault(u => u.PhoneNumber == phone);

        if (!user.IsPhoneVerified)
        {
            return false;
        }

        return true;
    }

    public void UpdatePhoneVerificationDate(RegisterUserDto registerUserDto)
    {
        var user = _context.User.Find(registerUserDto.UserId);

        user.PhoneVerifiedAt = DateTime.Now;
        user.IsPhoneVerified = true;
        _context.SaveChanges();
    }

    // SHA256 Encryption
    private static void HashPassword(string password, ref string hashedPassword)
    {
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
        byte[] hashedBytes = SHA256.HashData(passwordBytes);
        hashedPassword = Convert.ToHexString(hashedBytes);
    }
}
