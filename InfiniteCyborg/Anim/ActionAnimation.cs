using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfCy.Anim
{
    public class ActionAnimation : IAnimation
    {
        Action toRun;

        public ActionAnimation(Action toRun)
        {
            this.toRun = toRun;
        }

        public void Update(float dt)
        {
            if (!this.Finished)
            {
                this.Finished = true;
                this.toRun();
            }
        }

        public bool Finished { get; private set; }
    }
}
