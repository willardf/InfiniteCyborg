using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfCy.Anim;

namespace InfCy.Anim
{
    /// <summary>
    /// Handles real time animations. Stuff that happens independant of turns.
    /// </summary>
    public class AnimationManager
    {
        private Queue<IAnimation> Anims = new Queue<IAnimation>();
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
                IAnimation a = Anims.Peek();
                a.Update(dt);
                if (a.Finished)
                {
                    Anims.Dequeue();
                }
            }
        }

        internal void Enqueue(IAnimation item)
        {
            Anims.Enqueue(item);
        }
    }
}
