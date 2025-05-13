using ChessGameApplication.JsonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ChessGameApplication.Windows
{
    public partial class SettingsWindow : Window
    {
        private AppSettings Settings;
        public SettingsWindow()
        {
            Settings = SettingsManager.Instance.Settings!;

            InitializeComponent();

            ApplyTheme();
        }

        private void ThemeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is not RadioButton radio || radio.Tag == null) return;

            string themeTag = radio.Tag.ToString()!;

            SettingsManager.SetTheme(themeTag);

            ApplyTheme();
        }
        private void ApplyTheme()
        {
            var themeActions = new Dictionary<string, Action>
            {
                ["Dark"] = () =>
                {
                    DarkRadio.IsChecked = true;
                    ApplyThemeResource("Styles/Themes/DarkTheme.xaml");
                },
                ["Light"] = () =>
                {
                    LightRadio.IsChecked = true;
                    ApplyThemeResource("Styles/Themes/LightTheme.xaml");
                }
            };

            if (themeActions.TryGetValue(Settings.Theme, out var apply))
            {
                apply();
            }
            else
            {
                throw new ArgumentException("Wrong theme selection");
            }
        }

        private void ApplyThemeResource(string path)
        {
            var dict = new ResourceDictionary
            {
                Source = new Uri(path, UriKind.Relative)
            };

            var oldThemeDict = Application.Current.Resources.MergedDictionaries
                .FirstOrDefault(d => d.Source != null && d.Source.OriginalString.StartsWith("Styles/Themes/"));

            Application.Current.Resources.MergedDictionaries.Remove(oldThemeDict);
            Application.Current.Resources.MergedDictionaries.Add(dict);
        }
        private void WindowModeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void BackToMenu_Click(object sender, RoutedEventArgs e)
        {
            var menu = new MainMenuWindow();
            menu.Show();

            this.Close();
        }
    }
}
