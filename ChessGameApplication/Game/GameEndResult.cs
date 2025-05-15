using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGameApplication.Game
{
    public class GameEndResult
    {
        public PieceColor? Winner { get; }
        public EndReason Reason { get; }

        public GameEndResult(PieceColor? winner, EndReason reason)
        {
            Winner = winner;
            Reason = reason;
        }
    }
}
