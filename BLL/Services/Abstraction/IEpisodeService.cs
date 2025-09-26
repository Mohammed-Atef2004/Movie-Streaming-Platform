using BLL.ViewModels;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Abstraction
{
    public interface IEpisodeService
    {
        (bool,string) CreateEpisode(EpisodeVM episodeVM);
        (bool,string) UpdateEpisode(EpisodeVM episodeVM,int Id);
        (bool, string) DeleteEpisode(int id);
        EpisodeVM GetEpisodeById(int id);
        IEnumerable<EpisodeVM> GetAllEpisodes();
        public IEnumerable<Episode> GetRecentEpisodes(int count);
    }
}
