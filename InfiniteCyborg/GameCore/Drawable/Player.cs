using InfCy.GameCore.Utilities;
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
        private class TargettingInfo
        {
            public Action<Path> waitForTarget;
            public Action<Camera, Path> drawTarget;
            public Path target;
            public bool TargetSet { get; private set; }

            public TargettingInfo(Player p)
            {
                Reset(p);
            }

            public void Reset(Player p)
            {
                this.TargetSet = false;
                this.waitForTarget = (path) => { p.autoWalk = path; this.Reset(p); };
                drawTarget = PathDrawers.DrawMovement;
            }

            public void SetTarget(Path target)
            {
                this.target = target;
                this.TargetSet = true;
            }
        }

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
        private Path autoWalk;

        private readonly TargettingInfo targettingInfo;

        // TODO: Balance
        public override int Speed { get { return Math.Min(75, (int)(1024 - Weight)); } set { } }
        
        public Player()
        {
            targettingInfo = new TargettingInfo(this);
            this.Name = "Player";
            Inventory = new List<Item>();
            var randy = TCODRandom.getInstance();
            Demeanor = Hostile.Friendly;
            var weap = new Weapon("Rando", new Genetics.BitField(Weapon.BitCount).Randomize(() => randy.getFloat(0, 1) > .5f));
            weap.AddComponent(new Component() { Range = 5, Melee = false });

            Hardpoints = new List<Weapon>() { 
                Weapon.Fists, 
                weap
            };
        }

        public override void Draw(Camera root)
        {
            root.setChar(X, Y, '@');
            if (targettingInfo != null)
            {
                targettingInfo.drawTarget(root, targettingInfo.target);
            }
        }

        public override void DrawInfo(Camera info, int y)
        {
            info.print(1, y, "Health {0}/{1}", Health, MaxHealth);
            for (int i = 0; i < Hardpoints.Count; ++i)
            {
                info.print(1, y + i + 2, "{0}", Hardpoints[i]);
            }

            if (targettingInfo != null)
            {
                info.print(1, info.Bottom - 5, "Targetting");
            }

            info.print(1, info.Bottom - 4, "Tab for inventory");
        }

        /// <summary>
        /// </summary>
        /// <param name="key">True if key was handled</param>
        /// <returns></returns>
        public bool HandleKey(KeyEvent key)
        {
            switch (key.button)
            {
                case Buttons.Pickup:
                    var groundItems = GameScreen.CurrentMap.FindItems(this.X, this.Y);
                    if (groundItems.Count > 0)
                    {
                        if (groundItems.Count == 1)
                        {
                            var item = groundItems.First();
                            this.Inventory.Add(item);
                            GameScreen.CurrentMap.Items.Remove(item);
                            Logger.Log("Picked up {0}", item.Name);
                        }
                        else
                        {
                            // Open menu for Count > 1
                        }
                    }
                    break;
                case Buttons.Inventory:
                    Program.PushScreen(new InventoryScreen(GameScreen.CurrentGame.Root, this));
                    break;
            }

            IntVector dp;
            if (Directions.Cardinal8.TryGetValue(key.button, out dp))
            {
                bool doMove = true;
                foreach (Weapon w in Hardpoints.Where(h => h.Melee && h.Usable()))
                {
                    dp = dp.Add(X, Y);
                    var enemies = GameScreen.CurrentGame.FindEnemies(this, w, dp.X, dp.Y);

                    if (enemies.Length > 0)
                    {
                        doMove = false;
                        Battle.ResolveAttack(this, w, dp, GameScreen.CurrentGame);
                    }
                }

                if (doMove)
                {
                    Walk(dp.X, dp.Y);
                }

                return true;
            }
            else if (key.tcodKey >= TCODKeyCode.Zero && key.tcodKey <= TCODKeyCode.Nine)
            {
                var hardpointIdx = (key.tcodKey - TCODKeyCode.Zero + 9) % 10;
                if (hardpointIdx < this.Hardpoints.Count)
                {
                    var weapon = this.Hardpoints[hardpointIdx];
                    targettingInfo.waitForTarget = target => Attack(weapon, target.Last());
                    targettingInfo.drawTarget = (c, p) => PathDrawers.DrawRangedTarget(c, p, weapon.Range);
                }
            }

            return false;
        }

        private bool Attack(Weapon weapon, IntVector t)
        {
            targettingInfo.Reset(this);

            this.Timer = 0;
            this.Duration = 100 - weapon.Speed;

            return Battle.ResolveAttack(this, weapon, t, GameScreen.CurrentGame);
        }

        protected override void OnMove()
        {
            GameScreen.CurrentMap.UpdateFov(this.X, this.Y);
        }

        public override void DoTurn()
        {
            if (autoWalk != null && autoWalk.Count > 0)
            {
                var p = autoWalk.Pop();
                Walk(p.X - X, p.Y - Y);
            }
            else if (targettingInfo != null && targettingInfo.TargetSet)
            {
                targettingInfo.waitForTarget(targettingInfo.target);
            }
        }

        public void UpdateTarget(Path target)
        {
            this.targettingInfo.target = target;
        }

        public void SetTarget(Path target)
        {
            this.targettingInfo.SetTarget(target);
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
