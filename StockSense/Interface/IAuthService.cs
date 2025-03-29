using StockSense.Models;
using StockSense.Models.ViewModels.Auth;

namespace StockSense.Interface
{
    public interface IAuthService
    {
        Task<User> RegisterAsync(RegisterViewModel registerViewModel);
        Task<User> LoginAsync(string email, string password);
        Task<bool> ResetPasswordAsync(string currentPassword, string newPassword);
    }
}
