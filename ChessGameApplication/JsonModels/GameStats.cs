using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGameApplication.JsonModels
{
    public class GameStats
    {
        public int WhiteWins { get; set; }
        public int BlackWins { get; set; }
        public int Draws { get; set; }

        public override string ToString()
        {
            return $"Білі: {WhiteWins} | Чорні: {BlackWins} | Нічиї: {Draws}";
        }
    }
}
