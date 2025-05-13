using ChessGameApplication.Game.Figures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGameApplication.Game
{
    public class Board
    {
        private readonly Piece?[,] Squares = new Piece?[8, 8];

        public void PlacePiece(Piece piece, Position position)
        {
            Squares[position.Row, position.Column] = piece;
            piece.Position = position;
        }

        public Piece? GetPieceAt(Position position)
        {
            return Squares[position.Row, position.Column];
        }

        public void MovePiece(Position from, Position to)
        {
            var piece = GetPieceAt(from);
            if (piece == null) return;

            Squares[to.Row, to.Column] = piece;
            Squares[from.Row, from.Column] = null;
            piece.Position = to;
        }

        public bool IsInsideBoard(Position pos)
            => pos.Row >= 0 && pos.Row < 8 && pos.Column >= 0 && pos.Column < 8;
    }
}
