using InfCy.Maths;
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

        public int Health { get; set; }
        public int MaxHealth { get; set; }

        public int Steps { get; private set; }
        public string Name { get; set; }
        public int X { get; private set; }
        public int Y { get; private set; }

        private int id;
        private static int MoverIds = 1;

        public Mover()
        {
            Health = MaxHealth = 10;

            id = MoverIds++;
            Name = "Mover " + id;
        }

        public bool Move(int dx, int dy, bool updateStep = false)
        {
            if (Game.CurrentGame.Walkable(X + dx, Y + dy))
            {
                X += dx;
                Y += dy;
                if (updateStep) Steps++;
                return true;
            }

            return false;
        }

        public void SetPosition(int x, int y)
        {
            X = x;
            Y = y;
        }

        protected abstract void OnMove();

        protected virtual void OnDeath(Mover killer)
        {
            Logger.Log("{0} killed {1}.", killer.Name, this.Name);
        }

        public void TakeDamage(int dmg, Mover attacker)
        {
            this.Health -= dmg;
            if (this.Health <= 0)
            {
                OnDeath(attacker);
            }
        }

        public void Push(IntVector dir, int amount, Mover cause)
        {
            int dmg = 0;
            for (int i = 0; i < amount;  ++i)
            {
                if (!Move(dir.X, dir.Y))
                {
                    dmg = amount - i;
                    break;
                }
            }

            if (dmg > 0)
            {
                Logger.Log("{0} was knocked back and took {1} damage!", this.Name, dmg);
                this.TakeDamage(dmg, cause);
            }
            else
            {
                Logger.Log("{0} was knocked back!", this.Name);
            }
        }

        public IntVector Position { get { return new IntVector(X, Y); } }
    }
}
