﻿using ChessGameApplication.Game.Figures;
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
        public void Initialize()
        {
            ClearBoard();

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

            for (int x = 0; x < 8; x++)
            {
                PlacePiece(new Pawn(PieceColor.Black, new Position(x, 1)), new Position(x, 1));
                PlacePiece(new Pawn(PieceColor.White, new Position(x, 6)), new Position(x, 6));
            }
        }
        public void ClearBoard()
        {
            Array.Clear(Squares, 0, Squares.Length);
        }
        public void PlacePiece(Piece? piece, Position position)
        {
            Squares[position.Row, position.Column] = piece;
            if (piece != null) piece.Position = position;
        }

        public Piece? GetPieceAt(Position position)
        {
            return Squares[position.Row, position.Column];
        }
        private void HandleCastling(King king, Position from, Position to)
        {
            if (to.Row > from.Row)
            {
                Squares[to.Row, to.Column] = king;
                Squares[from.Row, from.Column] = null;
                king.Position = to;

                var rookFrom = new Position(7, from.Column);
                var rookTo = new Position(5, from.Column);
                var rook = GetPieceAt(rookFrom);
                Squares[rookTo.Row, rookTo.Column] = rook;
                Squares[rookFrom.Row, rookFrom.Column] = null;
                rook.Position = rookTo;
                rook.HasMoved = true;
            }
            else
            {
                Squares[to.Row, to.Column] = king;
                Squares[from.Row, from.Column] = null;
                king.Position = to;

                var rookFrom = new Position(0, from.Column);
                var rookTo = new Position(3, from.Column);
                var rook = GetPieceAt(rookFrom);
                Squares[rookTo.Row, rookTo.Column] = rook;
                Squares[rookFrom.Row, rookFrom.Column] = null;
                rook.Position = rookTo;
                rook.HasMoved = true;
            }

            king.HasMoved = true;
        }
        public void MovePiece(Position from, Position to)
        {
            var piece = GetPieceAt(from);
            if (piece == null) return;

            if (piece is King king && !piece.HasMoved && Math.Abs(from.Row - to.Row) == 2)
            {
                HandleCastling(king, from, to);
                return;
            }

            Squares[to.Row, to.Column] = piece;
            Squares[from.Row, from.Column] = null;
            piece.Position = to;
            piece.HasMoved = true;
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
        public IEnumerable<Piece> GetAllPieces()
        {
            return Squares.Cast<Piece?>()
                         .Where(piece => piece != null)
                         .Select(piece => piece!);
        }
        public Position FindKingPosition(PieceColor color)
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    var piece = Squares[row, col];
                    if (piece is King king && king.Color == color)
                    {
                        return new Position(row, col);
                    }
                }
            }
            throw new Exception("Король не знайдений");
        }
        public bool IsSquareUnderAttack(Position position, PieceColor defenderColor)
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    var piece = Squares[row, col];
                    if (piece != null && piece.Color != defenderColor)
                    {
                        var moves = piece.GetAvailableMoves(this);
                        if (moves.Contains(position))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public bool IsInCheck(PieceColor kingColor)
        {
            Position kingPosition = FindKingPosition(kingColor);
            return IsSquareUnderAttack(kingPosition, kingColor);
        }
        public void MovePieceBeInCheck(Position from, Position to)
        {
            var piece = GetPieceAt(from);
            if (piece == null) return;

            Squares[to.Row, to.Column] = piece;
            Squares[from.Row, from.Column] = null;
            piece.Position = to;
        }
        public bool WouldBeInCheckAfterMove(Position from, Position to, PieceColor movingColor)
        {
            var originalPiece = GetPieceAt(to);
            MovePieceBeInCheck(from, to);

            bool isInCheck = IsInCheck(movingColor);

            MovePieceBeInCheck(to, from);
            PlacePiece(originalPiece, to);

            return isInCheck;
        }
        private Dictionary<Position, List<Position>> GetAllPossibleMoves(PieceColor color)
        {
            var allMoves = new Dictionary<Position, List<Position>>();

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    var piece = Squares[row, col];
                    if (piece != null && piece.Color == color)
                    {
                        var position = new Position(row, col);
                        var validMoves = new List<Position>();

                        foreach (var move in piece.GetAvailableMoves(this))
                        {
                            if (!WouldBeInCheckAfterMove(position, move, color))
                            {
                                validMoves.Add(move);
                            }
                        }

                        if (validMoves.Any())
                        {
                            allMoves[position] = validMoves;
                        }
                    }
                }
            }

            return allMoves;
        }
        public bool IsCheckmate(PieceColor kingColor)
        {
            if (!IsInCheck(kingColor))
                return false;

            var allPossibleMoves = GetAllPossibleMoves(kingColor);

            return !allPossibleMoves.Any();
        }

        public bool IsStalemate(PieceColor currentPlayer)
        {
            if (IsInCheck(currentPlayer))
                return false;

            var allPossibleMoves = GetAllPossibleMoves(currentPlayer);
            return !allPossibleMoves.Any();
        }
    }
}
