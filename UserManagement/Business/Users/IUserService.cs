using System;
using UserManagement.Models;

namespace UserManagement.Business.Users;

public interface IUserService
{
    public IEnumerable<RegisterUserDto> FindAllUsers();
    public RegisterUserDto FindUserByUsername(string username);
    public RegisterUserDto FindUserById(int id);
    public RegisterUserDto FindUserByEmail(string email);
    public RegisterUserDto ModifyUser(RegisterUserDto userDto, int userId);
    public void DeleteUserById(int id);
}
