using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RubiksCubeSolver
{
    // one instance of Graph for phase 1 and another instance for phase 2
    // make it such that if a node already exists in the graph, it is linked and created as one node
    // this means that we get a graph not a tree and will be easier to search through 
    // we will also be more likely to find a better solution that way
    public class Graph
    {
        private List<Node> graph;
        private Queue<Node> queue;
        private char[] moves = { 'U', 'D', 'F', 'B', 'L', 'R' };
        private int[] directions = { -1, 1, 2 };
        private string[] G1moves = { "R2", "L2", "F2", "B2", "U1", "U'", "U2", "D1", "D'", "D2" };
        private Cube initialState;
        private Node root;
        private long count;
        public Graph(Cube initialState)
        {
            root = new Node(null, initialState, "");
            graph = new List<Node>();
            queue = new Queue<Node>(50000000); // change later
            graph.Add(root);
            queue.Enqueue(root);
            this.initialState = initialState;
            count = 0;
        }
        public Node GeneratePhase1()
        {
            Node current = queue.Peek(); // this will be kept constant while temp changes
            if (Phase1Complete(current.state)) return current;
            bool end = false;
            while (!end)
            {
                current = queue.Dequeue();
                for (int i = 0; i < moves.Length; i++)
                {
                    for (int j = 0; j < directions.Length; j++)
                    {
                        if (current.move.Contains(moves[i])) break;
                        Node temp = DeepCopy(current);
                        temp.parent = current;
                        temp.state.Rotate(moves[i], directions[j]);
                        temp.move = Combine(moves[i], directions[j]);
                        if (Phase1Complete(temp.state))
                        {
                            Console.WriteLine(count + " nodes generated");
                            return temp;
                        }
                        queue.Enqueue(temp);
                        graph.Add(temp);
                        count++;
                    }
                }
            }
            return null;
        }
        public Node GeneratePhase2()
        {
            Node current = queue.Peek(); // this will be kept constant while temp changes
            if (Phase2Complete(current.state)) return current;
            bool end = false;
            while (!end)
            {
                current = queue.Dequeue();
                for (int i = 0; i < moves.Length; i++)
                {
                    for (int j = 0; j < directions.Length; j++)
                    {
                        if (current.move.Contains(moves[i])) break;
                        Node temp = DeepCopy(current);
                        temp.parent = current;
                        temp.state.Rotate(moves[i], directions[j]);
                        temp.move = Combine(moves[i], directions[j]);
                        if (Phase2Complete(temp.state))
                        {
                            Console.WriteLine(count + " nodes generated");
                            return temp;
                        }
                        queue.Enqueue(temp);
                        graph.Add(temp);
                        count++;
                    }
                }
            }
            return null;
        }
        public List<Node> GetGraph()
        {
            return graph;
        }
        public Node GetFirst()
        {
            return root;
        }
        private bool Phase1Complete(Cube state)
        {
            Kociemba kociemba = new Kociemba(state);
            double[] d = kociemba.Phase1();
            if (d[0] == 0 && d[1] == 0 && d[2] == 0) return true;
            return false;
        }
        private bool Phase2Complete(Cube state)
        {
            Kociemba kociemba = new Kociemba(state);
            double[] d = kociemba.Phase2();
            if (d[0] == 0 && d[1] == 0 && d[2] == 0) return true;
            return false;
        }
        private Node DeepCopy(Node current)
        {
            Cubie[] tempcorner = current.state.GetCube()[0];
            Cubie[] tempedge = current.state.GetCube()[1];
            Node temp = new Node(current, new Cube(tempcorner, tempedge), "");
            return temp;
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
        private string[] DeCombine(string s)
        {
            string[] result = new string[2];
            result[0] = s[0].ToString();
            if (s[1] == '\'') result[1] = "-1";
            if (s[1] == '2') result[1] = "2";
            if (s[1] == '1') result[1] = "1";
            return result;
        }
    }
}
