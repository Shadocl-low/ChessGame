using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGameApplication.Commands
{
    public interface IChessCommand
    {
        bool Execute();
        void Undo();
    }
}
