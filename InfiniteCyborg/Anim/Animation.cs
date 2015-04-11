using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfCy.Anim
{
    public class Animation
    {
        protected float duration = 0;
        protected float timer = 0;
        public Animation(float duration)
        {
            this.duration = duration;
        }

        public virtual void Update(float dt)
        {
            timer = Math.Min(duration, timer + dt);
        }

        public bool Finished { get { return timer >= duration; } }
    }
}
