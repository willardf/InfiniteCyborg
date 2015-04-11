using System;

namespace InfCy.GameCore
{
    interface IScreen : IDisposable
    {
        void Draw();
        void HandleKey(KeyEvent key);
        void Update();
    }
}
