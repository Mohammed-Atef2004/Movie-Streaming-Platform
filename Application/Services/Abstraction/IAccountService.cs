using BLL.ViewModels;
using Core.Models;

namespace BLL.Services.Abstraction
{
    public interface IAccountService
    {
        Task<(ApplicationUser? User, string? Error)> RegisterAsync(RegisterUserVM registerUserVM);
        Task<bool> LoginAsync(LoginUserVM loginUserVM);
        Task LogoutAsync();
    }
}
