using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCubeSolver
{
    public class BackTrack
    {
        public List<string> Search(Node node, Graph ingraph)
        {
            List<Node> graph = ingraph.GetGraph();
            List<string> solution = new List<string>();
            Cube cube = node.state;
            Node current = null;
            foreach (Node node1 in graph)
            {
                if (node1.state.Equals(cube)) current = node1;
            }
            while (current != null)
            {
                solution.Add(current.move);
                current = current.parent;
            }
            solution.Reverse();
            return solution;
        }
    }
}
