﻿using System;
namespace InfCy.GameCore
{
    interface IDrawable
    {
        void Draw(Camera root);
        void DrawInfo(Camera root, int y);
        int X { get; }
        int Y { get; }
    }
}
