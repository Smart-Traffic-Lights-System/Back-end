using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using UserData;
using UserData.Entities;
using UserManagement.Business.Users;
using UserManagement.Models;

namespace UserManagement.Business.Auth;

public class AuthService : IAuthService
{
    private readonly UserDbContext _context;
    private readonly IMapper _mapper;

    private readonly IUserService _userService;

    public AuthService(UserDbContext context, IMapper mapper, IUserService userService)
    {
        _context = context;
        _mapper = mapper;
        _userService = userService;
    }

    public JwtSecurityToken GenerateToken(string userId, string username, int roleId, string key, string issuer, string audience)
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
                new Claim("RoleId", roleId.ToString())
            },
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return token;
    }

    public string Login(LoginUserDto loginUserDto)
    {
        var userDto = _userService.FindUserByUsername(loginUserDto.Username);
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

    public void Logout(string token)
    {
        throw new NotImplementedException();
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
        user.Role.RoleId = 1; // Assuming 1 is the role ID for a regular user
        _context.User.Add(user);
        _context.SaveChanges();

        return _mapper.Map<RegisterUserDto>(user);
    }

    // SHA256 Encryption
    private void HashPassword(string password, ref string hashedPassword)
    {
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashedBytes = sha256.ComputeHash(passwordBytes);
            hashedPassword = BitConverter.ToString(hashedBytes).Replace("-", "");
        }
    }
}
