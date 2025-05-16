using ChessGameApplication.Game;
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
    public partial class PromotionDialog : Window
    {
        public PromotionPiece SelectedPiece { get; private set; }
        private readonly PieceColor _color;

        public PromotionDialog(PieceColor color)
        {
            _color = color;
            InitializeComponent();
        }

        private void Queen_Click(object sender, RoutedEventArgs e)
        {
            SelectedPiece = PromotionPiece.Queen;
            DialogResult = true;
        }

        private void Rook_Click(object sender, RoutedEventArgs e)
        {
            SelectedPiece = PromotionPiece.Rook;
            DialogResult = true;
        }

        private void Bishop_Click(object sender, RoutedEventArgs e)
        {
            SelectedPiece = PromotionPiece.Bishop;
            DialogResult = true;
        }

        private void Knight_Click(object sender, RoutedEventArgs e)
        {
            SelectedPiece = PromotionPiece.Knight;
            DialogResult = true;
        }
    }
}
