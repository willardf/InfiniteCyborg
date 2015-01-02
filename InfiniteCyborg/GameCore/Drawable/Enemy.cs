using libtcod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfCy.GameCore
{
    class Enemy : Mover
    {
        public Weapon weapon = Weapon.Fists;

        public Enemy()
        {
            Demeanor = Hostile.Enemy;
        }

        public void takeTurn()
        {
            var randy = TCODRandom.getInstance();
            var dx = randy.getInt(-1, 1);
            var dy = randy.getInt(-1, 1);
            var enemies = Game.CurrentGame.FindEnemies(this, weapon, X + dx, Y + dy);
            if (enemies.Length > 0)
            {
                Battle.ResolveAttack(this, this.weapon, enemies[0]);
            }
            else
            {
                Move(dx, dy);
            }
        }

        public override void Draw(Camera root)
        {
            root.setChar(X, Y, Name[0]);
        }

        public override void DrawInfo(Camera root, int y)
        {
            root.print(1, y, "{0}: {1}/{2}", Name, Health, MaxHealth);
        }

        public override void OnDeath(Mover killer)
        {
            base.OnDeath(killer);
            Game.CurrentGame.RemoveEnemy(this);
        }

        protected override void OnMove()
        {
            
        }
    }
}
