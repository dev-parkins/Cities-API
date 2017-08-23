using AutoMapper;
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
using CityInfo.API.Entities;

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
            var poiForCityResults = Mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterestForCity);

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

            var poiResult = Mapper.Map<PointOfInterestDto>(poi);

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

            if (!_cityInfoRepo.CityExists(cityId))
                return NotFound();

            var maxPointOfInterest = Mapper.Map<PointOfInterest>(pointOfInterest);

            _cityInfoRepo.AddPointOfInterestForCity(cityId, maxPointOfInterest);

            if (!_cityInfoRepo.Save())
                return StatusCode(500, "A problem occurred when processing your request.");

            var createdPoi = Mapper.Map<PointOfInterestDto>(maxPointOfInterest);
            return CreatedAtRoute("GetPointOfInterest", new {cityId = cityId, id = createdPoi.Id}, createdPoi);
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

            if (!_cityInfoRepo.CityExists(cityId))
                return NotFound();

            var poi = _cityInfoRepo.GetPointOfInterestForCity(cityId, id);
            if (poi == null)
                return NotFound();

            Mapper.Map(pointOfInterest, poi);
            if (!_cityInfoRepo.Save())
                return StatusCode(500, "A problem occurred when processing your request.");

            return NoContent();
        }

        [HttpPatch("{cityId}/pointsofinterest/{id}")]
        public IActionResult PatchPointOfInterest(int cityId, int id,
            [FromBody] JsonPatchDocument<PointOfInterestUpdateDto> patchDocument)
        {
            if (patchDocument == null)
                return BadRequest();

            if (!_cityInfoRepo.CityExists(cityId))
                return NotFound();

            var poiEntity = _cityInfoRepo.GetPointOfInterestForCity(cityId, id);
            if (poiEntity == null)
                NotFound();

            var poiToPatch = Mapper.Map<PointOfInterestUpdateDto>(poiEntity);

            patchDocument.ApplyTo(poiToPatch, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (poiToPatch.Name?.Contains("Trump") == true)
                ModelState.AddModelError("Name", "Contains SAD man's name - SAD");

            TryValidateModel(poiToPatch);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Mapper.Map(poiToPatch, poiEntity);
            if(!_cityInfoRepo.Save())
                return StatusCode(500, "A problem occurred when processing your request.");

            return NoContent();
        }

        [HttpDelete("{cityId}/pointsofinterest/{id}")]
        public IActionResult DeletePointOfInterest(int cityId, int id)
        {
            if (!_cityInfoRepo.CityExists(cityId))
                NotFound();

            var poi = _cityInfoRepo.GetPointOfInterestForCity(cityId, id);
            if (poi == null)
                return NotFound();

            _cityInfoRepo.DeletePointOfInterest(poi);


            if (!_cityInfoRepo.Save())
                _mailService.Send("Point of interest deletion.", $"Point of interest {poi.Name} (id:{poi.Id} was deleted.");

            return NoContent();
        }
    }
}
