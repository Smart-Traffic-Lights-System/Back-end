using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("GetUserById/{UserId}")]
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

        [HttpGet("GetUserByUsername/{Username}")]
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

        [HttpGet("GetUserByEmail/{Email}")]
        public ActionResult<RegisterUserDto> GetUserByEmail(string email)
        {
            try
            {
                var user = _userService.FindUserByEmail(email);
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
        
        [HttpGet("GetUserByPhone/{Phone}")]
        public ActionResult<RegisterUserDto> GetUserByPhone(string phone)
        {
            try
            {
                var user = _userService.FindUserByPhone(phone);
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
        
        [HttpPut("UpdateUser/{UserId}")]
        public ActionResult<RegisterUserDto> ModifyUser([FromBody] RegisterUserDto userDto)
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new ApiResponse { Status = "Error", Message = "Invalid token" });
                }
                
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);

                var userId = jwtToken.Payload["UserId"].ToString();

                var existingUser = _userService.FindUserById(int.Parse(userId));
                if (existingUser == null)
                {
                    return NotFound(new ApiResponse { Status = "Error", Message = "User not found" });
                }

                var updatedUser = _userService.ModifyUser(userDto, int.Parse(userId));

                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Status = "Error", Message = ex.Message });
            }
        }

        [HttpDelete("DeleteUser/{UserId}"), Authorize(Roles = "System Administrator")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                var existingUser = _userService.FindUserById(id);
                if (existingUser == null)
                {
                    return NotFound("User not found.");
                }

                _userService.DeleteUserById(id);

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
