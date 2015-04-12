using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfCy.GameCore
{
    public class MoveScheduler
    {
        public const float SecsPerTurn = .1f;
        private float turnTimer = 0;
        Mover lastMover;

        private List<Mover> movers = new List<Mover>();
        public List<Mover> Movers { get { return movers.ToList(); } }

        public void Add(Mover m)
        {
            movers.Add(m);
        }

        public void Update(float dt)
        {
            turnTimer += dt;
            if (turnTimer > SecsPerTurn)
            {
                CurrentMover.DoTurn();
            }
        }

        public Mover CurrentMover
        {
            get
            {
                movers.Sort(TurnComparer.instance);
                var output = movers.First();
                if (lastMover != output)
                {
                    turnTimer = 0;
                    lastMover = output;
                }

                if (output.Timer < output.Duration)
                {
                    var diff = Math.Max(5, output.Duration - output.Timer);
                    foreach (var m in movers)
                    {
                        m.Timer += diff;
                    }
                }

                return output;
            }
        }

        private class TurnComparer : IComparer<Mover>
        {
            public readonly static TurnComparer instance = new TurnComparer();

            public int Compare(Mover x, Mover y)
            {
                return (x.Duration - x.Timer).CompareTo(y.Duration - y.Timer);
            }
        }

        internal bool HandleKey(KeyEvent key)
        {
            if (turnTimer > SecsPerTurn)
            {
                Player p = CurrentMover as Player;
                if (p != null)
                {
                    if (p.HandleKey(key))
                    {
                        this.turnTimer = 0;
                        return true;
                    }
                }
            }

            return false;
        }

        public void Draw(Camera gameCam)
        {
            movers.ForEach(e => e.Draw(gameCam));
        }

        internal IEnumerable<Mover> SelectByDemeanor(Hostile p)
        {
            return this.movers.Where(c => (c.Demeanor & p) != 0);
        }

        internal void Remove(Mover e)
        {
            this.movers.Remove(e);
        }
    }
}
