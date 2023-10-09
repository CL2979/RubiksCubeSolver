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
        public List<string> Search(Node start, Node target)
        {
            List<string> solution = new List<string>();
            Node current = target;
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
