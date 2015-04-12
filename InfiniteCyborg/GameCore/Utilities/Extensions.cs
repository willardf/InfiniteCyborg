using libtcod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfCy.GameCore.Utilities
{
    public static class Extensions
    {
        public static void setRGB(this TCODColor self, byte r, byte g, byte b)
        {
            self.Red = r;
            self.Green = g;
            self.Blue = b;
        }
    }
}
