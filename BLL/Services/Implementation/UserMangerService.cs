using AutoMapper;
using BLL.Services.Abstraction;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Implementation
{
    public class UserMangerService : IUserMangerService
    {
        private readonly RoleManager<ApplicationUser> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        public UserMangerService(RoleManager<ApplicationUser> roleManager, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;
        }

    }
}
