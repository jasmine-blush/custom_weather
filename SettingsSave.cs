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

        [JsonProperty("shortradiation")]
        public int ShortRadiation = 0;

        [JsonProperty("directradiation")]
        public int DirectRadiation = 0;

        [JsonProperty("diffuseradiation")]
        public int DiffuseRadiation = 0;

        [JsonProperty("vpdeficit")]
        public int VPDeficit = 0;

        [JsonProperty("cape")]
        public int CAPE = 0;

        [JsonProperty("evapo")]
        public int Evapo = 0;

        [JsonProperty("freezingheight")]
        public int FreezingHeight = 0;

        [JsonProperty("soiltemperature")]
        public int SoilTemperature = 0;

        [JsonProperty("soilmoisture")]
        public int SoilMoisture = 0;

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

            ShortRadiation = IsValid(ShortRadiation) ? ShortRadiation : 0;
            DirectRadiation = IsValid(DirectRadiation) ? DirectRadiation : 0;
            DiffuseRadiation = IsValid(DiffuseRadiation) ? DiffuseRadiation : 0;
            VPDeficit = IsValid(VPDeficit) ? VPDeficit : 0;
            CAPE = IsValid(CAPE) ? CAPE : 0;
            Evapo = IsValid(Evapo) ? Evapo : 0;
            FreezingHeight = IsValid(FreezingHeight) ? FreezingHeight : 0;
            SoilTemperature = IsValid(SoilTemperature) ? SoilTemperature : 0;
            SoilMoisture = IsValid(SoilMoisture) ? SoilMoisture : 0;
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

            if(ShortRadiation == 1)
                data.Add("shortwave_radiation");
            if(DirectRadiation == 1)
                data.Add("direct_radiation");
            if(DiffuseRadiation == 1)
                data.Add("diffuse_radiation");
            if(VPDeficit == 1)
                data.Add("vapour_pressure_deficit");
            if(CAPE == 1)
                data.Add("cape");
            if(Evapo == 1)
                data.Add("et0_fao_evapotranspiration");
            if(FreezingHeight == 1)
                data.Add("freezing_level_height");
            if(SoilTemperature == 1)
                data.Add("soil_temperature_0cm");
            if(SoilMoisture == 1)
                data.Add("soil_moisture_0_to_1cm");

            return string.Join(",", data);
        }

        public string GetDaily()
        {
            List<string> data = new List<string>();
            /*if(MaxTemp == 1)
                data.Add("temperature_2m_max");
            if(MinTemp == 1)
                data.Add("temperature_2m_min");*/

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
