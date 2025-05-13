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
            var moves = new List<Position>();

            int[] directions = { -1, 1 };
            foreach (int d in directions)
            {
                for (int i = Position.Row + d; i >= 0 && i < 8; i += d)
                {
                    var pos = new Position(i, Position.Column);
                    var piece = board.GetPieceAt(pos);
                    if (piece == null)
                        moves.Add(pos);
                    else
                    {
                        if (piece.Color != this.Color)
                            moves.Add(pos);
                        break;
                    }
                }

                for (int j = Position.Column + d; j >= 0 && j < 8; j += d)
                {
                    var pos = new Position(Position.Row, j);
                    var piece = board.GetPieceAt(pos);
                    if (piece == null)
                        moves.Add(pos);
                    else
                    {
                        if (piece.Color != this.Color)
                            moves.Add(pos);
                        break;
                    }
                }
            }

            return moves;
        }
    }
}
