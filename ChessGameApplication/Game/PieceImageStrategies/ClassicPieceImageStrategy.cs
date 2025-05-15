using ChessGameApplication.Game.Figures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace ChessGameApplication.Game.PieceImageStrategies
{
    public class ClassicPieceImageStrategy : IPieceImageStrategy
    {
        public ImageSource GetImageForPiece(Piece piece)
        {
            string imageName = piece switch
            {
                King _ => "king",
                Queen _ => "queen",
                Rook _ => "rook",
                Bishop _ => "bishop",
                Knight _ => "knight",
                Pawn _ => "pawn",
                _ => throw new ArgumentException("Unknown piece type")
            };

            if (piece.Color == PieceColor.Black)
                imageName += "_dark";
            else
                imageName += "_light";

            return new BitmapImage(new Uri($"pack://application:,,,/images/classic/{imageName}.png"));
        }
    }
}
