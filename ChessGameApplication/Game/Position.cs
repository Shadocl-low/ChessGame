using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGameApplication.Game
{
    public struct Position
    {
        public int Row { get; }
        public int Column { get; }

        public Position(int row, int column)
        {
            Row = row;
            Column = column;
        }
        public Position Add(int rowOffset, int columnOffset)
        {
            return new Position(Row + rowOffset, Column + columnOffset);
        }
        public override string ToString() => $"({Row}, {Column})";
    }
}
