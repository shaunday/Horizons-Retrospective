using Xunit;
using HsR.UserService.Models;
using HsR.UserService.Entities;
using HsR.UserService.Services;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Grpc.Core;

namespace HsR.UserService.Tests
{
    public class UserServiceUnitTests
    {
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly HsR.UserService.Services.UserService _userService;
        private readonly Mock<IServiceProvider> _mockServiceProvider;

        public UserServiceUnitTests()
        {
            var userStore = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(userStore.Object, default!, default!, default!, default!, default!, default!, default!, default!);
            _mockServiceProvider = new Mock<IServiceProvider>();
            _userService = new HsR.UserService.Services.UserService(_mockUserManager.Object, _mockServiceProvider.Object);
        }

        [Fact]
        public void RegisterRequest_ValidData_ValidationPasses()
        {
            // Arrange
            var request = new RegisterRequest
            {
                Email = "test@example.com",
                Password = "password123",
                FirstName = "John",
                LastName = "Doe"
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(request, new ValidationContext(request), validationResults, true);

            // Assert
            Assert.True(isValid);
            Assert.Empty(validationResults);
        }

        [Theory]
        [InlineData("invalid-email", "password123", "John", "Doe", "Email")]
        [InlineData("test@example.com", "password123", "J", "Doe", "FirstName")] // Too short
        [InlineData("test@example.com", "password123", "John", "D", "LastName")] // Too short
        public void RegisterRequest_InvalidData_ValidationFails(string email, string password, string firstName, string lastName, string expectedErrorField)
        {
            // Arrange
            var request = new RegisterRequest
            {
                Email = email,
                Password = password,
                FirstName = firstName,
                LastName = lastName
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(request, new ValidationContext(request), validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.NotEmpty(validationResults);
            Assert.Contains(validationResults, vr => vr.MemberNames.Contains(expectedErrorField));
        }

        [Fact]
        public void LoginRequest_ValidData_ValidationPasses()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "test@example.com",
                Password = "password123"
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(request, new ValidationContext(request), validationResults, true);

            // Assert
            Assert.True(isValid);
            Assert.Empty(validationResults);
        }

        [Theory]
        [InlineData("invalid-email", "password123")]
        public void LoginRequest_InvalidData_ValidationFails(string email, string password)
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = email,
                Password = password
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(request, new ValidationContext(request), validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.NotEmpty(validationResults);
        }

        [Fact]
        public void User_Entity_PropertiesWorkCorrectly()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe",
                CreatedAt = DateTime.UtcNow
            };

            // Act & Assert
            Assert.Equal("John Doe", user.FullName);
            Assert.NotEqual(Guid.Empty, user.Id);
            Assert.Equal("test@example.com", user.Email);
        }

        [Fact]
        public void User_FullName_HandlesEmptyNames()
        {
            // Arrange
            var user = new User
            {
                FirstName = "",
                LastName = "Doe"
            };

            // Act & Assert
            Assert.Equal("Doe", user.FullName);
        }

        [Fact]
        public async Task UserService_LoginAsync_UserNotFound_ReturnsFailure()
        {
            // Arrange
            var loginRequest = new LoginRequest
            {
                Email = "nonexistent@example.com",
                Password = "password123"
            };

            _mockUserManager.Setup(x => x.FindByEmailAsync(loginRequest.Email))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _userService.LoginAsync(loginRequest);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Invalid email or password", result.Message);
            Assert.Null(result.User);
        }

        [Fact]
        public async Task UserService_LoginAsync_InvalidPassword_ReturnsFailure()
        {
            // Arrange
            var loginRequest = new LoginRequest
            {
                Email = "test@example.com",
                Password = "wrongpassword"
            };

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe"
            };

            _mockUserManager.Setup(x => x.FindByEmailAsync(loginRequest.Email))
                .ReturnsAsync(user);
            _mockUserManager.Setup(x => x.CheckPasswordAsync(user, loginRequest.Password))
                .ReturnsAsync(false);

            // Act
            var result = await _userService.LoginAsync(loginRequest);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Invalid email or password", result.Message);
            Assert.Null(result.User);
        }

        [Fact]
        public async Task UserService_LoginAsync_ValidCredentials_ReturnsSuccess()
        {
            // Arrange
            var loginRequest = new LoginRequest
            {
                Email = "test@example.com",
                Password = "correctpassword"
            };

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe",
                CreatedAt = DateTime.UtcNow
            };

            _mockUserManager.Setup(x => x.FindByEmailAsync(loginRequest.Email))
                .ReturnsAsync(user);
            _mockUserManager.Setup(x => x.CheckPasswordAsync(user, loginRequest.Password))
                .ReturnsAsync(true);

            // Act
            var result = await _userService.LoginAsync(loginRequest);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Login successful", result.Message);
            Assert.NotNull(result.User);
            Assert.Equal(user.Id, result.User.Id);
            Assert.Equal(user.Email, result.User.Email);
            Assert.Equal(user.FullName, result.User.FullName);
        }

        [Fact]
        public async Task UserService_RegisterAsync_UserExists_ReturnsFailure()
        {
            // Arrange
            var registerRequest = new RegisterRequest
            {
                Email = "existing@example.com",
                Password = "password123",
                FirstName = "John",
                LastName = "Doe"
            };

            var existingUser = new User
            {
                Id = Guid.NewGuid(),
                Email = "existing@example.com"
            };

            _mockUserManager.Setup(x => x.FindByEmailAsync(registerRequest.Email))
                .ReturnsAsync(existingUser);

            // Act
            var result = await _userService.RegisterAsync(registerRequest);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("User with this email already exists", result.Message);
            Assert.Null(result.User);
        }

        [Fact]
        public async Task UserService_RegisterAsync_ValidRequest_ReturnsSuccess()
        {
            // Arrange
            var registerRequest = new RegisterRequest
            {
                Email = "newuser@example.com",
                Password = "password123",
                FirstName = "Jane",
                LastName = "Smith"
            };

            _mockUserManager.Setup(x => x.FindByEmailAsync(registerRequest.Email))
                .ReturnsAsync((User?)null);

            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), registerRequest.Password))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _userService.RegisterAsync(registerRequest);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Registration successful", result.Message);
            Assert.NotNull(result.User);
            Assert.Equal(registerRequest.Email, result.User.Email);
            Assert.Equal("Jane Smith", result.User.FullName);
        }

        [Fact]
        public async Task UserService_RegisterAsync_CreationFails_ReturnsFailure()
        {
            // Arrange
            var registerRequest = new RegisterRequest
            {
                Email = "newuser@example.com",
                Password = "password123",
                FirstName = "Jane",
                LastName = "Smith"
            };

            var errors = new List<IdentityError>
            {
                new IdentityError { Description = "Password is too weak" }
            };

            _mockUserManager.Setup(x => x.FindByEmailAsync(registerRequest.Email))
                .ReturnsAsync((User?)null);

            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), registerRequest.Password))
                .ReturnsAsync(IdentityResult.Failed(errors.ToArray()));

            // Act
            var result = await _userService.RegisterAsync(registerRequest);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Password is too weak", result.Message);
            Assert.Null(result.User);
        }
    }

    public class UserGrpcServiceUnitTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly Mock<ILogger<UserGrpcService>> _mockLogger;
        private readonly UserGrpcService _grpcService;

        public UserGrpcServiceUnitTests()
        {
            var userStore = new Mock<IUserStore<User>>();
            _mockUserService = new Mock<IUserService>();
            _mockUserManager = new Mock<UserManager<User>>(userStore.Object, default!, default!, default!, default!, default!, default!, default!, default!);
            _mockLogger = new Mock<ILogger<UserGrpcService>>();
            _grpcService = new UserGrpcService(_mockUserService.Object, _mockUserManager.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task UserGrpcService_HealthCheck_ReturnsHealthyStatus()
        {
            // Act
            var result = await _grpcService.HealthCheck(new Protos.HealthCheckRequest { Service = "UserService" }, null!);

            // Assert
            Assert.Equal("Healthy", result.Status);
            Assert.Equal("User Service is running", result.Message);
            Assert.NotNull(result.Timestamp);
        }

        [Fact]
        public async Task UserGrpcService_Login_Success_ReturnsMappedResponse()
        {
            // Arrange
            var grpcRequest = new Protos.LoginRequest
            {
                Email = "test@example.com",
                Password = "password123"
            };

            var serviceResponse = new AuthResponse
            {
                Success = true,
                Message = "Login successful",
                User = new UserDto
                {
                    Id = Guid.NewGuid(),
                    Email = "test@example.com",
                    FullName = "Test User",
                    FirstName = "Test",
                    LastName = "User",
                    CreatedAt = DateTime.UtcNow
                }
            };

            _mockUserService.Setup(x => x.LoginAsync(It.IsAny<LoginRequest>()))
                .ReturnsAsync(serviceResponse);

            // Act
            var result = await _grpcService.Login(grpcRequest, null!);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Login successful", result.Message);
            Assert.NotNull(result.User);
            Assert.Equal(serviceResponse.User!.Id.ToString(), result.User.Id);
            Assert.Equal(serviceResponse.User.Email, result.User.Email);
        }

        [Fact]
        public async Task UserGrpcService_Login_Exception_ReturnsErrorResponse()
        {
            // Arrange
            var grpcRequest = new Protos.LoginRequest
            {
                Email = "test@example.com",
                Password = "password123"
            };

            _mockUserService.Setup(x => x.LoginAsync(It.IsAny<LoginRequest>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _grpcService.Login(grpcRequest, null!);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("An error occurred during login", result.Message);
            Assert.Null(result.User);
        }

        [Fact]
        public async Task UserGrpcService_Register_Success_ReturnsMappedResponse()
        {
            // Arrange
            var grpcRequest = new Protos.RegisterRequest
            {
                Email = "newuser@example.com",
                Password = "password123",
                FirstName = "New",
                LastName = "User"
            };

            var serviceResponse = new AuthResponse
            {
                Success = true,
                Message = "Registration successful",
                User = new UserDto
                {
                    Id = Guid.NewGuid(),
                    Email = "newuser@example.com",
                    FullName = "New User",
                    FirstName = "New",
                    LastName = "User",
                    CreatedAt = DateTime.UtcNow
                }
            };

            _mockUserService.Setup(x => x.RegisterAsync(It.IsAny<RegisterRequest>()))
                .ReturnsAsync(serviceResponse);

            // Act
            var result = await _grpcService.Register(grpcRequest, null!);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Registration successful", result.Message);
            Assert.NotNull(result.User);
            Assert.Equal(serviceResponse.User!.Id.ToString(), result.User.Id);
            Assert.Equal(serviceResponse.User.Email, result.User.Email);
        }

        [Fact]
        public async Task UserGrpcService_GetUserById_InvalidGuid_ReturnsError()
        {
            // Arrange
            var grpcRequest = new Protos.GetUserRequest
            {
                UserId = "invalid-guid"
            };

            // Act
            var result = await _grpcService.GetUserById(grpcRequest, null!);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Invalid user ID format", result.Message);
            Assert.Null(result.User);
        }

        [Fact]
        public async Task UserGrpcService_GetUserById_UserNotFound_ReturnsError()
        {
            // Arrange
            var grpcRequest = new Protos.GetUserRequest
            {
                UserId = Guid.NewGuid().ToString()
            };

            _mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _grpcService.GetUserById(grpcRequest, null!);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("User not found", result.Message);
            Assert.Null(result.User);
        }

        [Fact]
        public async Task UserGrpcService_GetUserById_Success_ReturnsUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var grpcRequest = new Protos.GetUserRequest
            {
                UserId = userId.ToString()
            };

            var user = new User
            {
                Id = userId,
                Email = "test@example.com",
                FirstName = "Test",
                LastName = "User",
                CreatedAt = DateTime.UtcNow
            };

            _mockUserManager.Setup(x => x.FindByIdAsync(userId.ToString()))
                .ReturnsAsync(user);

            // Act
            var result = await _grpcService.GetUserById(grpcRequest, null!);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("User found", result.Message);
            Assert.NotNull(result.User);
            Assert.Equal(userId.ToString(), result.User.Id);
            Assert.Equal(user.Email, result.User.Email);
            Assert.Equal(user.FullName, result.User.FullName);
        }

        [Fact]
        public async Task UserGrpcService_GetUserByEmail_UserNotFound_ReturnsError()
        {
            // Arrange
            var grpcRequest = new Protos.GetUserByEmailRequest
            {
                Email = "nonexistent@example.com"
            };

            _mockUserManager.Setup(x => x.FindByEmailAsync(grpcRequest.Email))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _grpcService.GetUserByEmail(grpcRequest, null!);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("User not found", result.Message);
            Assert.Null(result.User);
        }

        [Fact]
        public async Task UserGrpcService_GetUserByEmail_Success_ReturnsUser()
        {
            // Arrange
            var grpcRequest = new Protos.GetUserByEmailRequest
            {
                Email = "test@example.com"
            };

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "test@example.com",
                FirstName = "Test",
                LastName = "User",
                CreatedAt = DateTime.UtcNow
            };

            _mockUserManager.Setup(x => x.FindByEmailAsync(grpcRequest.Email))
                .ReturnsAsync(user);

            // Act
            var result = await _grpcService.GetUserByEmail(grpcRequest, null!);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("User found", result.Message);
            Assert.NotNull(result.User);
            Assert.Equal(user.Id.ToString(), result.User.Id);
            Assert.Equal(user.Email, result.User.Email);
            Assert.Equal(user.FullName, result.User.FullName);
        }

        [Fact]
        public async Task UserGrpcService_ChangePassword_InvalidGuid_ReturnsError()
        {
            // Arrange
            var grpcRequest = new Protos.ChangePasswordRequest
            {
                UserId = "invalid-guid",
                CurrentPassword = "oldpass",
                NewPassword = "newpass"
            };

            // Act
            var result = await _grpcService.ChangePassword(grpcRequest, null!);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Invalid user ID format", result.Message);
        }

        [Fact]
        public async Task UserGrpcService_ChangePassword_UserNotFound_ReturnsError()
        {
            // Arrange
            var grpcRequest = new Protos.ChangePasswordRequest
            {
                UserId = Guid.NewGuid().ToString(),
                CurrentPassword = "oldpass",
                NewPassword = "newpass"
            };

            _mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _grpcService.ChangePassword(grpcRequest, null!);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("User not found", result.Message);
        }

        [Fact]
        public async Task UserGrpcService_ChangePassword_Success_ReturnsSuccess()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var grpcRequest = new Protos.ChangePasswordRequest
            {
                UserId = userId.ToString(),
                CurrentPassword = "oldpass",
                NewPassword = "newpass"
            };

            var user = new User
            {
                Id = userId,
                Email = "test@example.com"
            };

            _mockUserManager.Setup(x => x.FindByIdAsync(userId.ToString()))
                .ReturnsAsync(user);
            _mockUserManager.Setup(x => x.ChangePasswordAsync(user, grpcRequest.CurrentPassword, grpcRequest.NewPassword))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _grpcService.ChangePassword(grpcRequest, null!);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Password changed successfully", result.Message);
        }

        [Fact]
        public async Task UserGrpcService_ChangePassword_Failure_ReturnsError()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var grpcRequest = new Protos.ChangePasswordRequest
            {
                UserId = userId.ToString(),
                CurrentPassword = "oldpass",
                NewPassword = "newpass"
            };

            var user = new User
            {
                Id = userId,
                Email = "test@example.com"
            };

            var errors = new List<IdentityError>
            {
                new IdentityError { Description = "Current password is incorrect" }
            };

            _mockUserManager.Setup(x => x.FindByIdAsync(userId.ToString()))
                .ReturnsAsync(user);
            _mockUserManager.Setup(x => x.ChangePasswordAsync(user, grpcRequest.CurrentPassword, grpcRequest.NewPassword))
                .ReturnsAsync(IdentityResult.Failed(errors.ToArray()));

            // Act
            var result = await _grpcService.ChangePassword(grpcRequest, null!);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Current password is incorrect", result.Message);
        }
    }
}
