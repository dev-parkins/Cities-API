using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.Models;

namespace CityInfo.API
{
    public class CitiesDataStore
    {
        // = new CitiesDataStore() allows assigning on init, good for readonly
        public static CitiesDataStore Current { get; } = new CitiesDataStore();
        public List<CityDto> Cities { get; set; }

        public CitiesDataStore()
        {
            //dummy data
            Cities = new List<CityDto>()
            {
                new CityDto()
                {
                    Id = 1,
                    Name = "New York City",
                    Description = "Known as the Big Apple"
                },
                new CityDto()
                {
                    Id = 2,
                    Name = "Phoenix",
                    Description = "'Hot as Heck' - Everyone"
                },
                new CityDto()
                {
                    Id = 3,
                    Name = "Lake Havasu City",
                    Description = "'Hotter than Phoenix' - All residents"
                }
            };
        }
    }
}
