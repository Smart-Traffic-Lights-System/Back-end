using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UserData;
using UserManagement.Models;

namespace UserManagement.Business.Users;

public class UserService : IUserService
{
    private readonly UserDbContext _context;
    private readonly IMapper _mapper;

    public UserService(UserDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
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
        throw new NotImplementedException();
    }
}
