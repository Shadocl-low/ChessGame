using System;
using System.Collections.Generic;
using System.Linq;
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

namespace ChessGameApplication
{
    public partial class SettingsWindow : Window
    {
        private bool isDarkTheme = true;

        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void ThemeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var radio = sender as RadioButton;
            if (radio == null || radio.Tag == null) return;

            string themeTag = radio.Tag.ToString()!;
            string themePath = themeTag == "Dark"
            ? "Styles/Themes/DarkTheme.xaml"
            : "Styles/Themes/LightTheme.xaml";

            var newThemeDict = new ResourceDictionary { Source = new Uri(themePath, UriKind.Relative) };

            // Видаляємо стару тему (якщо є)
            var oldThemeDict = Application.Current.Resources.MergedDictionaries
                .FirstOrDefault(d => d.Source != null && d.Source.OriginalString.StartsWith("Styles/Themes/"));

            if (oldThemeDict != null)
                Application.Current.Resources.MergedDictionaries.Remove(oldThemeDict);

            // Додаємо нову тему
            Application.Current.Resources.MergedDictionaries.Insert(0, newThemeDict);
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
