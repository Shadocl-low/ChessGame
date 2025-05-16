using ChessGameApplication.Windows.Manager;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ChessGameApplication.Windows
{
    public partial class MainMenuWindow : Window, IChangeWindowMode
    {
        private readonly IWindowManager Manager;
        public MainMenuWindow(IWindowManager manager)
        {
            SettingsJsonOperator.Instance.WindowModeChanged += OnWindowModeChanged;

            Manager = manager;

            InitializeComponent();
        }
        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            Manager.Notify(WindowActions.OpenGame);
        }

        private void ContinueGame_Click(object sender, RoutedEventArgs e)
        {
            Manager.Notify(WindowActions.ContinueGame);
        }
        private void Stats_Click(object sender, RoutedEventArgs e)
        {
            Manager.Notify(WindowActions.OpenStats);
        }
        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            Manager.Notify(WindowActions.OpenSettings);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Manager.Notify(WindowActions.Exit);
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
                    ResizeMode = ResizeMode.CanResizeWithGrip;
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
    }
}
