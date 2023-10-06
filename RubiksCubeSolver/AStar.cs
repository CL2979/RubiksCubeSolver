using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCubeSolver
{
    public class AStar
    {
        public List<string> Search(Dictionary<Node, List<Node>> adjacencyList, Node initial, Node target)
        {
            // temporary heuristic values in node rn
            PRIORITYQUEUE<Node> openList = new PRIORITYQUEUE<Node>(int.MaxValue);
            HashSet<Node> closedList = new HashSet<Node>();
            Dictionary<Node, Node> from = new Dictionary<Node, Node>();
            List<string> path = new List<string>();
            // initialise the heuristic values
            initial.G = 0;
            initial.H = HeuristicFunction(initial, target);
            initial.F = initial.G + initial.H;
            openList.Enqueue(initial, (long)initial.F);
            // initialise the queue
            while (openList.Count() > 0)
            {
                Node currentNode = openList.Dequeue();
                if (currentNode == target) return GetPath(from, currentNode);
                closedList.Add(currentNode);
                foreach (Node neighbor in adjacencyList[currentNode])
                {
                    if (closedList.Contains(neighbor)) continue;
                    double tentativeG = currentNode.G + DistanceBetween(currentNode, neighbor);
                    if (!openList.Contains(neighbor) || tentativeG < neighbor.G)
                    {
                        from[neighbor] = currentNode;
                        neighbor.G = tentativeG;
                        neighbor.H = HeuristicFunction(neighbor, target);
                        neighbor.F = neighbor.G + neighbor.H;
                        if (!openList.Contains(neighbor))
                        {
                            openList.Enqueue(neighbor, (long)neighbor.F);
                        }
                    }
                }
            }
            return null; // base case
        }
        private List<string> GetPath(Dictionary<Node, Node> from, Node current)
        {
            List<string> path = new List<string>();
            while (from.ContainsKey(current))
            {
                path.Insert(0, current.move);
                current = from[current];
            }
            path.Insert(0, current.move);
            return path;
        }
        private double HeuristicFunction(Node node, Node target)
        {
            double edgeDistance = 
            return;
        }
        private double DistanceBetween(Node node1, Node node2)
        {
            return Math.Sqrt(Math.Pow(node1.X - node2.X, 2) + Math.Pow(node1.Y - node2.Y, 2));
        }
    }
}
