using InfCy.GameCore;
using InfCy.Maths;
using libtcod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfCy.Lights
{
    class ConeLight : Light
    {
        public IntVector Direction { get; set; }
        public int R { get { return solver.Radius; } set { solver = new FOVSolver(value); } }
        public float Degrees { get; set; }

        private FOVSolver solver;

        public ConeLight()
        {
            B = 1;
            R = 5;
        }

        public void update()
        {
            
        }

        public override void draw(Camera root)
        {
            solver.calculateFOV(Game.CurrentMap, X, Y, d => (1 - (d / R) * (d / R)) * B);

            for (int xx = X - R; xx < X + R; ++xx)
            {
                var lx = xx - X + R;
                for (int yy = Y - R; yy < Y + R; ++yy)
                {
                    var ly = yy - Y + R;

                    color.setHSV(h, s, solver.getIntensity(lx, ly));
                    root.setCharBackColor(xx, yy, color, TCODBackgroundFlag.Add);
                }
            }
        }
    }
}
