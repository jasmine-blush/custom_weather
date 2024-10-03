using System;
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
