using ChessGameApplication.Game.Figures;
using ChessGameApplication.Game.PieceImageStrategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;

namespace ChessGameApplication.Game
{
    public class Board
    {
        private readonly Piece?[,] Squares = new Piece?[8, 8];
        private IPieceImageStrategy? ImageStrategy;
        public void Initialize()
        {
            var startingPieces = new (Type pieceType, int x, int y)[]
            {
                (typeof(Rook), 0, 0), (typeof(Knight), 1, 0), (typeof(Bishop), 2, 0),
                (typeof(Queen), 3, 0), (typeof(King), 4, 0),
                (typeof(Bishop), 5, 0), (typeof(Knight), 6, 0), (typeof(Rook), 7, 0)
            };

            foreach (var (type, x, y) in startingPieces)
                PlacePiece((Piece)Activator.CreateInstance(type, PieceColor.Black, new Position(x, y), ImageStrategy)!, new Position(x, y));

            foreach (var (type, x, y) in startingPieces)
                PlacePiece((Piece)Activator.CreateInstance(type, PieceColor.White, new Position(x, 7), ImageStrategy)!, new Position(x, 7));

            for (int x = 0; x < 8; x++)
            {
                PlacePiece(new Pawn(PieceColor.Black, new Position(x, 1), ImageStrategy), new Position(x, 1));
                PlacePiece(new Pawn(PieceColor.White, new Position(x, 6), ImageStrategy), new Position(x, 6));
            }
        }
        public void SetStrategy(IPieceImageStrategy imageStrategy)
        {
            ImageStrategy = imageStrategy;
        }
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
        public bool IsEmpty(Position pos)
        {
            var piece = GetPieceAt(pos);
            return piece == null;
        }
        public bool IsEnemyPiece(Position pos, PieceColor color)
        {
            var piece = GetPieceAt(pos);
            return piece != null && piece.Color != color;
        }
        public bool IsInsideBoard(Position pos)
            => pos.Row >= 0 && pos.Row < 8 && pos.Column >= 0 && pos.Column < 8;
        public List<Position> GetMovesInDirections(Position start, PieceColor color, (int dx, int dy)[] directions)
        {
            var moves = new List<Position>();

            foreach (var (dx, dy) in directions)
            {
                var pos = start.Add(dx, dy);
                while (IsInsideBoard(pos))
                {
                    if (IsEmpty(pos))
                    {
                        moves.Add(pos);
                    }
                    else
                    {
                        if (IsEnemyPiece(pos, color))
                            moves.Add(pos);
                        break;
                    }
                    pos = pos.Add(dx, dy);
                }
            }

            return moves;
        }
        public bool IsKingCaptured(PieceColor color)
        {
            return !Squares.Cast<Piece?>().Any(p => p is King k && k.Color == color);
        }
    }
}
