﻿using ChessGameApplication.Game.Figures;
using ChessGameApplication.Game.PieceImageStrategies;
using ChessGameApplication.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGameApplication.Game
{
    public class GameManager
    {
        public event Action<GameEndResult>? GameEnded;
        public event Action? Check;

        public Board Board { get; private set; }
        public PieceColor CurrentTurn { get; private set; } = PieceColor.White;
        private IPieceImageStrategy? _currentStrategy;
        public bool IsGameOver { get; private set; }

        public GameManager(IPieceImageStrategy imageStrategy)
        {
            _currentStrategy = imageStrategy;

            Board = new Board();

            StartNewGame();
        }
        public void StartNewGame()
        {
            Board.Initialize(); 
            CurrentTurn = PieceColor.White;
            IsGameOver = false;

            UpdateImageStrategy(_currentStrategy!);
        }
        private bool IsMoveLegalConsideringCheck(Position from, Position to)
        {
            var piece = Board.GetPieceAt(from);
            if (!piece.GetAvailableMoves(Board).Contains(to))
                return false;

            return !Board.WouldBeInCheckAfterMove(from, to, CurrentTurn);
        }
        private void CheckGameEndConditions()
        {
            var opponentColor = CurrentTurn == PieceColor.White ? PieceColor.Black : PieceColor.White;

            if (Board.IsCheckmate(opponentColor))
            {
                IsGameOver = true;
                GameEnded?.Invoke(new GameEndResult(CurrentTurn, EndReason.Checkmate));
            }
            else if (Board.IsStalemate(opponentColor))
            {
                IsGameOver = true;
                GameEnded?.Invoke(new GameEndResult(null, EndReason.Stalemate));
            }
            else if (Board.IsInCheck(opponentColor))
            {
                Check?.Invoke();
            }
        }
        private void PromotePawn(Position pawnPosition)
        {
            var pawn = Board.GetPieceAt(pawnPosition) as Pawn;
            if (pawn == null) return;

            var promotionDialog = new PromotionDialog();
            if (promotionDialog.ShowDialog() == true)
            {
                Piece newPiece;
                switch (promotionDialog.SelectedPiece)
                {
                    case PromotionPiece.Queen:
                        newPiece = new Queen(pawn.Color, pawnPosition);
                        break;
                    case PromotionPiece.Rook:
                        newPiece = new Rook(pawn.Color, pawnPosition);
                        break;
                    case PromotionPiece.Bishop:
                        newPiece = new Bishop(pawn.Color, pawnPosition);
                        break;
                    case PromotionPiece.Knight:
                        newPiece = new Knight(pawn.Color, pawnPosition);
                        break;
                    default:
                        newPiece = new Queen(pawn.Color, pawnPosition);
                        break;
                }

                newPiece.UpdateImage(_currentStrategy!);
                Board.PlacePiece(newPiece, pawnPosition);
            }
        }
        public bool TryMakeMove(Position from, Position to)
        {
            if (IsGameOver) return false;

            var piece = Board.GetPieceAt(from);
            if (piece == null || piece.Color != CurrentTurn)
                return false;

            if (!IsMoveLegalConsideringCheck(from, to))
                return false;

            Board.MovePiece(from, to);

            if (piece is Pawn pawn && pawn.CanPromote())
            {
                PromotePawn(pawn.Position);
            }

            CheckGameEndConditions();

            if (!IsGameOver)
            {
                ChangeTurn();
            }

            return true;
        }
        private void ChangeTurn()
        {
            if (CurrentTurn == PieceColor.White)
            {
                CurrentTurn = PieceColor.Black;
            }
            else
            {
                CurrentTurn = PieceColor.White;
            }
        }
        public Piece? GetPiece(Position position)
        {
            return Board.GetPieceAt(position);
        }
        public List<Position> GetPieceMoves(Piece piece)
        {
            return (List<Position>)piece.GetAvailableMoves(Board);
        }
        public bool IsEnemyPiece(Position pos)
        {
            return Board.IsEnemyPiece(pos, CurrentTurn);
        }
        public void UpdateImageStrategy(IPieceImageStrategy newStrategy)
        {
            _currentStrategy = newStrategy;

            foreach (var piece in Board.GetAllPieces())
            {
                piece.UpdateImage(_currentStrategy);
            }
        }
        public Position FindKingPosition(PieceColor color)
        {
            return Board.FindKingPosition(color);
        }
        public List<Position> GetAllowedMoves(Position from)
        {
            var piece = Board.GetPieceAt(from);
            if (piece == null) return new List<Position>();

            var allMoves = piece.GetAvailableMoves(Board).ToList();
            var allowedMoves = new List<Position>();

            foreach (var move in allMoves)
            {
                if (!Board.WouldBeInCheckAfterMove(from, move, piece.Color))
                {
                    allowedMoves.Add(move);
                }
            }

            return allowedMoves;
        }
    }
}
