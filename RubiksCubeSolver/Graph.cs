using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCubeSolver
{
    // one instance of Graph for phase 1 and another instance for phase 2
    // make it such that if a node already exists in the graph, it is linked and created as one node
    // this means that we get a graph not a tree and will be easier to search through 
    // we will also be more likely to find a better solution that way
    public class Graph
    {
        public Dictionary<Node, List<Node>> adjacencyList;
        private char[] moves = { 'U', 'D', 'F', 'B', 'L', 'R' };
        private int[] directions = { -1, 1, 2 };
        private Cube initialState;
        public Graph(Cube initialState)
        {
            Node root = new Node(null, initialState, "-");
            adjacencyList = new Dictionary<Node, List<Node>>();
            adjacencyList.Add(root, new List<Node>());
            this.initialState = initialState;
        }
        // GC.Collect();
        // GC.WaitForPendingFinalizers();
        public void GenerateTreePhase1()
        {
            List<Node> children = new List<Node>();
            Node current = adjacencyList.Last().Key;
            // creating deep copies
            Cubie[] tempcorner = current.state.GetCube()[0];
            Cubie[] tempedge = current.state.GetCube()[1];
            //
            Node temp = new Node(current, new Cube(tempcorner, tempedge), "");
            current.state.Print();
            Console.WriteLine("-------");
            temp.state.Rotate('U', 1);
            temp.state.Print();
            Console.WriteLine("-------");
            current.state.Print(); // finish logic
        }
        public Dictionary<Node, List<Node>> GetGraph()
        {
            return adjacencyList;
        }
        private void Coordinates(Cube state)
        {
            Kociemba solve = new Kociemba(state);
            double[] phase1 = solve.Phase1();
            foreach (double part in phase1)
            {
                Console.Write(part + ", ");
            }
            Console.WriteLine("\n-------------");
        }
        private string Combine(char c, int i)
        {
            if (i == -1) return c.ToString() + "'";
            if (i == 1) return c.ToString();
            if (i == 2) return c.ToString() + "2";
            throw new Exception("Move does not exist.");
        }
    }
}
