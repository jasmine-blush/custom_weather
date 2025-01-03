﻿using System;
using System.Collections.Generic;

namespace custom_weather
{
    internal static class GeocodeCache
    {
        private static readonly Dictionary<string, List<Coordinates>> _cache = new Dictionary<string, List<Coordinates>>();

        internal static void Cache(string key, List<Coordinates> value)
        {
            if(!_cache.ContainsKey(key))
            {
                _cache.Add(key, value);
            }
        }

        internal static bool HasCached(string key)
        {
            return _cache.ContainsKey(key);
        }

        internal static List<Coordinates> Retrieve(string key)
        {
            if(_cache.ContainsKey(key))
            {
                return _cache[key];
            }
            return null;
        }
    }

    internal static class WeatherCache
    {
        private static readonly Dictionary<string, WeatherResult> _cache = new Dictionary<string, WeatherResult>();

        internal static void Cache(string key, WeatherResult value)
        {
            value.CacheTime = DateTime.Now.Ticks / TimeSpan.TicksPerMinute;
            if(_cache.ContainsKey(key))
            {
                _cache[key] = value;
            }
            else
            {
                _cache.Add(key, value);
            }
        }

        internal static bool HasCached(string key, int cacheDuration)
        {
            if(_cache.ContainsKey(key))
            {
                WeatherResult result = _cache[key];
                long currentTimeInMinutes = DateTime.Now.Ticks / TimeSpan.TicksPerMinute;
                if(currentTimeInMinutes - result.CacheTime >= cacheDuration) // Cache expired
                {
                    _cache.Remove(key);
                    return false;
                }
                return true;
            }
            return false;
        }

        internal static WeatherResult Retrieve(string key)
        {
            if(_cache.ContainsKey(key))
            {
                return _cache[key];
            }
            return new WeatherResult();
        }
    }
}
