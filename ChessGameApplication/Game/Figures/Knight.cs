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

        public override List<Position> GetAvailableMoves(Board board)
        {
            var moves = new List<Position>();

            int[][] offsets = 
            {
                [2, 1], [2, -1], [-2, 1], [-2, -1],
                [1, 2], [1, -2], [-1, 2], [-1, -2]
            };

            foreach (var offset in offsets)
            {
                var pos = Position.Add(offset[0], offset[1]);
                if (board.IsInsideBoard(pos) && (board.IsEnemyPiece(pos, Color) || board.IsEmpty(pos)))
                    moves.Add(pos);
            }

            return moves;
        }
    }
}
