using ChessGameApplication.Game.Figures;
using ChessGameApplication.Game;
using ChessGameApplication.Windows.Manager;
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

namespace ChessGameApplication.Windows
{
    public partial class GameWindow : Window
    {
        private readonly SolidColorBrush lightCell = new SolidColorBrush(Color.FromRgb(240, 217, 181));
        private readonly SolidColorBrush darkCell = new SolidColorBrush(Color.FromRgb(181, 136, 99));

        private readonly IWindowManager Manager;
        private readonly GameManager Game;
        public GameWindow(IWindowManager manager)
        {
            Manager = manager;
            Game = new GameManager();

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

                    Piece? piece = Game.GetPiece(new Position(row, col));
                    if (piece != null)
                    {
                        var text = new TextBlock
                        {
                            Text = GetPieceSymbol(piece),
                            FontSize = 32,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center
                        };
                        cell.Child = text;
                    }

                    ChessBoard.Children.Add(cell);
                }
            }
        }

        private void SaveGame_Click(object sender, RoutedEventArgs e) { }
        private void BackToMenu_Click(object sender, RoutedEventArgs e) 
        {
            Manager.Notify(WindowActions.OpenMainMenu);
        }
        private string GetPieceSymbol(Piece piece)
        {
            return piece switch
            {
                Pawn   p when p.Color == PieceColor.White => "♙",
                Rook   r when r.Color == PieceColor.White => "♖",
                Knight k when k.Color == PieceColor.White => "♘",
                Bishop b when b.Color == PieceColor.White => "♗",
                Queen  q when q.Color == PieceColor.White => "♕",
                King   k when k.Color == PieceColor.White => "♔",
                Pawn   p when p.Color == PieceColor.Black => "♟",
                Rook   r when r.Color == PieceColor.Black => "♜",
                Knight k when k.Color == PieceColor.Black => "♞",
                Bishop b when b.Color == PieceColor.Black => "♝",
                Queen  q when q.Color == PieceColor.Black => "♛",
                King   k when k.Color == PieceColor.Black => "♚",
                _ => ""
            };
        }
    }
}