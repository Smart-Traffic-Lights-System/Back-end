using System;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using UserData;
using UserData.Entities;
using UserManagement.Business.Users;
using UserManagement.Models;

namespace UserManagement.Business.Auth;

public class AuthService : IAuthService
{
    private UserDbContext _context;
    private IMapper _mapper;

    public AuthService(UserDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public string GenerateToken(string userId, string role)
    {
        throw new NotImplementedException();
    }

    public LoginUserDto Login(LoginUserDto loginUserDto)
    {
        throw new NotImplementedException();
    }

    public void Logout(string token)
    {
        throw new NotImplementedException();
    }

    public string RefreshToken(string token)
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
        user.RoleId = 2; // Assuming 2 is the role ID for a regular user
        _context.User.Add(user);
        _context.SaveChanges();

        return _mapper.Map<RegisterUserDto>(user);
    }

    // SHA256 Encryption
    public void HashPassword(string password, ref string hashedPassword)
    {
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashedBytes = sha256.ComputeHash(passwordBytes);
            hashedPassword = BitConverter.ToString(hashedBytes).Replace("-", "");
        }
    }

}
