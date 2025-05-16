using ChessGameApplication.Game.PieceImageStrategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGameApplication.Game.Figures
{
    public class Knight : Piece
    {
        public Knight(PieceColor color, Position position) : base(color, position) { }
        public Knight(PieceColor color, Position position, bool hasMoved) : base(color, position, hasMoved) { }

        public override List<Position> GetAvailableMoves(Board board)
        {
            var moves = new List<Position>();

            var directions = new[] {
                (2, 1), (2, -1), (-2, 1), (-2, -1),
                (1, 2), (1, -2), (-1, 2), (-1, -2)
            };

            foreach (var (dx, dy) in directions)
            {
                var pos = Position.Add(dx, dy);
                if (board.IsInsideBoard(pos) && (board.IsEnemyPiece(pos, Color) || board.IsEmpty(pos)))
                    moves.Add(pos);
            }

            return moves;
        }
    }
}
