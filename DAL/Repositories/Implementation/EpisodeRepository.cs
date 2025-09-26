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
    public class EpisodeRepository:GenericRepository<Episode>, IEpisodeRepository
    {
        private readonly ApplicationDbContext _context;
        public EpisodeRepository(ApplicationDbContext context):base(context)
        {
            _context= context;
        }
        public bool Update(Episode episode)
        { 
            var EpisodewFromDb = _context.Episodes.FirstOrDefault(u => u.Id == episode.Id);
            if(EpisodewFromDb != null)
            {
                EpisodewFromDb.Update(episode.Title, episode.Description,episode.UpdatedBy);
                _context.Episodes.Update(EpisodewFromDb);

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
