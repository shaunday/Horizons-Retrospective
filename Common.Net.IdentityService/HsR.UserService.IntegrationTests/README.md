# UserService Integration Tests

This project contains xUnit integration tests for the HsR UserService gRPC API.

## Configuration

The tests can be configured using either `appsettings.json` or environment variables. Environment variables take precedence over appsettings.json.

### Configuration Options

| Setting | appsettings.json | Environment Variable | Default |
|---------|------------------|---------------------|---------|
| gRPC Service URL | `GrpcService:BaseUrl` | `GRPC_SERVICE_URL` | `https://localhost:7001` |
| Default Test Password | `TestSettings:DefaultPassword` | `TEST_DEFAULT_PASSWORD` | `test1234` |
| New Test Password | `TestSettings:NewPassword` | `TEST_NEW_PASSWORD` | `newpass1234` |

### Example appsettings.json

```json
{
  "GrpcService": {
    "BaseUrl": "https://localhost:7001"
  },
  "TestSettings": {
    "DefaultPassword": "test1234",
    "NewPassword": "newpass1234"
  }
}
```

### Example Environment Variables

```bash
GRPC_SERVICE_URL=https://localhost:7001
TEST_DEFAULT_PASSWORD=test1234
TEST_NEW_PASSWORD=newpass1234
```

## Running Tests

1. Ensure the gRPC UserService is running
2. Configure the service URL if different from default
3. Run the tests:

```bash
dotnet test HsR.UserService.IntegrationTests/HsR.UserService.IntegrationTests.csproj
```

## Test Coverage

- HealthCheck
- User Registration
- User Login
- Get User by Email
- Get User by ID
- Change Password

Each test uses a unique email address to avoid conflicts between test runs. 