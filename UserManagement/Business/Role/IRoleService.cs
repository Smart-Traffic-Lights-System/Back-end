using UserManagement.Models;

namespace UserManagement.Business.Role;

public interface IRoleService
{
    public IEnumerable<RoleDto> GetAllRoles();
    public RoleDto GetRoleById(int roleId);
}