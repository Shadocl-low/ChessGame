using ChessGameApplication.Game;
using ChessGameApplication.Game.Figures;
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

        public PromotionDialog()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

            SelectedPiece = (PromotionPiece)Convert.ToUInt32(button?.Tag);
            DialogResult = true;
        }
    }
}
