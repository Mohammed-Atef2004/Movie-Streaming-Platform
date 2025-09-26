using DAL.Database;
using DAL.Models;
using DAL.Repositories.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Implementation
{
    public class SeriesRepository:GenericRepository<Series>, ISeriesRepository
    {
        private readonly ApplicationDbContext _context;
        public SeriesRepository(ApplicationDbContext context):base(context)
        {
            _context= context;
        }
        public bool Update(Series series)
        { 
            var SerieswFromDb = _context.Series.FirstOrDefault(u => u.Id == series.Id);
            if(SerieswFromDb != null)
            {
                SerieswFromDb.Update(series.Title, series.Description,series.IsFree, series.ImageUrl);
                _context.Series.Update(SerieswFromDb);

                if (_context.SaveChanges() > 0)
                {
                    return true;
                }
                else return false;
            }
            return false;
        }
    }
}
