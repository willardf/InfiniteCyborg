using InfCy.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfCy.GameCore
{
    public enum Hostile { None, Friendly = 1 << 0, Enemy = 1 << 1, Neutral = 1 << 2 }

    public abstract class Mover : IDrawable
    {
        public Hostile Demeanor { get; set; }

        public int Health { get; set; }
        public int MaxHealth { get; set; }

        public int Steps { get; private set; }
        public string Name { get; set; }
        public int X { get; private set; }
        public int Y { get; private set; }

        public abstract int Speed { get; set; }

        public int Timer { get; set; }
        public int Duration { get; protected set; }

        private int id;
        private static int MoverIds = 1;

        public Mover()
        {
            Health = MaxHealth = 10;

            id = MoverIds++;
            Name = "Mover " + id;
            this.Duration = 100;
        }

        public bool Walk(IntVector input) { return Walk(input.X, input.Y); }
        public bool Walk(int dx, int dy)
        {
            if (GameScreen.CurrentGame.Walkable(X + dx, Y + dy))
            {
                X += dx;
                Y += dy;
                Steps++;
                OnMove();
                this.Timer = 0;
                this.Duration = 100 - Speed; // Based on speed
                return true;
            }

            return false;
        }

        public bool Push(int dx, int dy)
        {
            if (GameScreen.CurrentGame.Walkable(X + dx, Y + dy))
            {
                X += dx;
                Y += dy;
                OnMove();
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

        public abstract void OnDeath(Mover killer);

        public void TakeDamage(int dmg, Mover attacker)
        {
            this.Health -= dmg;
        }

        public void Push(IntVector dir, int amount, Mover cause)
        {
            int dmg = 0;
            for (int i = 0; i < amount;  ++i)
            {
                if (!Push(dir.X, dir.Y))
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

        public abstract void Draw(Camera root);

        public abstract void DrawInfo(Camera root, int y);

        public abstract void DoTurn();
    }
}
