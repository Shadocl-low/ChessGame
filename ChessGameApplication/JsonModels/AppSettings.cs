using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGameApplication.JsonModels
{
    public class AppSettings
    {
        public string Theme { get; set; } = "Light";
        public string WindowMode { get; set; } = "Windowed";
        public string PieceSkin { get; set; } = "Classic";
    }
}
