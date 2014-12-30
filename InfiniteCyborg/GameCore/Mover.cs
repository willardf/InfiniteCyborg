using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfCy.GameCore
{
    public enum Hostile { Friendly, Enemy }

    abstract class Mover
    {
        public Hostile Demeanor { get; set; }

        public int Health = 10;
        public int MaxHealth = 10;
        public uint Experience { get; private set; }
        public uint ExpYield { get; set; }

        public int Steps { get; private set; }
        public string Name { get; set; }
        public int X { get; private set; }
        public int Y { get; private set; }

        private int id;
        private static int MoverIds = 1;

        public Mover()
        {
            ExpYield = 10;
            id = MoverIds++;
            Name = "Mover " + id;
        }

        public void Move(int dx, int dy, bool updateStep = false)
        {
            if (Game.CurrentGame.Walkable(X + dx, Y + dy))
            {
                X += dx;
                Y += dy;
                if (updateStep) Steps++;
            }
        }

        public void SetPosition(int x, int y)
        {
            X = x;
            Y = y;
        }

        protected abstract void OnMove();

        protected virtual void OnDeath(Mover killer)
        {
            Logger.Log("{0} killed {1}. Gained {2}xp.", killer.Name, this.Name, this.ExpYield);
            killer.GainExperience(ExpYield);
        }

        private void GainExperience(uint xp)
        {
            Experience += xp;
            // TODO: Levels
        }

        internal void TakeDamage(int dmg, Mover attacker)
        {
            this.Health -= dmg;
            if (this.Health <= 0)
            {
                OnDeath(attacker);
            }
        }
    }
}
