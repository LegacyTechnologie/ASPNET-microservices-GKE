using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace myMicroservice.Controllers
{
    [ApiController]
    [Route("/")]
    public class WeatherForecastController : ControllerBase
    {
        [HttpGet]
        public String Get()
        {
            return "Hello World!";
        }

        [HttpGet("{id}", Name = "Get")]
        public String Get(String id)
        {
            return id;
        }

        [HttpGet("name/{name}", Name = "GetName")]
        public String GetName(String name)
        {
            return "My name is "+name+".";
        }
    }
}
