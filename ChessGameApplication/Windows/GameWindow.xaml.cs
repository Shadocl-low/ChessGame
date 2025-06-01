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
using System.Windows.Media.Animation;
using ChessGameApplication.JsonModels;

namespace ChessGameApplication.Windows
{
    public partial class GameWindow : Window, IChangeWindowMode
    {
        private readonly SolidColorBrush lightCell = new SolidColorBrush(Color.FromRgb(240, 217, 181));
        private readonly SolidColorBrush darkCell = new SolidColorBrush(Color.FromRgb(181, 136, 99));
        private Dictionary<Position, Border> positionToCellMap = new();
        private Position? _selectedPosition;
        private bool _ckeckedState = false;

        private readonly IWindowManager Manager;
        private readonly GameManager Game;
        public GameWindow(IWindowManager manager, IPieceImageStrategy imageStrategy)
        {
            SettingsJsonOperator.Instance.PieceSkinChanged += OnPieceSkinChanged;
            SettingsJsonOperator.Instance.WindowModeChanged += OnWindowModeChanged;

            Manager = manager;
            Game = new GameManager(imageStrategy);
            Game.GameEnded += OnGameEnded;
            Game.Check += HighlightCheck;

            InitializeComponent();
            RenderChessBoard();
        }
        public void StartNewGame()
        {
            Game.StartNewGame();
            UpdateTurnIndicators();
            RenderChessBoard();
        }
        private void RenderChessBoard()
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
                        RenderChessBoard();
                        ClearHighlights();
                        OnTurnChanged();
                        _selectedPosition = null;
                        _ckeckedState = false;
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
        private void SaveGame_Click(object sender, RoutedEventArgs e) 
        {
            GameJsonOperator.Instance.Save(Game.CurrentTurn, _ckeckedState, Game.GetAllPieces());
        }
        private void BackToMenu_Click(object sender, RoutedEventArgs e)
        {
            Manager.Notify(WindowActions.OpenMainMenu);
        }
        private void HighlightCells(Piece piece)
        {
            var positions = Game.GetAllowedMoves(piece.Position);
            var cellBgFree = new SolidColorBrush(Color.FromArgb(100, 50, 200, 50));
            var cellBgEnemy = new SolidColorBrush(Color.FromArgb(100, 200, 100, 50));

            if (positionToCellMap.TryGetValue(piece.Position, out var border))
            {
                border.BorderThickness = new Thickness(3);
            }

            foreach (var pos in positions)
            {
                if (positionToCellMap.TryGetValue(pos, out border))
                {
                    border.Background = Game.IsEnemyPiece(pos) ? cellBgEnemy : cellBgFree;
                }
            }
        }
        private void HighlightCheck()
        {
            _ckeckedState = true;

            var opponentColor = Game.CurrentTurn == PieceColor.Black ? PieceColor.Black : PieceColor.White;

            var kingPosition = Game.Board.FindKingPosition(opponentColor);
            if (positionToCellMap.TryGetValue(kingPosition, out var cell))
            {
                cell.Background = new SolidColorBrush(Color.FromArgb(255, 255, 50, 50));

                cell.BorderBrush = Brushes.Red;
                cell.BorderThickness = new Thickness(6);
            }
        }
        private void ClearHighlights()
        {
            foreach (var cell in positionToCellMap.Values)
            {
                cell.Background = GetOriginalCellColor((Position)cell.Tag);
                cell.BorderThickness = new Thickness(0.5);
            }

            if (_ckeckedState) HighlightCheck();
        }
        private Brush GetOriginalCellColor(Position pos)
        {
            return (pos.Row + pos.Column) % 2 == 0
                ? lightCell
                : darkCell;
        }
        private void OnPieceSkinChanged(IPieceImageStrategy newStrategy)
        {
            Game.UpdateImageStrategy(newStrategy);

            RenderChessBoard();
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
                    Topmost = false;
                    break;
            }
        }
        private void OnTurnChanged()
        {
            UpdateTurnIndicators();
        }
        private void UpdateTurnIndicators()
        {
            if (Game.CurrentTurn == PieceColor.Black)
            {
                FadeIn(TopTurnIndicator);
                FadeOut(BottomTurnIndicator);
                TurnTextBlock.Text = "Хід: Чорний";
            }
            else
            {
                FadeOut(TopTurnIndicator);
                FadeIn(BottomTurnIndicator);
                TurnTextBlock.Text = "Хід: Білий";
            }
        }
        protected override void OnClosed(EventArgs e)
        {
            SettingsJsonOperator.Instance.PieceSkinChanged -= OnPieceSkinChanged;
            SettingsJsonOperator.Instance.WindowModeChanged -= OnWindowModeChanged;
            base.OnClosed(e);
        }
        private void AnimateOpacity(UIElement element, double from, double to, TimeSpan duration)
        {
            var animation = new DoubleAnimation
            {
                From = from,
                To = to,
                Duration = duration,
                FillBehavior = FillBehavior.HoldEnd
            };

            element.BeginAnimation(UIElement.OpacityProperty, animation);
        }
        public void FadeIn(UIElement element)
        {
            element.Visibility = Visibility.Visible;
            AnimateOpacity(element, 0, 1, TimeSpan.FromSeconds(0.3));
        }
        public async void FadeOut(UIElement element)
        {
            AnimateOpacity(element, 1, 0, TimeSpan.FromSeconds(0.3));

            await Task.Delay(300);
            element.Visibility = Visibility.Collapsed;
        }
        private string GetColorName(PieceColor color) =>
            color == PieceColor.White ? "білий" : "чорний";
        private void OnGameEnded(GameEndResult result)
        {
            Dispatcher.Invoke(() =>
            {
                string message = result.Reason switch
                {
                    EndReason.Checkmate => $"Мат! Переміг {GetColorName(result.Winner!.Value)}.",
                    EndReason.Stalemate => "Пат - нічия!",
                    _ => "Гра завершена."
                };

                StatsJsonOperator.Instance.RecordGameResult(result);

                MessageBox.Show(message, "Гра завершена", MessageBoxButton.OK);

                Manager.Notify(WindowActions.OpenMainMenu);
            });
        }
        public void LoadGameState(GameState gameState)
        {
            Game.LoadGameState(gameState);
            _ckeckedState = gameState.IsCheck;

            RenderChessBoard();
            UpdateTurnIndicators();

            if (_ckeckedState)
            {
                HighlightCheck();
            }
        }
    }
}