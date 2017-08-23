using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class PointsOfInterestController : Controller
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _mailService;
        private ICityInfoRepository _cityInfoRepo;


        public PointsOfInterestController(ILogger<PointsOfInterestController> logger
            , IMailService mailService
            , ICityInfoRepository cityInfoRepo)
        {
            _logger = logger;
            _mailService = mailService;
            _cityInfoRepo = cityInfoRepo;
        }

        [HttpGet("{cityId}/pointsofinterest")]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            if (!_cityInfoRepo.CityExists(cityId))
            {
                _logger.LogInformation($"Invalid city id {cityId} passed to GetPointsOfInterest.");
                return NotFound();
            }

            var pointsOfInterestForCity = _cityInfoRepo.GetPointsOfInterestForCity(cityId);

            var poiForCityResults = new List<PointOfInterestDto>();
            foreach(var poi in pointsOfInterestForCity)
            {
                poiForCityResults.Add(new PointOfInterestDto()
                {
                    Id = poi.Id,
                    Name = poi.Name,
                    Description = poi.Description
                });
            }

            return Ok(poiForCityResults);
        }

        [HttpGet("{cityId}/pointsofinterest/{id}", Name = "GetPointOfInterest")]
        public IActionResult GetPointOfInterest(int cityId, int id)
        {
            if (!_cityInfoRepo.CityExists(cityId))
            {
                _logger.LogInformation($"Invalid city id {cityId} passed to GetPointOfInterest.");
                return NotFound();
            }

            var poi = _cityInfoRepo.GetPointOfInterestForCity(cityId, id);

            if (poi == null) return NotFound();

            var poiResult = new PointOfInterestDto()
            {
                Id = poi.Id,
                Name = poi.Name,
                Description = poi.Description
            };

            return Ok(poiResult);
        }

        [HttpPost("{cityId}/pointsofinterest")]
        public IActionResult CreatePointOfInterest(int cityId, 
            [FromBody] PointOfInterestCreateDto pointOfInterest)
        {
            if (pointOfInterest == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (pointOfInterest.Name.Contains("Trump"))
            {
                ModelState.AddModelError("Name", "Contains SAD man's name - SAD");
                return BadRequest(ModelState);
            }
                
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
                return NotFound();

            //for demo only - improvements inc
            var maxPointOfInterest = CitiesDataStore.Current.Cities.SelectMany(
                c => c.PointsOfInterest).Max(p => p.Id);

            var newPointOfInterest = new PointOfInterestDto()
            {
                Id = ++maxPointOfInterest,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description
            };

            city.PointsOfInterest.Add(newPointOfInterest);

            return CreatedAtRoute("GetPointOfInterest", new {cityId = cityId, id = newPointOfInterest.Id}, newPointOfInterest);
        }

        [HttpPut("{cityId}/pointsofinterest/{id}")]
        public IActionResult UpdatePointOfInterest(int cityId, int id, 
            [FromBody] PointOfInterestUpdateDto pointOfInterest)
        {
            if (pointOfInterest == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (pointOfInterest.Name.Contains("Trump"))
            {
                ModelState.AddModelError("Name", "Contains SAD man's name - SAD");
                return BadRequest(ModelState);
            }

            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
                return NotFound();

            var storePoi = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);
            if (storePoi == null)
                return NotFound();

            storePoi.Name = pointOfInterest.Name;
            storePoi.Description = pointOfInterest.Description;

            return NoContent();
        }

        [HttpPatch("{cityId}/pointsofinterest/{id}")]
        public IActionResult PatchPointOfInterest(int cityId, int id,
            [FromBody] JsonPatchDocument<PointOfInterestUpdateDto> patchDocument)
        {
            if (patchDocument == null)
                return BadRequest();

            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
                return NotFound();

            var storePoi = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);
            if (storePoi == null)
                return NotFound();

            var storePoiToPatch =
                new PointOfInterestUpdateDto()
                {
                    Name = storePoi.Name,
                    Description = storePoi.Description
                };

            patchDocument.ApplyTo(storePoiToPatch, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (storePoiToPatch.Name?.Contains("Trump") == true)
                ModelState.AddModelError("Name", "Contains SAD man's name - SAD");

            TryValidateModel(storePoiToPatch);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            storePoi.Name = storePoiToPatch.Name;
            storePoi.Description = storePoiToPatch.Description;

            return NoContent();
        }

        [HttpDelete("{cityId}/pointsofinterest/{id}")]
        public IActionResult DeletePointOfInterest(int cityId, int id)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
                return NotFound();

            var storePoi = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);
            if (storePoi == null)
                return NotFound();

            city.PointsOfInterest.Remove(storePoi);
            _mailService.Send("Point of interest deletion.", $"Point of interest {storePoi.Name} (id:{storePoi.Id} was deleted.");
            return NoContent();
        }
    }
}
