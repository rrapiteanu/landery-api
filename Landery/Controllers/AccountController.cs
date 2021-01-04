using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Landery.Infrastructure;
using Landery.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Landery.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IUserService _userService;
        private readonly IJwtAuthManager _jwtAuthManager;

        public AccountController(ILogger<AccountController> logger, IUserService userService, IJwtAuthManager jwtAuthManager)
        {
            _logger = logger;
            _userService = userService;
            _jwtAuthManager = jwtAuthManager;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!_userService.IsValidUserCredentials(request.Email, request.Password))
            {
                return Unauthorized();
            }

            var role = _userService.GetUserRole(request.Email);
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, request.Email),
                new Claim(ClaimTypes.Email, request.Email),
                new Claim(ClaimTypes.Role, role)
            };

            var jwtResult = _jwtAuthManager.GenerateToken(request.Email, claims, DateTime.Now);
            _logger.LogInformation($"User [{request.Email}] logged in the system.");
            return Ok(new LoginResult
            {
                Email = request.Email,
                Role = role,
                AccessToken = jwtResult.AccessToken
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public ActionResult Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (_userService.IsAnExistingUser(request.Email))
            {
                return Conflict();
            }

            // add in db
            _userService.AddUser(request.FirstName, request.LastName, request.Email, request.Password);


            return Ok(new RegisterResult
            {
                Email = request.Email,
            });
        }

        [HttpGet("user")]
        [Authorize]
        public ActionResult GetCurrentUser()
        {
            return Ok(new LoginResult
            {
                Email = User.Identity.Name,
                Role = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty,
            });
        }

        public class LoginRequest
        {
            [Required]
            [JsonPropertyName("Email")]
            public string Email { get; set; }

            [Required]
            [JsonPropertyName("password")]
            public string Password { get; set; }
        }

        public class RegisterRequest
        {
            [Required]
            [JsonPropertyName("firstname")]
            public string FirstName { get; set; }

            [Required]
            [JsonPropertyName("lastname")]
            public string LastName { get; set; }


            [Required]
            [JsonPropertyName("email")]
            public string Email { get; set; }

            [Required]
            [JsonPropertyName("password")]
            public string Password { get; set; }
        }

        public class RegisterResult
        {
            [JsonPropertyName("email")]
            public string Email { get; set; }
        }

        public class LoginResult
        {
            [JsonPropertyName("email")]
            public string Email { get; set; }

            [JsonPropertyName("role")]
            public string Role { get; set; }

            [JsonPropertyName("accessToken")]
            public string AccessToken { get; set; }
        }

    }
}