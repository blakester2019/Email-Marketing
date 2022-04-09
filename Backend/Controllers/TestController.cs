using Microsoft.AspNetCore.Mvc;
using Backend.Models;

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public TestController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetTest")]
        public int Get()
        {
            Test test = new Test();
            test.Age = 10;
            return test.Age;
        }
    }
}