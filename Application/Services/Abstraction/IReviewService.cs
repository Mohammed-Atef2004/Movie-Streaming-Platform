using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Abstraction
{
    public interface IReviewService
    {
        (bool, string) CreateReview(string userId, int movieId, string comment);
        (bool, string) UpdateReview(string userId, int movieId, string comment);
        (bool,string) DeleteReview(string userId, int movieId);
         IEnumerable<Review> GetUserReviews(string userId);
        IEnumerable<Review> GetFilmReviews(int movieId);



    }
}
