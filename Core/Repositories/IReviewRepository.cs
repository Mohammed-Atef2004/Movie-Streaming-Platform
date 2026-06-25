using Core.Models;
using DAL.Repositories.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public interface IReviewRepository:IGenericRepository<Review>
    {
        IEnumerable<Review> GetMovieReviewsWithUsers(int movieId);
    }
}
