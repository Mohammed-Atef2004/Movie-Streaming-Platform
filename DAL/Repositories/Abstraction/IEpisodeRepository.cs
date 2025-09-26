using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Abstraction
{
    public interface IEpisodeRepository: IGenericRepository<Episode>
    {
        public bool Update(Episode episode);
    }
}
