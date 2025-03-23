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

    [HttpGet("GetAllRoles")]
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
}