using ChessGameApplication.Game.PieceImageStrategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGameApplication.Game.Figures
{
    public class Queen : Piece
    {
        public Queen(PieceColor color, Position position) : base(color, position) { }
        public Queen(PieceColor color, Position position, bool hasMoved) : base(color, position, hasMoved) { }

        public override List<Position> GetAvailableMoves(Board board)
        {
            return board.GetMovesInDirections(Position, Color, [
                (1, 0), (-1, 0), (0, 1), (0, -1),
                (1, 1), (1, -1), (-1, 1), (-1, -1)
            ]);
        }
    }
}
