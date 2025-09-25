using AutoMapper;
using BLL.Services.Abstraction;
using BLL.ViewModels;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Implementation
{
    public class AccountService : IAccountService
    {
        private readonly RoleManager<ApplicationUser> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        public AccountService(RoleManager<ApplicationUser> roleManager, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

       public async Task<bool> Login(LoginUserVM loginUserVM)
        {
            var user=_mapper.Map<ApplicationUser>(loginUserVM);
            var result=await _signInManager.PasswordSignInAsync(user.UserName,loginUserVM.Password,loginUserVM.RememberMe,false);
            return result.Succeeded;

        }

        public async Task<ApplicationUser> Register(RegisterUserVM registerUserVM)
        {
            var user=_mapper.Map<ApplicationUser>(registerUserVM);
            user.EmailConfirmed = true;
            var result= await _userManager.CreateAsync(user,registerUserVM.Password);
            if(result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
                return user;
            }
            return null;
        }
        public async Task<bool> Logout(LoginUserVM loginUserVM)
        {
           await _signInManager.SignOutAsync();
            return true;
        }
    }
}
