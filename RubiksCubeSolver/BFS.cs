using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCubeSolver
{
    public class BFS
    {
        // partially broken for some reason, too slow to bother fixing
        public List<string> Search(Dictionary<Node, List<Node>> adjacencyList, Node start, Node target)
        {
            HashSet<Node> visited = new HashSet<Node>();
            PRIORITYQUEUE<Node> queue = new PRIORITYQUEUE<Node>(int.MaxValue); // change later after testing
            queue.Enqueue(start, 0);
            Dictionary<Node, Node> parentMap = new Dictionary<Node, Node>();
            while (!queue.IsEmpty())
            {
                Node currentNode = queue.Dequeue();
                if (currentNode.state == target.state) return GetPath(target, parentMap);
                if (!visited.Contains(currentNode))
                {
                    visited.Add(currentNode);
                    if (adjacencyList.ContainsKey(currentNode))
                    {
                        foreach (Node neighbor in adjacencyList[currentNode])
                        {
                            if (!visited.Contains(neighbor))
                            {
                                queue.Enqueue(neighbor, 1);
                                parentMap[neighbor] = currentNode;
                            }
                        }
                    }
                }
            }
            Console.WriteLine("Something went wrong.");
            return null;
        }
        private static List<string> GetPath(Node node, Dictionary<Node, Node> parentMap)
        {
            List<string> path = new List<string>();
            while (parentMap.ContainsKey(node))
            {
                Node parent = parentMap[node];
                path.Insert(0, node.move);
                node = parent;
            }
            return path;
        }
    }
}
