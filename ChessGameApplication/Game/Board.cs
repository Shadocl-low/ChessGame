using ChessGameApplication.Game.Figures;
using ChessGameApplication.Game.PieceImageStrategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using ChessGameApplication.Game.BoardServices;

namespace ChessGameApplication.Game
{
    public class Board
    {
        private const int BoardSize = 8;
        private readonly Piece?[,] Squares = new Piece?[BoardSize, BoardSize];

        private readonly PiecePlacementService _piecePlacementService;
        private readonly MovementService _movementService;
        private readonly CastlingService _castlingService;
        private readonly AttackDetectionService _attackDetectionService;
        private readonly GameStateService _gameStateService;
        private readonly BoardValidationService _boardValidationService;

        public Board()
        {
            _piecePlacementService = new PiecePlacementService(this);
            _movementService = new MovementService(this);
            _castlingService = new CastlingService(this);
            _attackDetectionService = new AttackDetectionService(this);
            _gameStateService = new GameStateService(this, _attackDetectionService);
            _boardValidationService = new BoardValidationService(this);
        }

        public void Initialize()
        {
            _piecePlacementService.ClearBoard();
            _piecePlacementService.SetStartingPieces();
        }

        public Piece? GetPieceAt(Position position) => Squares[position.Row, position.Column];

        public void SetPieceAt(Position position, Piece? piece)
        {
            Squares[position.Row, position.Column] = piece;
            piece?.SetPosition(position);
        }

        public void PlacePiece(Piece? piece, Position position)
        {
            _piecePlacementService.PlacePiece(piece, position);
        }

        public void MovePiece(Position from, Position to)
        {
            var piece = GetPieceAt(from);
            if (piece == null) return;

            if (piece is King king && _castlingService.IsCastling(from, to))
            {
                _castlingService.HandleCastling(king, from, to);
                return;
            }

            _movementService.UpdatePiecePosition(piece, from, to);
        }

        public bool IsEmpty(Position pos) => _boardValidationService.IsEmpty(pos);

        public bool IsEnemyPiece(Position pos, PieceColor color) => _boardValidationService.IsEnemyPiece(pos, color);

        public bool IsInsideBoard(Position pos) => _boardValidationService.IsInsideBoard(pos);

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

        public bool IsSquareUnderAttack(Position position, PieceColor defenderColor) =>
            _attackDetectionService.IsSquareUnderAttack(position, defenderColor);

        public bool IsInCheck(PieceColor kingColor) =>
            _attackDetectionService.IsInCheck(kingColor);

        public bool WouldBeInCheckAfterMove(Position from, Position to, PieceColor movingColor)
        {
            var originalPiece = GetPieceAt(to);
            _movementService.MovePieceTemporarily(from, to);

            bool isInCheck = IsInCheck(movingColor);

            _movementService.MovePieceTemporarily(to, from);
            PlacePiece(originalPiece, to);

            return isInCheck;
        }

        public bool IsCheckmate(PieceColor kingColor) => _gameStateService.IsCheckmate(kingColor);

        public bool IsStalemate(PieceColor currentPlayer) => _gameStateService.IsStalemate(currentPlayer);
        
        public void ClearBoard()
        {
            _piecePlacementService.ClearBoard();
        }
    }
    
}

