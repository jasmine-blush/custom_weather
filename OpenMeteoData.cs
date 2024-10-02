using Newtonsoft.Json;

namespace custom_weather
{
    public struct Coordinates
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("country")]
        public string Country;

        [JsonProperty("country_code")]
        public string CountryCode;

        [JsonProperty("latitude")]
        public string Latitude;

        [JsonProperty("longitude")]
        public string Longitude;

        [JsonProperty("admin1")]
        public string Region;

        [JsonProperty("postcodes")]
        public string[] PostCodes;

        public override string ToString()
        {
            return string.Format("{{{0}, {1}}}", Latitude, Longitude);
        }
    }

    public struct OpenMeteoData
    {
        [JsonProperty("current")]
        public CurrentData Current;

        [JsonProperty("daily")]
        public DailyData Daily;
    }

    public struct CurrentData
    {
        [JsonProperty("weather_code")]
        public int WeatherCode;

        [JsonProperty("temperature_2m")]
        public string Temperature;

        [JsonProperty("surface_pressure")]
        public string Pressure;

        [JsonProperty("wind_speed_10m")]
        public string WindSpeed;

        [JsonProperty("wind_direction_10m")]
        public string WindDirection;

        [JsonProperty("relative_humidity_2m")]
        public string Humidity;

        [JsonProperty("is_day")]
        public int IsDay;

        [JsonProperty("precipitation_probability")]
        public string RainChance;

        [JsonProperty("apparent_temperature")]
        public string FeelsLike;
    }

    public struct DailyData
    {
        [JsonProperty("temperature_2m_min")]
        public string[] MinTemps;

        [JsonProperty("temperature_2m_max")]
        public string[] MaxTemps;
    }

    public struct WeatherResult
    {
        public long CacheTime;
        public string Title;
        public string SubTitle;
        public string IcoPath;
    }
}
