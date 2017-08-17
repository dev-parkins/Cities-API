using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
namespace CityInfo.API.Controllers
{
    //Route [Route("api/[controller]")] //Will Get name from controller class
    [Route("api/cities")]
    public class CitiesController : Controller
    {

        [HttpGet]
        public JsonResult GetCities()
        {
            return new JsonResult(CitiesDataStore.Current.Cities);
        }

        //[HttpGet("api/cities/{id}")] //Since we defined Route at Class-level, can just use last part of URI
        [HttpGet("{id}")]
        public JsonResult GetCity(int id)
        {
            return new JsonResult(CitiesDataStore.Current.Cities.FirstOrDefault(dto => dto.Id == id));
        }
    }
}