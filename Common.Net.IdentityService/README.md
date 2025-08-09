# HsR UserService - gRPC Implementation

A minimal identity-based user microservice built with ASP.NET Core and gRPC, featuring user registration, authentication, and profile management.

## Features

- **gRPC Transport**: High-performance RPC communication
- **User Authentication**: Registration and login functionality
- **Profile Management**: Update user profiles and change passwords
- **ASP.NET Identity**: Built on Microsoft's identity framework
- **PostgreSQL**: Database storage with Entity Framework Core
- **Demo User**: Auto-created demo user for testing
- **Health Checks**: Built-in health monitoring

## Project Structure

```
HsR.UserService/
├── HsR.UserService/                 # Core business logic
│   ├── Entities/                    # Domain entities
│   ├── Models/                      # DTOs and request/response models
│   ├── Services/                    # Business services
│   ├── Data/                        # Database context and configuration
│   └── Protos/                      # gRPC protocol definitions
├── HsR.UserService.Host/            # gRPC server host
└── HsR.UserService.TestClient/      # Test client for development
```

## Quick Start

### Prerequisites

- .NET 8.0 SDK
- PostgreSQL database
- Visual Studio 2022 or VS Code

### 1. Database Setup

Update the connection string in `HsR.UserService.Host/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=AssetsFlow_Users;Username=postgres;Password=your_password;"
  }
}
```

### 2. Run the Service

```bash
cd HsR.UserService.Host
dotnet run
```

The service will start on:
- HTTPS: https://localhost:7001
- HTTP: http://localhost:7000

### 3. Test with Client

```bash
cd HsR.UserService.TestClient
dotnet run
```

## gRPC Service Methods

### Authentication

#### Register User
```protobuf
rpc Register (RegisterRequest) returns (AuthResponse)
```

#### Login User
```protobuf
rpc Login (LoginRequest) returns (AuthResponse)
```

### User Management

#### Get User by ID
```protobuf
rpc GetUserById (GetUserRequest) returns (UserResponse)
```

#### Get User by Email
```protobuf
rpc GetUserByEmail (GetUserByEmailRequest) returns (UserResponse)
```

#### Update Profile
```protobuf
rpc UpdateProfile (UpdateProfileRequest) returns (UserResponse)
```

#### Change Password
```protobuf
rpc ChangePassword (ChangePasswordRequest) returns (ChangePasswordResponse)
```

### Health Check
```protobuf
rpc HealthCheck (HealthCheckRequest) returns (HealthCheckResponse)
```

## Demo User

A demo user is automatically created when the service starts:

- **Email**: demo@horizons-retrospective.com
- **Password**: demo123
- **Name**: Demo User

## Client Integration

### C# Client Example

```csharp
using var channel = GrpcChannel.ForAddress("https://localhost:7001");
var client = new UserService.UserServiceClient(channel);

// Login
var loginRequest = new LoginRequest
{
    Email = "demo@horizons-retrospective.com",
    Password = "demo123"
};

var response = await client.LoginAsync(loginRequest);
```

### JavaScript/TypeScript Client

```javascript
import { UserServiceClient } from './generated/user_service_grpc_web_pb.js';

const client = new UserServiceClient('https://localhost:7001');

const request = new LoginRequest();
request.setEmail('demo@horizons-retrospective.com');
request.setPassword('demo123');

client.login(request, (error, response) => {
    if (error) {
        console.error('Error:', error);
        return;
    }
    console.log('Login successful:', response.toObject());
});
```

## Configuration

### Password Requirements

- Minimum length: 4 characters
- Configured in `AuthValidation.PasswordMinLength`

### Database

- Uses PostgreSQL with Entity Framework Core
- Automatic migrations and database creation
- ASP.NET Identity tables for user management

## Development

### Building

```bash
dotnet build
```

### Testing

```bash
cd HsR.UserService.TestClient
dotnet run
```

### Adding New Methods

1. Update the proto file in `Protos/user_service.proto`
2. Implement the method in `Services/UserGrpcService.cs`
3. Add corresponding business logic in `Services/UserService.cs`

## Next Steps

1. **JWT Integration**: Add JWT token generation and validation
2. **Journal Integration**: Connect users to the existing journal database
3. **Role-based Authorization**: Implement user roles and permissions
4. **Email Verification**: Add email confirmation functionality
5. **Password Reset**: Implement password reset via email
6. **Rate Limiting**: Add request rate limiting
7. **Metrics**: Add Prometheus metrics and monitoring

## Troubleshooting

### Common Issues

1. **Database Connection**: Ensure PostgreSQL is running and connection string is correct
2. **Port Conflicts**: Check if ports 7000/7001 are available
3. **SSL Certificate**: In development, the service uses development certificates

### Logs

The service uses Serilog for structured logging. Check console output for detailed error messages.

## Security Notes

- This is a development setup with minimal security
- For production, implement proper SSL certificates
- Add authentication middleware for protected endpoints
- Implement proper password hashing and validation
- Add rate limiting and request validation 