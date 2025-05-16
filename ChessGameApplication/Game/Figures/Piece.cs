using ChessGameApplication.Game.PieceImageStrategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;

namespace ChessGameApplication.Game.Figures
{
    public abstract class Piece
    {
        public PieceColor Color { get; }
        public Position Position { get; set; }
        public ImageSource? Image { get; protected set; }
        public bool HasMoved { get; set; }

        protected Piece(PieceColor color, Position position)
        {
            Color = color;
            Position = position;
            HasMoved = false;
        }
        public abstract IEnumerable<Position> GetAvailableMoves(Board board);
        public void UpdateImage(IPieceImageStrategy strategy)
        {
            Image = strategy.GetImageForPiece(this);
        }
    }
}
