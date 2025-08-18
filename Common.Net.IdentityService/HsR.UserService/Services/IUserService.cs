using HsR.UserService.Models;
using System.Threading.Tasks;

namespace HsR.UserService.Services
{
    public interface IUserService
    {
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        string GetVersion();
    }
} 