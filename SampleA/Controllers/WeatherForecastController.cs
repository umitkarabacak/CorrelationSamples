using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;

namespace SampleA.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var getBeforeCollerationHeader = HttpContext.Request.Headers
                .FirstOrDefault(i => i.Key.ToLower().Contains("Correlation".ToLower()));

            _logger.LogInformation(JsonSerializer.Serialize(getBeforeCollerationHeader));

            var client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:6001")
            };
            var request = client.GetAsync("weatherforecast");

            var getafterCollerationHeader = HttpContext.Request.Headers
                .FirstOrDefault(i => i.Key.ToLower().Contains("Correlation".ToLower()));

            _logger.LogInformation(JsonSerializer.Serialize(getafterCollerationHeader));


            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
