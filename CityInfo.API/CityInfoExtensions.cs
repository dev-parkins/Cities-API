using CityInfo.API.Entities;
using CityInfo.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API
{
    public static class CityInfoExtensions
    {
        public static void EnsureSeedDataForContext(this CityInfoContext context)
        {
            if (context.Cities.Any()) //check if there is any data in our "main" table
            {
                return;
            }

            // initialize seed data
            var cities = new List<City>()
            {
                new City()
                {
                    Name = "New York City",
                    Description = "Known as the Big Apple",
                    PointsOfInterest = new List<PointOfInterest>()
                    {
                        new PointOfInterest()
                        {
                            Name = "Central Park",
                            Description = "Urban Park to see!"
                        },
                        new PointOfInterest()
                        {
                            Name = "Empire State Building",
                            Description = "Skyscraper worlds high"
                        }
                    }
                },
                new City()
                {
                    Name = "Phoenix",
                    Description = "'Hot as Heck' - Everyone",
                    PointsOfInterest = new List<PointOfInterest>()
                    {
                        new PointOfInterest()
                        {
                            Name = "Papago Park",
                            Description = "Great Hiking spot near Chris' Home"
                        },
                        new PointOfInterest()
                        {
                            Name = "Arizona State University",
                            Description = "Phoenix's prime university"
                        }
                    }
                },
                new City()
                {
                    Name = "Lake Havasu City",
                    Description = "'Hotter than Phoenix' - All residents",
                    PointsOfInterest = new List<PointOfInterest>()
                    {
                        new PointOfInterest()
                        {
                            Name = "Havasu Lake",
                            Description = "Hosted MTV show in the 90's"
                        },
                        new PointOfInterest()
                        {
                            Name = "London Bridge",
                            Description = "Original bridge brought from London stone-by-stone."
                        }
                    }
                }
            };

            context.Cities.AddRange(cities);
            context.SaveChanges();
        }
    }
}
