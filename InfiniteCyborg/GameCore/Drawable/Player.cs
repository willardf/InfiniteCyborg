using InfCy.Maths;
using libtcod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfCy.GameCore
{
    public class Player : Mover
    {
        #region Stats
        public byte Pyro { get; set; }
        public byte Elec { get; set; }
        public byte Aero { get; set; }
        public byte Aqua { get; set; }
        public byte Smithy { get; set; }
        #endregion

        public List<Item> Inventory { get; set; }
        public List<Weapon> Hardpoints { get; set; }

        // TODO: Bodyparts which have weapon hardpoints
        // TODO: Bodytypes which have bodypart hardpoints

        public int Weight { get { return Hardpoints.Sum(h => h.Weight); } }

        // TODO: This is definitely going to need some adjusting...
        public float Speed { get { return Weight / 1024f; } } 

        public Player()
        {
            this.Name = "Player";
            Inventory = new List<Item>();
            var randy = TCODRandom.getInstance();
            Demeanor = Hostile.Friendly;
            Hardpoints = new List<Weapon>() { 
                Weapon.Fists, 
                new Weapon("Rando", new Genetics.BitField(Weapon.BitCount).Randomize(() => randy.getFloat(0, 1) > .5f))
            };
        }

        public override void Draw(Camera root)
        {
            root.setChar(X, Y, '@');
        }

        public override void DrawInfo(Camera info, int y)
        {
            info.print(1, y, "Health {0}/{1}", Health, MaxHealth);
            for (int i = 0; i < Hardpoints.Count; ++i)
            {
                info.print(1, y + i + 2, "{0}", Hardpoints[i]);
            }

            info.print(1, info.Bottom - 4, "Tab for inventory");
        }

        public bool HandleKey(KeyEvent key)
        {
            IntVector dp;
            if (Directions.Cardinal8.TryGetValue(key.button, out dp))
            {
                var enemies = GameScreen.CurrentGame.FindEnemies(this, Hardpoints.First(), X + dp.X, Y + dp.Y);
                if (enemies.Length > 0)
                {
                    foreach (Weapon w in Hardpoints.Where(h => h.Melee && h.Usable()))
                    {
                        Battle.ResolveAttack(this, w, enemies[0]);
                    }
                }
                else
                {
                    Move(dp.X, dp.Y);
                }

                return true;
            }

            return false;
        }

        protected override void OnMove()
        {
            GameScreen.CurrentMap.UpdateFov(this.X, this.Y);
        }

        public void Dispose()
        {
        }

        public override void OnDeath(Mover killer)
        {
            // You gone and died
        }
    }
}
