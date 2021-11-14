using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

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
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            var rng = new Random();
            var result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();

            var beforeRequestHeaderKeys = HttpContext.Request.Headers.Select(i => i.Key).ToList();
            var beforeResponseHeaderKeys = HttpContext.Response.Headers.Select(i => i.Key).ToList();

            var client = new HttpClient()
            {
                BaseAddress = new Uri("https://localhost:6001"),
            };
            var response = await client.GetAsync("weatherforecast");

            var afterRequestHeaderKeys = HttpContext.Request.Headers.Select(i => i.Key).ToList();
            var afterResponseHeaderKeys = HttpContext.Response.Headers.Select(i => i.Key).ToList();

            var responseData = new
            {
                beforeRequestHeaderKeys,
                beforeResponseHeaderKeys,
                afterRequestHeaderKeys,
                afterResponseHeaderKeys,
            };

            _logger.LogInformation(JsonSerializer.Serialize(responseData));

            return result;
        }
    }
}
