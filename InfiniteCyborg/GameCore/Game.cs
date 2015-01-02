using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libtcod;
using InfCy.Maths;
using InfCy.Lights;

namespace InfCy.GameCore
{
    public class Game : IScreen
    {
        TCODRandom randy = TCODRandom.getInstance();
        List<Enemy> enemies = new List<Enemy>();
        List<Mover> movers = new List<Mover>();

        internal static Game CurrentGame { get; private set; }
        internal static Map CurrentMap { get; private set; }
        Player player;
        Camera gameCam, playerCam, groundCam, logCam;
        TCODConsole root;
        ConeLight light;

        public Game(TCODConsole root)
        {
            CurrentGame = this;

            this.root = root;
            gameCam = new Camera(root, 50, 50) { X = 2, Y = 2, Border = true };
            playerCam = new Camera(root, 40, 10) { X = gameCam.Right + 2, Y = 2, Border = true };
            groundCam = new Camera(root, 40, 30) { X = gameCam.Right + 2, Y = playerCam.Bottom + 2, Border = true };
            logCam = new Camera(root, 40, 20) { X = 2, Y = gameCam.Bottom + 2, Border = true };

            Map.LightsOn = true;
            CurrentMap = new Map(120, 120);
            Room.Rectangle(3, 3, 100, 100, CurrentMap);

            CurrentMap.setCell(12, 12, new Cell());
            CurrentMap.setCell(12, 13, new Cell());
            CurrentMap.setCell(13, 13, new Cell());
            CurrentMap.setCell(14, 14, new Cell());
            CurrentMap.Items.Add(new Component() { Name = "test", X = 15, Y = 16 });

            var en = new Enemy();
            en.SetPosition(20, 20);
            enemies.Add(en);
            movers.Add(en);

            player = new Player();
            player.SetPosition(10, 10);
            gameCam.FollowPlayer(player);
            movers.Add(player);

            light = new ConeLight();
            light.R = 10;
        }

        public void handleKey(Buttons key)
        {
            switch (key)
            {
                case Buttons.Lights:
                    Map.LightsOn = !Map.LightsOn;
                    break;
                case Buttons.Pickup:
                    var groundItems = CurrentMap.FindItems(player.X, player.Y);
                    if (groundItems.Count > 0)
                    {
                        if (groundItems.Count == 1)
                        {
                            var item = groundItems.First();
                            player.Items.Add(item);
                            CurrentMap.Items.Remove(item);
                            Logger.Log("Picked up {0}", item.Name);
                        }
                        else
                        {
                            // Open menu for Count > 1
                        }
                    }
                    break;
                default:
                    if (player.handleKey(key))
                    {
                        enemies.ForEach(e => e.takeTurn());
                    }
                    break;
            }
        }

        IntVector mos;
        public void update()
        {
            var status = TCODMouse.getStatus();
            mos.Set(status.CellX, status.CellY);
            light.X = player.X;
            light.Y = player.Y;
            light.setColor(1, 0, .5f);
            light.Direction = gameCam.localCoords(mos).Sub(player.X, player.Y);

            gameCam.update();
        }

        public void draw()
        {
            gameCam.draw();
            CurrentMap.draw(gameCam);
            player.draw(gameCam);
            enemies.ForEach(e => e.draw(gameCam));

            playerCam.draw();
            player.drawInfo(playerCam, 1);

            groundCam.draw();
            var sorty = enemies.OrderBy(e => IntVector.Distance(player.X, player.Y, e.X, e.Y)).ToList();
            for (int i = 0; i < sorty.Count; ++i)
            {
                sorty[i].drawInfo(groundCam, i * 2 + 1);
            }

            light.draw(gameCam);

            logCam.draw();
            Logger.drawLog(logCam);

            // DEBUG
            root.setForegroundColor(TCODColor.white);

            mos = gameCam.localCoords(mos);
            root.print(0, 1, string.Format("x:{0}\ty:{1}", mos.X, mos.Y));
        }

        internal Mover[] findEnemies(Mover attacker, Weapon weapon, int destx, int desty)
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
