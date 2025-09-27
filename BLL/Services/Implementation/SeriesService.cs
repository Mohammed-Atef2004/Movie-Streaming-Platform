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
    public class SeriesService : ISeriesService
    {
        private readonly IMapper _mapper;
        private readonly ISeriesRepository _seriesRepository;
        public SeriesService(IMapper mapper,ISeriesRepository seriesRepository)
        {
            _mapper = mapper;
            _seriesRepository = seriesRepository;
        }
        public (bool, string) CreateSeries(SeriesVM seriesVM)
        {
            var result = Helpers.Load.UploadFile("Images", seriesVM.Image);
            seriesVM.ImageUrl = result;

            var series = _mapper.Map<Series>(seriesVM);
            if(_seriesRepository.Add(series))
            {
                return (true, "Series Created Successfully");
            }    
            return (false,"Failed to Create Series");
        }

        public (bool, string) DeleteSeries(int id)
        {
            var series = _seriesRepository.GetFirstOrDefault(x => x.Id == id, includeword: "Category");
            if (series == null)
            {
                return (false, "Series Not Found");
            }
            // Only remove the file if the series exists and has an image
            if (!string.IsNullOrEmpty(series.ImageUrl))
            {
                Helpers.Load.RemoveFile("Images", series.ImageUrl);
            }
            if (_seriesRepository.Remove(series))
            {
                return (true, "Series Deleted Successfully");
            }
            return (false, "Failed to Delete Series");
        }

        public IEnumerable<SeriesVM> GetAllSeries()
        {
            var series = _seriesRepository.GetAll(includeword: "Category");
            return _mapper.Map<IEnumerable<SeriesVM>>(series);
        }

        public SeriesVM GetSeriesById(int id)
        {
            var series = _seriesRepository.GetFirstOrDefault(x => x.Id == id, includeword: "Category");
            return _mapper.Map<SeriesVM>(series);
        }

        (bool, string) ISeriesService.UpdateSeries(SeriesVM seriesVM, int Id)
        {
            var series = _seriesRepository.GetFirstOrDefault(x => x.Id == Id, includeword: "Category");
            if (series == null)
            {
                return (false, "Series Not Found");
            }

            // Remove old image if a new one is uploaded
            if (seriesVM.Image != null)
            {
                if (!string.IsNullOrEmpty(series.ImageUrl))
                {
                    Helpers.Load.RemoveFile("Images", series.ImageUrl);
                }
                var result = Helpers.Load.UploadFile("Images", seriesVM.Image);
                series.ImageUrl = result;
            }

            // Update other properties
            series.Update(seriesVM.Title,seriesVM.ViewCount,seriesVM.DownloadCount,seriesVM.Description, imageUrl: seriesVM.ImageUrl);
         
            if (_seriesRepository.Update(series))
            {
                return (true, "Series Updated Successfully");
            }
            return (false, "Failed to Update Series");
        }
    }
}
