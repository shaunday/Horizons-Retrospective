# HsR.UserService.Client

A .NET client library for communicating with the HsR User Service via gRPC.

## Features

- **gRPC Communication**: High-performance communication with the User Service
- **Dependency Injection**: Easy integration with ASP.NET Core DI container
- **Configuration**: Flexible configuration through appsettings.json or code
- **Logging**: Comprehensive logging for debugging and monitoring
- **Error Handling**: Proper exception handling and logging
- **Connection Pooling**: Optimized connection management

## Installation

Add the project reference to your ASP.NET Core project:

```xml
<ProjectReference Include="..\HsR.UserService.Client\HsR.UserService.Client.csproj" />
```

## Configuration

### Option 1: Using appsettings.json

Add the following configuration to your `appsettings.json`:

```json
{
  "UserService": {
    "BaseUrl": "https://your-user-service-url:port",
    "TimeoutSeconds": 30,
    "MaxRetryAttempts": 3,
    "EnableRetryOnFailure": true
  }
}
```

### Option 2: Using Code Configuration

```csharp
services.AddUserServiceClient(options =>
{
    options.BaseUrl = "https://your-user-service-url:port";
    options.TimeoutSeconds = 30;
    options.MaxRetryAttempts = 3;
    options.EnableRetryOnFailure = true;
});
```

## Usage

### 1. Register the Client

In your `Program.cs` or `Startup.cs`:

```csharp
using HsR.UserService.Client.Extensions;

// Option 1: Using configuration
builder.Services.AddUserServiceClient(builder.Configuration);

// Option 2: Using code configuration
builder.Services.AddUserServiceClient(options =>
{
    options.BaseUrl = "https://your-user-service-url:port";
});
```

### 2. Inject and Use the Client

```csharp
using HsR.UserService.Client.Interfaces;
using HsR.UserService.Protos;

public class UserController : ControllerBase
{
    private readonly IUserServiceClient _userServiceClient;

    public UserController(IUserServiceClient userServiceClient)
    {
        _userServiceClient = userServiceClient;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var response = await _userServiceClient.LoginAsync(request);
            
            if (response.Success)
            {
                return Ok(response);
            }
            
            return BadRequest(response.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error");
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
                return Ok(response.User);
            }
            
            return NotFound(response.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error");
        }
    }
}
```

## Available Operations

The client provides the following operations:

- `LoginAsync(LoginRequest)` - Authenticate a user
- `RegisterAsync(RegisterRequest)` - Register a new user
- `GetUserByIdAsync(GetUserRequest)` - Get user by ID
- `GetUserByEmailAsync(GetUserByEmailRequest)` - Get user by email
- `HealthCheckAsync(HealthCheckRequest)` - Check service health
- `ChangePasswordAsync(ChangePasswordRequest)` - Change user password

## Error Handling

The client includes comprehensive error handling:

- **Network Errors**: Automatically logged and re-thrown
- **gRPC Errors**: Properly handled and logged
- **Timeout Errors**: Configurable timeout handling
- **Retry Logic**: Optional retry on failure

## Logging

The client logs all operations with appropriate log levels:

- **Debug**: Detailed operation information
- **Information**: Successful operations
- **Error**: Failed operations with exception details

## Performance Considerations

- **Connection Pooling**: The client uses connection pooling for optimal performance
- **Keep-Alive**: Configured keep-alive settings for long-running connections
- **Message Size Limits**: Configurable message size limits (default: 10MB)
- **Timeout Configuration**: Configurable timeout settings

## Dependencies

- `Grpc.Net.Client` - gRPC client library
- `Google.Protobuf` - Protocol Buffers
- `Microsoft.Extensions.*` - Configuration and DI support 