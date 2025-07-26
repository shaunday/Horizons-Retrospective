using HsR.UserService.Protos;

namespace HsR.UserService.Client.Interfaces;

public interface IUserServiceClient
{
    Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
    Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
    Task<UserResponse> GetUserByIdAsync(GetUserRequest request, CancellationToken cancellationToken = default);
    Task<UserResponse> GetUserByEmailAsync(GetUserByEmailRequest request, CancellationToken cancellationToken = default);
    Task<HealthCheckResponse> HealthCheckAsync(HealthCheckRequest request, CancellationToken cancellationToken = default);
    Task<ChangePasswordResponse> ChangePasswordAsync(ChangePasswordRequest request, CancellationToken cancellationToken = default);
    Task<IList<string>> GetUserRolesAsync(string userId, CancellationToken cancellationToken = default);
} 