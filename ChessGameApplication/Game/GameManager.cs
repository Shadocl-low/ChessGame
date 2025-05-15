using ChessGameApplication.Game.Figures;
using ChessGameApplication.Game.PieceImageStrategies;
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
                
            }
            else if (Board.IsStalemate(opponentColor))
            {
                
            }
            else if (Board.IsInCheck(opponentColor))
            {
                
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
    }
}
