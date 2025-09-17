using BLL.ViewModels;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Abstraction
{
    public interface IMovieService
    {
        (bool,string) CreateMovie(MovieVM movieVM);
        (bool,string) UpdateMovie(MovieVM movieVM,int Id);
        (bool, string) DeleteMovie(int id);
        MovieVM GetMovieById(int id);
        IEnumerable<MovieVM> GetAllMovies();
    }
}
