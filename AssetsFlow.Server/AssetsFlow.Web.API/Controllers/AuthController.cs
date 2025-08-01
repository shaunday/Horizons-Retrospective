using Asp.Versioning;
using HsR.UserService.Client.Interfaces;
using HsR.UserService.Contracts;
using HsR.UserService.Protos;
using HsR.Web.API.Controllers;
using HsR.Web.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace AssetsFlowWeb.API.Controllers;

[Route("hsr-api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class AuthController : HsRControllerBase
{
    private readonly IUserServiceClient _userServiceClient;
    private readonly IJwtService _jwtService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IUserServiceClient userServiceClient, IJwtService jwtService, ILogger<AuthController> logger)
    {
        _userServiceClient = userServiceClient;
        _jwtService = jwtService;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            _logger.LogInformation("Login attempt for user: {Email}", request.Email);
            
            var response = await _userServiceClient.LoginAsync(request);
            
            if (response.Success)
            {
                _logger.LogInformation("Login successful for user: {Email}", request.Email);
                var roles = await _userServiceClient.GetUserRolesAsync(response.User.Id);
                var token = _jwtService.GenerateToken(Guid.Parse(response.User.Id), roles);
                return Ok(new
                {
                    success = true,
                    message = response.Message,
                    user = response.User,
                    token
                });
            }
            
            _logger.LogWarning("Login failed for user: {Email}, reason: {Message}", request.Email, response.Message);
            return BadRequest(new
            {
                success = false,
                message = response.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for user: {Email}", request.Email);
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred during login"
            });
        }
    }

    [HttpPost("login-demo")]
    public async Task<IActionResult> LoginAsDemo()
    {
        try
        {
            _logger.LogInformation("Demo login attempt");

            var request = new LoginRequest
            {
                Email = DemoUserData.Email,
                Password = DemoUserData.Password
            };

            var response = await _userServiceClient.LoginAsync(request);

            if (response.Success)
            {
                _logger.LogInformation("Demo login successful");

                var roles = await _userServiceClient.GetUserRolesAsync(response.User.Id);
                var token = _jwtService.GenerateToken(Guid.Parse(response.User.Id), roles);

                return Ok(new
                {
                    success = true,
                    message = response.Message,
                    user = response.User,
                    token
                });
            }

            _logger.LogWarning("Demo login failed: {Message}", response.Message);
            return BadRequest(new
            {
                success = false,
                message = response.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during demo login");
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred during demo login"
            });
        }
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            _logger.LogInformation("Registration attempt for user: {Email}", request.Email);
            
            var response = await _userServiceClient.RegisterAsync(request);
            
            if (response.Success)
            {
                _logger.LogInformation("Registration successful for user: {Email}", request.Email);
                var roles = await _userServiceClient.GetUserRolesAsync(response.User.Id);
                var token = _jwtService.GenerateToken(Guid.Parse(response.User.Id), roles);
                return Ok(new
                {
                    success = true,
                    message = response.Message,
                    user = response.User,
                    token
                });
            }
            
            _logger.LogWarning("Registration failed for user: {Email}, reason: {Message}", request.Email, response.Message);
            return BadRequest(new
            {
                success = false,
                message = response.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration for user: {Email}", request.Email);
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred during registration"
            });
        }
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUser(string userId)
    {
        try
        {
            var request = new GetUserRequest { UserId = userId };
            var response = await _userServiceClient.GetUserByIdAsync(request);
            
            if (response.Success)
            {
                return Ok(new
                {
                    success = true,
                    user = response.User
                });
            }
            
            return NotFound(new
            {
                success = false,
                message = response.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user: {UserId}", userId);
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred while retrieving user"
            });
        }
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        try
        {
            var response = await _userServiceClient.ChangePasswordAsync(request);
            
            if (response.Success)
            {
                return Ok(new
                {
                    success = true,
                    message = response.Message
                });
            }
            
            return BadRequest(new
            {
                success = false,
                message = response.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error changing password for user: {UserId}", request.UserId);
            return StatusCode(500, new
            {
                success = false,
                message = "An error occurred while changing password"
            });
        }
    }

    [HttpGet("health")]
    public async Task<IActionResult> HealthCheck()
    {
        try
        {
            var request = new HealthCheckRequest { Service = "UserService" };
            var response = await _userServiceClient.HealthCheckAsync(request);
            
            return Ok(new
            {
                status = response.Status,
                message = response.Message,
                timestamp = response.Timestamp
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during health check");
            return StatusCode(500, new
            {
                status = "Unhealthy",
                message = "Service unavailable",
                timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss UTC")
            });
        }
    }
} 