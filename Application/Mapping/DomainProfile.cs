using AutoMapper;
using BLL.ViewModels;
using Core.Models;

namespace BLL.Mapping
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            // Forward maps ignore the form-only members (Image/CategoryList are not
            // part of the entities). Reverse maps stay plain: entities simply have
            // fewer members, and AutoMapper skips what doesn't exist on both sides.
            CreateMap<Movie, MovieVM>()
                .ForMember(d => d.Image, o => o.Ignore())
                .ForMember(d => d.CategoryList, o => o.Ignore())
                .ReverseMap();

            CreateMap<Category, CategoryVM>().ReverseMap();

            CreateMap<Series, SeriesVM>()
                .ForMember(d => d.Image, o => o.Ignore())
                .ForMember(d => d.CategoryList, o => o.Ignore())
                .ReverseMap();

            CreateMap<Episode, EpisodeVM>()
                .ForMember(d => d.SeriesList, o => o.Ignore())
                .ReverseMap();

            CreateMap<ApplicationUser, RegisterUserVM>()
                .ForMember(d => d.Password, o => o.Ignore())
                .ForMember(d => d.ConfirmPassword, o => o.Ignore())
                .ForMember(d => d.Image, o => o.Ignore())
                .ReverseMap();
        }
    }
}
