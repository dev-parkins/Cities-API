using CityInfo.API.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Controllers
{
    [Route("api/dummydb")]
    public class DummyDBController : Controller
    {
        private CityInfoContext _cityInfoContext;

        public DummyDBController(CityInfoContext ctx)
        {
            _cityInfoContext = ctx;
        }

        [HttpGet("testdatabase")]
        public IActionResult TestDatabase()
        {
            return Ok();
        }
    }
}
