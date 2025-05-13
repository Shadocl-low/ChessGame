using ChessGameApplication.Windows.Manager;
using System.Configuration;
using System.Data;
using System.Windows;

namespace ChessGameApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var manager = new WindowManager();
            manager.Notify(WindowActions.OpenMainMenu);
        }
    }

}
