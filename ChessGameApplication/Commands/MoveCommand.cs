using ChessGameApplication.Game.Figures;
using ChessGameApplication.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGameApplication.Commands
{
    public class MoveCommand : IChessCommand
    {
        private readonly GameManager _game;
        private readonly Position _from;
        private readonly Position _to;
        private Piece? _capturedPiece;

        public MoveCommand(GameManager game, Position from, Position to)
        {
            _game = game;
            _from = from;
            _to = to;
        }

        public bool Execute()
        {
            var piece = _game.Board.GetPieceAt(_from);
            if (piece == null || !piece.GetAvailableMoves(_game.Board).Contains(_to))
                return false;

            _capturedPiece = _game.Board.GetPieceAt(_to);
            _game.Board.MovePiece(_from, _to);
            return true;
        }

        public void Undo()
        {
            _game.Board.MovePiece(_to, _from);
            if (_capturedPiece != null)
                _game.Board.PlacePiece(_capturedPiece, _to);
        }
    }
}
