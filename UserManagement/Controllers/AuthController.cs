using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Business;
using UserManagement.Business.Auth;
using UserManagement.Models;

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;

        public AuthController(IConfiguration configuration, IAuthService authService)
        {
            _configuration = configuration;
            _authService = authService;
        }

        [HttpPost("Register")]
        public ActionResult<RegisterUserDto> AddUser([FromBody] RegisterUserDto model)
        {
            try 
            {
                var userDto = _authService.FindUserByUsername(model.Username);
                if (userDto != null)
                {
                    return BadRequest($"Username \"{model.Username}\" already exists.");
                }

                var user = _authService.Register(model);

                return Ok(user);
            }
            catch (ArgumentNullException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Status = "Error", Message = ex.Message });
            }
            catch (MyException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse { Status = "Error", Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Status = "Error", Message = ex.Message });
            }
        }
    }
}
