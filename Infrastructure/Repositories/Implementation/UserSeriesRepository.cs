using Core.Models;
using Core.Repositories;
using DAL.Repositories.Implementation;
using Infrastructure.Presistance.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Implementation
{
    public class UserSeriesRepository:GenericRepository<UserSeries>,IUserSeriesRepository
    {
        private readonly ApplicationDbContext _context;

        public UserSeriesRepository(ApplicationDbContext context):base(context) 
        {
        
            _context = context;
        }
        public UserSeries GetById(string userId,int seriesId)
        {
            var result=_context.UserSeries.FirstOrDefault(x=>x.SeriesId==seriesId&&x.UserId==userId);
            return result;
        }
    }
}
