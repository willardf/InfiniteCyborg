using System;
namespace InfCy.GameCore
{
    interface IScreen : IDisposable
    {
        void draw();
        void handleKey(Buttons key);
        void update();
    }
}
