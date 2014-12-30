using InfCy.Maths;
using libtcod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfCy.GameCore
{
    public class Camera
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int OffX { get; set; }
        public int OffY { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public int Right { get { return X + Width + (Border ? 1 : 0) + 1; } }
        public int Bottom { get { return Y + Height + (Border ? 1 : 0) + 1; } }
        public bool Border { get; set; }

        private Player follow;

        TCODConsole console;
        public Camera(TCODConsole output)
        {
            console = output;
        }

        public Camera(TCODConsole output, int w, int h)
        {
            console = output;
            Width = w;
            Height = h;
        }

        public void update()
        {
            if (follow != null)
            {
                this.OffX = follow.X - this.Width / 2;
                this.OffY = follow.Y - this.Height / 2;
            }
        }

        public void draw()
        {
            if (Border)
            {
                console.setChar(X - 1, Y - 1, (int)TCODSpecialCharacter.DoubleNW);
                console.setChar(X + Width + 1, Y - 1, (int)TCODSpecialCharacter.DoubleNE);
                console.setChar(X - 1, Y + Height + 1, (int)TCODSpecialCharacter.DoubleSW);
                console.setChar(X + Width + 1, Y + Height + 1, (int)TCODSpecialCharacter.DoubleSE);

                for (int x = X; x <= X + Width; ++x)
                {
                    console.setChar(x, Y - 1, (int)TCODSpecialCharacter.DoubleHorzLine);
                    console.setChar(x, Y + Height + 1, (int)TCODSpecialCharacter.DoubleHorzLine);
                }

                for (int y = Y; y <= Y + Height; ++y)
                {
                    console.setChar(X - 1, y, (int)TCODSpecialCharacter.DoubleVertLine);
                    console.setChar(X + Width + 1, y, (int)TCODSpecialCharacter.DoubleVertLine);
                }
            }
        }

        internal void setChar(int x, int y, char c)
        {
            x -= OffX - X;
            y -= OffY - Y;
            if (validCoord(x, y))
            {
                console.setChar(x + X, y + Y, c);
            }
        }

        public void print(int x, int y, string format, params object[] args)
        {
            x -= OffX - X;
            y -= OffY - Y;
            console.setForegroundColor(TCODColor.white);
            console.print(x, y, string.Format(format, args));
        }

        internal void setCharBackColor(int x, int y, TCODColor color, TCODBackgroundFlag flags)
        {
            x -= OffX - X;
            y -= OffY - Y;
            if (validCoord(x, y))
            {
                console.setCharBackground(x + X, y + Y, color, flags);
            }
        }

        private bool validCoord(int x, int y)
        {
            return x >= 0 && y >= 0 && x <= Width && y <= Height;
        }

        public bool isVisibleX(int x)
        {
            x -= OffX - X;
            return x >= 0 && x <= Width;
        }

        public bool isVisibleY(int y)
        {
            y -= OffY - Y;
            return y >= 0 && y <= Height;
        }

        internal void FollowPlayer(Player p)
        {
            follow = p;
        }

        internal IntVector localCoords(IntVector mos)
        {
            return mos.Sub(X, Y);
        }
    }
}
