using Microsoft.AspNetCore.Mvc;
using RestaurantAPI;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherForecastService _service;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForecastService service)
        {
            _logger = logger;
            _service = service;
        }

        //[HttpGet]
        //public IEnumerable<WeatherForecast> Get()
        //{
        //    var result = _service.Get();
        //    return result;
        //}
        //[HttpGet("currentDay/{max}")]
        //public IEnumerable<WeatherForecast> Get2([FromQuery] int take, [FromRoute] int max)
        //{
        //    var result = _service.Get();
        //    return result;
        //}
        [HttpPost]
        public ActionResult<string> Hello([FromBody] string name)
        {
            //HttpContext.Response.StatusCode = 401; // Unauthorized
            //return StatusCode(401, $"Hello {name}");
            return NotFound($"Hello {name}");
        }
        [HttpPost("generate")]
        public ActionResult<IEnumerable<WeatherForecast>> Generate([FromQuery] int count, [FromBody] TemperatureRequest request)
        {
            if (count < 0 || request.MaxTemperature < request.MinTemperature)
            {
                return BadRequest();
            }
            var result = _service.Get(count, request.MinTemperature, request.MaxTemperature);
            return Ok(result);
        }


    }
}
