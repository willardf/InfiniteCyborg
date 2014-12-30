using InfCy.GameCore;
using InfCy.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfCy.Lights
{
    class FOVSolver
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        private float[][] lightMap;
        public int Radius { get; private set; }
        private Func<float, float> rStrat;

        private Map map;
        private readonly static int[] Directions = { 1, 1, -1, -1, 1 };
        
        public FOVSolver(int radius)
        {
            this.Radius = radius;

            lightMap = new float[radius * 2][];
            for (int i = 0; i < lightMap.Length; ++i)
            {
                lightMap[i] = new float[radius * 2];
            }
        }

        public void calculateFOV(Map resistanceMap, int startx, int starty, Func<float, float> rStrat)
        {
            this.X = startx;
            this.Y = starty;
            this.rStrat = rStrat;
            this.map = resistanceMap;

            for (int i = 0; i < lightMap.Length; ++i)
            {
                for (int j = 0; j < lightMap.Length; ++j)
                    lightMap[i][j] = 0;
            }

            lightMap[startx - X + Radius][starty - Y + Radius] = rStrat(0);//light the starting cell
            for (int i = 0; i < Directions.Length - 1; ++i)
            {
                castLight(1, 1.0f, 0.0f, 0, Directions[i], Directions[i + 1], 0);
                castLight(1, 1.0f, 0.0f, Directions[i], 0, 0, Directions[i + 1]);
            }
        }

        private void castLight(int row, float start, float end, int xx, int xy, int yx, int yy)
        {
            if (start < end) return;

            float newStart = 0.0f;
            bool blocked = false;

            for (int distance = row; distance <= Radius && !blocked; distance++)
            {
                int deltaY = -distance;
                for (int deltaX = -distance; deltaX <= 0; deltaX++)
                {
                    int currentX = X + deltaX * xx + deltaY * xy;
                    int currentY = Y + deltaX * yx + deltaY * yy;
                    float leftSlope = (deltaX - 0.5f) / (deltaY + 0.5f);
                    float rightSlope = (deltaX + 0.5f) / (deltaY - 0.5f);

                    if (!(currentX >= 0 && currentY >= 0 && currentX < map.Width && currentY < map.Height) || start < rightSlope)
                    {
                        continue;
                    }
                    else if (end > leftSlope)
                    {
                        break;
                    }

                    //check if it's within the lightable area and light if needed
                    var rLen = Length(deltaX, deltaY);
                    if (rLen < Radius)
                    {
                        lightMap[currentX - X + Radius][currentY - Y + Radius] = rStrat(rLen);
                    }

                    if (blocked) //previous cell was a blocking one
                    {
                        if (!map.Transparent(currentX, currentY)) //hit a wall
                        { 
                            newStart = rightSlope;
                            continue;
                        }
                        else
                        {
                            blocked = false;
                            start = newStart;
                        }
                    }
                    else
                    {
                        if (!map.Transparent(currentX, currentY) && distance < Radius)
                        {   // hit a wall within sight line
                            blocked = true;
                            castLight(distance + 1, start, leftSlope, xx, xy, yx, yy);
                            newStart = rightSlope;
                        }
                    }
                }
            }
        }

        private static float Length(int x, int y)
        {
            return (float)Math.Sqrt(x * x + y * y);
        }

        internal float getIntensity(int lx, int ly)
        {
            return lightMap[lx][ly];
        }
    }
}
