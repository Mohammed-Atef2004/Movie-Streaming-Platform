using BLL.ViewModels;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Abstraction
{
    public interface IAccountService
    {
        Task<ApplicationUser> Register(RegisterUserVM registerUserVM);
        Task<bool> Login(LoginUserVM loginUserVM);
        Task<bool> Logout(LoginUserVM loginUserVM);
    }
}
