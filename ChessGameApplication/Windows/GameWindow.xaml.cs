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
using System.Diagnostics;
using System.Reflection;

namespace ChessGameApplication.Windows
{
    public partial class GameWindow : Window
    {
        private readonly SolidColorBrush lightCell = new SolidColorBrush(Color.FromRgb(240, 217, 181));
        private readonly SolidColorBrush darkCell = new SolidColorBrush(Color.FromRgb(181, 136, 99));
        private List<Position> allowedMoves = new();
        private Dictionary<Position, Border> positionToCellMap = new();

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
            for (int col = 0; col < 8; col++)
            {
                for (int row = 0; row < 8; row++)
                {
                    Border cell = new Border
                    {
                        Background = (row + col) % 2 == 0 ? lightCell : darkCell,
                        BorderThickness = new Thickness(0.5),
                        BorderBrush = Brushes.Black,
                        AllowDrop = true,
                        Tag = (row, col),
                    };
                    cell.Drop += OnCellDrop;

                    Piece? piece = Game.GetPiece(new Position(row, col));
                    if (piece != null)
                    {
                        var text = new TextBlock
                        {
                            Text = GetPieceSymbol(piece),
                            FontSize = 32,
                            Tag = (row, col),
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center
                        };
                        text.PreviewMouseDown += OnPieceMouseDown;
                        cell.Child = text;
                    }

                    ChessBoard.Children.Add(cell);
                    positionToCellMap[new Position(row, col)] = cell;
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
                Pawn p when p.Color == PieceColor.White => "♙",
                Rook r when r.Color == PieceColor.White => "♖",
                Knight k when k.Color == PieceColor.White => "♘",
                Bishop b when b.Color == PieceColor.White => "♗",
                Queen q when q.Color == PieceColor.White => "♕",
                King k when k.Color == PieceColor.White => "♔",
                Pawn p when p.Color == PieceColor.Black => "♟",
                Rook r when r.Color == PieceColor.Black => "♜",
                Knight k when k.Color == PieceColor.Black => "♞",
                Bishop b when b.Color == PieceColor.Black => "♝",
                Queen q when q.Color == PieceColor.Black => "♛",
                King k when k.Color == PieceColor.Black => "♚",
                _ => ""
            };
        }
        private void OnPieceMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBlock textBlock && textBlock.Tag is Position pos)
            {
                var piece = Game.Board.GetPieceAt(pos);
                if (piece == null || piece.Color != Game.CurrentTurn)
                    return;

                ClearHighlights();

                allowedMoves = (List<Position>)piece.GetAvailableMoves(Game.Board);

                HighlightCells(allowedMoves);

                DragDrop.DoDragDrop(textBlock, new DataObject("ChessPiece", pos), DragDropEffects.Move);
            }
        }
        private void OnCellDrop(object sender, DragEventArgs e)
        {
            ClearHighlights();
            if (e.Data.GetDataPresent("ChessPiece") && sender is Border border && border.Tag is (int toRow, int toCol))
            {
                var from = (Position)e.Data.GetData("ChessPiece");

                if (Game.TryMakeMove(from, new Position(toRow, toCol)))
                {
                    CreateChessBoard();
                }
            }
        }
        private void HighlightCells(List<Position> positions)
        {
            foreach (var pos in positions)
            {
                if (positionToCellMap.TryGetValue(pos, out var border))
                {
                    border.Background = new SolidColorBrush(Color.FromArgb(100, 50, 200, 50));
                }
            }
        }
        private void ClearHighlights()
        {
            foreach (var pos in allowedMoves)
            {
                if (positionToCellMap.TryGetValue(pos, out var border))
                {
                    border.Background = GetOriginalCellColor(pos);
                }
            }
            allowedMoves.Clear();
        }
        private Brush GetOriginalCellColor(Position pos)
        {
            return (pos.Row + pos.Column) % 2 == 0
                ? Brushes.Beige
                : Brushes.SaddleBrown;
        }
    }
}