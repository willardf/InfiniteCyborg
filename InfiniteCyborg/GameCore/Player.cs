using libtcod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfCy.GameCore
{
    class Player : Mover
    {
        public Weapon MainWeapon { get; set; }

        public Player()
        {
            Demeanor = Hostile.Friendly;
            MainWeapon = Weapon.Fists;
        }

        public void draw(Camera root)
        {
            root.setChar(X, Y, '@');
        }

        public void drawInfo(Camera info)
        {
            info.print(1, 1, "Health {0}/{1}", Health, MaxHealth);
            info.print(1, 3, "{0}", MainWeapon);
        }

        public bool handleKey(Buttons key)
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

            var enemies = Game.CurrentGame.findEnemies(this, MainWeapon, X + dx, Y + dy);
            if (enemies.Length > 0)
            {
                MainWeapon.attack(this, enemies[0]);

            }
            else
            {
                Move(dx, dy);
            }

            return true;
        }

        protected override void OnMove()
        {
            Game.CurrentMap.updateFov(this.X, this.Y);
        }

        public void Dispose()
        {
        }
    }
}
