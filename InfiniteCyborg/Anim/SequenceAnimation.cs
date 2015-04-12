using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfCy.Anim
{
    public class SequenceAnimation : IAnimation
    {
        private Queue<IAnimation> Anims = new Queue<IAnimation>();
        public SequenceAnimation() { }
        public SequenceAnimation(params IAnimation[] anims)
        {
            foreach (var a in anims)
            {
                Anims.Enqueue(a);
            }
        }

        public void Update(float dt)
        {
            if (Anims.Count > 0)
            {
                IAnimation a = this.Anims.Peek();
                a.Update(dt);
                if (a.Finished)
                {
                    Anims.Dequeue();
                }
            }
        }

        public bool Finished { get { return this.Anims.Count == 0; } }
    }
}
