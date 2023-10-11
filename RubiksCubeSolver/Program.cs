using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        static void Main(string[] args)
        {
            Cube cube = new Cube();
            cube.Rotate('L', 2);
            cube.Rotate('R', 2);
            cube.Rotate('F', 2);
            cube.Rotate('B', 2);
            cube.Rotate('U', 2);
            cube.Rotate('D', 2);
            //cube.Rotate('F', 2);
            var start = DateTime.Now;
            Console.WriteLine("Phase 1: ");
            Graph graph1 = new Graph(cube);
            Node endof1 = graph1.GeneratePhase1();
            BackTrack search1 = new BackTrack();
            List<string> stage1solution = search1.Search(graph1.GetFirst(), endof1);
            var end = DateTime.Now;
            Console.WriteLine("Stage one in: " + (end - start).TotalSeconds + " seconds.");
            Console.WriteLine();

            start = DateTime.Now;
            Console.WriteLine("Phase 2: "); // multithreading the second stage as we have a goal state
            Graph graph2 = new Graph(endof1.state);
            Node endof2 = graph2.GeneratePhase2();
            BackTrack search2 = new BackTrack();
            List<string> stage2solution = search2.Search(graph2.GetFirst(), endof2);
            //List<string> stage2solution = await FindCommonNode(endof1.state, endof1);
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
        private static async Task<List<Node>> FindCommonNodePhase2(Node endof1) // this function generates two graphs and checks for common node
        {
            return null;
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
