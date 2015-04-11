using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfCy.Anim;

namespace InfCy.Anim
{
    public class AnimationManager
    {
        private Queue<Animation> Anims = new Queue<Animation>();
        public bool Animating { get { return Anims.Count > 0; } }

        #region Singleton junk
        private static AnimationManager instance = new AnimationManager();
        public static AnimationManager Instance { get { return instance; } }
        private AnimationManager() { }
        #endregion

        internal void Update(float dt)
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
        }

        internal void Enqueue(Animation item)
        {
            Anims.Enqueue(item);
        }
    }
}
