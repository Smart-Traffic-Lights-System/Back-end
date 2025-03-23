using AutoMapper;
using UserData;
using UserManagement.Models;

namespace UserManagement.Business.Role;

public class RoleService : IRoleService
{
    private readonly UserDbContext _context;
    private readonly IMapper _mapper;

    public RoleService(UserDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public IEnumerable<RoleDto> FindAllRoles()
    {
        return _mapper.Map<IEnumerable<RoleDto>>(_context.UserRole);
    }

    public RoleDto FindRoleById(int roleId)
    {
        throw new NotImplementedException();
    }
}