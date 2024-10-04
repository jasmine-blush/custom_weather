using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Newtonsoft.Json;

namespace custom_weather
{
    public class SettingsSave
    {
        [JsonIgnore]
        public string ConfigPath;

        [JsonProperty("hometown")]
        public string Hometown = "";

        [JsonProperty("cacheduration")]
        public CacheDurations CacheDuration = CacheDurations.fivemins;

        [JsonProperty("tempunit")]
        public TemperatureUnit TempUnit = TemperatureUnit.celsius;

        [JsonProperty("windunit")]
        public WindSpeedUnit WindUnit = WindSpeedUnit.kmh;

        [JsonProperty("rainunit")]
        public PrecipitationUnit RainUnit = PrecipitationUnit.mm;

        [JsonProperty("directionunit")]
        public DirectionUnit DirectionUnit = DirectionUnit.degrees;

        [JsonProperty("weatherdata")]
        public DataSettings WeatherData = new DataSettings();

        public void Validate()
        {
            if(!Enum.IsDefined(typeof(CacheDurations), CacheDuration))
            {
                CacheDuration = CacheDurations.fivemins;
            }
            if(!Enum.IsDefined(typeof(TemperatureUnit), TempUnit))
            {
                TempUnit = TemperatureUnit.celsius;
            }
            if(!Enum.IsDefined(typeof(WindSpeedUnit), WindUnit))
            {
                WindUnit = WindSpeedUnit.kmh;
            }
            if(!Enum.IsDefined(typeof(PrecipitationUnit), RainUnit))
            {
                RainUnit = PrecipitationUnit.mm;
            }
            if(!Enum.IsDefined(typeof(DirectionUnit), DirectionUnit))
            {
                DirectionUnit = DirectionUnit.degrees;
            }

            if(WeatherData == null)
            {
                WeatherData = new DataSettings();
            }
            else
            {
                WeatherData.Validate();
            }
        }

        public void Save()
        {
            File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(this));
        }
    }

    public enum CacheDurations
    {
        [Description("5")]
        fivemins = 0,

        [Description("15")]
        fifteenmins = 1,

        [Description("60")]
        onehour = 2,

        [Description("1440")]
        oneday = 3,
    }

    public enum TemperatureUnit
    {
        [Description("°C")]
        celsius = 0,

        [Description("°F")]
        fahrenheit = 1,
    }

    public enum WindSpeedUnit
    {
        [Description("km/h")]
        kmh = 0,

        [Description("m/s")]
        ms = 1,

        [Description("mph")]
        mph = 2,

        [Description("kn")]
        kn = 3,
    }

    public enum PrecipitationUnit
    {
        [Description("mm")]
        mm = 0,

        [Description("inch")]
        inch = 1,
    }

    public enum DirectionUnit
    {
        [Description("°")]
        degrees = 0,

        [Description("")]
        compass = 1,
    }

    public class DataSettings
    {
        [JsonProperty("maxtemp")]
        public int MaxTemp = 1;

        [JsonProperty("mintemp")]
        public int MinTemp = 1;

        [JsonProperty("windspeed")]
        public int WindSpeed = 1;

        [JsonProperty("winddirection")]
        public int WindDirection = 1;

        [JsonProperty("feelslike")]
        public int FeelsLike = 1;

        [JsonProperty("humidity")]
        public int Humidity = 1;

        [JsonProperty("dewpoint")]
        public int DewPoint = 0;

        [JsonProperty("pressure")]
        public int Pressure = 0;

        [JsonProperty("cloudcover")]
        public int CloudCover = 0;

        [JsonProperty("totalprecip")]
        public int TotalPrecip = 0;

        [JsonProperty("precipchance")]
        public int PrecipChance = 0;

        [JsonProperty("snowfall")]
        public int Snowfall = 0;

        [JsonProperty("snowdepth")]
        public int SnowDepth = 0;

        [JsonProperty("visibility")]
        public int Visibility = 0;

        public void Validate()
        {
            MaxTemp = IsValid(MaxTemp) ? MaxTemp : 1;
            MinTemp = IsValid(MinTemp) ? MinTemp : 1;
            WindSpeed = IsValid(WindSpeed) ? WindSpeed : 1;
            WindDirection = IsValid(WindDirection) ? WindDirection : 1;
            FeelsLike = IsValid(FeelsLike) ? FeelsLike : 1;
            Humidity = IsValid(Humidity) ? Humidity : 1;

            DewPoint = IsValid(DewPoint) ? DewPoint : 0;
            Pressure = IsValid(Pressure) ? Pressure : 0;
            CloudCover = IsValid(CloudCover) ? CloudCover : 0;
            TotalPrecip = IsValid(TotalPrecip) ? TotalPrecip : 0;
            PrecipChance = IsValid(PrecipChance) ? PrecipChance : 0;
            Snowfall = IsValid(Snowfall) ? Snowfall : 0;
            SnowDepth = IsValid(SnowDepth) ? SnowDepth : 0;
            Visibility = IsValid(Visibility) ? Visibility : 0;
        }

        private bool IsValid(int value)
        {
            return (value == 0 || value == 1);
        }

        public string GetCurrent()
        {
            List<string> data = new List<string>();
            if(WindSpeed == 1)
                data.Add("wind_speed_10m");
            if(WindDirection == 1)
                data.Add("wind_direction_10m");
            if(FeelsLike == 1)
                data.Add("apparent_temperature");
            if(Humidity == 1)
                data.Add("relative_humidity_2m");
            if(DewPoint == 1)
                data.Add("dew_point_2m");
            if(Pressure == 1)
                data.Add("surface_pressure");
            if(CloudCover == 1)
                data.Add("cloud_cover");
            if(TotalPrecip == 1)
                data.Add("precipitation");
            if(PrecipChance == 1)
                data.Add("precipitation_probability");
            if(Snowfall == 1)
                data.Add("snowfall");
            if(SnowDepth == 1)
                data.Add("snow_depth");
            if(Visibility == 1)
                data.Add("visibility");

            return string.Join(",", data);
        }

        public string GetDaily()
        {
            List<string> data = new List<string>();
            if(MaxTemp == 1)
                data.Add("temperature_2m_max");
            if(MinTemp == 1)
                data.Add("temperature_2m_min");

            return string.Join(",", data);
        }
    }

    public static class EnumExtension
    {
        public static string GetDescription(this Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }
    }
}
