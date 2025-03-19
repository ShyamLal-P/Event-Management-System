using EventManagementSystem.Interface;
using EventManagementSystem.Models.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EventManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;
        private readonly ILogger<AuthController> logger;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository, ILogger<AuthController> logger)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
            this.logger = logger;
        }

        //Post: /api/Auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var idetityUser = new IdentityUser
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username,
            };
            var identityResult = await userManager.CreateAsync(idetityUser, registerRequestDto.Password);
            if (identityResult.Succeeded)
            {
                //Add roles to this user
                if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                {
                    identityResult = await userManager.AddToRolesAsync(idetityUser, registerRequestDto.Roles);
                    if (identityResult.Succeeded)
                    {
                        return Ok("User Was registered! Please Log in Now");
                    }
                }
            }
            return BadRequest("Something Went Wrong! Try Again");
        }

        //Post: /api/Auth/Login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            logger.LogInformation("Login attempt for user: {Username}", loginRequestDto.Username);

            var user = await userManager.FindByEmailAsync(loginRequestDto.Username);
            if (user != null)
            {
                logger.LogInformation("User found: {Username}", loginRequestDto.Username);

                var checkPasswordResult = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);
                if (checkPasswordResult)
                {
                    logger.LogInformation("Password check succeeded for user: {Username}", loginRequestDto.Username);

                    // Get the roles for this user
                    var roles = await userManager.GetRolesAsync(user);
                    if (roles != null)
                    {
                        logger.LogInformation("Roles retrieved for user: {Username}", loginRequestDto.Username);

                        // Creating a JWT Token after logging in 
                        var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList());
                        var response = new LoginReponseDto
                        {
                            jwtToken = jwtToken
                        };
                        return Ok(response);
                    }
                    else
                    {
                        logger.LogWarning("No roles found for user: {Username}", loginRequestDto.Username);
                    }
                }
                else
                {
                    logger.LogWarning("Invalid password for user: {Username}", loginRequestDto.Username);
                    return BadRequest("Invalid password.");
                }
            }
            else
            {
                logger.LogWarning("User not found: {Username}", loginRequestDto.Username);
                return BadRequest("User not found.");
            }
            return BadRequest("Something went wrong! Please try again.");
        }
}
}
