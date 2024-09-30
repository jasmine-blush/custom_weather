using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace custom_weather
{
    internal class WeatherService
    {
        private static readonly Dictionary<int, string[]> _wmoCodes = new Dictionary<int, string[]>{
            { 0, new string[] { "Clear sky", "Images\\clear" } },
            { 1, new string[] { "Mainly clear", "Images\\clouds" } },
            { 2, new string[] { "Partly cloudy", "Images\\clouds" } },
            { 3, new string[] { "Overcast", "Images\\overcast" } },
            { 45, new string[] { "Fog", "Images\\overcast" } },
            { 48, new string[] { "Depositing Rime Fog", "Images\\overcast" } },
            { 51, new string[] { "Light drizzle", "Images\\drizzle" } },
            { 53, new string[] { "Moderate drizzle", "Images\\drizzle" } },
            { 55, new string[] { "Dense drizzle", "Images\\drizzle" } },
            { 56, new string[] { "Light freezing drizzle", "Images\\drizzle" } },
            { 57, new string[] { "Dense freezing drizzle", "Images\\drizzle" } },
            { 61, new string[] { "Slight rain", "Images\\rain" } },
            { 63, new string[] { "Moderate rain", "Images\\rain" } },
            { 65, new string[] { "Heavy rain", "Images\\rain" } },
            { 66, new string[] { "Light freezing rain", "Images\\rain" } },
            { 67, new string[] { "Heavy freezing rain", "Images\\rain" } },
            { 71, new string[] { "Slight snow fall", "Images\\snow" } },
            { 73, new string[] { "Moderate snow fall", "Images\\snow" } },
            { 75, new string[] { "Heavy snow fall", "Images\\snow" } },
            { 77, new string[] { "Snow grains", "Images\\snow" } },
            { 80, new string[] { "Slight rain showers", "Images\\shower" } },
            { 81, new string[] { "Moderate rain showers", "Images\\shower" } },
            { 82, new string[] { "Violent rain showers", "Images\\shower" } },
            { 85, new string[] { "Slight snow showers", "Images\\snow" } },
            { 86, new string[] { "Heavy snow showers", "Images\\snow" } },
            { 95, new string[] { "Thunderstorm", "Images\\thunderstorm" } }, //Slight or moderate
            { 96, new string[] { "Thunderstorm and slight hail", "Images\\hail" } },
            { 99, new string[] { "Thunderstorm and heavy hail", "Images\\hail" } },
        };

        private static readonly HttpClient _client = new HttpClient();

        public static async Task<WeatherResult> GetWeather(Coordinates coords)
        {
            Dictionary<string, string> values = new Dictionary<string, string>(){
                { "latitude", coords.Latitude },
                { "longitude", coords.Longitude },
                { "current", "weather_code,temperature_2m,surface_pressure,wind_speed_10m,wind_direction_10m,relative_humidity_2m,is_day,precipitation_probability,apparent_temperature" },
                { "daily", "temperature_2m_min,temperature_2m_max" }
            };
            FormUrlEncodedContent body = new FormUrlEncodedContent(values);
            var response = await _client.PostAsync("https://api.open-meteo.com/v1/forecast", body);

            if(response.IsSuccessStatusCode)
            {
                string responseString = await response.Content.ReadAsStringAsync();
                OpenMeteoData omData = JsonConvert.DeserializeObject<OpenMeteoData>(responseString);

                WeatherResult result = new WeatherResult();
                if(_wmoCodes.TryGetValue(omData.Current.WeatherCode, out string[] weatherType))
                {
                    result.Title = weatherType[0];
                    result.IcoPath = weatherType[1];
                    if(omData.Current.IsDay == 0 && omData.Current.WeatherCode <= 2)
                    {
                        result.IcoPath += "_night";
                    }
                    result.IcoPath += ".png";
                }
                else
                {
                    result.Title = "Unknown Weather";
                    result.IcoPath = "Images\\plugin.png";
                }
                result.Title += " @ " + omData.Current.Temperature + " °C";

                List<string> subTitleData = new List<string> {
                    "Max: " + omData.Daily.MaxTemps[0] + " °C",
                    "Min: " + omData.Daily.MinTemps[0] + " °C",
                    "Wind Speed: " + omData.Current.WindSpeed + " km/h",
                    "Direction: " + omData.Current.WindDirection + "°",
                    "Feels Like: " + omData.Current.FeelsLike + " °C",
                    "Rain Chance: " + omData.Current.RainChance + " %",
                    "Humidity: " + omData.Current.Humidity + " %",
                };

                result.SubTitle = string.Join("     ", subTitleData);
                return result;
            }
            throw new Exception("Can't fetch weather data");
        }

        public static async Task<List<Coordinates>> GetCoordinates(string location)
        {
            string requestUrl = string.Format("https://geocoding-api.open-meteo.com/v1/search?name={0}&count=5", location);
            var response = await _client.GetAsync(requestUrl);

            if(response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var jsonDocument = JsonDocument.Parse(responseString);
                if(jsonDocument.RootElement.TryGetProperty("results", out JsonElement results))
                {
                    List<Coordinates> coordinates = new List<Coordinates>();
                    foreach(JsonElement result in results.EnumerateArray())
                    {
                        coordinates.Add(JsonConvert.DeserializeObject<Coordinates>(result.ToString()));
                    }
                    return coordinates;
                }
                else
                {
                    string[] cityAndCountry = location.Split(',');
                    if(cityAndCountry.Length == 2)
                    {
                        List<Coordinates> cityCoords = await GetCoordinates(cityAndCountry[0]);

                        List<Coordinates> coordinates = new List<Coordinates>();
                        foreach(Coordinates city in cityCoords)
                        {
                            string searchCountry = cityAndCountry[1].ToLower().Trim();
                            if(city.Country.ToLower() == searchCountry || city.CountryCode.ToLower() == searchCountry)
                            {
                                coordinates.Add(city);
                            }
                        }
                        if(coordinates.Count > 0)
                        {
                            return coordinates;
                        }
                    }
                }
                throw new Exception("Can't find this city");

            }
            throw new Exception("Request Error: " + response.StatusCode.ToString());
        }
    }
}
