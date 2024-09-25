using Newtonsoft.Json;

namespace custom_weather
{
    public struct Coordinates
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("country")]
        public string Country;

        [JsonProperty("latitude")]
        public string Latitude;

        [JsonProperty("longitude")]
        public string Longitude;

        public override string ToString()
        {
            return string.Format("{{{0}, {1}}}", Latitude, Longitude);
        }
    }

    public struct OpenMeteoData
    {
        [JsonProperty("current")]
        public CurrentData Current;
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

        [JsonProperty("relative_humidity_2m")]
        public string Humidity;

        [JsonProperty("is_day")]
        public int IsDay;
    }

    public struct WeatherResult
    {
        public string Title;
        public string SubTitle;
        public string IcoPath;
    }
}
