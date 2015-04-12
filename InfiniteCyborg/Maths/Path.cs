using InfCy.GameCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfCy.Maths
{
    public class Path : List<IntVector>
    {
        private class PathNode : IComparer<PathNode>, IEqualityComparer<PathNode>
        {
            public int X;
            public int Y;
            public float Distance;
            public PathNode Parent;
            public int Depth = 0;

            public int Compare(PathNode x, PathNode y)
            {
                if (x.Distance == y.Distance) return x.Depth.CompareTo(y.Depth);
                return x.Distance > y.Distance ? 1 : -1;
            }

            public override bool Equals(object obj)
            {
                var pew = obj as PathNode;
                return pew.X == this.X && pew.Y == this.Y;
            }

            public override int GetHashCode()
            {
                return X + Y;
            }

            public bool Equals(PathNode x, PathNode y)
            {
                return x.X == y.X && x.Y == y.Y;
            }

            public int GetHashCode(PathNode obj)
            {
                return X + Y;
            }
        }

        public static Path FindPath(IntVector start, IntVector dest, Map block)
        {
            int[] vals = new int[] { -1, 0, 1, 0, -1 };

            Path output = new Path();

            List<PathNode> nodes = new List<PathNode>();
            nodes.Add(new PathNode()
            {
                Distance = start.DistanceSquared(dest),
                X = (int)start.X,
                Y = (int)start.Y
            });

            HashSet<PathNode> visited = new HashSet<PathNode>() { nodes.First() };
            while (nodes.Count > 0 && visited.Count < 150)
            {
                nodes.Sort(nodes.First());
                var node = nodes[0];
                nodes.RemoveAt(0);

                if (!block.Walkable(node.X, node.Y))
                {
                    continue;
                }

                // Expand nodes
                foreach (var next in Directions.Cardinal8.Values)
                {
                    int x = next.X;
                    int y = next.Y;

                    var newNode = new PathNode()
                    {
                        X = x + node.X,
                        Y = y + node.Y,
                        Parent = node,
                        Depth = node.Depth + 1
                    };

                    var end = new IntVector(newNode.X, newNode.Y);
                    newNode.Distance = dest.DistanceSquared(end);

                    if (!visited.Contains(newNode))
                    {
                        visited.Add(newNode);
                        nodes.Add(newNode);
                    }
                    else
                    {
                        continue;
                    }
                }

                //Evaluate node
                if (node.Distance == 0)
                {
                    while (node.Parent != null)
                    {
                        output.Push(new IntVector(node.X, node.Y));
                        node = node.Parent;
                    }

                    break;
                }

            }

            // This is fun to see sometimes.
            //if (output.Count == 0) visited.ToList().ForEach(o => output.Enqueue( new IntVector(o.X, o.Y)));

            return output;
        }

        public void Push(IntVector v)
        {
            this.Insert(0, v);
        }

        public IntVector Pop()
        {
            var output = this[0];
            this.RemoveAt(0);
            return output;
        }
    }
}