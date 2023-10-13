using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RubiksCubeSolver
{
    // try old pochman blind method using location and original location on each cubie
    // combine list function is truncating section 2 at the end
    // work on the multithreading thing
    // new idea for multithreading: 
    // make the generate function for 2nd phase go only once, then return each node, check for equality then repeat and put while loop in thread generation part
    // also multithread the generation of the 1st phase, generate the 18 initial nodes then assign a thread to two branches as most cpus dont have 18 threads
    // figure out if pruning is possible, but only after solutions can be generates within a reasonable time on multiple threads
    internal class Program
    {
        // R U L2 F2 B2 U2 works in 84 seconds (2.7GB memory)
        // did the checkerboard pattern in 94 seconds (2.6GB memory)
        static async Task Main(string[] args)
        {
            Cube solved = new Cube();
            Cube cube = new Cube();
            cube.Rotate('L', 2);
            cube.Rotate('R', 2);
            cube.Rotate('F', 2);
            cube.Rotate('B', 2);
            cube.Rotate('U', 2);
            cube.Rotate('D', 2);
            var start = DateTime.Now;
            Console.WriteLine("Phase 1: ");
            Graph graph1 = new Graph(cube);
            Node endof1 = graph1.GeneratePhase1();
            BackTrack search1 = new BackTrack();
            List<string> stage1solution = search1.Search(graph1.GetFirst(), graph1);
            var end = DateTime.Now;
            Console.WriteLine("Stage one in: " + (end - start).TotalSeconds + " seconds.");
            Console.WriteLine();

            start = DateTime.Now;
            Console.WriteLine("Phase 2: "); // multithreading the second stage as we have a goal state
            /*Graph graph2 = new Graph(endof1.state);
            Node endof2 = graph2.GeneratePhase2();
            BackTrack search2 = new BackTrack();
            List<string> stage2solution = search2.Search(graph2.GetFirst(), endof2);
            //List<string> stage2solution = await FindCommonNode(endof1.state, endof1);*/
            List<string> stage2solution = await InitThreads2(endof1, solved);
            end = DateTime.Now;
            Console.WriteLine("Stage two in: " + (end - start).TotalSeconds + " seconds.");
            Console.WriteLine();
            Console.WriteLine("Solution: ");
            List<string> final = Simplify(stage1solution, stage2solution);
            for (int i = 0; i < final.Count; i++)
            {
                Console.Write(final[i] + (i < final.Count - 1 ? ", " : ""));
            }
            Console.ReadKey();
        }
        private static async Task<Node> InitThreads1(Cube solved) // only generating from one direction due to lack of target node
        {
            return null;
        }
        private static object lockObject = new object();
        private static async Task<List<string>> InitThreads2(Node endof1, Cube solved)
        {
            Node solvedState = new Node(null, solved, "");
            Graph initialGraph = new Graph(endof1.state);
            Graph finalGraph = new Graph(solved);
            Task<Node> thread1 = Task.Run(() => GeneratePhase2Parallel(initialGraph));
            Task<Node> thread2 = Task.Run(() => GeneratePhase2Parallel(finalGraph));
            Node commonNode = null;
            Task<Node> completed = await Task.WhenAny(thread1, thread2);
            if (completed.Result != null)
            {
                Node temp = completed.Result;
                if (Phase2Complete(temp.state))
                {
                    commonNode = completed.Result;
                }
            }
            if (commonNode != null && Phase2Complete(commonNode.state))
            {
                List<string> path1 = Backtracker(commonNode, initialGraph);
                List<string> path2 = Backtracker(commonNode, finalGraph);
                path2.Reverse();
                path1.AddRange(path2);
                return path1;
            }
            return null;
        }
        private static Node GeneratePhase2Parallel(Graph graph)
        {
            Node node = new Node(null, null, "");
            lock (lockObject)
            {
                node = graph.GeneratePhase2();
            }
            return node;
        }
        private static bool Phase1Complete(Cube state)
        {
            if (state == null) return false;
            Kociemba kociemba = new Kociemba(state);
            double[] d = kociemba.Phase1();
            if (d[0] == 0 && d[1] == 0 && d[2] == 0) return true;
            return false;
        }
        private static bool Phase2Complete(Cube state)
        {
            if (state == null) return false;
            Kociemba kociemba = new Kociemba(state);
            double[] d = kociemba.Phase2();
            if (d[0] == 0 && d[1] == 0 && d[2] == 0) return true;
            return false;
        }
        private static List<string> Backtracker(Node node, Graph graph)
        {
            BackTrack search = new BackTrack();
            List<string> list = search.Search(node, graph);
            return list;
        }
        private static List<string> Simplify(List<string> part1, List<string> part2)
        {
            List<string> moveList = new List<string>();
            List<string> simplifiedList = new List<string>();
            moveList.AddRange(part1);
            moveList.AddRange(part2);
            foreach (string part in moveList.ToList())
            {
                if (part == "") moveList.Remove(part);
            }
            Console.WriteLine();
            int i = 0;
            while (i < moveList.Count - 1)
            {
                string currentMove = moveList[i];
                string nextMove = moveList[i + 1];
                char currentDirection = directionAllocater(currentMove);
                char nextDirection = directionAllocater(nextMove);
                if (currentMove[0] == nextMove[0])
                {
                    if (currentDirection == 1 && nextDirection == 1)
                    {
                        simplifiedList.Add(currentMove + 2);
                        moveList.RemoveAt(i + 1);
                    }
                    else if (currentDirection == 2 && nextDirection == 2)
                    {
                        moveList.RemoveAt(i + 1);
                    }
                    else if ((currentDirection == 1 && nextDirection == 2) || (currentDirection == 2 && nextDirection == 1))
                    {
                        simplifiedList.Add(currentMove + "'");
                        moveList.RemoveAt(i + 1);
                    }
                    else if ((currentDirection == '\'' && nextDirection == 2) || (currentDirection == 2 && nextDirection == '\''))
                    {
                        simplifiedList.Add(currentMove);
                        moveList.RemoveAt(i + 1);
                    }
                    else if ((currentDirection == 1 && nextDirection == '\'') || (currentDirection == '\'' && nextDirection == 1))
                    {
                        moveList.RemoveAt(i + 1);
                    }
                    else simplifiedList.Add(currentMove);
                    i++;
                }
                else
                {
                    simplifiedList.Add(currentMove);
                    i++;
                }
                if (i == moveList.Count - 1) simplifiedList.Add(nextMove);
            }
            Console.WriteLine();
            return simplifiedList;
        }
        private static char directionAllocater(string move)
        {
            char direction = ' ';
            if (move.Count() == 1) direction = '1';
            else direction = move[1];
            return direction;
        }
        private static void SuperFlip(Cube cube)
        {
            cube.Rotate('U', 1);
            cube.Rotate('R', 2);
            cube.Rotate('F', 1);
            cube.Rotate('B', 1);
            cube.Rotate('R', 1);
            cube.Rotate('B', 2);
            cube.Rotate('R', 1);
            cube.Rotate('U', 2);
            cube.Rotate('L', 1);
            cube.Rotate('B', 2);
            cube.Rotate('R', 1);
            cube.Rotate('U', -1);
            cube.Rotate('D', -1);
            cube.Rotate('R', 2);
            cube.Rotate('F', 1);
            cube.Rotate('R', -1);
            cube.Rotate('L', 1);
            cube.Rotate('B', 2);
            cube.Rotate('U', 2);
            cube.Rotate('F', 2);
        }
        private static void Sexy(Cube cube)
        {
            cube.Rotate('R', 1);
            cube.Rotate('U', 1);
            cube.Rotate('R', -1);
            cube.Rotate('U', -1);
        }
    }
}
