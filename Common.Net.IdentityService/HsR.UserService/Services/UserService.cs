using HsR.UserService.Data;
using HsR.UserService.Entities;
using HsR.UserService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using HsR.UserService.Contracts;
using Microsoft.Extensions.DependencyInjection;
using DotNetEnv;


namespace HsR.UserService.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IServiceProvider _serviceProvider;
        private readonly UserServiceVersion _version;  

        public UserService(UserManager<User> userManager, IServiceProvider serviceProvider, UserServiceVersion version)
        {
            _userManager = userManager;
            _serviceProvider = serviceProvider;
            _version = version;
        }

        public string GetVersion() => _version.Version;

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Invalid email or password"
                };
            }

            var isValidPassword = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isValidPassword)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Invalid email or password"
                };
            }

            return new AuthResponse
            {
                Success = true,
                Message = "Login successful",
                User = MapToDto(user)
            };
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser is not null)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "User with this email already exists"
                };
            }

            var user = new User
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = string.Join(", ", result.Errors.Select(e => e.Description))
                };
            }

            return new AuthResponse
            {
                Success = true,
                Message = "Registration successful",
                User = MapToDto(user)
            };
        }

        public async Task EnsureDemoUserExistsAsync()
        {
            var demoId = Guid.Parse(DemoUserData.Id);
            var user = await _userManager.FindByIdAsync(demoId.ToString());
            if (user == null)
            {
                user = new User
                {
                    Id = demoId,
                    UserName = DemoUserData.Email,
                    Email = DemoUserData.Email,
                    FirstName = DemoUserData.FirstName,
                    LastName = DemoUserData.LastName,
                    EmailConfirmed = true
                };
                await _userManager.CreateAsync(user, DemoUserData.Password);
            }

            // Ensure the 'User' role exists
            var roleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            if (!await roleManager.RoleExistsAsync(RoleNames.User))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(RoleNames.User));
            }

            // Add demo user to 'User' role if not already
            if (!await _userManager.IsInRoleAsync(user, RoleNames.User))
            {
                await _userManager.AddToRoleAsync(user, RoleNames.User);
            }
        }

        public async Task EnsureAdminUserExistsAsync()
        {
            var adminId = Guid.Parse(Environment.GetEnvironmentVariable("ADMIN_USER_ID")!);
            var email = Environment.GetEnvironmentVariable("ADMIN_USER_EMAIL")!;
            var password = Environment.GetEnvironmentVariable("ADMIN_USER_PASSWORD")!;
            var firstName = Environment.GetEnvironmentVariable("ADMIN_USER_FIRSTNAME")!;
            var lastName = Environment.GetEnvironmentVariable("ADMIN_USER_LASTNAME")!;

            var user = await _userManager.FindByIdAsync(adminId.ToString());
            if (user == null)
            {
                user = new User
                {
                    Id = adminId,
                    UserName = email,
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    EmailConfirmed = true
                };
                await _userManager.CreateAsync(user, password);
            }

            // Ensure the Admin role exists
            var roleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            if (!await roleManager.RoleExistsAsync(RoleNames.Admin))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(RoleNames.Admin));
            }

            // Add user to Admin role if not already
            if (!await _userManager.IsInRoleAsync(user, RoleNames.Admin))
            {
                await _userManager.AddToRoleAsync(user, RoleNames.Admin);
            }
        }

        private static UserDto MapToDto(User user) => new()
        {
            Id = user.Id,
            Email = user.Email ?? string.Empty,
            FullName = user.FullName,
            CreatedAt = user.CreatedAt,
            FirstName = user.FirstName,
            LastName = user.LastName
        };
    }
} 