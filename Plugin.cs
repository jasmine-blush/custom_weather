using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Wox.Plugin;

namespace custom_weather
{
    internal class Plugin : IPlugin, ISettingProvider
    {
        private PluginInitContext _context;

        public Control CreateSettingPanel()
        {
            return new WeatherSettings();
        }

        public void Init(PluginInitContext context)
        {
            _context = context;
        }

        public List<Result> Query(Query query)
        {
            List<Result> results = new List<Result>();
            string search = query.Search;
            if(search.Length <= 2)
            {
                if(WeatherSettings.UserHometown != "")
                {
                    Result weatherResult = GetWeather(WeatherSettings.UserHometown);
                    results.Add(weatherResult);
                }
                else
                {
                    results.Add(new Result() {
                        Title = "No home town",
                        SubTitle = "Add a home town for default weather",
                        IcoPath = "Images\\plugin.png"
                    });
                }
            }
            else
            {
                Result weatherResult = GetWeather(search);
                results.Add(weatherResult);
            }
            return results;
        }

        private Result GetWeather(string search)
        {
            try
            {
                Coordinates coords = WeatherService.GetCoordinates(search).Result;

                WeatherResult weatherResult = WeatherService.GetWeather(coords).Result;

                string title = coords.Name + ", " + coords.Country + " - " + weatherResult.Title;
                string subTitle = weatherResult.SubTitle;
                string icoPath = weatherResult.IcoPath;
                return new Result() { Title = title, SubTitle = subTitle, IcoPath = icoPath };
            }
            catch(Exception e)
            {
                return new Result() { Title = search, SubTitle = e.InnerException.Message, IcoPath = "Images\\plugin.png" };
            }
        }
    }
}