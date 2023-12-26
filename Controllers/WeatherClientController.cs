using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Runtime.CompilerServices;
using WeatherAPIClient.Models;

namespace WeatherAPIClient.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class WeatherClientController : ControllerBase
    {

        [HttpGet("City")]

        public async Task<IActionResult> GetWeather(string city)
        {
            try
            {
                string apiKey = "0365e97ebf22bcfce4e23c5abe24d5c2";
                string apiUrl = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}";

                using (var httpClient = new HttpClient())
                {

                    HttpResponseMessage response = await httpClient.GetAsync(apiUrl);


                    if (response.IsSuccessStatusCode)
                    {

                        string content = await response.Content.ReadAsStringAsync();
                        var weatherInfo = ParseWeatherInformation(content);

                        return Ok(weatherInfo);
                    }
                    else
                    {
                        return StatusCode((int)response.StatusCode, response.ReasonPhrase);
                    }

                }

            }
            catch (Exception ex)
            {

                return BadRequest($"Error:{ex}");
            }


        }

        private WeatherModel ParseWeatherInformation(string jsonContent)
        {

            JObject json = JObject.Parse(jsonContent);

            int temperatureK = (int)json["main"]["temp"];
            int temperatureC = temperatureK - 273;
            int humidity = (int)json["main"]["humidity"];
            double windSpeed = (double)json["wind"]["speed"];
            string description = json["weather"][0]["description"].ToString();

            return new WeatherModel
            {
                Temperature = temperatureC.ToString() + "°C",
                Humidity = humidity.ToString()+"%",
                WindSpeed = windSpeed.ToString("N2")+ " m/s",
                Description = description
            };




        }


    }
}
