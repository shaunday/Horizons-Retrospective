using System;
using System.Threading.Tasks;
using Grpc.Net.Client;
using HsR.UserService.Protos;
using Xunit;
using static HsR.UserService.Protos.UserService;

namespace HsR.UserService.IntegrationTests
{
    public class UserServiceGrpcTests : IAsyncLifetime
    {
        private readonly UserServiceClient _client;
        private readonly string _testEmail;
        private string? _userId;

        public UserServiceGrpcTests()
        {
            var channel = GrpcChannel.ForAddress(TestConfiguration.GrpcServiceUrl);
            _client = new UserServiceClient(channel);
            _testEmail = $"testuser_{Guid.NewGuid():N}@example.com";
        }

        [Fact(DisplayName = "HealthCheck returns Healthy status")]
        public async Task HealthCheck_Works()
        {
            var response = await _client.HealthCheckAsync(new HealthCheckRequest { Service = "UserService" });
            Assert.Equal("Healthy", response.Status);
            Assert.NotNull(response.Message);
            Assert.NotNull(response.Timestamp);
        }

        [Fact(DisplayName = "HealthCheck with different service name still works")]
        public async Task HealthCheck_WithDifferentServiceName_Works()
        {
            var response = await _client.HealthCheckAsync(new HealthCheckRequest { Service = "AnyService" });
            Assert.Equal("Healthy", response.Status);
            Assert.NotNull(response.Message);
            Assert.NotNull(response.Timestamp);
        }

        [Fact(DisplayName = "Register creates a new user")]
        public async Task Register_Works()
        {
            var response = await _client.RegisterAsync(new RegisterRequest
            {
                Email = _testEmail,
                Password = TestConfiguration.DefaultPassword,
                FirstName = "Test",
                LastName = "User"
            });
            Assert.True(response.Success, response.Message);
            Assert.NotNull(response.User);
            Assert.Equal(_testEmail, response.User.Email);
            Assert.Equal("Test User", response.User.FullName);
            _userId = response.User.Id;
        }

        [Fact(DisplayName = "Register with existing email returns failure")]
        public async Task Register_ExistingEmail_ReturnsFailure()
        {
            // First registration
            await Register_Works();
            
            // Second registration with same email
            var response = await _client.RegisterAsync(new RegisterRequest
            {
                Email = _testEmail,
                Password = TestConfiguration.DefaultPassword,
                FirstName = "Test",
                LastName = "User"
            });
            Assert.False(response.Success);
            Assert.Contains("already exists", response.Message);
        }

        [Fact(DisplayName = "Register with invalid email returns failure")]
        public async Task Register_InvalidEmail_ReturnsFailure()
        {
            var response = await _client.RegisterAsync(new RegisterRequest
            {
                Email = "invalid-email",
                Password = TestConfiguration.DefaultPassword,
                FirstName = "Test",
                LastName = "User"
            });
            Assert.False(response.Success);
        }

        [Fact(DisplayName = "Login authenticates the user")]
        public async Task Login_Works()
        {
            // Register first
            await Register_Works();
            var response = await _client.LoginAsync(new LoginRequest
            {
                Email = _testEmail,
                Password = TestConfiguration.DefaultPassword
            });
            Assert.True(response.Success, response.Message);
            Assert.NotNull(response.User);
            Assert.Equal(_testEmail, response.User.Email);
            _userId = response.User.Id;
        }

        [Fact(DisplayName = "Login with wrong password returns failure")]
        public async Task Login_WrongPassword_ReturnsFailure()
        {
            // Register first
            await Register_Works();
            var response = await _client.LoginAsync(new LoginRequest
            {
                Email = _testEmail,
                Password = "wrongpassword"
            });
            Assert.False(response.Success);
            Assert.Contains("Invalid email or password", response.Message);
        }

        [Fact(DisplayName = "Login with non-existent email returns failure")]
        public async Task Login_NonExistentEmail_ReturnsFailure()
        {
            var response = await _client.LoginAsync(new LoginRequest
            {
                Email = "nonexistent@example.com",
                Password = TestConfiguration.DefaultPassword
            });
            Assert.False(response.Success);
            Assert.Contains("Invalid email or password", response.Message);
        }

        [Fact(DisplayName = "GetUserByEmail returns the user")]
        public async Task GetUserByEmail_Works()
        {
            await Register_Works();
            var response = await _client.GetUserByEmailAsync(new GetUserByEmailRequest { Email = _testEmail });
            Assert.True(response.Success, response.Message);
            Assert.NotNull(response.User);
            Assert.Equal(_testEmail, response.User.Email);
            Assert.Equal("Test User", response.User.FullName);
            _userId = response.User.Id;
        }

        [Fact(DisplayName = "GetUserByEmail with non-existent email returns failure")]
        public async Task GetUserByEmail_NonExistentEmail_ReturnsFailure()
        {
            var response = await _client.GetUserByEmailAsync(new GetUserByEmailRequest { Email = "nonexistent@example.com" });
            Assert.False(response.Success);
            Assert.Contains("User not found", response.Message);
        }

        [Fact(DisplayName = "GetUserById returns the user")]
        public async Task GetUserById_Works()
        {
            await Register_Works();
            var getByEmail = await _client.GetUserByEmailAsync(new GetUserByEmailRequest { Email = _testEmail });
            var userId = getByEmail.User!.Id;
            var response = await _client.GetUserByIdAsync(new GetUserRequest { UserId = userId });
            Assert.True(response.Success, response.Message);
            Assert.NotNull(response.User);
            Assert.Equal(_testEmail, response.User.Email);
            Assert.Equal("Test User", response.User.FullName);
        }

        [Fact(DisplayName = "GetUserById with invalid GUID returns failure")]
        public async Task GetUserById_InvalidGuid_ReturnsFailure()
        {
            var response = await _client.GetUserByIdAsync(new GetUserRequest { UserId = "invalid-guid" });
            Assert.False(response.Success);
            Assert.Contains("Invalid user ID format", response.Message);
        }

        [Fact(DisplayName = "GetUserById with non-existent ID returns failure")]
        public async Task GetUserById_NonExistentId_ReturnsFailure()
        {
            var response = await _client.GetUserByIdAsync(new GetUserRequest { UserId = Guid.NewGuid().ToString() });
            Assert.False(response.Success);
            Assert.Contains("User not found", response.Message);
        }

        [Fact(DisplayName = "ChangePassword updates the user's password")]
        public async Task ChangePassword_Works()
        {
            // Register and login first
            await Login_Works();
            var newPassword = "newpassword123";

            var response = await _client.ChangePasswordAsync(new ChangePasswordRequest
            {
                UserId = _userId!,
                CurrentPassword = TestConfiguration.DefaultPassword,
                NewPassword = newPassword
            });
            Assert.True(response.Success, response.Message);

            // Verify we can login with new password
            var loginResponse = await _client.LoginAsync(new LoginRequest
            {
                Email = _testEmail,
                Password = newPassword
            });
            Assert.True(loginResponse.Success, loginResponse.Message);
        }

        [Fact(DisplayName = "ChangePassword with wrong current password returns failure")]
        public async Task ChangePassword_WrongCurrentPassword_ReturnsFailure()
        {
            await Login_Works();
            var response = await _client.ChangePasswordAsync(new ChangePasswordRequest
            {
                UserId = _userId!,
                CurrentPassword = "wrongpassword",
                NewPassword = "newpassword123"
            });
            Assert.False(response.Success);
            Assert.Contains("Incorrect password", response.Message);
        }

        [Fact(DisplayName = "ChangePassword with invalid user ID returns failure")]
        public async Task ChangePassword_InvalidUserId_ReturnsFailure()
        {
            var response = await _client.ChangePasswordAsync(new ChangePasswordRequest
            {
                UserId = "invalid-guid",
                CurrentPassword = "oldpassword",
                NewPassword = "newpassword123"
            });
            Assert.False(response.Success);
            Assert.Contains("Invalid user ID format", response.Message);
        }

        [Fact(DisplayName = "ChangePassword with non-existent user ID returns failure")]
        public async Task ChangePassword_NonExistentUserId_ReturnsFailure()
        {
            var response = await _client.ChangePasswordAsync(new ChangePasswordRequest
            {
                UserId = Guid.NewGuid().ToString(),
                CurrentPassword = "oldpassword",
                NewPassword = "newpassword123"
            });
            Assert.False(response.Success);
            Assert.Contains("User not found", response.Message);
        }

        [Fact(DisplayName = "Complete user workflow: register, login, get user, change password")]
        public async Task CompleteUserWorkflow_Works()
        {
            // Register
            var registerResponse = await _client.RegisterAsync(new RegisterRequest
            {
                Email = _testEmail,
                Password = TestConfiguration.DefaultPassword,
                FirstName = "Test",
                LastName = "User"
            });
            Assert.True(registerResponse.Success, registerResponse.Message);
            Assert.NotNull(registerResponse.User);
            var userId = registerResponse.User.Id;

            // Login
            var loginResponse = await _client.LoginAsync(new LoginRequest
            {
                Email = _testEmail,
                Password = TestConfiguration.DefaultPassword
            });
            Assert.True(loginResponse.Success, loginResponse.Message);
            Assert.NotNull(loginResponse.User);

            // Get user by email
            var getUserByEmailResponse = await _client.GetUserByEmailAsync(new GetUserByEmailRequest { Email = _testEmail });
            Assert.True(getUserByEmailResponse.Success, getUserByEmailResponse.Message);
            Assert.NotNull(getUserByEmailResponse.User);

            // Get user by ID
            var getUserByIdResponse = await _client.GetUserByIdAsync(new GetUserRequest { UserId = userId });
            Assert.True(getUserByIdResponse.Success, getUserByIdResponse.Message);
            Assert.NotNull(getUserByIdResponse.User);

            // Change password
            var changePasswordResponse = await _client.ChangePasswordAsync(new ChangePasswordRequest
            {
                UserId = userId,
                CurrentPassword = TestConfiguration.DefaultPassword,
                NewPassword = TestConfiguration.NewPassword
            });
            Assert.True(changePasswordResponse.Success, changePasswordResponse.Message);

            // Verify new password works
            var newLoginResponse = await _client.LoginAsync(new LoginRequest
            {
                Email = _testEmail,
                Password = TestConfiguration.NewPassword
            });
            Assert.True(newLoginResponse.Success, newLoginResponse.Message);
        }

        [Fact(DisplayName = "Multiple user registrations work independently")]
        public async Task MultipleUserRegistrations_WorkIndependently()
        {
            var email1 = $"testuser1_{Guid.NewGuid():N}@example.com";
            var email2 = $"testuser2_{Guid.NewGuid():N}@example.com";

            // Register first user
            var response1 = await _client.RegisterAsync(new RegisterRequest
            {
                Email = email1,
                Password = TestConfiguration.DefaultPassword,
                FirstName = "User",
                LastName = "One"
            });
            Assert.True(response1.Success, response1.Message);

            // Register second user
            var response2 = await _client.RegisterAsync(new RegisterRequest
            {
                Email = email2,
                Password = TestConfiguration.DefaultPassword,
                FirstName = "User",
                LastName = "Two"
            });
            Assert.True(response2.Success, response2.Message);

            // Verify both users can login independently
            var login1 = await _client.LoginAsync(new LoginRequest { Email = email1, Password = TestConfiguration.DefaultPassword });
            var login2 = await _client.LoginAsync(new LoginRequest { Email = email2, Password = TestConfiguration.DefaultPassword });

            Assert.True(login1.Success, login1.Message);
            Assert.True(login2.Success, login2.Message);
            Assert.NotEqual(login1.User!.Id, login2.User!.Id);
        }

        [Fact(DisplayName = "User data persistence across requests")]
        public async Task UserDataPersistence_Works()
        {
            await Register_Works();
            
            // Get user multiple times to verify persistence
            for (int i = 0; i < 3; i++)
            {
                var response = await _client.GetUserByEmailAsync(new GetUserByEmailRequest { Email = _testEmail });
                Assert.True(response.Success, response.Message);
                Assert.NotNull(response.User);
                Assert.Equal(_testEmail, response.User.Email);
                Assert.Equal("Test User", response.User.FullName);
            }
        }

        [Fact(DisplayName = "Concurrent user operations work correctly")]
        public async Task ConcurrentUserOperations_Work()
        {
            var tasks = new List<Task<AuthResponse>>();
            var emails = new List<string>();

            // Create multiple concurrent registration tasks
            for (int i = 0; i < 5; i++)
            {
                var email = $"concurrentuser_{Guid.NewGuid():N}@example.com";
                emails.Add(email);
                
                tasks.Add(_client.RegisterAsync(new RegisterRequest
                {
                    Email = email,
                    Password = TestConfiguration.DefaultPassword,
                    FirstName = $"User{i}",
                    LastName = "Concurrent"
                }).ResponseAsync);
            }

            // Wait for all registrations to complete
            var results = await Task.WhenAll(tasks);
            
            // Verify all registrations succeeded
            foreach (var result in results)
            {
                Assert.True(result.Success, result.Message);
            }
        }

        public Task InitializeAsync() => Task.CompletedTask;
        public Task DisposeAsync() => Task.CompletedTask;
    }
}
