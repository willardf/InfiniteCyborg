using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfCy.Anim
{
    public class SequenceAnimation : Animation
    {
        private Queue<Animation> Anims = new Queue<Animation>();
        public SequenceAnimation() : base(0) { }
        public SequenceAnimation(params Animation[] anims)
            : base(anims.Sum(a=>a.Duration))
        {
            foreach (var a in anims)
            {
                Anims.Enqueue(a);
            }
        }

        public override void Update(float dt)
        {
            if (Anims.Count > 0)
            {
                Animation a = Anims.Peek();
                a.Update(dt);
                if (a.Finished)
                {
                    Anims.Dequeue();
                }
            }

            base.Update(dt);
        }
    }
}
