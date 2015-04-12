using InfCy.Maths;
using libtcod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfCy.GameCore.Utilities
{
    public static class PathDrawers
    {
        private static TCODColor color = new TCODColor();
        public static void DrawRangedTarget(Camera root, Path target, byte range)
        {
            if (target != null)
            {
                color.setRGB(0, 255, 0);
                for (int i = 0; i < target.Count; ++i)
                {
                    var p = target[i];
                    if (i > range)
                    {
                        color.setRGB(255, 0, 0);
                    }

                    root.setCharBackColor(p.X, p.Y, color, TCODBackgroundFlag.Add);
                }
            }
        }

        public static void DrawMovement(Camera root, Path target)
        {
            if (target != null)
            {
                color.setRGB(0, 0, 255);
                foreach (var p in target)
                {
                    root.setCharBackColor(p.X, p.Y, color, TCODBackgroundFlag.Add);
                }
            }
        }
    }
}
