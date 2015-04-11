using InfCy.Anim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfCy.GameCore
{
    static class Battle
    {
        public static void ResolveAttack(Mover attacker, Weapon weapon, Mover defender)
        {
            Animation baseAnim = weapon.Use();

            ActionAnimation finishAnimation = new ActionAnimation(() =>
            {
                defender.TakeDamage(weapon.TotalDamage, attacker);

                Logger.Log("{0} dealt {1} damage to {2} with {3}", attacker.Name, weapon.TotalDamage, defender.Name, weapon.Name);
                if (defender.Health <= 0)
                {
                    defender.OnDeath(attacker);
                    Logger.Log("{0} killed {1}.", attacker.Name, defender.Name);
                }
                else // Apply after effects to alive targets.
                {
                    if (weapon.Push != 0)
                    {
                        var vec = (defender.Position - attacker.Position);
                        vec.Norm();
                        defender.Push(vec, weapon.Push, attacker);
                    }
                }
            });

            SequenceAnimation output = new SequenceAnimation(baseAnim, finishAnimation);
            AnimationManager.Instance.Enqueue(output);
        }
    }
}
