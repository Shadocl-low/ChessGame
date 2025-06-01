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
        private const int BoardSize = 8;
        private readonly Piece?[,] Squares = new Piece?[BoardSize, BoardSize];

        public void Initialize()
        {
            ClearBoard();
            SetStartingPieces();
        }

        private void SetStartingPieces()
        {
            var startingPieces = new (Type pieceType, int x, int y)[]
            {
                (typeof(Rook), 0, 0), (typeof(Knight), 1, 0), (typeof(Bishop), 2, 0),
                (typeof(Queen), 3, 0), (typeof(King), 4, 0),
                (typeof(Bishop), 5, 0), (typeof(Knight), 6, 0), (typeof(Rook), 7, 0)
            };

            foreach (var (type, x, y) in startingPieces)
                PlacePiece((Piece)Activator.CreateInstance(type, PieceColor.Black, new Position(x, y))!, new Position(x, y));

            foreach (var (type, x, y) in startingPieces)
                PlacePiece((Piece)Activator.CreateInstance(type, PieceColor.White, new Position(x, 7))!, new Position(x, 7));

            for (int x = 0; x < BoardSize; x++)
            {
                PlacePiece(new Pawn(PieceColor.Black, new Position(x, 1)), new Position(x, 1));
                PlacePiece(new Pawn(PieceColor.White, new Position(x, 6)), new Position(x, 6));
            }
        }

        public void ClearBoard() => Array.Clear(Squares, 0, Squares.Length);

        public void PlacePiece(Piece? piece, Position position)
        {
            Squares[position.Row, position.Column] = piece;
            piece?.SetPosition(position);
        }

        public Piece? GetPieceAt(Position position) => Squares[position.Row, position.Column];

        public void MovePiece(Position from, Position to)
        {
            var piece = GetPieceAt(from);
            if (piece == null) return;

            if (piece is King king && IsCastling(from, to))
            {
                HandleCastling(king, from, to);
                return;
            }

            UpdatePiecePosition(piece, from, to);
        }

        private bool IsCastling(Position from, Position to) =>
            Math.Abs(from.Row - to.Row) == 2 && from.Column == to.Column;

        private void HandleCastling(King king, Position from, Position to)
        {
            var rookFrom = to.Row == 0 ? new Position(0, from.Column) : new Position(7, from.Column);
            var rookTo = to.Row == 0 ? new Position(3, from.Column) : new Position(5, from.Column);

            ExecuteCastling(king, from, to, rookFrom, rookTo);
        }

        private void ExecuteCastling(King king, Position from, Position to, Position rookFrom, Position rookTo)
        {
            // Move the king
            Squares[to.Row, to.Column] = king;
            Squares[from.Row, from.Column] = null;
            king.SetPosition(to);
            king.HasMoved = true;

            // Move the rook
            var rook = GetPieceAt(rookFrom);
            if (rook != null)
            {
                Squares[rookTo.Row, rookTo.Column] = rook;
                Squares[rookFrom.Row, rookFrom.Column] = null;
                rook.SetPosition(rookTo);
                rook.HasMoved = true;
            }
        }

        private void UpdatePiecePosition(Piece piece, Position from, Position to)
        {
            Squares[to.Row, to.Column] = piece;
            Squares[from.Row, from.Column] = null;
            piece.SetPosition(to);
            piece.HasMoved = true;
        }

        public bool IsEmpty(Position pos) => GetPieceAt(pos) == null;

        public bool IsEnemyPiece(Position pos, PieceColor color) =>
            GetPieceAt(pos) is Piece piece && piece.Color != color;

        public bool IsInsideBoard(Position pos) =>
            pos.Row >= 0 && pos.Row < BoardSize && pos.Column >= 0 && pos.Column < BoardSize;

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

        public bool IsKingCaptured(PieceColor color) =>
            !Squares.Cast<Piece?>().Any(p => p is King k && k.Color == color);

        public IEnumerable<Piece> GetAllPieces() =>
            Squares.Cast<Piece?>().Where(piece => piece != null).Select(piece => piece!);

        public Position FindKingPosition(PieceColor color)
        {
            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    if (GetPieceAt(new Position(row, col)) is King king && king.Color == color)
                    {
                        return new Position(row, col);
                    }
                }
            }

            throw new Exception("Король не знайдений");
        }

        public bool IsSquareUnderAttack(Position position, PieceColor defenderColor)
        {
            foreach (var piece in GetAllPieces())
            {
                if (piece.Color != defenderColor && piece.GetAvailableMoves(this).Contains(position))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsInCheck(PieceColor kingColor) =>
            IsSquareUnderAttack(FindKingPosition(kingColor), kingColor);

        public bool WouldBeInCheckAfterMove(Position from, Position to, PieceColor movingColor)
        {
            var originalPiece = GetPieceAt(to);
            MovePieceBeInCheck(from, to);

            bool isInCheck = IsInCheck(movingColor);

            MovePieceBeInCheck(to, from);
            PlacePiece(originalPiece, to);

            return isInCheck;
        }

        public void MovePieceBeInCheck(Position from, Position to)
        {
            var piece = GetPieceAt(from);
            if (piece == null) return;

            Squares[to.Row, to.Column] = piece;
            Squares[from.Row, from.Column] = null;
            piece.SetPosition(to);
        }

        private Dictionary<Position, List<Position>> GetAllPossibleMoves(PieceColor color)
        {
            var allMoves = new Dictionary<Position, List<Position>>();

            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    var piece = Squares[row, col];
                    if (piece?.Color == color)
                    {
                        var position = new Position(row, col);
                        var validMoves = piece.GetAvailableMoves(this)
                                              .Where(move => !WouldBeInCheckAfterMove(position, move, color))
                                              .ToList();

                        if (validMoves.Any())
                        {
                            allMoves[position] = validMoves;
                        }
                    }
                }
            }

            return allMoves;
        }

        public bool IsCheckmate(PieceColor kingColor) =>
            IsInCheck(kingColor) && !GetAllPossibleMoves(kingColor).Any();

        public bool IsStalemate(PieceColor currentPlayer) =>
            !IsInCheck(currentPlayer) && !GetAllPossibleMoves(currentPlayer).Any();
    }
}
