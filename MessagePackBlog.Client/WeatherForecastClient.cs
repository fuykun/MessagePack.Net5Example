using MessagePackBlog.Common;
using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using MessagePackBlog.Model;
using MessagePackBlog.Model.WeatherModels;

namespace MessagePackBlog.Client
{
    public class WeatherForecastClient
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = @"https://localhost:44378";

        public WeatherForecastClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<WeatherForecast[]> GetAsync()
        {
            try
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/weatherforecast/get");

                requestMessage.Headers.Add("Accept", Constants.MediaTypes.MPack);

                HttpResponseMessage response = await _httpClient.SendAsync(requestMessage).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var mediaTypeFormatter = new MediaTypeFormatter[]
                    {
                        new MessagePackMediaTypeFormatter(Constants.MediaTypes.MPack)
                    };

                    WeatherForecast[] forecasts = await response
                        .Content
                        .ReadAsAsync<WeatherForecast[]>(mediaTypeFormatter)
                        .ConfigureAwait(false);

                    return forecasts;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task PostAsync(WeatherForecast weatherForecast)
        {
            try
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/weatherforecast/post");

                requestMessage.Headers.Add("Accept", Constants.MediaTypes.MPack);
                requestMessage.Method = HttpMethod.Post;
                requestMessage.Content = new ObjectContent<WeatherForecast>(
                    weatherForecast,
                    new MessagePackMediaTypeFormatter(Constants.MediaTypes.MPack));

                HttpResponseMessage response = await _httpClient.SendAsync(requestMessage).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
