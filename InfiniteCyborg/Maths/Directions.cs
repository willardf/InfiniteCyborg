using InfCy.GameCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfCy.Maths
{
    public static class Directions
    {
        public static readonly Dictionary<Buttons, IntVector> Cardinal8 = new Dictionary<Buttons, IntVector>(){
            { Buttons.Up, new IntVector(0, -1) },
            { Buttons.UpLeft, new IntVector(-1, -1) },
            { Buttons.UpRight, new IntVector(1, -1) },
            { Buttons.Down, new IntVector(0, 1) },
            { Buttons.DownLeft, new IntVector(-1, 1) },
            { Buttons.DownRight, new IntVector(1, 1) },
            { Buttons.Left, new IntVector(-1, 0) },
            { Buttons.Right, new IntVector(1, 0) }
        };
    }
}
