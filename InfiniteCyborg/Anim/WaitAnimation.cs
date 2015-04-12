using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfCy.Anim
{
    public class WaitAnimation : IAnimation
    {
        public virtual float Duration { get; protected set; }

        protected float timer = 0;
        public WaitAnimation(float duration)
        {
            this.Duration = duration;
        }

        protected WaitAnimation() { }

        public virtual void Update(float dt)
        {
            timer = Math.Min(Duration, timer + dt);
        }

        public virtual bool Finished { get { return timer >= Duration; } }
    }
}
