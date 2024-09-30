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

        public void Save()
        {
            File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(this));
        }
    }
}
