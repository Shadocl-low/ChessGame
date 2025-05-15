using ChessGameApplication.Game.PieceImageStrategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGameApplication.Game.Figures
{
    public class Bishop : Piece
    {
        public Bishop(PieceColor color, Position position, IPieceImageStrategy imageStrategy) : base(color, position, imageStrategy) { }

        public override List<Position> GetAvailableMoves(Board board)
        {
            return board.GetMovesInDirections(Position, Color, [
                (1, 1), (1, -1), (-1, 1), (-1, -1)
            ]);
        }
    }
}
