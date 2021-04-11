using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using FastSerialization;
using MessagePack;
using MessagePackBlog.Model.WeatherModels;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace MessagePackBlog.Benchmark
{
    [HtmlExporter, CoreJob]
    public class Serialization
    {
        [Params(1, 100, 1000)]
        public int WeatherForecastCount { get; set; }
        public List<WeatherForecast> WeatherForecast = new List<WeatherForecast>();
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [GlobalSetup]
        public void LoadDataset()
        {
            var rnd = new Random();
            for (int i = 1; i < WeatherForecastCount; i++)
            {
                WeatherForecast.Add(new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(rnd.Next(1, 1000)),
                    Summary = Summaries[rnd.Next(0, 9)],
                    TemperatureC = rnd.Next(-20, 55)
                });
            }
        }

        [Benchmark]
        public void ToNewtonsoftJson()
        {
            var result = JsonConvert.SerializeObject(WeatherForecast);
        }
        [Benchmark]
        public void ToSystemTextJson()
        {
            var result = JsonSerializer.Serialize(WeatherForecast);

        }
        [Benchmark]
        public void ToMessagePack()
        {
            var result = MessagePackSerializer.Serialize(WeatherForecast);
        }

        [Benchmark]
        public void ToMessagePackJson()
        {
            var content = MessagePackSerializer.Serialize(WeatherForecast);
            var result = MessagePackSerializer.ConvertToJson(content);
        }

    }
}
