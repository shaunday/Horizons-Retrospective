using Asp.Versioning;
using HsR.Common.AspNet.Authentication;
using HsR.UserService.Client.Interfaces;
using HsR.UserService.Contracts;
using HsR.UserService.Protos;
using HsR.Web.API.Controllers;
using HsR.Web.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AssetsFlowWeb.API.Controllers;

[Route("hsr-api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class AuthController : HsRControllerBase
{
    private readonly IUserServiceClient _userServiceClient;
    private readonly IJwtService _jwtService;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IUserServiceClient userServiceClient,
        IJwtService jwtService,
        IRefreshTokenService refreshTokenService,
        ILogger<AuthController> logger)
    {
        _userServiceClient = userServiceClient;
        _jwtService = jwtService;
        _refreshTokenService = refreshTokenService;
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

                var roles = await _userServiceClient.GetUserRolesAsync(response.User.Id) ?? Array.Empty<string>();
                var userId = Guid.Parse(response.User.Id);

                var token = _jwtService.GenerateToken(userId, roles);
                var refreshToken = _refreshTokenService.GenerateRefreshToken();
                await _refreshTokenService.SaveRefreshTokenAsync(userId, refreshToken);

                return Ok(new
                {
                    success = true,
                    message = response.Message,
                    user = response.User,
                    token,
                    refreshToken
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

                var roles = await _userServiceClient.GetUserRolesAsync(response.User.Id) ?? Array.Empty<string>();
                var userId = Guid.Parse(response.User.Id);

                var token = _jwtService.GenerateToken(userId, roles);
                var refreshToken = _refreshTokenService.GenerateRefreshToken();
                await _refreshTokenService.SaveRefreshTokenAsync(userId, refreshToken);

                return Ok(new
                {
                    success = true,
                    message = response.Message,
                    user = response.User,
                    token,
                    refreshToken
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

                var roles = await _userServiceClient.GetUserRolesAsync(response.User.Id) ?? Array.Empty<string>();
                var userId = Guid.Parse(response.User.Id);

                var token = _jwtService.GenerateToken(userId, roles);
                var refreshToken = _refreshTokenService.GenerateRefreshToken();
                await _refreshTokenService.SaveRefreshTokenAsync(userId, refreshToken);

                return Ok(new
                {
                    success = true,
                    message = response.Message,
                    user = response.User,
                    token,
                    refreshToken
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


    //[HttpPost("refresh-token")]
    //public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    //{
    //    try
    //    {
    //        var principal = _refreshTokenService.GetPrincipalFromExpiredToken(request.AccessToken);
    //        if (principal == null)
    //            return BadRequest(new { success = false, message = "Invalid access token" });

    //        var userIdString = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    //        if (!Guid.TryParse(userIdString, out var userId))
    //            return BadRequest(new { success = false, message = "Invalid user ID in token" });

    //        var isValidRefresh = await _refreshTokenService.ValidateRefreshTokenAsync(userId, request.RefreshToken);
    //        if (!isValidRefresh)
    //            return Unauthorized(new { success = false, message = "Invalid refresh token" });

    //        var userRoles = await _userServiceClient.GetUserRolesAsync(userIdString) ?? Array.Empty<string>();

    //        var newJwtToken = _jwtService.GenerateToken(userId, userRoles);
    //        var newRefreshToken = _refreshTokenService.GenerateRefreshToken();

    //        await _refreshTokenService.ReplaceRefreshTokenAsync(userId, request.RefreshToken, newRefreshToken);

    //        _logger.LogInformation("Refresh token used for user {UserId}", userId);

    //        return Ok(new
    //        {
    //            success = true,
    //            token = newJwtToken,
    //            refreshToken = newRefreshToken
    //        });
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex, "Error refreshing token");
    //        return StatusCode(500, new { success = false, message = "An error occurred refreshing token" });
    //    }
    //}

    //[HttpPost("revoke-refresh-token")]
    //public async Task<IActionResult> RevokeRefreshToken([FromBody] RevokeRefreshTokenRequest request)
    //{
    //    try
    //    {
    //        if (!Guid.TryParse(request.UserId, out var userId))
    //            return BadRequest(new { success = false, message = "Invalid user ID" });

    //        await _refreshTokenService.RevokeRefreshTokenAsync(userId);
    //        return Ok(new { success = true, message = "Refresh token revoked" });
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex, "Error revoking refresh token");
    //        return StatusCode(500, new { success = false, message = "An error occurred revoking token" });
    //    }
    //}


    //public class RefreshTokenRequest
    //{
    //    public string AccessToken { get; set; } = null!; // expired JWT token
    //    public string RefreshToken { get; set; } = null!;
    //}

    //public class RevokeRefreshTokenRequest
    //{
    //    public string UserId { get; set; } = null!;
    //}
}