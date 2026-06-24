using Application.Services.Abstraction;
using Core.Models;
using Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Implementation
{
    public class ReviewService:IReviewService
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewService(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public (bool, string) CreateReview(string userId, int movieId, string comment)
        {
            var review= Review.Create(userId, movieId, comment);
           if( _reviewRepository.Add(review)==true)
           {
                return (true, "Review Added Successfully");
           }
            return (false, "Failed to Add Review");
            
        }

        public (bool, string) DeleteReview(string userId, int movieId)
        {
            var result = _reviewRepository.GetFirstOrDefault(x => x.UserId == userId && x.MovieId == movieId);
            if(result==null)
            {
                return (false, "Failed to find review to delete");
            }
            _reviewRepository.Remove(result);
            return (true, "Review Deleted Successfully");

        }

        public (bool, string) UpdateReview(string userId, int movieId, string comment)
        {
            var result = _reviewRepository.GetFirstOrDefault(x => x.UserId == userId && x.MovieId == movieId);
            if (result == null)
            {
                return (false, "Failed to find review to delete");
            }
            result.Update(comment);
            _reviewRepository.Update(result);
            return (true, "Review Deleted Successfully");
        }

        public IEnumerable<Review> GetUserReviews(string userId)
        {
            return _reviewRepository.GetAll(x=>x.UserId == userId);
        }
        public IEnumerable<Review> GetFilmReviews(int  movieId)
        { 
            return _reviewRepository.GetAll(x=> x.MovieId == movieId); 
        }
    }
}
