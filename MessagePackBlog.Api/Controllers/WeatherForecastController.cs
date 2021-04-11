using MessagePackBlog.Model;
using MessagePackBlog.Model.WeatherModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MessagePackBlog.Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private static List<WeatherForecast> WeatherForecastList;

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            var rng = new Random();
            WeatherForecastList = Summaries.Select((summary, index) => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = summary
            }).ToList();
        }

        [HttpGet]
        [Produces(Constants.MediaTypes.MPack)]
        public IEnumerable<WeatherForecast> Get()
        {
            return WeatherForecastList.ToArray();
        }


        [HttpPost]
        [Produces(Constants.MediaTypes.MPack)]
        public void Post(WeatherForecast weatherForecast)
        {
            WeatherForecastList.Add(weatherForecast);
        }
    }
}