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
    public class EpisodeRepository : GenericRepository<Episode>, IEpisodeRepository
    {
        private readonly ApplicationDbContext _context;
        public EpisodeRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public bool Update(Episode episode)
        {
            // Fix: Removed double-fetch (same EF tracking conflict as MovieRepository)
            _context.Episodes.Update(episode);

            return _context.SaveChanges() > 0;
        }
    }
}