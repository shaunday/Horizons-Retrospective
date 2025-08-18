using Grpc.Net.Client;
using HsR.UserService.Client.Interfaces;
using HsR.UserService.Client.Models;
using HsR.UserService.Contracts;
using HsR.UserService.Protos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HsR.UserService.Client;

public class UserServiceClient : IUserServiceClient
{
    private readonly Protos.UserService.UserServiceClient _grpcClient;
    private readonly ILogger<UserServiceClient> _logger;

    public UserServiceClient(GrpcChannel channel, ILogger<UserServiceClient> logger)
    {
        _logger = logger;
        _grpcClient = new HsR.UserService.Protos.UserService.UserServiceClient(channel);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Attempting to login user with email: {Email}", request.Email);

            var response = await _grpcClient.LoginAsync(request, cancellationToken: cancellationToken);

            _logger.LogInformation("Login attempt for user {Email} completed with success: {Success}",
                request.Email, response.Success);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during login for user {Email}", request.Email);
            throw;
        }
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Attempting to register user with email: {Email}", request.Email);

            var response = await _grpcClient.RegisterAsync(request, cancellationToken: cancellationToken);

            _logger.LogInformation("Registration attempt for user {Email} completed with success: {Success}",
                request.Email, response.Success);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during registration for user {Email}", request.Email);
            throw;
        }
    }

    public async Task<UserResponse> GetUserByIdAsync(GetUserRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Attempting to get user by ID: {UserId}", request.UserId);

            var response = await _grpcClient.GetUserByIdAsync(request, cancellationToken: cancellationToken);

            _logger.LogDebug("Get user by ID {UserId} completed with success: {Success}",
                request.UserId, response.Success);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting user by ID {UserId}", request.UserId);
            throw;
        }
    }

    public async Task<UserResponse> GetUserByEmailAsync(GetUserByEmailRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Attempting to get user by email: {Email}", request.Email);

            var response = await _grpcClient.GetUserByEmailAsync(request, cancellationToken: cancellationToken);

            _logger.LogDebug("Get user by email {Email} completed with success: {Success}",
                request.Email, response.Success);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting user by email {Email}", request.Email);
            throw;
        }
    }

    public async Task<HealthCheckResponse> HealthCheckAsync(HealthCheckRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Performing health check for service: {Service}", request.Service);

            var response = await _grpcClient.HealthCheckAsync(request, cancellationToken: cancellationToken);

            _logger.LogDebug("Health check for service {Service} completed with status: {Status}",
                request.Service, response.Status);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during health check for service {Service}", request.Service);
            throw;
        }
    }

    public async Task<string> GetServiceVersionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Fetching service version");

            var response = await _grpcClient.InfoAsync(new InfoRequest(), cancellationToken: cancellationToken);

            _logger.LogInformation("Service version received: {Version}", response.Version);

            return response.Version;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching service version");
            return "unknown";
        }
    }


    public async Task<ChangePasswordResponse> ChangePasswordAsync(ChangePasswordRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Attempting to change password for user: {UserId}", request.UserId);

            var response = await _grpcClient.ChangePasswordAsync(request, cancellationToken: cancellationToken);

            _logger.LogInformation("Change password attempt for user {UserId} completed with success: {Success}",
                request.UserId, response.Success);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during password change for user {UserId}", request.UserId);
            throw;
        }
    }

    public async Task<IList<string>> GetUserRolesAsync(string userId, CancellationToken cancellationToken = default)
    {
        var request = new GetUserRolesRequest { UserId = userId };
        var response = await _grpcClient.GetUserRolesAsync(request, cancellationToken: cancellationToken);
        return response.Roles;
    }
}