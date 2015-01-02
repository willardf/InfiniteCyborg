using libtcod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfCy.GameCore
{
    class Player : Mover
    {
        #region Stats
        public byte Pyro { get; set; }
        public byte Elec { get; set; }
        public byte Aero { get; set; }
        public byte Aqua { get; set; }
        public byte Smithy { get; set; }
        #endregion

        public List<Item> Items { get; set; }
        public List<Weapon> Hardpoints { get; set; }

        // TODO: Bodyparts which have weapon hardpoints
        // TODO: Bodytypes which have bodypart hardpoints

        public int Weight { get { return Hardpoints.Sum(h => h.Weight); } }

        // TODO: This is definitely going to need some adjusting...
        public float Speed { get { return Weight / 1024f; } } 

        public Player()
        {
            Items = new List<Item>();
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
        }

        public bool HandleKey(Buttons key)
        {
            var dx = 0;
            var dy = 0;
            switch (key)
            {
                case Buttons.UpLeft:
                    dx = -1;
                    dy = -1;
                    break;
                case Buttons.DownLeft:
                    dx = -1;
                    dy = 1;
                    break;
                case Buttons.DownRight:
                    dx = 1;
                    dy = 1;
                    break;
                case Buttons.UpRight:
                    dx = 1;
                    dy = -1;
                    break;
                case Buttons.Up:
                    dy = -1;
                    break;
                case Buttons.Down:
                    dy = 1;
                    break;
                case Buttons.Left:
                    dx = -1;
                    break;
                case Buttons.Right:
                    dx = 1;
                    break;
                default:
                    return false;
            }

            var enemies = Game.CurrentGame.FindEnemies(this, Hardpoints.First(), X + dx, Y + dy);
            if (enemies.Length > 0)
            {
                Battle.ResolveAttack(this, Hardpoints.First(), enemies[0]);
            }
            else
            {
                Move(dx, dy);
            }

            return true;
        }

        protected override void OnMove()
        {
            Game.CurrentMap.UpdateFov(this.X, this.Y);
        }

        public void Dispose()
        {
        }
    }
}
