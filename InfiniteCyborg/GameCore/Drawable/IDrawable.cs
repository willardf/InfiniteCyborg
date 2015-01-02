using System;
namespace InfCy.GameCore
{
    interface IDrawable
    {
        void draw(Camera root);
        void drawInfo(Camera root, int y);
    }
}
