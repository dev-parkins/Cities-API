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
                    Description = "Known as the Big Apple",
                    PointsOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id = 1,
                            Name = "Central Park",
                            Description = "Urban Park to see!"
                        },
                        new PointOfInterestDto()
                        {
                            Id = 2,
                            Name = "Empire State Building",
                            Description = "Skyscraper worlds high"
                        }
                    }
                },
                new CityDto()
                {
                    Id = 2,
                    Name = "Phoenix",
                    Description = "'Hot as Heck' - Everyone",
                    PointsOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id = 3,
                            Name = "Papago Park",
                            Description = "Great Hiking spot near Chris' Home"
                        },
                        new PointOfInterestDto()
                        {
                            Id = 4,
                            Name = "Arizona State University",
                            Description = "Phoenix's prime university"
                        }
                    }
                },
                new CityDto()
                {
                    Id = 3,
                    Name = "Lake Havasu City",
                    Description = "'Hotter than Phoenix' - All residents",
                    PointsOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id = 5,
                            Name = "Havasu Lake",
                            Description = "Hosted MTV show in the 90's"
                        },
                        new PointOfInterestDto()
                        {
                            Id = 6,
                            Name = "London Bridge",
                            Description = "Original bridge brought from London stone-by-stone."
                        }
                    }
                }
            };
        }
    }
}
