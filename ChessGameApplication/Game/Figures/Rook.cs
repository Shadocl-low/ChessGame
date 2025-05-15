using ChessGameApplication.Game.PieceImageStrategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGameApplication.Game.Figures
{
    public class Rook : Piece
    {
        public Rook(PieceColor color, Position position) : base(color, position) { }

        public override IEnumerable<Position> GetAvailableMoves(Board board)
        {
            return board.GetMovesInDirections(Position, Color, [
                (1, 0), (-1, 0), (0, 1), (0, -1)
            ]);
        }
    }
}
