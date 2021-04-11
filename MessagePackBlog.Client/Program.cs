using MessagePackBlog.Model.WeatherModels;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MessagePackBlog.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var httpClient = new HttpClient();


            var client = new WeatherForecastClient(httpClient);
            var weatherForecasts = await client.GetAsync();

            foreach (var weatherForecast in weatherForecasts)
            {
                Console.WriteLine($"{weatherForecast.Date.ToShortDateString()} - {weatherForecast.Summary} - {weatherForecast.TemperatureC}");
            }

            await client.PostAsync(new WeatherForecast
            {
                Date = DateTime.Now,
                Summary = "Freezing",
                TemperatureC = 2
            });

            Console.ReadKey();
        }
    }
}
