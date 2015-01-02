using InfCy.GameCore;
using System;
namespace InfCy
{
    interface IScreen : IDisposable
    {
        void Draw();
        void HandleKey(Buttons key);
        void Update();
    }
}
