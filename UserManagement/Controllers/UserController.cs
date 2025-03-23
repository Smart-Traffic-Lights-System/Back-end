using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Business.Users;
using UserManagement.Models;

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("GetAllUsers")]
        public ActionResult<IEnumerable<RegisterUserDto>> GetAllUsers()
        {
            try
            {
                var users = _userService.FindAllUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Status = "Error", Message = ex.Message });
            }
        }

        [HttpGet("GetUserById/{id}")]
        public ActionResult<RegisterUserDto> GetUserById(int id)
        {
            try
            {
                var user = _userService.FindUserById(id);
                if (user == null)
                {
                    return NotFound(new ApiResponse { Status = "Error", Message = "User not found" });
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Status = "Error", Message = ex.Message });
            }
        }

        [HttpGet("GetUserByUsername/{username}")]
        public ActionResult<RegisterUserDto> GetUserByUsername(string username)
        {
            try
            {
                var user = _userService.FindUserByUsername(username);
                if (user == null)
                {
                    return NotFound(new ApiResponse { Status = "Error", Message = "User not found" });
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Status = "Error", Message = ex.Message });
            }
        }
    }
}
