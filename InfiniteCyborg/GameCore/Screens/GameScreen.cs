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
        MoveScheduler scheduler = new MoveScheduler();

        internal static GameScreen CurrentGame { get; private set; }
        internal static Map CurrentMap { get; private set; }
        public Player Player { get; private set; }
        Camera gameCam, playerCam, groundCam, logCam;
        public TCODConsole Root { get; private set; }
        ConeLight light;

        public GameScreen(TCODConsole root)
        {
            CurrentGame = this;

            this.Root = root;
            gameCam = new Camera(root, 50, 50) { X = 2, Y = 2, Border = true };
            playerCam = new Camera(root, 40, 16) { X = gameCam.Right + 2, Y = 2, Border = true };
            groundCam = new Camera(root, 40, 30) { X = gameCam.Right + 2, Y = playerCam.Bottom + 2, Border = true };
            logCam = new Camera(root, 50, 20) { X = 2, Y = gameCam.Bottom + 2, Border = true };

            Map.LightsOn = true;
            CurrentMap = new Map(64, 64);
            Room.Rectangle(0, 0, 64, 64, CurrentMap);

            CurrentMap.setCell(12, 12, new Cell());
            CurrentMap.setCell(12, 13, new Cell());
            CurrentMap.setCell(13, 13, new Cell());
            CurrentMap.setCell(14, 14, new Cell());
            CurrentMap.Items.Add(new Component(new BitField(Component.BitCount).Randomize(() => TCODRandom.getInstance().getFloat(0, 1) > .5f)) { Name = "test", X = 15, Y = 16 });

            var en = new Enemy();
            en.SetPosition(20, 20);
            scheduler.Add(en);

            Player = new Player();
            Player.SetPosition(10, 10);
            gameCam.FollowPlayer(Player);
            scheduler.Add(Player);

            light = new ConeLight();
        }

        public void HandleKey(KeyEvent key)
        {
            if (AnimationManager.Instance.Animating) return;
            if (scheduler.HandleKey(key)) return;

            switch (key.button)
            {
                case Buttons.Lights:
                    Map.LightsOn = !Map.LightsOn;
                    break;
            }
        }

        IntVector mos;
        Path path;
        public void Update(float dt)
        {
            scheduler.Update(dt);

            var status = TCODMouse.getStatus();
            mos.Set(status.CellX, status.CellY);
            light.X = Player.X;
            light.Y = Player.Y;
            light.Direction = gameCam.localCoords(mos).Sub(Player.X, Player.Y);

            path = Path.FindPath(Player.Position, mos.Sub(gameCam.X + 2, gameCam.Y + 2).Add(gameCam.OffX, gameCam.OffY), CurrentMap);
            if (status.LeftButtonPressed)
            {
                Player.SetTarget(path);
            }

            gameCam.update();
            AnimationManager.Instance.Update(dt);
        }

        public void Draw()
        {
            gameCam.draw();
            CurrentMap.draw(gameCam);

            if (path != null)
            {
                using (TCODColor color = new TCODColor(0, 0, 255))
                {
                    foreach (var p in path)
                    {
                        gameCam.setCharBackColor(p.X, p.Y, color, TCODBackgroundFlag.Add);
                    }
                }
            }

            scheduler.Draw(gameCam);
            
            playerCam.draw();
            Player.DrawInfo(playerCam, 1);

            groundCam.draw();
            var sorty = scheduler.SelectByDemeanor(~Hostile.Friendly).Cast<IDrawable>().Union(CurrentMap.Items).OrderBy(e => IntVector.Distance(Player.X, Player.Y, e.X, e.Y)).ToList();
            for (int i = 0; i < sorty.Count; ++i)
            {
                sorty[i].DrawInfo(groundCam, i * 2 + 1);
            }

            light.draw(gameCam);

            logCam.draw();
            Logger.DrawLog(logCam);

            // DEBUG
            Root.setForegroundColor(TCODColor.white);

            mos = gameCam.localCoords(mos);
            Root.print(0, 1, string.Format("x:{0}\ty:{1}", mos.X, mos.Y));
        }

        internal Mover[] FindEnemies(Mover attacker, Weapon weapon, int destx, int desty)
        {
            var candidates = from e in scheduler.Movers
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

        internal void RemoveMover(Mover e)
        {
            scheduler.Remove(e);
        }

        internal bool Walkable(int x, int y)
        {
            return CurrentMap.Walkable(x, y) && scheduler.Movers.All(e => e.X != x || e.Y != y);
        }
    }
}
