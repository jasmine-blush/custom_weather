using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace custom_weather
{
    internal class WeatherService
    {
        private static readonly Dictionary<int, string> _wmoCodes = new Dictionary<int, string>{
            { 0, "Clear sky" },
            { 1, "Mainly clear" },
            { 2, "Partly cloudy" },
            { 3, "Overcast" },
            { 45, "Fog" },
            { 48, "Depositing Rime Fog" },
            { 51, "Light drizzle" },
            { 53, "Moderate drizzle" },
            { 55, "Dense drizzle" },
            { 56, "Light freezing drizzle" },
            { 57, "Dense freezing drizzle" },
            { 61, "Slight rain" },
            { 63, "Moderate rain" },
            { 65, "Heavy rain" },
            { 66, "Light freezing rain" },
            { 67, "Heavy freezing rain" },
            { 71, "Slight snow fall" },
            { 73, "Moderate snow fall" },
            { 75, "Heavy snow fall" },
            { 77, "Snow grains" },
            { 80, "Slight rain showers" },
            { 81, "Moderate rain showers" },
            { 82, "Violent rain showers" },
            { 85, "Slight snow showers" },
            { 86, "Heavy snow showers" },
            { 95, "Thunderstorm" }, //Slight or moderate
            { 96, "Thunderstorm and slight hail" },
            { 99, "Thunderstorm and heavy hail" },
        };

        private static readonly HttpClient _client = new HttpClient();

        public static async Task<string> GetWeather(Coordinates coords)
        {
            Dictionary<string, string> values = new Dictionary<string, string>(){
                { "latitude", coords.Latitude },
                { "longitude", coords.Longitude },
                { "current", "weather_code" }
            };

            FormUrlEncodedContent body = new FormUrlEncodedContent(values);
            var response = await _client.PostAsync("https://api.open-meteo.com/v1/forecast", body);
            if(response.IsSuccessStatusCode)
            {
                string responseString = await response.Content.ReadAsStringAsync();
                WeatherData weatherData = JsonConvert.DeserializeObject<WeatherData>(responseString);
                if(_wmoCodes.TryGetValue(weatherData.Current.WeatherCode, out string weatherString))
                {
                    return weatherString;
                }
                return "Unknown Weather";
            }
            return "Can't fetch weather data";
        }

        public static async Task<Coordinates> GetCoordinates(string location)
        {
            string requestUrl = string.Format("https://geocoding-api.open-meteo.com/v1/search?name={0}&count=1", location);
            var response = await _client.GetAsync(requestUrl);

            if(response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var jsonDocument = JsonDocument.Parse(responseString);
                if(jsonDocument.RootElement.TryGetProperty("results", out JsonElement results))
                {
                    Coordinates coordinates = JsonConvert.DeserializeObject<Coordinates>(results[0].ToString());
                    return coordinates;
                }
                return new Coordinates();
            }

            return new Coordinates() { Country = response.StatusCode.ToString() };
        }
    }
}
