using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfCy.Anim
{
    public class ActionAnimation : Animation
    {
        Action toRun;

        public ActionAnimation(Action toRun)
            : base(1)
        {
            this.toRun = toRun;
        }

        public override void Update(float dt)
        {
            if (!this.Finished)
            {
                this.toRun();
                base.Update(5);
            }
        }
    }
}
