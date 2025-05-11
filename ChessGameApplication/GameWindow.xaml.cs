using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChessGameApplication
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        private readonly SolidColorBrush lightCell = new SolidColorBrush(Color.FromRgb(240, 217, 181));
        private readonly SolidColorBrush darkCell = new SolidColorBrush(Color.FromRgb(181, 136, 99));

        public GameWindow()
        {
            InitializeComponent();
            CreateChessBoard();
        }

        private void CreateChessBoard()
        {
            ChessBoard.Children.Clear();
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Border cell = new Border
                    {
                        Background = (row + col) % 2 == 0 ? lightCell : darkCell,
                        BorderThickness = new Thickness(0.5),
                        BorderBrush = Brushes.Black
                    };

                    ChessBoard.Children.Add(cell);
                }
            }
        }

        private void SaveGame_Click(object sender, RoutedEventArgs e) { }
        private void BackToMenu_Click(object sender, RoutedEventArgs e) 
        {
            var menuWindow = new MainMenuWindow();
            menuWindow.Show();
            this.Hide();
            this.Close();
        }
    }
}