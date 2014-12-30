using libtcod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfCy.GameCore
{
    static class Room
    {
        public static void Rectangle(int x, int y, int w, int h, Map map)
        {
            for (int xx = x; xx < x + w; ++xx)
            {
                for (int yy = y; yy < y + h; ++yy)
                {
                    bool stuff = !(yy == y || yy == y + h - 1 || xx == x || xx == x + w - 1);
                    map.setCell(xx, yy, new Cell
                    {
                        DrawChar = stuff ? '.' : '#',
                        Transparent = stuff,
                        Blocking = stuff ? Cell.BlockType.None : Cell.BlockType.Full
                    });
                }
            }
        }

        public static void MergeRandomWalls(int x, int y, int w, int h, Map map)
        {
            TCODRandom r = TCODRandom.getInstance();
            for (int xx = x; xx < x + w; ++xx)
            {
                for (int yy = y; yy < y + h; ++yy)
                {
                    bool stuff = r.getFloat(0, 1) > .5f;
                    map.setCell(xx, yy, new Cell
                    {
                        DrawChar = stuff ? '.' : '#',
                        Transparent = stuff,
                        Blocking = stuff ? Cell.BlockType.None : Cell.BlockType.Full
                    });
                }
            }
        }
    }
}
