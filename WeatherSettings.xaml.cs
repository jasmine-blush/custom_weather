using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace custom_weather
{
    /// <summary>
    /// Interaction logic for WeatherSettings.xaml
    /// </summary>
    public partial class WeatherSettings : UserControl
    {
        private readonly string _hometown_help = "Whenever you type the action keyword by itself, the weather of your home town will be displayed.";
        private readonly string _cache_help = "Weather data on Open-Meteo is updated every 15mins. Set how long the plugin should cache weather data for before updating it.";
        private readonly string _temp_help = "Set the default unit for Temperature.";
        private readonly string _wind_help = "Set the default unit for Wind Speed.";
        private readonly string _rain_help = "Set the default unit for Precipitation amount.";
        private readonly string _direction_help = "Set whether Wind Direction is displayed in degrees or compass directions.";
        private readonly string _data_help = "Adjust which weather data is shown in the results.";
        private readonly string _cape_help = "Convective available potential energy";
        private readonly string _evapo_help = "ET₀ Reference Evapotranspiration";
        private readonly SettingsSave _settings;

        public WeatherSettings(SettingsSave settings)
        {
            InitializeComponent();

            CreateToolTip(HometownInfo, _hometown_help);
            CreateToolTip(CacheDurationInfo, _cache_help);
            CreateToolTip(TempUnitInfo, _temp_help);
            CreateToolTip(WindUnitInfo, _wind_help);
            CreateToolTip(RainUnitInfo, _rain_help);
            CreateToolTip(DirectionUnitInfo, _direction_help);
            CreateToolTip(WeatherDataInfo, _data_help);
            CreateToolTip(CAPEInfo, _cape_help);
            CreateToolTip(EvapoInfo, _evapo_help);

            _settings = settings;
        }

        private void CreateToolTip(TextBlock element, string tooltip)
        {
            ToolTip tt = new ToolTip {
                Content = tooltip
            };
            ToolTipService.SetInitialShowDelay(tt, 0);
            element.MouseLeftButtonDown += (s, e) => {
                tt.IsOpen = true;
            };
            element.MouseLeave += (s, e) => {
                tt.IsOpen = false;
            };
            element.ToolTip = tt;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            HometownTextbox.Text = _settings.Hometown;
            CacheDurationComboBox.SelectedIndex = (int)_settings.CacheDuration;
            TempUnitComboBox.SelectedIndex = (int)_settings.TempUnit;
            WindUnitComboBox.SelectedIndex = (int)_settings.WindUnit;
            RainUnitComboBox.SelectedIndex = (int)_settings.RainUnit;
            DirectionUnitComboBox.SelectedIndex = (int)_settings.DirectionUnit;

            MaxTempCheckBox.IsChecked = _settings.WeatherData.MaxTemp == 1 ? true : false;
            MinTempCheckBox.IsChecked = _settings.WeatherData.MinTemp == 1 ? true : false;
            WindSpeedCheckBox.IsChecked = _settings.WeatherData.WindSpeed == 1 ? true : false;
            WindDirectionCheckBox.IsChecked = _settings.WeatherData.WindDirection == 1 ? true : false;
            FeelsLikeCheckBox.IsChecked = _settings.WeatherData.FeelsLike == 1 ? true : false;
            HumidityCheckBox.IsChecked = _settings.WeatherData.Humidity == 1 ? true : false;
            DewPointCheckBox.IsChecked = _settings.WeatherData.DewPoint == 1 ? true : false;
            PressureCheckBox.IsChecked = _settings.WeatherData.Pressure == 1 ? true : false;
            CloudCoverCheckBox.IsChecked = _settings.WeatherData.CloudCover == 1 ? true : false;
            TotalPrecipCheckBox.IsChecked = _settings.WeatherData.TotalPrecip == 1 ? true : false;
            PrecipChanceCheckBox.IsChecked = _settings.WeatherData.PrecipChance == 1 ? true : false;
            SnowfallCheckBox.IsChecked = _settings.WeatherData.Snowfall == 1 ? true : false;
            SnowDepthCheckBox.IsChecked = _settings.WeatherData.SnowDepth == 1 ? true : false;
            VisibilityCheckBox.IsChecked = _settings.WeatherData.Visibility == 1 ? true : false;

            ShortRadiationCheckBox.IsChecked = _settings.WeatherData.ShortRadiation == 1 ? true : false;
            DirectRadiationCheckBox.IsChecked = _settings.WeatherData.DirectRadiation == 1 ? true : false;
            DiffuseRadiationCheckBox.IsChecked = _settings.WeatherData.DiffuseRadiation == 1 ? true : false;
            VPDeficitCheckBox.IsChecked = _settings.WeatherData.VPDeficit == 1 ? true : false;
            CAPECheckBox.IsChecked = _settings.WeatherData.CAPE == 1 ? true : false;
            EvapoCheckBox.IsChecked = _settings.WeatherData.Evapo == 1 ? true : false;
            FreezingHeightCheckBox.IsChecked = _settings.WeatherData.FreezingHeight == 1 ? true : false;
            SoilTemperatureCheckBox.IsChecked = _settings.WeatherData.SoilTemperature == 1 ? true : false;
            SoilMoistureCheckBox.IsChecked = _settings.WeatherData.SoilMoisture == 1 ? true : false;
        }

        private void HometownTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(IsLoaded)
            {
                _settings.Hometown = HometownTextbox.Text;
                _settings.Save();
            }
        }

        private void CacheDuration_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(IsLoaded)
            {
                _settings.CacheDuration = (CacheDurations)CacheDurationComboBox.SelectedIndex;
                _settings.Save();
            }
        }

        private void TempUnit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(IsLoaded)
            {
                _settings.TempUnit = (TemperatureUnit)TempUnitComboBox.SelectedIndex;
                _settings.Save();
            }
        }

        private void WindUnit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(IsLoaded)
            {
                _settings.WindUnit = (WindSpeedUnit)WindUnitComboBox.SelectedIndex;
                _settings.Save();
            }
        }

        private void RainUnit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(IsLoaded)
            {
                _settings.RainUnit = (PrecipitationUnit)RainUnitComboBox.SelectedIndex;
                _settings.Save();
            }
        }

        private void DirectionUnit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(IsLoaded)
            {
                _settings.DirectionUnit = (DirectionUnit)DirectionUnitComboBox.SelectedIndex;
                _settings.Save();
            }
        }

        private void MaxTempCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if(IsLoaded)
            {
                _settings.WeatherData.MaxTemp = MaxTempCheckBox.IsChecked.Value ? 1 : 0;
                _settings.Save();
            }
        }

        private void MinTempCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if(IsLoaded)
            {
                _settings.WeatherData.MinTemp = MinTempCheckBox.IsChecked.Value ? 1 : 0;
                _settings.Save();
            }
        }

        private void WindSpeedCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if(IsLoaded)
            {
                _settings.WeatherData.WindSpeed = WindSpeedCheckBox.IsChecked.Value ? 1 : 0;
                _settings.Save();
            }
        }

        private void WindDirectionCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if(IsLoaded)
            {
                _settings.WeatherData.WindDirection = WindDirectionCheckBox.IsChecked.Value ? 1 : 0;
                _settings.Save();
            }
        }

        private void FeelsLikeCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if(IsLoaded)
            {
                _settings.WeatherData.FeelsLike = FeelsLikeCheckBox.IsChecked.Value ? 1 : 0;
                _settings.Save();
            }
        }

        private void HumidityCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if(IsLoaded)
            {
                _settings.WeatherData.Humidity = HumidityCheckBox.IsChecked.Value ? 1 : 0;
                _settings.Save();
            }
        }

        private void DewPointCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if(IsLoaded)
            {
                _settings.WeatherData.DewPoint = DewPointCheckBox.IsChecked.Value ? 1 : 0;
                _settings.Save();
            }
        }

        private void PressureCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if(IsLoaded)
            {
                _settings.WeatherData.Pressure = PressureCheckBox.IsChecked.Value ? 1 : 0;
                _settings.Save();
            }
        }

        private void CloudCoverCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if(IsLoaded)
            {
                _settings.WeatherData.CloudCover = CloudCoverCheckBox.IsChecked.Value ? 1 : 0;
                _settings.Save();
            }
        }

        private void TotalPrecipCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if(IsLoaded)
            {
                _settings.WeatherData.TotalPrecip = TotalPrecipCheckBox.IsChecked.Value ? 1 : 0;
                _settings.Save();
            }
        }

        private void PrecipChanceCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if(IsLoaded)
            {
                _settings.WeatherData.PrecipChance = PrecipChanceCheckBox.IsChecked.Value ? 1 : 0;
                _settings.Save();
            }
        }

        private void SnowfallCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if(IsLoaded)
            {
                _settings.WeatherData.Snowfall = SnowfallCheckBox.IsChecked.Value ? 1 : 0;
                _settings.Save();
            }
        }

        private void SnowDepthCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if(IsLoaded)
            {
                _settings.WeatherData.SnowDepth = SnowDepthCheckBox.IsChecked.Value ? 1 : 0;
                _settings.Save();
            }
        }

        private void VisibilityCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if(IsLoaded)
            {
                _settings.WeatherData.Visibility = VisibilityCheckBox.IsChecked.Value ? 1 : 0;
                _settings.Save();
            }
        }

        private void ShortRadiationCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if(IsLoaded)
            {
                _settings.WeatherData.ShortRadiation = ShortRadiationCheckBox.IsChecked.Value ? 1 : 0;
                _settings.Save();
            }
        }

        private void DirectRadiationCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if(IsLoaded)
            {
                _settings.WeatherData.DirectRadiation = DirectRadiationCheckBox.IsChecked.Value ? 1 : 0;
                _settings.Save();
            }
        }

        private void DiffuseRadiationCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if(IsLoaded)
            {
                _settings.WeatherData.DiffuseRadiation = DiffuseRadiationCheckBox.IsChecked.Value ? 1 : 0;
                _settings.Save();
            }
        }

        private void VPDeficitCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if(IsLoaded)
            {
                _settings.WeatherData.VPDeficit = VPDeficitCheckBox.IsChecked.Value ? 1 : 0;
                _settings.Save();
            }
        }

        private void CAPECheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if(IsLoaded)
            {
                _settings.WeatherData.CAPE = CAPECheckBox.IsChecked.Value ? 1 : 0;
                _settings.Save();
            }
        }

        private void EvapoCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if(IsLoaded)
            {
                _settings.WeatherData.Evapo = EvapoCheckBox.IsChecked.Value ? 1 : 0;
                _settings.Save();
            }
        }

        private void FreezingHeightCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if(IsLoaded)
            {
                _settings.WeatherData.FreezingHeight = FreezingHeightCheckBox.IsChecked.Value ? 1 : 0;
                _settings.Save();
            }
        }

        private void SoilTemperatureCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if(IsLoaded)
            {
                _settings.WeatherData.SoilTemperature = SoilTemperatureCheckBox.IsChecked.Value ? 1 : 0;
                _settings.Save();
            }
        }

        private void SoilMoistureCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if(IsLoaded)
            {
                _settings.WeatherData.SoilMoisture = SoilMoistureCheckBox.IsChecked.Value ? 1 : 0;
                _settings.Save();
            }
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.AbsoluteUri);
            e.Handled = true;
        }
    }
}
