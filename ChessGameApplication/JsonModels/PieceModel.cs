using ChessGameApplication.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGameApplication.JsonModels
{
    public class PieceModel
    {
        public string Type { get; set; } = "";
        public PieceColor Color { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public bool HasMoved { get; set; }
    }
}
