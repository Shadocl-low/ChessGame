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
                King _ => "King",
                Queen _ => "Queen",
                Rook _ => "Rook",
                Bishop _ => "Bishop",
                Knight _ => "Knight",
                Pawn _ => "Pawn",
                _ => throw new ArgumentException("Unknown piece type")
            };

            if (piece.Color == PieceColor.Black)
                imageName += "_dark";
            else
                imageName += "_light";

            return new BitmapImage(new Uri($"pack://application:,,,/Images/Classic/{imageName}.png"));
        }
    }
}
