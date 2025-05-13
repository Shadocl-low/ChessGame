using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace ChessGameApplication.Game.Figures
{
    public abstract class Piece
    {
        public PieceColor Color { get; }
        public Position Position { get; set; }

        protected Piece(PieceColor color, Position position)
        {
            Color = color;
            Position = position;
        }
    }
}
