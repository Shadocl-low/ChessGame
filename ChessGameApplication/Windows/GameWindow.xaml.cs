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
using System.Xml.Linq;
using ChessGameApplication.Game.PieceImageStrategies;

namespace ChessGameApplication.Windows
{
    public partial class GameWindow : Window
    {
        private readonly SolidColorBrush lightCell = new SolidColorBrush(Color.FromRgb(240, 217, 181));
        private readonly SolidColorBrush darkCell = new SolidColorBrush(Color.FromRgb(181, 136, 99));
        private Dictionary<Position, Border> positionToCellMap = new();
        private Position? _selectedPosition;

        private readonly IWindowManager Manager;
        private readonly GameManager Game;
        public GameWindow(IWindowManager manager, IPieceImageStrategy imageStrategy)
        {
            Manager = manager;
            Game = new GameManager(imageStrategy);

            InitializeComponent();
            CreateChessBoard();
        }
        private void CreateChessBoard()
        {
            ChessBoard.Children.Clear();
            positionToCellMap.Clear();

            for (int col = 0; col < 8; col++)
            {
                for (int row = 0; row < 8; row++)
                {
                    var pos = new Position(row, col);
                    Border cell = new Border
                    {
                        Background = (row + col) % 2 == 0 ? lightCell : darkCell,
                        BorderThickness = new Thickness(0.5),
                        BorderBrush = Brushes.Black,
                        AllowDrop = true,
                        Tag = pos,
                    };
                    cell.PreviewMouseLeftButtonDown += OnCellClick;

                    Piece? piece = Game.GetPiece(pos);
                    if (piece != null)
                    {
                        var image = new Image
                        {
                            Source = piece.Image,
                            Stretch = Stretch.Uniform,
                            Width = 40,
                            Height = 40,
                            Tag = pos
                        };
                        cell.Child = image;
                    }

                    ChessBoard.Children.Add(cell);
                    positionToCellMap[pos] = cell;
                }
            }
        }
        private void UpdateBoardAfterMove(Position from, Position to)
        {
            if (positionToCellMap.TryGetValue(from, out var fromCell))
            {
                fromCell.Child = null;
            }

            if (positionToCellMap.TryGetValue(to, out var toCell))
            {
                var piece = Game.GetPiece(to);
                var image = new Image
                {
                    Source = piece?.Image,
                    Stretch = Stretch.Uniform,
                    Width = 40,
                    Height = 40,
                    Tag = to
                };
                toCell.Child = piece != null
                    ? image
                    : null;
            }
        }

        private void OnCellClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border cell && cell.Tag is Position clickedPos)
            {
                var clickedPiece = Game.GetPiece(clickedPos);

                if (_selectedPosition.HasValue)
                {
                    if (clickedPiece != null && clickedPiece.Color == Game.CurrentTurn)
                    {
                        ClearHighlights();
                        _selectedPosition = clickedPos;
                        HighlightCells(clickedPiece);
                    }
                    
                    else if (Game.TryMakeMove(_selectedPosition.Value, clickedPos))
                    {
                        UpdateBoardAfterMove(_selectedPosition.Value, clickedPos);
                        ClearHighlights();
                        _selectedPosition = null;
                    }
                }
                else if (clickedPiece != null && clickedPiece.Color == Game.CurrentTurn)
                {
                    _selectedPosition = clickedPos;
                    HighlightCells(clickedPiece);
                }

                e.Handled = true;
            }
        }
        private void SaveGame_Click(object sender, RoutedEventArgs e) { }
        private void BackToMenu_Click(object sender, RoutedEventArgs e)
        {
            Manager.Notify(WindowActions.OpenMainMenu);
        }
        private void HighlightCells(Piece piece)
        {
            var positions = Game.GetPieceMoves(piece);

            if (positionToCellMap.TryGetValue(piece.Position, out var border))
            {
                border.BorderThickness = new Thickness(3);
            }

            foreach (var pos in positions)
            {
                if (positionToCellMap.TryGetValue(pos, out border))
                {
                    border.Background = new SolidColorBrush(Color.FromArgb(100, 50, 200, 50));
                }
            }
        }
        private void ClearHighlights()
        {
            foreach (var cell in positionToCellMap.Values)
            {
                cell.Background = GetOriginalCellColor((Position)cell.Tag);
                cell.BorderThickness = new Thickness(0.5);
            }
        }
        private Brush GetOriginalCellColor(Position pos)
        {
            return (pos.Row + pos.Column) % 2 == 0
                ? lightCell
                : darkCell;
        }
    }
}