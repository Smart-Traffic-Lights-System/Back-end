using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UserData;
using UserManagement.Business.Auth;
using UserManagement.Models;

namespace UserManagement.Business.Users;

public class UserService : IUserService
{
    private readonly UserDbContext _context;
    private readonly IMapper _mapper;
    private readonly IAuthService _authService;

    public UserService(UserDbContext context, IMapper mapper, IAuthService authService)
    {
        _context = context;
        _mapper = mapper;
        _authService = authService;
    }

    public void DeleteUserById(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<RegisterUserDto> FindAllUsers()
    {
        return _mapper.Map<IEnumerable<RegisterUserDto>>(_context.User);
    }

    public RegisterUserDto FindUserById(int id)
    {
        var user = _context.User.FirstOrDefault(x => x.UserId == id);
        if (user == null)
        {
            return null;
        }

        var userDto = _mapper.Map<RegisterUserDto>(user);
        return userDto;
    }

    public RegisterUserDto FindUserByUsername(string username)
    {
        var usersList = _mapper.Map<IEnumerable<RegisterUserDto>>(_context.User);
        var user = usersList.FirstOrDefault(x => x.Username == username);

        return user;
    }

    public RegisterUserDto ModifyUser(RegisterUserDto userDto, int userId)
    {
        string firstName = userDto.FirstName;
        UserBusinessLogic.DefineNameBL(firstName, "FirstName");
        string lastName = userDto.LastName;
        UserBusinessLogic.DefineNameBL(lastName, "LastName");
        string email = userDto.Email;
        UserBusinessLogic.DefineEmailBL(email);
        string phone = userDto.PhoneNumber;
        UserBusinessLogic.DefinePhoneNumberBL(phone);
        string username = userDto.Username;
        UserBusinessLogic.DefineUsernameBL(username);
        string password = userDto.Password;
        UserBusinessLogic.DefinePasswordBL(password);

        string hashedPassword = string.Empty;
        _authService.HashPassword(password, ref hashedPassword);
        userDto.Password = hashedPassword;

        var user = _context.User.FirstOrDefault(x => x.UserId == userId);
        if (user == null)
        {
            throw new MyException("User not found");
        }

        if (userDto.FirstName != null)
        {
            user.FirstName = userDto.FirstName;
        }
        if (userDto.LastName != null)
        {
            user.LastName = userDto.LastName;
        }
        if (userDto.Email != null)
        {
            user.Email = userDto.Email;
        }
        if (userDto.PhoneNumber != null)
        {
            user.PhoneNumber = userDto.PhoneNumber;
        }
        if (userDto.Username != null)
        {
            user.Username = userDto.Username;
        }
        if (userDto.Password != null)
        {
            user.PasswordHash = userDto.Password;
        }

        var updatedUserDto = _mapper.Map<RegisterUserDto>(user);
        updatedUserDto.UserId = 0;

        return updatedUserDto;
    }
}
