using AutoMapper;
using BLL.Services.Abstraction;
using BLL.ViewModels;
using Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BLL.Services.Implementation
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountService> _logger;

        public AccountService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IMapper mapper,
            ILogger<AccountService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> LoginAsync(LoginUserVM loginUserVM)
        {
            var result = await _signInManager.PasswordSignInAsync(
                loginUserVM.UserName,
                loginUserVM.Password,
                loginUserVM.RememberMe,
                lockoutOnFailure: false);

            return result.Succeeded;
        }

        public async Task<(ApplicationUser? User, string? Error)> RegisterAsync(RegisterUserVM registerUserVM)
        {
            try
            {
                if (registerUserVM.Image != null)
                {
                    registerUserVM.ImageURL = BLL.Helpers.Load.UploadImage(registerUserVM.Image);
                }

                var user = _mapper.Map<ApplicationUser>(registerUserVM);
                user.EmailConfirmed = true;

                var result = await _userManager.CreateAsync(user, registerUserVM.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    return (user, null);
                }

                // Bug fix: Identity errors used to be swallowed silently.
                return (null, string.Join(" ", result.Errors.Select(e => e.Description)));
            }
            catch (InvalidDataException ex)
            {
                return (null, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to register user {UserName}", registerUserVM.UserName);
                return (null, "An unexpected error occurred while creating your account. Please try again.");
            }
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
