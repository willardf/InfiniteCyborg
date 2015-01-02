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
        Player p;
        Camera gameCam, playerCam, groundCam, logCam;
        TCODConsole root;
        ConeLight light;

        public Game(TCODConsole root)
        {
            CurrentGame = this;

            this.root = root;
            gameCam = new Camera(root, 50, 50) { X = 4, Y = 4, Border = true };
            playerCam = new Camera(root, 40, 10) { X = gameCam.Right + 2, Y = 2, Border = true };
            groundCam = new Camera(root, 40, 30) { X = gameCam.Right + 2, Y = playerCam.Bottom + 2, Border = true };
            logCam = new Camera(root, 40, 20) { X = 4, Y = gameCam.Bottom + 2, Border = true };

            Map.LightsOn = true;
            CurrentMap = new Map(120, 120);
            Room.Rectangle(3, 3, 100, 100, CurrentMap);

            CurrentMap.setCell(12, 12, new Cell());
            CurrentMap.setCell(12, 13, new Cell());
            CurrentMap.setCell(13, 13, new Cell());
            CurrentMap.setCell(14, 14, new Cell());

            var en = new Enemy();
            en.SetPosition(20, 20);
            enemies.Add(en);
            movers.Add(en);

            p = new Player();
            p.SetPosition(10, 10);
            gameCam.FollowPlayer(p);
            movers.Add(p);

            light = new ConeLight();
            light.R = 10;
        }

        public void handleKey(Buttons key)
        {
            if (key == Buttons.Lights) Map.LightsOn = !Map.LightsOn;
            if (p.handleKey(key))
            {
                enemies.ForEach(e => e.takeTurn());
            }
        }

        IntVector mos;
        public void update()
        {
            var status = TCODMouse.getStatus();
            mos.Set(status.CellX, status.CellY);
            light.X = p.X;
            light.Y = p.Y;
            light.setColor(1, 0, .5f);
            light.Direction = gameCam.localCoords(mos).Sub(p.X, p.Y);

            gameCam.update();
        }

        public void draw()
        {
            gameCam.draw();
            CurrentMap.draw(gameCam);
            p.draw(gameCam);
            enemies.ForEach(e => e.draw(gameCam));

            playerCam.draw();
            p.drawInfo(playerCam);

            groundCam.draw();
            var sorty = enemies.OrderBy(e => IntVector.Distance(p.X, p.Y, e.X, e.Y)).ToList();
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
