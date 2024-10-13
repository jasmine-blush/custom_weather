using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;
using Newtonsoft.Json;
using Wox.Plugin;

namespace custom_weather
{
    internal class Plugin : IPlugin, ISettingProvider
    {
        private PluginInitContext _context;
        private SettingsSave _settings;

        public Control CreateSettingPanel()
        {
            return new WeatherSettings(_settings);
        }

        public void Init(PluginInitContext context)
        {
            _context = context;
            string configPath = _context.CurrentPluginMetadata.PluginDirectory + "\\config.json";
            if(File.Exists(configPath))
            {
                try
                {
                    _settings = JsonConvert.DeserializeObject<SettingsSave>(File.ReadAllText(configPath));
                    if(_settings == null)
                    {
                        _settings = new SettingsSave();
                    }
                    _settings.ConfigPath = configPath;
                    _settings.Validate();
                }
                catch(Exception)
                {
                    _settings = new SettingsSave();
                    _settings.ConfigPath = configPath;
                    _settings.Validate();
                }

            }
            else
            {
                _settings = new SettingsSave() { ConfigPath = configPath };
            }
        }

        public List<Result> Query(Query query)
        {
            List<Result> results = new List<Result>();
            string search = query.Search;
            if(search.Length <= 2)
            {
                if(!string.IsNullOrEmpty(_settings.Hometown))
                {
                    List<Result> weatherResults = GetWeather(query.ActionKeyword, _settings.Hometown);
                    foreach(Result weatherResult in weatherResults)
                    {
                        results.Add(weatherResult);
                    }
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
                if(search.Contains("!"))
                {
                    List<Result> detailResults = GetDetailWeather(query.ActionKeyword, search);
                    foreach(Result detailResult in detailResults)
                    {
                        results.Add(detailResult);
                    }
                }
                else
                {
                    List<Result> weatherResults = GetWeather(query.ActionKeyword, search);
                    foreach(Result weatherResult in weatherResults)
                    {
                        results.Add(weatherResult);
                    }
                }
            }
            return results;
        }

        private List<Result> GetDetailWeather(string keyword, string search)
        {
            try
            {
                string[] searchAndSelection = search.Split('!');
                search = searchAndSelection[0];

                List<Coordinates> coords = WeatherService.GetCoordinates(search).Result;
                if(coords.Count > 1)
                {
                    if(int.TryParse(searchAndSelection[1], out int selection))
                    {
                        selection--;
                        if(selection >= 0 && selection < coords.Count)
                        {
                            return GetDetailResults(coords, selection);
                        }
                        return new List<Result>() { new Result() { Title = search, SubTitle = "Can't findd this city", IcoPath = "Images\\plugin.png" } };
                    }
                    return GetWeather(keyword, search);
                }
                else if(coords.Count == 1)
                {
                    return GetDetailResults(coords, 0);

                }
                return new List<Result>() { new Result() { Title = search, SubTitle = "Can't finddd this city", IcoPath = "Images\\plugin.png" } };
            }
            catch(Exception e)
            {
                return new List<Result>() { new Result() { Title = search, SubTitle = e.InnerException.Message, IcoPath = "Images\\plugin.png" } };
            }
        }

        private List<Result> GetDetailResults(List<Coordinates> coords, int index)
        {
            List<Result> results = new List<Result>();
            List<WeatherResult> detailResults = WeatherService.GetDetailWeather(coords[index], _settings).Result;
            foreach(WeatherResult detailResult in detailResults)
            {
                results.Add(new Result() { Title = detailResult.Title, SubTitle = detailResult.SubTitle, IcoPath = detailResult.IcoPath });
            }
            return results;
        }

        private List<Result> GetWeather(string keyword, string search)
        {
            try
            {
                List<Coordinates> coords = WeatherService.GetCoordinates(search).Result;

                List<Result> results = new List<Result>();
                foreach(Coordinates coord in coords)
                {
                    WeatherResult weatherResult = WeatherService.GetWeather(coord, _settings).Result;

                    string title = coord.Name + ", " + coord.Country;
                    if(HasMultipleInSameCountry(coord, coords))
                    {
                        if(coord.Region != null)
                        {
                            title += " (" + coord.Region + ")";
                        }
                        else if(coord.PostCodes != null)
                        {
                            if(coord.PostCodes.Length > 0)
                            {
                                title += " (Post: " + coord.PostCodes[0] + ")";
                            }
                        }
                    }
                    string locationOnly = title;
                    Func<ActionContext, bool> DetailViewAction = ac => { _context.API.ChangeQuery(keyword + " " + locationOnly + "!"); return false; };
                    title += " - " + weatherResult.Title;
                    string subTitle = weatherResult.SubTitle;
                    string icoPath = weatherResult.IcoPath;
                    results.Add(new Result() { Title = title, SubTitle = subTitle, IcoPath = icoPath, Action = DetailViewAction });
                }
                return results;
            }
            catch(Exception e)
            {
                return new List<Result>() { new Result() { Title = search, SubTitle = e.InnerException.Message, IcoPath = "Images\\plugin.png" } };
            }
        }

        private bool HasMultipleInSameCountry(Coordinates coord, List<Coordinates> coords)
        {
            foreach(Coordinates other in coords)
            {
                if(other.Name == coord.Name && other.Country == coord.Country && !other.Equals(coord))
                {
                    return true;
                }
            }
            return false;
        }
    }
}