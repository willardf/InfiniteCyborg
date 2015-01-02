using InfCy.GameCore;
using System;
namespace InfCy
{
    interface IScreen : IDisposable
    {
        void draw();
        void handleKey(Buttons key);
        void update();
    }
}
