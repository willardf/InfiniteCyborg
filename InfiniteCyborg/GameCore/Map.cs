using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using libtcod;

namespace InfCy.GameCore
{
    class Map : IDisposable
    {
        TCODMap visMap;
        Cell[][] objMap;
        public int Width { get; private set; }
        public int Height { get; private set; }

        public static bool LightsOn { get; set; }

        public List<Enemy> Enemies = new List<Enemy>();

        public Map(int w, int h)
        {
            Width = w;
            Height = h;

            visMap = new TCODMap(w, h);
            objMap = new Cell[w][];
            for (int x = 0; x < w; ++x)
            {
                objMap[x] = new Cell[h];
            }
        }

        public void draw(Camera root)
        {
             for (int x = 0; x < Width; ++x)
             {
                 if (!root.isVisibleX(x)) continue;
                 for (int y = 0; y < Height; ++y)
                 {
                     if (!root.isVisibleY(y)) continue;

                     Cell c = objMap[x][y];
                     if (c != null)
                     {
                         if (LightsOn || visMap.isInFov(x, y))
                         {
                             root.setChar(x, y, c.DrawChar);
                         }
                     }
                 }
             }
        }

        internal void setCell(int x, int y, Cell cell)
        {
            if (validCoord(x, y))
            {
                objMap[x][y] = cell;
                visMap.setProperties(x, y, cell.Transparent, cell.Blocking == Cell.BlockType.None);
            }
        }

        internal void mergeCell(int x, int y, Cell cell)
        {
            if (validCoord(x, y))
            {
                var oldCell = objMap[x][y];
                if (oldCell == null && oldCell.Blocking == Cell.BlockType.None)
                {
                    objMap[x][y] = cell;
                    visMap.setProperties(x, y, cell.Transparent, cell.Blocking == Cell.BlockType.None);
                }
            }
        }

        internal bool Walkable(int x, int y)
        {
            return validCoord(x, y) && objMap[x][y] != null && objMap[x][y].Blocking == Cell.BlockType.None;
        }


        internal bool Transparent(int y, int x)
        {
            return validCoord(x, y) && objMap[x][y] != null && objMap[x][y].Transparent;
        }

        private bool validCoord(int x, int y)
        {
            return x >= 0 && y >= 0 && x < Width && y < Height;
        }

        public void Dispose()
        {
            visMap.Dispose();
        }

        internal void updateFov(int x, int y)
        {
            visMap.computeFov(x, y);
        }
    }
}
