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
        private readonly string _temp_help = "Set the default unit for Temperature.";
        private readonly string _wind_help = "Set the default unit for Wind Speed.";
        private readonly string _rain_help = "Set the default unit for Precipitation amount.";
        private readonly string _direction_help = "Set whether Wind Direction is displayed in degrees or compass directions.";
        private readonly SettingsSave _settings;

        public WeatherSettings(SettingsSave settings)
        {
            InitializeComponent();

            CreateToolTip(HometownInfo, _hometown_help);
            CreateToolTip(TempUnitInfo, _temp_help);
            CreateToolTip(WindUnitInfo, _wind_help);
            CreateToolTip(RainUnitInfo, _rain_help);
            CreateToolTip(DirectionUnitInfo, _direction_help);

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
            TempUnitComboBox.SelectedIndex = (int)_settings.TempUnit;
            WindUnitComboBox.SelectedIndex = (int)_settings.WindUnit;
            RainUnitComboBox.SelectedIndex = (int)_settings.RainUnit;
            DirectionUnitComboBox.SelectedIndex = (int)_settings.DirectionUnit;
        }

        private void HometownTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _settings.Hometown = HometownTextbox.Text;
            _settings.Save();
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

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.AbsoluteUri);
            e.Handled = true;
        }
    }
}
