using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChessGameApplication.Windows.Manager
{
    public interface IWindowManager
    {
        void Notify(WindowActions action);
    }
}
