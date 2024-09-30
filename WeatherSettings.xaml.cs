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
        private readonly SettingsSave _settings;

        public WeatherSettings(SettingsSave settings)
        {
            InitializeComponent();

            ToolTip tt = new ToolTip {
                Content = _hometown_help
            };
            ToolTipService.SetInitialShowDelay(tt, 0);
            HometownInfo.MouseLeftButtonDown += (s, e) => {
                tt.IsOpen = true;
            };
            HometownInfo.MouseLeave += (s, e) => {
                tt.IsOpen = false;
            };
            HometownInfo.ToolTip = tt;

            _settings = settings;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            HometownTextbox.Text = _settings.Hometown;
        }

        private void HometownTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _settings.Hometown = HometownTextbox.Text;
            _settings.Save();
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.AbsoluteUri);
            e.Handled = true;
        }
    }
}
