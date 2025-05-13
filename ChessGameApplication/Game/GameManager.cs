using ChessGameApplication.Game.Figures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGameApplication.Game
{
    public class GameManager
    {
        public Board Board { get; private set; }
        public PieceColor CurrentTurn { get; private set; } = PieceColor.White;
        public bool IsGameOver { get; private set; }

        public GameManager()
        {
            Board = new Board();
            StartNewGame();
        }

        public void StartNewGame()
        {
            Board.Initialize(); 
            CurrentTurn = PieceColor.White;
            IsGameOver = false;
        }

        public bool TryMakeMove(Position from, Position to)
        {
            if (IsGameOver) return false;

            var piece = Board.GetPieceAt(from);
            if (piece == null || piece.Color != CurrentTurn)
                return false;

            var legalMoves = piece.GetAvailableMoves(Board);
            if (!legalMoves.Contains(to))
                return false;

            Board.MovePiece(from, to);

            if (Board.IsKingCaptured(PieceColor.Black) || Board.IsKingCaptured(PieceColor.White))
            {
                IsGameOver = true;
            }

            ChangeTurn();

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
    }
}
