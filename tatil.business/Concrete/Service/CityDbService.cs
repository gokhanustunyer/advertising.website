using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tatil.business.Abstract.Service;
using tatil.data.Services.Abstract;

namespace tatil.business.Concrete.Service
{
    public class CityDbService : ICityDbService
    {
        private readonly ICitiesDbService _citiesDbService;

        public CityDbService(ICitiesDbService citiesDbService)
        {
            _citiesDbService = citiesDbService;
        }

        public List<Tuple<int, string>> GetAllCities()
            => _citiesDbService.AllCities;

        public List<Tuple<int, string>> GetAllDistrictByCityId(int id)
            => _citiesDbService.GetDistrictByCityId(id);

        public List<Tuple<int, string>> GetAllNeighborhoodsByDistrict(int id)
            => _citiesDbService.GetNeighborhoodsByDistrictId(id);

    }
}
