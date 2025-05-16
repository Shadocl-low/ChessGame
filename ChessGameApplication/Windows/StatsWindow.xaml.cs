using ChessGameApplication.Windows.Manager;
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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ChessGameApplication.Windows
{
    public partial class StatsWindow : Window
    {
        private readonly IWindowManager _manager;

        public StatsWindow(IWindowManager manager)
        {
            InitializeComponent();
            _manager = manager;
            LoadStats();
        }

        private void LoadStats()
        {
            var stats = StatsJsonOperator.Instance.Stats;
            WhiteWinsText.Text = stats.WhiteWins.ToString();
            BlackWinsText.Text = stats.BlackWins.ToString();
            DrawsText.Text = stats.Draws.ToString();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            StatsJsonOperator.Instance.ResetStats();
            LoadStats();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            _manager.Notify(WindowActions.OpenMainMenu);
        }
    }
}
