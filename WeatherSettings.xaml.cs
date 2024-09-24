using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace custom_weather
{
    /// <summary>
    /// Interaction logic for WeatherSettings.xaml
    /// </summary>
    public partial class WeatherSettings : UserControl
    {
        private readonly string _hometown_help = "Whenever you type the action keyword by itself, the weather of your home town will be displayed.";
        private string _hometown = "";

        public WeatherSettings()
        {
            InitializeComponent();

            ToolTip tt = new ToolTip {
                Content = _hometown_help
            };
            ToolTipService.SetInitialShowDelay(tt, 0);
            HomeTownInfo.MouseLeftButtonDown += (s, e) => {
                tt.IsOpen = true;
            };
            HomeTownInfo.MouseLeave += (s, e) => {
                tt.IsOpen = false;
            };
            HomeTownInfo.ToolTip = tt;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Keyboard.ClearFocus();
        }

        private void HomeTown_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                Keyboard.ClearFocus();
            }
        }

        private void HomeTown_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            _hometown = HomeTown.Text;
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.AbsoluteUri);
            e.Handled = true;
        }
    }
}
