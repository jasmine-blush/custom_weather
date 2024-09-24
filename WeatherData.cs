using Newtonsoft.Json;

namespace custom_weather
{
    internal struct Coordinates
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

    internal struct WeatherData
    {
        [JsonProperty("current")]
        public CurrentData Current;
    }

    internal struct CurrentData
    {
        [JsonProperty("weather_code")]
        public int WeatherCode;
    }
}
