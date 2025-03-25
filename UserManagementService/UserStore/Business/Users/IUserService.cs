using UserStore.Models;

namespace UserStore.Business.Users;

public interface IUserService
{
    public IEnumerable<RegisterUserDto> FindAllUsers();
    public RegisterUserDto FindUserById(int id);
    public RegisterUserDto FindUserByUsername(string username);
    public RegisterUserDto FindUserByEmail(string email);
    public RegisterUserDto FindUserByPhone(string phone);
    public RegisterUserDto ModifyUser(RegisterUserDto userDto, int userId);
    public void DeleteUserById(int id);
}
