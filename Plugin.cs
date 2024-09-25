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
                results.Add(new Result() {
                    Title = "Default City Name",
                    SubTitle = "weather type | temp | pressure | humid | ",
                    IcoPath = "Images\\plugin.png"
                });
            }
            else
            {
                string title = search;
                string subTitle = "";
                string icoPath = "Images\\plugin.png";
                Coordinates coords = WeatherService.GetCoordinates(search).Result;
                if(coords.Name == null)
                {
                    if(coords.Country == null)
                    {
                        subTitle = "Can't find this city";
                    }
                    else
                    {
                        subTitle = coords.Country; //outputs http status code
                    }
                }
                else
                {
                    string location = coords.Name + ", " + coords.Country;
                    WeatherResult weatherResult = WeatherService.GetWeather(location, coords).Result;
                    title = weatherResult.Title;
                    subTitle = weatherResult.SubTitle;
                    if(weatherResult.IcoPath != null)
                    {
                        icoPath = weatherResult.IcoPath;
                    }
                }
                results.Add(new Result() {
                    Title = title,
                    SubTitle = subTitle,
                    IcoPath = icoPath
                });
            }
            return results;
        }
    }
}
