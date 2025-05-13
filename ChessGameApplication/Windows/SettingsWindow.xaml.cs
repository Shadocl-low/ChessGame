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

            SetRadioThemeCheck();
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
            var newThemeDict = GetThemeResourceDictionary();
            
            var oldThemeDict = Application.Current.Resources.MergedDictionaries
                .FirstOrDefault(d => d.Source != null && d.Source.OriginalString.StartsWith("Styles/Themes/"));

            if (oldThemeDict != null)
                Application.Current.Resources.MergedDictionaries.Remove(oldThemeDict);

            Application.Current.Resources.MergedDictionaries.Add(newThemeDict);
        }
        private ResourceDictionary GetThemeResourceDictionary()
        {
            var resource = new ResourceDictionary();
            Uri uri;

            switch (Settings.Theme)
            {
                case "Dark":
                    {
                        uri = new Uri("Styles/Themes/DarkTheme.xaml", UriKind.Relative);
                        break;
                    }
                case "Light":
                    {
                        uri = new Uri("Styles/Themes/LightTheme.xaml", UriKind.Relative);
                        break;
                    }
                default:
                    {
                        throw new ArgumentException("Wrong theme selection");
                    }
            }

            resource.Source = uri;
            return resource;
        }
        private void SetRadioThemeCheck()
        {
            switch (Settings.Theme)
            {
                case "Dark":
                    {
                        DarkRadio.IsChecked = true;
                        break;
                    }
                case "Light":
                    {
                        LightRadio.IsChecked = true;
                        break;
                    }
                default:
                    {
                        throw new ArgumentException("Wrong theme selection");
                    }
            }
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
