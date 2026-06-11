using BLL.ViewModels;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Abstraction
{
    public interface ISeriesService
    {
        (bool,string) CreateSeries(SeriesVM seriesVM);
        (bool,string) UpdateSeries(SeriesVM seriesVM,int Id);
        (bool, string) DeleteSeries(int id);
        SeriesVM GetSeriesById(int id);
        IEnumerable<SeriesVM> GetAllSeries();
    }
}
