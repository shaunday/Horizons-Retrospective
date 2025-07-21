using HsR.UserService.Protos;
using Grpc.Net.Client;
using static HsR.UserService.Protos.UserService;

Console.WriteLine("HsR UserService gRPC Test Client");
Console.WriteLine("================================\n");

// Create a channel to the gRPC server
using var channel = GrpcChannel.ForAddress("https://localhost:7001");
var client = new UserServiceClient(channel);

try
{
    // Test health check
    Console.WriteLine("Testing Health Check...");
    var healthResponse = await client.HealthCheckAsync(new HealthCheckRequest { Service = "UserService" });
    Console.WriteLine($"Health Status: {healthResponse.Status}");
    Console.WriteLine($"Message: {healthResponse.Message}");
    Console.WriteLine($"Timestamp: {healthResponse.Timestamp}\n");

    // Test registration
    Console.WriteLine("Testing User Registration...");
    var registerRequest = new RegisterRequest
    {
        Email = "test@example.com",
        Password = "test123",
        FirstName = "Test",
        LastName = "User"
    };

    var registerResponse = await client.RegisterAsync(registerRequest);
    Console.WriteLine($"Registration Success: {registerResponse.Success}");
    Console.WriteLine($"Message: {registerResponse.Message}");
    if (registerResponse.Success && registerResponse.User is not null)
    {
        Console.WriteLine($"User ID: {registerResponse.User.Id}");
        Console.WriteLine($"Email: {registerResponse.User.Email}");
        Console.WriteLine($"Full Name: {registerResponse.User.FullName}\n");
    }

    // Test login
    Console.WriteLine("Testing User Login...");
    var loginRequest = new LoginRequest
    {
        Email = "test@example.com",
        Password = "test123"
    };

    var loginResponse = await client.LoginAsync(loginRequest);
    Console.WriteLine($"Login Success: {loginResponse.Success}");
    Console.WriteLine($"Message: {loginResponse.Message}");
    if (loginResponse.Success && loginResponse.User is not null)
    {
        Console.WriteLine($"User ID: {loginResponse.User.Id}");
        Console.WriteLine($"Email: {loginResponse.User.Email}");
        Console.WriteLine($"Full Name: {loginResponse.User.FullName}\n");
    }

    // Test get user by email
    if (loginResponse.Success && loginResponse.User is not null)
    {
        Console.WriteLine("Testing Get User By Email...");
        var getUserRequest = new GetUserByEmailRequest
        {
            Email = "test@example.com"
        };

        var getUserResponse = await client.GetUserByEmailAsync(getUserRequest);
        Console.WriteLine($"Get User Success: {getUserResponse.Success}");
        Console.WriteLine($"Message: {getUserResponse.Message}");
        if (getUserResponse.Success && getUserResponse.User is not null)
        {
            Console.WriteLine($"User ID: {getUserResponse.User.Id}");
            Console.WriteLine($"Email: {getUserResponse.User.Email}");
            Console.WriteLine($"Full Name: {getUserResponse.User.FullName}\n");
        }
    }

    Console.WriteLine("All tests completed successfully!");
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
    Console.WriteLine("Make sure the gRPC server is running on https://localhost:7001");
}

Console.WriteLine("\nPress any key to exit...");
Console.ReadKey(); 