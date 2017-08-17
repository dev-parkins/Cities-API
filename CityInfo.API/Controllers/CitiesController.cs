using System;
using System.Collections.Generic;
using System.Linq;
using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;
namespace CityInfo.API.Controllers
{
    //Route [Route("api/[controller]")] //Will Get name from controller class
    [Route("api/cities")]
    public class CitiesController : Controller
    {

        [HttpGet]
        public IActionResult GetCities()
        {
            return Ok(CitiesDataStore.Current.Cities);
        }

        //[HttpGet("api/cities/{id}")] //Since we defined Route at Class-level, can just use last part of URI
        [HttpGet("{id}")]
        public IActionResult GetCity(int id)
        {
            CityDto result = CitiesDataStore.Current.Cities.FirstOrDefault(dto => dto.Id == id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}