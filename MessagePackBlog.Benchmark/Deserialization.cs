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
    public class Deserialization
    {
        [Params(1, 100, 1000)]
        public int WeatherForecastCount { get; set; }
        public List<WeatherForecast> WeatherForecast = new List<WeatherForecast>();
        private string NewtonsoftJson = "";
        private string SystemTextJson = "";
        private byte[] MessagePackByteArray;
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

            NewtonsoftJson = JsonConvert.SerializeObject(WeatherForecast);
            SystemTextJson = JsonSerializer.Serialize(WeatherForecast);
            MessagePackByteArray = MessagePackSerializer.Serialize(WeatherForecast);
        }

        [Benchmark]
        public void FromNewtonsoftJson()
        {
            var result = JsonConvert.DeserializeObject<List<WeatherForecast>>(NewtonsoftJson);
        }
        [Benchmark]
        public void FromSystemTextJson()
        {
            var result = JsonSerializer.Deserialize<List<WeatherForecast>>(SystemTextJson);

        }
        [Benchmark]
        public void FromMessagePack()
        {
            var result = MessagePackSerializer.Deserialize<List<WeatherForecast>>(MessagePackByteArray);
        }

    }
}
