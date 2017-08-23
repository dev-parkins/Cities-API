using System;
using System.Collections.Generic;
using System.Linq;
using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;
using CityInfo.API.Services;
using AutoMapper;

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
            var cityEntities = _cityInfoRepository.GetCities();
            var results = Mapper.Map<IEnumerable<CityWithoutPOITDto>>(cityEntities);
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
                var cityResult = Mapper.Map<CityDto>(city);
                return Ok(cityResult);
            }

            var cityWithNoPoiResult = Mapper.Map<CityWithoutPOITDto>(city);
            return Ok(cityWithNoPoiResult);
        }
    }
}