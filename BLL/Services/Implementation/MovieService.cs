using AutoMapper;
using BLL.Services.Abstraction;
using BLL.ViewModels;
using DAL.Models;
using DAL.Repositories.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Implementation
{
    public class MovieService : IMovieService
    {
        private readonly IMapper _mapper;
        private readonly IMovieRepository _movieRepository;
        public MovieService(IMapper mapper,IMovieRepository movieRepository)
        {
            _mapper = mapper;
            _movieRepository = movieRepository;
        }
        public (bool, string) CreateMovie(MovieVM movieVM)
        {
            if (movieVM.Image != null)
            {
                var result = Helpers.Load.UploadFile("Images", movieVM.Image);
                movieVM.ImageUrl = result;
            }

            var movie = _mapper.Map<Movie>(movieVM);
            if(_movieRepository.Add(movie))
            {
                return (true, "Movie Created Successfully");
            }    
            return (false,"Failed to Create Movie");
        }

        public (bool, string) DeleteMovie(int id)
        {
            var movie = _movieRepository.GetFirstOrDefault(x => x.Id == id, includeword: "Category");
            if (movie == null)
            {
                return (false, "Movie Not Found");
            }
            // Only remove the file if the movie exists and has an image
            if (!string.IsNullOrEmpty(movie.ImageUrl))
            {
                Helpers.Load.RemoveFile("Images", movie.ImageUrl);
            }
            if (_movieRepository.Remove(movie))
            {
                return (true, "Movie Deleted Successfully");
            }
            return (false, "Failed to Delete Movie");
        }

        public IEnumerable<MovieVM> GetAllMovies()
        {
            var movies = _movieRepository.GetAll(includeword:"Category");
            return _mapper.Map<IEnumerable<MovieVM>>(movies);
        }

        public MovieVM GetMovieById(int id)
        {
            var movie = _movieRepository.GetFirstOrDefault(x => x.Id == id);
            return _mapper.Map<MovieVM>(movie);
        }

        public IEnumerable<Movie> GetRecentMovies(int count)
        {
            return _movieRepository.GetAll(includeword : "Category")
                                   .OrderByDescending(m => m.CreatedAt) 
                                   .Take(count)
                                   .ToList();
        }

        (bool, string) IMovieService.UpdateMovie(MovieVM movieVM, int Id)
        {
            var movie = _movieRepository.GetFirstOrDefault(x => x.Id == Id,includeword:"Category");
            if (movie == null)
            {
                return (false, "Movie Not Found");
            }

            // Remove old image if a new one is uploaded
            if (movieVM.Image != null)
            {
                if (!string.IsNullOrEmpty(movie.ImageUrl))
                {
                    Helpers.Load.RemoveFile("Images", movie.ImageUrl);
                }
                var result = Helpers.Load.UploadFile("Images", movieVM.Image);
                movie.ImageUrl = result;
            }

            // Update other properties
            movie.Update(movieVM.Title,movieVM.Description, imageUrl: movie.ImageUrl,categoryId:movieVM.CategoryId);
         
            if (_movieRepository.Update(movie))
            {
                return (true, "Movie Updated Successfully");
            }
            return (false, "Failed to Update Movie");
        }
    }
}
