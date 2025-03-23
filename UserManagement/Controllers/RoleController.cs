using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Business.Role;
using UserManagement.Models;

namespace UserManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoleController : ControllerBase
{  
    private readonly IConfiguration _configuration;
    private readonly IRoleService _roleService;

    public RoleController(IConfiguration configuration, IRoleService roleService)
    {
        _configuration = configuration;
        _roleService = roleService;
    }

    [HttpGet("GetAllRoles"), Authorize(Roles = "System Administrator")]
    public ActionResult<IEnumerable<RoleDto>> GetAllRoles()
    {
        try
        {
            var roles = _roleService.FindAllRoles();
            return Ok(roles);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Status = "Error", Message = ex.Message });
        }
    }

    [HttpGet("GetRole/{RoleId}"), Authorize(Roles = "System Administrator")]
    public ActionResult<RoleDto> GetRole(int roleId)
    {
        try
        {
            var role = _roleService.FindRoleById(roleId);
            if (role == null)
            {
                return NotFound(new ApiResponse { Status = "Error", Message = "Role not found" });
            }

            return Ok(role);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Status = "Error", Message = ex.Message });
        }
    }
}