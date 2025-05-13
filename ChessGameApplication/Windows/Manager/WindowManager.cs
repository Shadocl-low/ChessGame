using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChessGameApplication.Windows.Manager
{
    public class WindowManager : IWindowManager
    {
        private Window? CurrentWindow;

        private readonly GameWindow gameWindow;
        private readonly MainMenuWindow mainMenuWindow;
        private readonly SettingsWindow settingsWindow;

        public WindowManager()
        {
            gameWindow = new GameWindow(this);
            mainMenuWindow = new MainMenuWindow(this);
            settingsWindow = new SettingsWindow(this);
        }
        public void Notify(Window sender, string action)
        {
            switch (action)
            {
                case "OpenMainMenu":
                    SwitchWindow(mainMenuWindow);
                    break;
                case "OpenGame":
                    SwitchWindow(gameWindow);
                    break;
                case "OpenSettings":
                    SwitchWindow(settingsWindow);
                    break;
                case "Exit":
                    Application.Current.Shutdown();
                    break;
                default:
                    throw new ArgumentException($"Невідома дія: {action}");
            }
        }

        private void SwitchWindow(Window newWindow)
        {
            if (CurrentWindow == newWindow)
                return;

            CurrentWindow?.Hide();
            CurrentWindow = newWindow;
            CurrentWindow.Show();
        }
    }
}
