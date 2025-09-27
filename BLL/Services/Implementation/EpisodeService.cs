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
    public class EpisodeService : IEpisodeService
    {
        private readonly IMapper _mapper;
        private readonly IEpisodeRepository _episodeRepository;
        public EpisodeService(IMapper mapper,IEpisodeRepository episodeRepository)
        {
            _mapper = mapper;
            _episodeRepository = episodeRepository;
        }
        public (bool, string) CreateEpisode(EpisodeVM episodeVM)
        {
            
            var episode = _mapper.Map<Episode>(episodeVM);
            if(_episodeRepository.Add(episode))
            {
                return (true, "Episode Created Successfully");
            }    
            return (false,"Failed to Create Episode");
        }

        public (bool, string) DeleteEpisode(int id)
        {
            var episode = _episodeRepository.GetFirstOrDefault(x => x.Id == id, includeword: "Series");
            if (episode == null)
            {
                return (false, "Episode Not Found");
            }
           
            if (_episodeRepository.Remove(episode))
            {
                return (true, "Episode Deleted Successfully");
            }
            return (false, "Failed to Delete Episode");
        }

        public IEnumerable<EpisodeVM> GetAllEpisodes()
        {
            var episodes = _episodeRepository.GetAll(includeword:"Series");
            return _mapper.Map<IEnumerable<EpisodeVM>>(episodes);
        }

        public EpisodeVM GetEpisodeById(int id)
        {
            var episode = _episodeRepository.GetFirstOrDefault(x => x.Id == id);
            return _mapper.Map<EpisodeVM>(episode);
        }

        public IEnumerable<Episode> GetRecentEpisodes(int count)
        {
            return _episodeRepository.GetAll(includeword : "Series")
                                   .OrderByDescending(m => m.CreatedAt) 
                                   .Take(count)
                                   .ToList();
        }

        (bool, string) IEpisodeService.UpdateEpisode(EpisodeVM episodeVM, int Id)
        {
            var episode = _episodeRepository.GetFirstOrDefault(x => x.Id == Id,includeword:"Series");
            if (episode == null)
            {
                return (false, "Episode Not Found");
            }

        
            episode.Update(episodeVM.Title,episodeVM.Description);
         
            if (_episodeRepository.Update(episode))
            {
                return (true, "Episode Updated Successfully");
            }
            return (false, "Failed to Update Episode");
        }
    }
}
