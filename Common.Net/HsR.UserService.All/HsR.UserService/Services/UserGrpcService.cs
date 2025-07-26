using HsR.UserService.Models;
using HsR.UserService.Protos;
using Microsoft.AspNetCore.Identity;
using HsR.UserService.Entities;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace HsR.UserService.Services
{
    public class UserGrpcService : Protos.UserService.UserServiceBase
    {
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UserGrpcService> _logger;

        public UserGrpcService(IUserService userService, UserManager<User> userManager, ILogger<UserGrpcService> logger)
        {
            _userService = userService;
            _userManager = userManager;
            _logger = logger;
        }

        public override async Task<Protos.AuthResponse> Login(Protos.LoginRequest request, ServerCallContext context)
        {
            try
            {
                var loginRequest = new Models.LoginRequest
                {
                    Email = request.Email,
                    Password = request.Password
                };

                var result = await _userService.LoginAsync(loginRequest);
                return MapToGrpcAuthResponse(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for email: {Email}", request.Email);
                return new Protos.AuthResponse
                {
                    Success = false,
                    Message = "An error occurred during login"
                };
            }
        }

        public override async Task<Protos.AuthResponse> Register(Protos.RegisterRequest request, ServerCallContext context)
        {
            try
            {
                var registerRequest = new Models.RegisterRequest
                {
                    Email = request.Email,
                    Password = request.Password,
                    FirstName = request.FirstName,
                    LastName = request.LastName
                };

                var result = await _userService.RegisterAsync(registerRequest);
                return MapToGrpcAuthResponse(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration for email: {Email}", request.Email);
                return new Protos.AuthResponse
                {
                    Success = false,
                    Message = "An error occurred during registration"
                };
            }
        }

        public override async Task<Protos.UserResponse> GetUserById(Protos.GetUserRequest request, ServerCallContext context)
        {
            try
            {
                if (!Guid.TryParse(request.UserId, out var userId))
                {
                    return new Protos.UserResponse
                    {
                        Success = false,
                        Message = "Invalid user ID format"
                    };
                }

                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user is null)
                {
                    return new Protos.UserResponse
                    {
                        Success = false,
                        Message = "User not found"
                    };
                }

                return new Protos.UserResponse
                {
                    Success = true,
                    Message = "User found",
                    User = MapToGrpcUserDto(user)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by ID: {UserId}", request.UserId);
                return new Protos.UserResponse
                {
                    Success = false,
                    Message = "An error occurred while retrieving user"
                };
            }
        }

        public override async Task<Protos.UserResponse> GetUserByEmail(Protos.GetUserByEmailRequest request, ServerCallContext context)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user is null)
                {
                    return new Protos.UserResponse
                    {
                        Success = false,
                        Message = "User not found"
                    };
                }

                return new Protos.UserResponse
                {
                    Success = true,
                    Message = "User found",
                    User = MapToGrpcUserDto(user)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by email: {Email}", request.Email);
                return new Protos.UserResponse
                {
                    Success = false,
                    Message = "An error occurred while retrieving user"
                };
            }
        }

        public override async Task<GetUserRolesResponse> GetUserRoles(GetUserRolesRequest request, ServerCallContext context)
        {
            var response = new GetUserRolesResponse();
            if (!Guid.TryParse(request.UserId, out var userId))
                return response;
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return response;
            var roles = await _userManager.GetRolesAsync(user);
            response.Roles.AddRange(roles);
            return response;
        }

        public override Task<Protos.HealthCheckResponse> HealthCheck(Protos.HealthCheckRequest request, ServerCallContext context)
        {
            return Task.FromResult(new Protos.HealthCheckResponse
            {
                Status = "Healthy",
                Message = "User Service is running",
                Timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss UTC")
            });
        }

        public override async Task<Protos.ChangePasswordResponse> ChangePassword(Protos.ChangePasswordRequest request, ServerCallContext context)
        {
            try
            {
                if (!Guid.TryParse(request.UserId, out var userId))
                {
                    return new Protos.ChangePasswordResponse
                    {
                        Success = false,
                        Message = "Invalid user ID format"
                    };
                }

                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user is null)
                {
                    return new Protos.ChangePasswordResponse
                    {
                        Success = false,
                        Message = "User not found"
                    };
                }

                var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
                if (!result.Succeeded)
                {
                    return new Protos.ChangePasswordResponse
                    {
                        Success = false,
                        Message = string.Join(", ", result.Errors.Select(e => e.Description))
                    };
                }

                return new Protos.ChangePasswordResponse
                {
                    Success = true,
                    Message = "Password changed successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing password for user: {UserId}", request.UserId);
                return new Protos.ChangePasswordResponse
                {
                    Success = false,
                    Message = "An error occurred while changing password"
                };
            }
        }

        private static Protos.AuthResponse MapToGrpcAuthResponse(Models.AuthResponse response) => new()
        {
            Success = response.Success,
            Message = response.Message ?? string.Empty,
            User = response.User is not null ? MapToGrpcUserDto(response.User) : null
        };

        private static Protos.UserDto MapToGrpcUserDto(Models.UserDto user) => new()
        {
            Id = user.Id.ToString(),
            Email = user.Email,
            FullName = user.FullName,
            CreatedAt = user.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss UTC"),
            FirstName = user.FirstName ?? string.Empty,
            LastName = user.LastName ?? string.Empty
        };

        private static Protos.UserDto MapToGrpcUserDto(User user) => new()
        {
            Id = user.Id.ToString(),
            Email = user.Email ?? string.Empty,
            FullName = user.FullName,
            CreatedAt = user.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss UTC"),
            FirstName = user.FirstName ?? string.Empty,
            LastName = user.LastName ?? string.Empty
        };
    }
} 