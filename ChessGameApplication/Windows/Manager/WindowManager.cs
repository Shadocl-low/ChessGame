using ChessGameApplication.JsonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;

namespace ChessGameApplication.Windows.Manager
{
    public class WindowManager : IWindowManager
    {
        private Window? CurrentWindow;

        private readonly GameWindow gameWindow;
        private readonly MainMenuWindow mainMenuWindow;
        private readonly SettingsWindow settingsWindow;
        private readonly StatsWindow statsWindow;

        public WindowManager()
        {
            gameWindow = new GameWindow(this, SettingsJsonOperator.GetImageStrategy());
            mainMenuWindow = new MainMenuWindow(this);
            statsWindow = new StatsWindow(this);
            settingsWindow = new SettingsWindow(this);
        }
        public async void Notify(WindowActions action)
        {
            switch (action)
            {
                case WindowActions.OpenMainMenu:
                    await SwitchWindowAsync(mainMenuWindow);
                    break;
                case WindowActions.OpenGame:
                    gameWindow.StartNewGame();
                    await SwitchWindowAsync(gameWindow);
                    break;
                case WindowActions.OpenSettings:
                    await SwitchWindowAsync(settingsWindow);
                    break;
                case WindowActions.Exit:
                    Application.Current.Shutdown();
                    break;
                case WindowActions.ContinueGame:
                    gameWindow.LoadGameState(GameJsonOperator.Instance.GameState!);
                    await SwitchWindowAsync(gameWindow);
                    break;
                case WindowActions.OpenStats:
                    statsWindow.LoadStats();
                    await SwitchWindowAsync(statsWindow);
                    break;
                default:
                    throw new ArgumentException($"Невідома дія: {action}");
            }
        }
        private async Task FadeOut(Window window)
        {
            var duration = TimeSpan.FromSeconds(0.3);
            var animation = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                Duration = duration
            };

            window.BeginAnimation(UIElement.OpacityProperty, animation);
            await Task.Delay(duration);
        }

        private async Task FadeIn(Window window)
        {
            var duration = TimeSpan.FromSeconds(0.3);
            var animation = new DoubleAnimation
            {
                From = 0.0,
                To = 1.0,
                Duration = duration
            };

            window.BeginAnimation(UIElement.OpacityProperty, animation);
            await Task.Delay(duration);
        }
        private async Task SwitchWindowAsync(Window newWindow)
        {
            if (CurrentWindow == newWindow)
                return;

            if (CurrentWindow is not null)
            {
                CurrentWindow.Effect = new BlurEffect { Radius = 5 };
                await FadeOut(CurrentWindow);
            }

            newWindow.Effect = new BlurEffect { Radius = 5 };
            newWindow.Opacity = 0;
            newWindow.Show();
            await FadeIn(newWindow);
            newWindow.Effect = null;

            if (CurrentWindow is not null)
            {
                CurrentWindow.Hide();
                CurrentWindow.Effect = null;
            }


            CurrentWindow = newWindow;
        }
    }
}
