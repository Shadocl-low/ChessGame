using ChessGameApplication.Game.PieceImageStrategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGameApplication.Game.Figures
{
    public class Pawn : Piece
    {
        public Pawn(PieceColor color, Position position) : base(color, position) { }
        public Pawn(PieceColor color, Position position, bool hasMoved) : base(color, position, hasMoved) { }

        public override List<Position> GetAvailableMoves(Board board)
        {
            var moves = new List<Position>();

            int direction = Color == PieceColor.White ? -1 : 1;
            int startColumn = Color == PieceColor.White ? 6 : 1;

            var forward = Position.Add(0, direction);
            if (board.IsInsideBoard(forward) && board.IsEmpty(forward))
                moves.Add(forward);

            var doubleForward = Position.Add(0, direction * 2);
            if (Position.Column == startColumn && board.IsEmpty(forward) && board.IsEmpty(doubleForward))
                moves.Add(doubleForward);

            foreach (int dx in new[] { -1, 1 })
            {
                var diag = Position.Add(dx, direction);
                if (board.IsInsideBoard(diag) && board.IsEnemyPiece(diag, Color))
                    moves.Add(diag);
            }

            return moves;
        }
        public bool CanPromote()
        {
            return (Color == PieceColor.White && Position.Column == 0) ||
                   (Color == PieceColor.Black && Position.Column == 7);
        }
    }
}
