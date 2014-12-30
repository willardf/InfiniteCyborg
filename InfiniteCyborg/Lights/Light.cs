using InfCy.GameCore;
using libtcod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfCy.Lights
{
    abstract class Light
    {
        protected TCODColor color = new TCODColor();
        protected float h, s;

        public abstract void draw(Camera root);

        public void setColor(float r, float g, float b)
        {
            color.Red = (byte)(255 * r);
            color.Green = (byte)(255 * g);
            color.Blue = (byte)(255 * b);

            float v;
            color.getHSV(out h, out s, out v);
        }

        public void Dispose()
        {
            color.Dispose();
        }

        public int X { get; set; }
        public int Y { get; set; }
        public float B { get; set; }
    }
}
