using ChessGameApplication.Game.PieceImageStrategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGameApplication.Game.Figures
{
    public class King : Piece
    {
        public King(PieceColor color, Position position) : base(color, position) { }
        public King(PieceColor color, Position position, bool hasMoved) : base(color, position, hasMoved) { }

        public override List<Position> GetAvailableMoves(Board board)
        {
            var moves = new List<Position>();

            var directions = new[] {
                (1, 0), (-1, 0), (0, 1), (0, -1),
                (1, 1), (1, -1), (-1, 1), (-1, -1)
            };
            
            foreach (var (dx, dy) in directions)
            {
                var pos = Position.Add(dx, dy);
                if (board.IsInsideBoard(pos) && (board.IsEnemyPiece(pos, Color) || board.IsEmpty(pos)))
                    moves.Add(pos);
            }

            if (!HasMoved)
            {
                AddCastlingMoves(board, moves);
            }

            return moves;
        }
        private void AddCastlingMoves(Board board, List<Position> moves)
        {
            if (CanCastleKingside(board))
            {
                moves.Add(new Position(Position.Row + 2, Position.Column));
            }

            if (CanCastleQueenside(board))
            {
                moves.Add(new Position(Position.Row - 2, Position.Column));
            }
        }

        private bool CanCastleKingside(Board board)
        {
            if (HasMoved) return false;

            var rookPos = new Position(7, Position.Column);
            var rook = board.GetPieceAt(rookPos) as Rook;

            return rook != null && !rook.HasMoved &&
                   board.IsEmpty(new Position(5, Position.Column)) &&
                   board.IsEmpty(new Position(6, Position.Column));
        }

        private bool CanCastleQueenside(Board board)
        {
            if (HasMoved) return false;

            var rookPos = new Position(0, Position.Column);
            var rook = board.GetPieceAt(rookPos) as Rook;

            return rook != null && !rook.HasMoved &&
                   board.IsEmpty(new Position(1, Position.Column)) &&
                   board.IsEmpty(new Position(2, Position.Column)) &&
                   board.IsEmpty(new Position(3, Position.Column));
        }
    }
}
