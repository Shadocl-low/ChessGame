using ChessGameApplication.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGameApplication.JsonModels
{
    public class GameState
    {
        public PieceColor CurrentTurn { get; set; }
        public List<PieceModel> Pieces { get; set; } = new();
        public bool IsCheck { get; set; }
    }
}
