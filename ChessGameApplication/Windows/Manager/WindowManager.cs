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
            gameWindow = new GameWindow(this, SettingsManager.GetImageStrategy());
            mainMenuWindow = new MainMenuWindow(this);
            settingsWindow = new SettingsWindow(this);
        }
        public void Notify(WindowActions action)
        {
            switch (action)
            {
                case WindowActions.OpenMainMenu:
                    SwitchWindow(mainMenuWindow);
                    break;
                case WindowActions.OpenGame:
                    SwitchWindow(gameWindow);
                    break;
                case WindowActions.OpenSettings:
                    SwitchWindow(settingsWindow);
                    break;
                case WindowActions.Exit:
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
