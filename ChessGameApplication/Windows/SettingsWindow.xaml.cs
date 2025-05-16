using ChessGameApplication.Game.PieceImageStrategies;
using ChessGameApplication.JsonModels;
using ChessGameApplication.Windows.Manager;
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
    public partial class SettingsWindow : Window, IChangeWindowMode
    {
        private AppSettings Settings;
        private readonly IWindowManager Manager;
        private bool _isInitializing = true;
        public SettingsWindow(IWindowManager manager)
        {
            SettingsJsonOperator.Instance.WindowModeChanged += OnWindowModeChanged;

            Manager = manager;
            Settings = SettingsJsonOperator.Instance.Settings!;

            InitializeComponent();

            ApplySkinSettings();
            ApplyWindowModeSettings();
            ApplyTheme();

            _isInitializing = false;
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
        private void ThemeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (_isInitializing) return;
            if (sender is not RadioButton radio || radio.Tag == null) return;

            string themeTag = radio.Tag.ToString()!;
            SettingsJsonOperator.SetTheme(themeTag);
            ApplyTheme();
        }

        private void ApplyWindowModeSettings()
        {
            foreach (ComboBoxItem item in WindowModeComboBox.Items)
            {
                if (item.Tag.ToString() == Settings.WindowMode)
                {
                    WindowModeComboBox.SelectedItem = item;
                    break;
                }
            }
        }
        private void WindowModeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (WindowModeComboBox.SelectedItem is ComboBoxItem selectedItem && selectedItem.Tag != null)
            {
                string modeTag = selectedItem.Tag.ToString()!;
                SettingsJsonOperator.SetWindowMode(modeTag);
            }
        }

        private void ApplySkinSettings()
        {
            foreach (ComboBoxItem item in PieceSkinComboBox.Items)
            {
                if (item.Tag.ToString() == Settings.PieceSkin)
                {
                    PieceSkinComboBox.SelectedItem = item;
                    break;
                }
            }
        }
        private void PieceSkinComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PieceSkinComboBox.SelectedItem is ComboBoxItem selectedItem && selectedItem.Tag != null)
            {
                string skinTag = selectedItem.Tag.ToString()!;
                SettingsJsonOperator.SetPieceSkin(skinTag);
            }
        }
        private void OnWindowModeChanged(string mode)
        {
            ChangeWindowMode(mode);
        }
        public void ChangeWindowMode(string mode)
        {
            switch (mode)
            {
                case "Windowed":
                    WindowState = WindowState.Normal;
                    WindowStyle = WindowStyle.SingleBorderWindow;
                    ResizeMode = ResizeMode.NoResize;
                    Topmost = false;
                    Width = 1280;
                    Height = 800;
                    break;
                case "Fullscreen":
                    WindowState = WindowState.Maximized;
                    WindowStyle = WindowStyle.None;
                    ResizeMode = ResizeMode.NoResize;
                    Topmost = true;
                    break;
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            SettingsJsonOperator.Instance.WindowModeChanged -= OnWindowModeChanged;
            base.OnClosed(e);
        }
        private void BackToMenu_Click(object sender, RoutedEventArgs e)
        {
            Manager.Notify(WindowActions.OpenMainMenu);
        }
    }
}
