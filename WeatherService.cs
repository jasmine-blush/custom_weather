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

        public static async Task<List<WeatherResult>> GetDetailWeather(Coordinates coords, SettingsSave settings)
        {
            List<WeatherResult> weatherResults = new List<WeatherResult>();
            DateTime date = DateTime.Now;
            for(int i = 0; i < 7; i++)
            {
                WeatherResult weatherResult = await GetWeather(coords, settings, i);
                string dateString = "";
                if(i == 0)
                {
                    dateString = "Today";
                }
                else if(i == 1)
                {
                    dateString = "Tomorrow";
                }
                else
                {
                    dateString = date.DayOfWeek.ToString();
                }
                weatherResult.Title = dateString + " - " + weatherResult.Title;
                date = date.AddDays(1);
                weatherResults.Add(weatherResult);
            }
            return weatherResults;
        }

        public static async Task<WeatherResult> GetWeather(Coordinates coords, SettingsSave settings, int day)
        {
            Dictionary<string, string> values = new Dictionary<string, string>(){
                { "latitude", coords.Latitude },
                { "longitude", coords.Longitude },
                { "temperature_unit", settings.TempUnit.ToString() },
                { "wind_speed_unit", settings.WindUnit.ToString() },
                { "precipitation_unit", settings.RainUnit.ToString() },
                { "current", "weather_code,temperature_2m,is_day," + settings.WeatherData.GetCurrent() },
                { "daily", "weather_code,temperature_2m_max,temperature_2m_min," + settings.WeatherData.GetDaily() }
            };
            FormUrlEncodedContent body = new FormUrlEncodedContent(values);

            string requestKey = body.ReadAsStringAsync().Result + settings.WeatherData.MaxTemp.ToString() + settings.WeatherData.MinTemp.ToString() + settings.DirectionUnit + day.ToString();
            if(!WeatherCache.HasCached(requestKey, Int32.Parse(settings.CacheDuration.GetDescription())))
            {
                var response = await _client.PostAsync("https://api.open-meteo.com/v1/forecast", body);

                if(response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    OpenMeteoData omData = JsonConvert.DeserializeObject<OpenMeteoData>(responseString);

                    WeatherResult result = new WeatherResult();
                    if(_wmoCodes.TryGetValue(day == 0 ? omData.Current.WeatherCode : omData.Daily.WeatherCode[day], out string[] weatherType))
                    {
                        result.Title = weatherType[0];
                        result.IcoPath = weatherType[1];
                        if(omData.Current.IsDay == 0 && omData.Current.WeatherCode <= 2 && day == 0)
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
                    if(day == 0)
                    {
                        result.Title += " @ " + omData.Current.Temperature + " " + settings.TempUnit.GetDescription();
                    }

                    result.SubTitle = BuildSubtitle(settings, omData, day);

                    WeatherCache.Cache(requestKey, result);
                    return result;
                }
                throw new Exception("Can't fetch weather data");
            }
            return WeatherCache.Retrieve(requestKey);
        }

        private static string BuildSubtitle(SettingsSave settings, OpenMeteoData omData, int day)
        {
            List<string> data = new List<string>();

            if(day == 0)
            {
                if(settings.WeatherData.MaxTemp == 1)
                    data.Add("Max: " + omData.Daily.MaxTemps[day] + " " + omData.DailyUnits.MaxTemp);
                if(settings.WeatherData.MinTemp == 1)
                    data.Add("Min: " + omData.Daily.MinTemps[day] + " " + omData.DailyUnits.MinTemp);

                if(omData.Current.WindSpeed != null)
                    data.Add("Wind Speed: " + omData.Current.WindSpeed + " " + omData.CurrentUnits.WindSpeed);
                if(omData.Current.WindDirection != null)
                {
                    string windDirection = (settings.DirectionUnit == DirectionUnit.degrees) ? omData.Current.WindDirection + omData.CurrentUnits.WindDirection : ToCompass(omData.Current.WindDirection);
                    data.Add("Wind Direction: " + windDirection);
                }
                if(omData.Current.FeelsLike != null)
                    data.Add("Feels Like: " + omData.Current.FeelsLike + " " + omData.CurrentUnits.FeelsLike);
                if(omData.Current.Humidity != null)
                    data.Add("Humidity: " + omData.Current.Humidity + " " + omData.CurrentUnits.Humidity);
                if(omData.Current.DewPoint != null)
                    data.Add("Dew Point: " + omData.Current.DewPoint + " " + omData.CurrentUnits.DewPoint);
                if(omData.Current.Pressure != null)
                    data.Add("Pressure: " + omData.Current.Pressure + " " + omData.CurrentUnits.Pressure);
                if(omData.Current.CloudCover != null)
                    data.Add("Cloud Cover: " + omData.Current.CloudCover + " " + omData.CurrentUnits.CloudCover);
                if(omData.Current.TotalPrecip != null)
                    data.Add("Total Rain: " + omData.Current.TotalPrecip + " " + omData.CurrentUnits.TotalPrecip);
                if(omData.Current.PrecipChance != null)
                    data.Add("Rain Chance: " + omData.Current.PrecipChance + " " + omData.CurrentUnits.PrecipChance);
                if(omData.Current.Snowfall != null)
                    data.Add("Snowfall: " + omData.Current.Snowfall + " " + omData.CurrentUnits.Snowfall);
                if(omData.Current.SnowDepth != null)
                    data.Add("Snow Depth: " + omData.Current.SnowDepth + " " + omData.CurrentUnits.SnowDepth);
                if(omData.Current.Visibility != null)
                {
                    if(float.TryParse(omData.Current.Visibility, out float parsed))
                    {
                        int visibility = (int)parsed;
                        data.Add("Visibility: " + visibility.ToString() + " " + omData.CurrentUnits.Visibility);
                    }
                    else
                    {
                        data.Add("Visibility: " + omData.Current.Visibility + " " + omData.CurrentUnits.Visibility);
                    }
                }

                if(omData.Current.ShortRadiation != null)
                    data.Add("Shortwave: " + omData.Current.ShortRadiation + " " + omData.CurrentUnits.ShortRadiation);
                if(omData.Current.DirectRadiation != null)
                    data.Add("Direct: " + omData.Current.DirectRadiation + " " + omData.CurrentUnits.DirectRadiation);
                if(omData.Current.DiffuseRadiation != null)
                    data.Add("Diffuse: " + omData.Current.DiffuseRadiation + " " + omData.CurrentUnits.DiffuseRadiation);
                if(omData.Current.VPDeficit != null)
                    data.Add("Deficit: " + omData.Current.VPDeficit + " " + omData.CurrentUnits.VPDeficit);
                if(omData.Current.CAPE != null)
                    data.Add("CAPE: " + omData.Current.CAPE + " " + omData.CurrentUnits.CAPE);
                if(omData.Current.Evapo != null)
                    data.Add("ET0: " + omData.Current.Evapo + " " + omData.CurrentUnits.Evapo);
                if(omData.Current.FreezingHeight != null)
                {
                    if(float.TryParse(omData.Current.FreezingHeight, out float parsed))
                    {
                        int freezingHeight = (int)parsed;
                        data.Add("Freezing: " + freezingHeight.ToString() + " " + omData.CurrentUnits.FreezingHeight);
                    }
                    else
                    {
                        data.Add("Freezing: " + omData.Current.FreezingHeight + " " + omData.CurrentUnits.FreezingHeight);
                    }
                }
                if(omData.Current.SoilTemperature != null)
                    data.Add("Soil: " + omData.Current.SoilTemperature + " " + omData.CurrentUnits.SoilTemperature);
                if(omData.Current.SoilMoisture != null)
                    data.Add("Moisture: " + omData.Current.SoilMoisture + " " + omData.CurrentUnits.SoilMoisture);
            }
            else
            {
                if(settings.WeatherData.DailyMaxTemp == 1)
                    data.Add("Max: " + omData.Daily.MaxTemps[day] + " " + omData.DailyUnits.MaxTemp);
                if(settings.WeatherData.DailyMinTemp == 1)
                    data.Add("Min: " + omData.Daily.MinTemps[day] + " " + omData.DailyUnits.MinTemp);
                if(settings.WeatherData.DailyMaxWind == 1)
                    data.Add("Max Wind: " + omData.Daily.MaxWind[day] + " " + omData.DailyUnits.MaxWind);
                if(settings.WeatherData.DailySunshineDuration == 1)
                {
                    if(float.TryParse(omData.Daily.SunshineDuration[day], out float parsed))
                    {
                        int sunshine = (int)parsed;
                        if(sunshine > 60 && omData.DailyUnits.SunshineDuration == "s")
                        {
                            sunshine /= 60;
                            omData.DailyUnits.SunshineDuration = "m";
                        }
                        if(sunshine > 60 && omData.DailyUnits.SunshineDuration == "m")
                        {
                            sunshine /= 60;
                            omData.DailyUnits.SunshineDuration = "h";
                        }
                        data.Add("Sunshine: ~" + sunshine.ToString() + omData.DailyUnits.SunshineDuration);
                    }
                    else
                    {
                        data.Add("Sunshine: " + omData.Daily.SunshineDuration[day] + omData.DailyUnits.SunshineDuration);
                    }
                }
                if(settings.WeatherData.DailyPrecipChance == 1)
                    data.Add("Rain Chance: " + omData.Daily.PrecipChance[day] + " " + omData.DailyUnits.PrecipChance);
                if(settings.WeatherData.DailyTotalPrecip == 1)
                    data.Add("Total Rain: " + omData.Daily.TotalPrecip[day] + " " + omData.DailyUnits.TotalPrecip);
                if(settings.WeatherData.DailyFeelsLikeMax == 1)
                    data.Add("Feels Like Max: " + omData.Daily.FeelsLikeMax[day] + " " + omData.DailyUnits.FeelsLikeMax);
                if(settings.WeatherData.DailyFeelsLikeMin == 1)
                    data.Add("Feels Like Min: " + omData.Daily.FeelsLikeMin[day] + " " + omData.DailyUnits.FeelsLikeMin);
                if(settings.WeatherData.DailyTotalSnowfall == 1)
                    data.Add("Total Snowfall: " + omData.Daily.TotalSnowfall[day] + " " + omData.DailyUnits.TotalSnowfall);
                if(settings.WeatherData.DailyWindDirection == 1)
                {
                    string windDirection = (settings.DirectionUnit == DirectionUnit.degrees) ? omData.Daily.WindDirection[day] + omData.DailyUnits.WindDirection : ToCompass(omData.Daily.WindDirection[day]);
                    data.Add("Wind Direction: " + windDirection);
                }
                if(settings.WeatherData.DailyTotalShortRadiation == 1)
                    data.Add("Total Shortwave: " + omData.Daily.TotalShortRadiation[day] + " " + omData.DailyUnits.TotalShortRadiation);
                if(settings.WeatherData.DailyTotalEvapo == 1)
                    data.Add("Total ET0: " + omData.Daily.TotalEvapo[day] + " " + omData.DailyUnits.TotalEvapo);
                if(settings.WeatherData.DailyMaxUVIndex == 1)
                    data.Add("Max UV: " + omData.Daily.MaxUVIndex[day] + " " + omData.DailyUnits.MaxUVIndex);
            }

            return string.Join("     ", data);
        }

        private static string ToCompass(string direction)
        {
            Int32.TryParse(direction, out int degrees);
            if(degrees >= 22 && degrees <= 68)
            {
                return "NE";
            }
            else if(degrees > 68 && degrees < 112)
            {
                return "E";
            }
            else if(degrees >= 112 && degrees <= 158)
            {
                return "SE";
            }
            else if(degrees > 158 && degrees < 202)
            {
                return "S";
            }
            else if(degrees >= 202 && degrees <= 248)
            {
                return "SW";
            }
            else if(degrees > 248 && degrees < 292)
            {
                return "W";
            }
            else if(degrees >= 292 && degrees <= 338)
            {
                return "NW";
            }
            else
            {
                return "N";
            }
        }

        public static async Task<List<Coordinates>> GetCoordinates(string location)
        {
            if(!GeocodeCache.HasCached(location))
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
                        GeocodeCache.Cache(location, coordinates);
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
                                string region = "";
                                if(searchCountry.Contains("("))
                                {
                                    string[] countryAndRegion = searchCountry.Split('(');
                                    searchCountry = countryAndRegion[0].Trim();
                                    region = countryAndRegion[1].Trim().ToLower();
                                    if(region.Contains(")"))
                                    {
                                        region = region.Remove(region.IndexOf(")"), 1);
                                    }
                                }
                                if(city.Country.ToLower() == searchCountry || city.CountryCode.ToLower() == searchCountry)
                                {
                                    if(region != "")
                                    {
                                        if(city.Region.ToLower() == region)
                                        {
                                            coordinates.Add(city);
                                        }
                                        else if(region.Contains("Post: "))
                                        {
                                            if(city.PostCodes[0].ToLower() == region.Remove(region.IndexOf("Post: "), 1))
                                            {
                                                coordinates.Add(city);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        coordinates.Add(city);
                                    }
                                }
                            }
                            if(coordinates.Count > 0)
                            {
                                GeocodeCache.Cache(location, coordinates);
                                return coordinates;
                            }
                        }
                    }
                    throw new Exception("Can't find this city");
                }
                throw new Exception("Request Error: " + response.StatusCode.ToString());
            }
            return GeocodeCache.Retrieve(location);
        }
    }
}
