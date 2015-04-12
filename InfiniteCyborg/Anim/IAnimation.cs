using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfCy.Anim
{
    public interface IAnimation
    {
        void Update(float dt);
        bool Finished { get; }
    }
}
