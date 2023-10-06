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
        private PRIORITYQUEUE<Node> queue;
        private char[] moves = { 'U', 'D', 'F', 'B', 'L', 'R' };
        private int[] directions = { -1, 1, 2 };
        private string[] G1moves = { "R2", "L2", "F2", "B2", "U1", "U'", "U2", "D1", "D'", "D2" };
        private Cube initialState;
        private Node root;
        private long count;
        public Graph(Cube initialState)
        {
            root = new Node(null, initialState, "-");
            adjacencyList = new Dictionary<Node, List<Node>>();
            queue = new PRIORITYQUEUE<Node>(int.MaxValue); // change later
            adjacencyList.Add(root, new List<Node>());
            queue.Enqueue(root, 1);
            this.initialState = initialState;
            count = 0;
        }
        public Node GeneratePhase1()
        {
            Node returnNode = new Node(null, null, "");
            List<Node> children = new List<Node>();
            Node current = queue.Peek(); // this will be kept constant while temp changes
            // creating deep copies
            Node temp = DeepCopy(current);
            // now node temp is independent from current
            if (Phase1Complete(current.state)) return current;
            bool end = false;
            while (!end)
            {
                current = queue.Dequeue();
                for (int i = 0; i < moves.Length; i++)
                {
                    for (int j = 0; j < directions.Length; j++)
                    {
                        temp.state.Rotate(moves[i], directions[j]);
                        temp.move = Combine(moves[i], directions[j]);
                        if (Phase1Complete(temp.state))
                        {
                            returnNode = temp;
                            end = true;
                        }
                        queue.Enqueue(temp, 1);
                        children.Add(temp);
                        temp = DeepCopy(current);
                        count++;
                    }
                }
                Add(current, children);
            }
            Console.WriteLine(count + " nodes generated");
            return returnNode;
        }
        public Node GeneratePhase2()
        {
            Node returnNode = new Node(null, null, "");
            List<Node> children = new List<Node>();
            Node current = queue.Peek(); // this will be kept constant while temp changes
            // creating deep copies
            Node temp = DeepCopy(current);
            // now node temp is independent from current
            if (Phase2Complete(current.state)) return current;
            bool end = false;
            while (!end)
            {
                current = queue.Dequeue();
                for (int i = 0; i < G1moves.Length; i++)
                {
                    string[] temporary = DeCombine(G1moves[i]);
                    char move = char.Parse(temporary[0]);
                    int direction = int.Parse(temporary[1]);
                    temp.state.Rotate(move, direction);
                    temp.move = move + direction.ToString();
                    if (Phase2Complete(temp.state))
                    {
                        returnNode = temp;
                        end = true;
                    }
                    queue.Enqueue(temp, 1);
                    children.Add(temp);
                    temp = DeepCopy(current);
                    count++;
                }
                Add(current, children);
            }
            Console.WriteLine(count + " nodes generated");
            return returnNode;
        }
        public Dictionary<Node, List<Node>> GetGraph()
        {
            return adjacencyList;
        }
        public Node GetFirst()
        {
            return root;
        }
        private void Add(Node parent, List<Node> children)
        {
            if (adjacencyList.ContainsKey(parent))
            {
                List<Node> existingChildren = adjacencyList[parent];
                foreach (Node child in children.ToList())
                {
                    if (!existingChildren.Contains(child))
                    {
                        existingChildren.Add(child);
                    }
                    else Update(existingChildren, child);
                }
            }
            else adjacencyList[parent] = children;
        }
        private void Update(List<Node> existingChildren, Node child)
        {
            int index = existingChildren.IndexOf(child);
            if (index != -1) existingChildren[index] = child;
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
