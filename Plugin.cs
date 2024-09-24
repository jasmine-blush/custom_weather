using System.Collections.Generic;
using System.Diagnostics;
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
                Stopwatch stopwatch = Stopwatch.StartNew();
                Coordinates coords = WeatherService.GetCoordinates(search).Result;
                stopwatch.Stop();
                string getcoordstime = stopwatch.Elapsed.TotalMilliseconds.ToString();
                string getweathertime = "";
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
                    title = coords.Name + ", " + coords.Country;
                    stopwatch.Restart();
                    subTitle = WeatherService.GetWeather(coords).Result;
                    stopwatch.Stop();
                    getweathertime = stopwatch.Elapsed.TotalMilliseconds.ToString();
                }
                results.Add(new Result() {
                    Title = title,
                    SubTitle = subTitle + " | " + getcoordstime + " | " + getweathertime,
                    IcoPath = "Images\\plugin.png"
                });
            }
            return results;
        }
    }
}
