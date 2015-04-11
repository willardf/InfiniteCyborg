using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfCy.Anim
{
    public class Animation
    {
        public float Duration { get; private set; }

        protected float timer = 0;
        public Animation(float duration)
        {
            this.Duration = duration;
        }

        public virtual void Update(float dt)
        {
            timer = Math.Min(Duration, timer + dt);
        }

        public bool Finished { get { return timer >= Duration; } }
    }
}
