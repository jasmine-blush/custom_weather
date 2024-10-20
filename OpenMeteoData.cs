﻿using Newtonsoft.Json;

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

        [JsonProperty("current_units")]
        public CurrentUnits CurrentUnits;

        [JsonProperty("daily")]
        public DailyData Daily;

        [JsonProperty("daily_units")]
        public DailyUnits DailyUnits;
    }

    public struct CurrentData
    {
        [JsonProperty("weather_code")]
        public int WeatherCode;

        [JsonProperty("temperature_2m")]
        public string Temperature;

        [JsonProperty("is_day")]
        public int IsDay;

        [JsonProperty("wind_speed_10m")]
        public string WindSpeed;

        [JsonProperty("wind_direction_10m")]
        public string WindDirection;

        [JsonProperty("apparent_temperature")]
        public string FeelsLike;

        [JsonProperty("relative_humidity_2m")]
        public string Humidity;

        [JsonProperty("dew_point_2m")]
        public string DewPoint;

        [JsonProperty("surface_pressure")]
        public string Pressure;

        [JsonProperty("cloud_cover")]
        public string CloudCover;

        [JsonProperty("precipitation")]
        public string TotalPrecip;

        [JsonProperty("precipitation_probability")]
        public string PrecipChance;

        [JsonProperty("snowfall")]
        public string Snowfall;

        [JsonProperty("snow_depth")]
        public string SnowDepth;

        [JsonProperty("visibility")]
        public string Visibility;


        [JsonProperty("shortwave_radiation")]
        public string ShortRadiation;

        [JsonProperty("direct_radiation")]
        public string DirectRadiation;

        [JsonProperty("diffuse_radiation")]
        public string DiffuseRadiation;

        [JsonProperty("vapour_pressure_deficit")]
        public string VPDeficit;

        [JsonProperty("cape")]
        public string CAPE;

        [JsonProperty("et0_fao_evapotranspiration")]
        public string Evapo;

        [JsonProperty("freezing_level_height")]
        public string FreezingHeight;

        [JsonProperty("soil_temperature_0cm")]
        public string SoilTemperature;

        [JsonProperty("soil_moisture_0_to_1cm")]
        public string SoilMoisture;
    }

    public struct CurrentUnits
    {
        [JsonProperty("temperature_2m")]
        public string Temperature;

        [JsonProperty("wind_speed_10m")]
        public string WindSpeed;

        [JsonProperty("wind_direction_10m")]
        public string WindDirection;

        [JsonProperty("apparent_temperature")]
        public string FeelsLike;

        [JsonProperty("relative_humidity_2m")]
        public string Humidity;

        [JsonProperty("dew_point_2m")]
        public string DewPoint;

        [JsonProperty("surface_pressure")]
        public string Pressure;

        [JsonProperty("cloud_cover")]
        public string CloudCover;

        [JsonProperty("precipitation")]
        public string TotalPrecip;

        [JsonProperty("precipitation_probability")]
        public string PrecipChance;

        [JsonProperty("snowfall")]
        public string Snowfall;

        [JsonProperty("snow_depth")]
        public string SnowDepth;

        [JsonProperty("visibility")]
        public string Visibility;


        [JsonProperty("shortwave_radiation")]
        public string ShortRadiation;

        [JsonProperty("direct_radiation")]
        public string DirectRadiation;

        [JsonProperty("diffuse_radiation")]
        public string DiffuseRadiation;

        [JsonProperty("vapour_pressure_deficit")]
        public string VPDeficit;

        [JsonProperty("cape")]
        public string CAPE;

        [JsonProperty("et0_fao_evapotranspiration")]
        public string Evapo;

        [JsonProperty("freezing_level_height")]
        public string FreezingHeight;

        [JsonProperty("soil_temperature_0cm")]
        public string SoilTemperature;

        [JsonProperty("soil_moisture_0_to_1cm")]
        public string SoilMoisture;
    }

    public struct DailyData
    {
        [JsonProperty("weather_code")]
        public int[] WeatherCode;

        [JsonProperty("temperature_2m_max")]
        public string[] MaxTemps;

        [JsonProperty("temperature_2m_min")]
        public string[] MinTemps;

        [JsonProperty("wind_speed_10m_max")]
        public string[] MaxWind;

        [JsonProperty("sunshine_duration")]
        public string[] SunshineDuration;

        [JsonProperty("precipitation_probability_mean")]
        public string[] PrecipChance;

        [JsonProperty("precipitation_sum")]
        public string[] TotalPrecip;

        [JsonProperty("apparent_temperature_max")]
        public string[] FeelsLikeMax;

        [JsonProperty("apparent_temperature_min")]
        public string[] FeelsLikeMin;

        [JsonProperty("snowfall_sum")]
        public string[] TotalSnowfall;

        [JsonProperty("wind_direction_10m_dominant")]
        public string[] WindDirection;

        [JsonProperty("shortwave_radiation_sum")]
        public string[] TotalShortRadiation;

        [JsonProperty("et0_fao_evapotranspiration")]
        public string[] TotalEvapo;

        [JsonProperty("uv_index_max")]
        public string[] MaxUVIndex;
    }

    public struct DailyUnits
    {
        [JsonProperty("temperature_2m_max")]
        public string MaxTemp;

        [JsonProperty("temperature_2m_min")]
        public string MinTemp;

        [JsonProperty("wind_speed_10m_max")]
        public string MaxWind;

        [JsonProperty("sunshine_duration")]
        public string SunshineDuration;

        [JsonProperty("precipitation_probability_mean")]
        public string PrecipChance;

        [JsonProperty("precipitation_sum")]
        public string TotalPrecip;

        [JsonProperty("apparent_temperature_max")]
        public string FeelsLikeMax;

        [JsonProperty("apparent_temperature_min")]
        public string FeelsLikeMin;

        [JsonProperty("snowfall_sum")]
        public string TotalSnowfall;

        [JsonProperty("wind_direction_10m_dominant")]
        public string WindDirection;

        [JsonProperty("shortwave_radiation_sum")]
        public string TotalShortRadiation;

        [JsonProperty("et0_fao_evapotranspiration")]
        public string TotalEvapo;

        [JsonProperty("uv_index_max")]
        public string MaxUVIndex;
    }

    public struct WeatherResult
    {
        public long CacheTime;
        public string Title;
        public string SubTitle;
        public string IcoPath;
    }
}
