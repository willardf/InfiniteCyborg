using InfCy.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfCy.GameCore.Drawable
{
    public class Projectile : Mover
    {
        public int Damage { get; set; }
        public int Speed { get; set; }
        public int DestX { get; private set; }
        public int DestY { get; private set; }

        public Projectile()
        {
        }

        protected override void OnMove()
        {
        }

        public override void OnDeath(Mover killer)
        {
        }

        public override void Draw(Camera root)
        {
            var ang = (IntVector.Angle(X, Y, DestX, DestY) + 360) % 360;
            char c = '-';
            if (ang < 22.5) c = '-';
            if (ang < 67.5) c = '/';
            if (ang < 112.5) c = '|';
            if (ang < 157.5) c = '\\';
            if (ang < 202.5) c = '-';
            if (ang < 247.5) c = '/';
            if (ang < 292.5) c = '|';
            if (ang < 337.5) c = '\\';

            root.setChar(X, Y, c);
        }

        public override void DrawInfo(Camera root, int y)
        {
        }
    }
}
