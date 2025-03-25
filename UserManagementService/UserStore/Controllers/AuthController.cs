using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserStore.Business;
using UserStore.Business.Auth;
using UserStore.Business.Users;
using UserStore.Models;

namespace UserStore.Controllers
{
    [Route("API/v1.0/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;

        private readonly IUserService _userService;

        public AuthController(IConfiguration configuration, IAuthService authService, IUserService userService)
        {
            _configuration = configuration;
            _authService = authService;
            _userService = userService;
        }
        
        [HttpPost("Register")]
        public ActionResult<RegisterUserDto> AddUser([FromBody] RegisterUserDto model)
        {
            try 
            {
                var userDto = _userService.FindUserByUsername(model.Username);
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
        
        [HttpPost("Login")]
        public IActionResult ValidateUser([FromBody] LoginUserDto model)
        {
            try 
            {
                var userStatus = _authService.Login(model);
                
                if (userStatus.Equals("User not found"))
                {
                    return NotFound(userStatus);
                }
                
                if (userStatus.Equals("Invalid password"))
                {
                    return Unauthorized(userStatus);
                }
                
                if (userStatus.Equals("Login successful"))
                {
                    var user = _userService.FindUserByUsername(model.Username);

                    string key = _configuration["JWT:Key"];
                    string issuer = _configuration["JWT:Issuer"];
                    string audience = _configuration["JWT:Audience"];
                    
                    if (user.UserId > 1)
                    {
                        var token = _authService.GenerateToken(user.UserId.ToString(), user.Username, "User", key, issuer, audience);
                        var jwtHandler = new JwtSecurityTokenHandler();
                        var tokenString = jwtHandler.WriteToken(token);

                        return Ok(tokenString); // User Token
                    }
                    
                    return Ok(Setup.token); // Admin Token
                }

                return Unauthorized();
            }
            catch (ArgumentNullException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Status = "Error", Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Status = "Error", Message = ex.Message });
            }
        }
        
        [HttpPost]
        [Route("Verify-Email")]
        public ActionResult<string> VerifyEmail([FromBody] LoginUserDto loginUserDto, string email)
        {
            var user = _userService.FindUserByEmail(email);
            var isVerified = _authService.IsEmailVerified(email);
            if (isVerified)
            {
                return Ok("Email already verified");
            }
            _authService.UpdateEmailVerificationDate(user);

            return Ok("Email verified successfully");
        }
        
        [HttpPost]
        [Route("Verify-Phone")]
        public ActionResult<string> VerifyPhone([FromBody] LoginUserDto loginUserDto, string phone)
        {
            var user = _userService.FindUserByPhone(phone);
            var isVerified = _authService.IsPhoneVerified(phone);
            if (isVerified)
            {
                return Ok("Phone number already verified");
            }
            _authService.UpdatePhoneVerificationDate(user);

            return Ok("Phone number verified successfully");
        }
    }
}
