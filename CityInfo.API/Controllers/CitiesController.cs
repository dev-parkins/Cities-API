using System;
using System.Collections.Generic;
using System.Linq;
using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;
using CityInfo.API.Services;

namespace CityInfo.API.Controllers
{
    //Route [Route("api/[controller]")] //Will Get name from controller class
    [Route("api/cities")]
    public class CitiesController : Controller
    {
        private ICityInfoRepository _cityInfoRepository;

        public CitiesController(ICityInfoRepository cityInfoRepository)
        {
            _cityInfoRepository = cityInfoRepository;
        }

        [HttpGet]
        public IActionResult GetCities()
        {
            //return Ok(CitiesDataStore.Current.Cities);
            var cityEntities = _cityInfoRepository.GetCities();

            var results = new List<CityWithoutPOITDto>();

            foreach(var cityEntity in cityEntities)
            {
                results.Add(new CityWithoutPOITDto()
                {
                    Id = cityEntity.Id,
                    Description = cityEntity.Description,
                    Name = cityEntity.Name
                });
            }

            return Ok(results);
        }

        //[HttpGet("api/cities/{id}")] //Since we defined Route at Class-level, can just use last part of URI
        [HttpGet("{id}")]
        public IActionResult GetCity(int id, bool includePointsOfInterest = false)
        {
            var city = _cityInfoRepository.GetCity(id, includePointsOfInterest);

            if (city == null) return NotFound();

            if (includePointsOfInterest)
            {
                var cityResult = new CityDto()
                {
                    Id = city.Id,
                    Name = city.Name,
                    Description = city.Description
                };

                foreach(var poi in city.PointsOfInterest)
                {
                    cityResult.PointsOfInterest.Add(new PointOfInterestDto()
                    {
                        Id = poi.Id,
                        Name = poi.Name,
                        Description = poi.Description
                    });
                }
                return Ok(cityResult);
            }

            var cityWithNoPoiResult = new CityWithoutPOITDto()
            {
                Id = city.Id,
                Description = city.Description,
                Name = city.Name
            };

            return Ok(cityWithNoPoiResult);

            //var result = CitiesDataStore.Current.Cities.FirstOrDefault(dto => dto.Id == id);

            //if (result == null)
            //    return NotFound();

            //return Ok(result);
        }
    }
}