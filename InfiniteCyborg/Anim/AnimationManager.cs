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
        private Queue<Animation> anims = new Queue<Animation>();
        public bool Animating { get { return anims.Count > 0; } }

        #region Singleton junk
        private static AnimationManager instance = new AnimationManager();
        public static AnimationManager Instance { get { return instance; } }
        private AnimationManager() { }
        #endregion
    }
}
