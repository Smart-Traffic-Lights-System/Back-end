using UserManagement.Models;

namespace UserManagement.Business.Role;

public interface IRoleService
{
    public IEnumerable<RoleDto> FindAllRoles();
    public RoleDto FindRoleById(int roleId);
}