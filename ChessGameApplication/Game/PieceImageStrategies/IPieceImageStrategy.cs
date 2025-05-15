using ChessGameApplication.Game.Figures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ChessGameApplication.Game.PieceImageStrategies
{
    public interface IPieceImageStrategy
    {
        ImageSource GetImageForPiece(Piece piece);
    }
}
