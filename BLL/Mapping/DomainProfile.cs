using AutoMapper;
using BLL.ViewModels;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Mapping
{
    public class DomainProfile:Profile
    {
        public DomainProfile()
        {
            CreateMap<Movie, MovieVM>().ReverseMap();
            CreateMap<Category, CategoryVM>().ReverseMap();
            CreateMap<Series, SeriesVM>().ReverseMap();
            CreateMap<ApplicationUser,RegisterUserVM>().ReverseMap();
            CreateMap<ApplicationUser,LoginUserVM>().ReverseMap();
        }
    }
}
