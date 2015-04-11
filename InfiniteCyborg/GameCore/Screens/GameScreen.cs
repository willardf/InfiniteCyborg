using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libtcod;
using InfCy.Maths;
using InfCy.Lights;
using InfCy.Genetics;
using InfCy.Anim;

namespace InfCy.GameCore
{
    public class GameScreen : IScreen
    {
        TCODRandom randy = TCODRandom.getInstance();
        List<Enemy> enemies = new List<Enemy>();
        List<Mover> movers = new List<Mover>();

        internal static GameScreen CurrentGame { get; private set; }
        internal static Map CurrentMap { get; private set; }
        public Player Player { get; private set; }
        Camera gameCam, playerCam, groundCam, logCam;
        TCODConsole root;
        ConeLight light;

        public GameScreen(TCODConsole root)
        {
            CurrentGame = this;

            this.root = root;
            gameCam = new Camera(root, 50, 50) { X = 2, Y = 2, Border = true };
            playerCam = new Camera(root, 40, 16) { X = gameCam.Right + 2, Y = 2, Border = true };
            groundCam = new Camera(root, 40, 30) { X = gameCam.Right + 2, Y = playerCam.Bottom + 2, Border = true };
            logCam = new Camera(root, 50, 20) { X = 2, Y = gameCam.Bottom + 2, Border = true };

            Map.LightsOn = true;
            CurrentMap = new Map(120, 120);
            Room.Rectangle(3, 3, 100, 100, CurrentMap);

            CurrentMap.setCell(12, 12, new Cell());
            CurrentMap.setCell(12, 13, new Cell());
            CurrentMap.setCell(13, 13, new Cell());
            CurrentMap.setCell(14, 14, new Cell());
            CurrentMap.Items.Add(new Component(new BitField(Component.BitCount).Randomize(() => TCODRandom.getInstance().getFloat(0, 1) > .5f)) { Name = "test", X = 15, Y = 16 });

            var en = new Enemy();
            en.SetPosition(20, 20);
            enemies.Add(en);
            movers.Add(en);

            Player = new Player();
            Player.SetPosition(10, 10);
            gameCam.FollowPlayer(Player);
            movers.Add(Player);

            light = new ConeLight();
            light.R = 10;
        }

        public void HandleKey(KeyEvent key)
        {
            if (AnimationManager.Instance.Animating) return;

            switch (key.button)
            {
                case Buttons.Lights:
                    Map.LightsOn = !Map.LightsOn;
                    break;
                case Buttons.Pickup:
                    var groundItems = CurrentMap.FindItems(Player.X, Player.Y);
                    if (groundItems.Count > 0)
                    {
                        if (groundItems.Count == 1)
                        {
                            var item = groundItems.First();
                            Player.Inventory.Add(item);
                            CurrentMap.Items.Remove(item);
                            Logger.Log("Picked up {0}", item.Name);
                        }
                        else
                        {
                            // Open menu for Count > 1
                        }
                    }
                    break;
                case Buttons.Inventory:
                    Program.PushScreen(new InventoryScreen(this.root, this.Player));
                    break;
                default:
                    if (Player.HandleKey(key))
                    {
                        enemies.ForEach(e => e.takeTurn());
                    }
                    break;
            }
        }

        IntVector mos;
        public void Update(float dt)
        {
            var status = TCODMouse.getStatus();
            mos.Set(status.CellX, status.CellY);
            light.X = Player.X;
            light.Y = Player.Y;
            light.setColor(1, 0, .5f);
            light.Direction = gameCam.localCoords(mos).Sub(Player.X, Player.Y);

            gameCam.update();
            AnimationManager.Instance.Update(dt);
        }

        public void Draw()
        {
            gameCam.draw();
            CurrentMap.draw(gameCam);
            Player.Draw(gameCam);
            enemies.ForEach(e => e.Draw(gameCam));

            playerCam.draw();
            Player.DrawInfo(playerCam, 1);

            groundCam.draw();
            var sorty = enemies.Cast<IDrawable>().Union(CurrentMap.Items).OrderBy(e => IntVector.Distance(Player.X, Player.Y, e.X, e.Y)).ToList();
            for (int i = 0; i < sorty.Count; ++i)
            {
                sorty[i].DrawInfo(groundCam, i * 2 + 1);
            }

            light.draw(gameCam);

            logCam.draw();
            Logger.DrawLog(logCam);

            // DEBUG
            root.setForegroundColor(TCODColor.white);

            mos = gameCam.localCoords(mos);
            root.print(0, 1, string.Format("x:{0}\ty:{1}", mos.X, mos.Y));
        }

        internal Mover[] FindEnemies(Mover attacker, Weapon weapon, int destx, int desty)
        {
            var candidates = from e in movers
                             where e.Demeanor != attacker.Demeanor
                             where weapon.CheckRange(destx, desty, e.X, e.Y)
                             where e != attacker
                             select e;

            return candidates.ToArray();
        }

        public void Dispose()
        {
            randy.Dispose();
        }

        internal void RemoveEnemy(Enemy e)
        {
            movers.Remove(e);
            enemies.Remove(e);
        }

        internal bool Walkable(int x, int y)
        {
            return CurrentMap.Walkable(x, y) && movers.All(e => e.X != x || e.Y != y);
        }
    }
}
