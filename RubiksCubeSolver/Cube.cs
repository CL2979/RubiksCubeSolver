using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCubeSolver
{
    // 1 (top or bottom)
    // 2 (front or back)
    // 3 (left or right)
    // move location, location shows the position of the piece relative to the solved cube
    // list structure and order implies 3d physical order on the cube, so move around the lists to rotate pieces
    // location = original locatin and orientation = 0 implies solved state
    public class Cube
    {
        public Cubie[][] cube;
        private string[] corner = { "URF", "UFL", "ULB", "UBR", "DFR", "DLF", "DBL", "DRB" };
        private string[] edge = { "UR", "UF", "UL", "UB", "DR", "DF", "DL", "DB", "FR", "FL", "BL", "BR" };
        private Cubie[] corners;
        private Cubie[] edges;
        private int cLimit = 3; // this is the limit used for mod on corners
        private int eLimit = 2; // mod for edges
        public Cube()
        {
            cube = new Cubie[2][];
            Initialise();
            corners = cube[0];
            edges = cube[1];
        }
        public Cube(Cubie[] c, Cubie[] e) // cloning constructor
        {
            cube = new Cubie[2][];
            corners = new Cubie[c.Length];
            edges = new Cubie[e.Length];
            Initialise();
            for (int i = 0; i < c.Length; i++)
            {
                corners[i] = (Cubie)c[i].Clone();
            }
            for (int i = 0; i < e.Length; i++)
            {
                edges[i] = (Cubie)e[i].Clone();
            }
            cube[0] = corners;
            cube[1] = edges;
        }
        public Cubie[][] GetCube()
        {
            return cube;
        }
        public void PrintPiece() // to confirm all pieces are correctly initialised
        {
            Cubie[] corners = cube[0];
            Cubie[] edges = cube[1];
            for (int i = 0; i < corners.Length; i++)
            {
                Console.WriteLine("Corner: " + corners[i].location + " : " + corners[i].orientation);
            }
            for (int i = 0; i < edges.Length; i++)
            {
                Console.WriteLine("Edge: " + edges[i].location + " : " + edges[i].orientation);
            }
        }
        public void Rotate(char face, int num)
        {
            try
            {
                switch (num)
                {
                    case 1:
                        FunctionAssigner(face, num);
                        break;
                    case 2:
                        for (int i = 0; i < 2; i++)
                            FunctionAssigner(face, num);
                        break;
                    case 3:
                        for (int i = 0; i < 3; i++)
                            FunctionAssigner(face, num);
                        break;
                    case -1:
                        for (int i = 0; i < 3; i++)
                            FunctionAssigner(face, num);
                        break;
                    default:
                        throw new Exception("Invalid entry for direction or face.");
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Please enter a valid face/rotation number.");
            }
        }
        public void Print()
        {
            // do with loop later
            Cubie[] corners = cube[0];
            Cubie[] edges = cube[1];
            Console.WriteLine("White Face: ");
            Console.WriteLine($"|{corners[2].colour1}||{edges[3].colour1}||{corners[3].colour1}|");
            Console.WriteLine($"|{edges[2].colour1}||W||{edges[0].colour1}|");
            Console.WriteLine($"|{corners[1].colour1}||{edges[1].colour1}||{corners[0].colour1}|");
            Console.WriteLine();
            Console.WriteLine("Green Face: ");
            Console.WriteLine($"|{corners[1].colour2}||{edges[1].colour2}||{corners[0].colour2}|");
            Console.WriteLine($"|{edges[9].colour1}||G||{edges[8].colour1}|");
            Console.WriteLine($"|{corners[5].colour2}||{edges[5].colour2}||{corners[4].colour2}|");
            Console.WriteLine();
            Console.WriteLine("Orange Face: ");
            Console.WriteLine($"|{corners[2].colour3}||{edges[2].colour2}||{corners[1].colour3}|");
            Console.WriteLine($"|{edges[10].colour2}||O||{edges[9].colour2}|");
            Console.WriteLine($"|{corners[6].colour3}||{edges[6].colour2}||{corners[5].colour3}|");
            Console.WriteLine();
            Console.WriteLine("Blue Face: ");
            Console.WriteLine($"|{corners[3].colour2}||{edges[3].colour2}||{corners[2].colour2}|");
            Console.WriteLine($"|{edges[11].colour1}||B||{edges[10].colour1}|");
            Console.WriteLine($"|{corners[7].colour2}||{edges[7].colour2}||{corners[6].colour2}|");
            Console.WriteLine();
            Console.WriteLine("Red Face: ");
            Console.WriteLine($"|{corners[0].colour3}||{edges[0].colour2}||{corners[3].colour3}|");
            Console.WriteLine($"|{edges[8].colour2}||R||{edges[11].colour2}|");
            Console.WriteLine($"|{corners[4].colour3}||{edges[4].colour2}||{corners[7].colour3}|");
            Console.WriteLine();
            Console.WriteLine("Yellow Face: ");
            Console.WriteLine($"|{corners[5].colour1}||{edges[5].colour1}||{corners[4].colour1}|");
            Console.WriteLine($"|{edges[6].colour1}||Y||{edges[4].colour1}|");
            Console.WriteLine($"|{corners[6].colour1}||{edges[7].colour1}||{corners[7].colour1}|");
        }
        // {U, D} is element of G1 so orientation is preserved
        private void U(int times)
        {
            // corners
            Cubie c = corners[0];
            corners[0] = corners[3];
            SwapCorner23(0);
            corners[3] = corners[2];
            SwapCorner23(3);
            corners[2] = corners[1];
            SwapCorner23(2);
            corners[1] = c;
            SwapCorner23(1);
            //edges
            Cubie e = edges[0];
            edges[0] = edges[3];
            edges[3] = edges[2];
            edges[2] = edges[1];
            edges[1] = e;
        }
        private void D(int times)
        {
            // corners
            Cubie c = corners[4];
            corners[4] = corners[5];
            SwapCorner23(4);
            corners[5] = corners[6];
            SwapCorner23(5);
            corners[6] = corners[7];
            SwapCorner23(6);
            corners[7] = c;
            SwapCorner23(7);
            //edges
            Cubie e = edges[4];
            edges[4] = edges[5];
            edges[5] = edges[6];
            edges[6] = edges[7];
            edges[7] = e;
        }
        // {F, B} mis orients each edge and corner
        private void F(int times)
        {
            // corners
            Cubie c = corners[0];
            corners[0] = corners[1];
            SwapCorner13(0);
            corners[0].orientation = (corners[0].orientation + 1) % cLimit;
            corners[1] = corners[5];
            SwapCorner13(1);
            corners[1].orientation = (corners[1].orientation + 2) % cLimit;
            corners[5] = corners[4];
            SwapCorner13(5);
            corners[5].orientation = (corners[5].orientation + 1) % cLimit;
            corners[4] = c;
            SwapCorner13(4);
            corners[4].orientation = (corners[4].orientation + 2) % cLimit;
            //edges
            Cubie e = edges[1];
            edges[1] = edges[9];
            SwapEdges(1);
            edges[9] = edges[5];
            SwapEdges(9);
            edges[5] = edges[8];
            SwapEdges(5);
            edges[8] = e;
            SwapEdges(8);
        }
        private void B(int times)
        {
            // corners
            Cubie c = corners[2];
            corners[2] = corners[3];
            SwapCorner13(2);
            corners[2].orientation = (corners[2].orientation + 1) % cLimit;
            corners[3] = corners[7];
            SwapCorner13(3);
            corners[3].orientation = (corners[3].orientation + 2) % cLimit;
            corners[7] = corners[6];
            SwapCorner13(7);
            corners[7].orientation = (corners[7].orientation + 1) % cLimit;
            corners[6] = c;
            SwapCorner13(6);
            corners[6].orientation = (corners[6].orientation + 2) % cLimit;
            //edges
            Cubie e = edges[3];
            edges[3] = edges[11];
            SwapEdges(3);
            edges[11] = edges[7];
            SwapEdges(11);
            edges[7] = edges[10];
            SwapEdges(7);
            edges[10] = e;
            SwapEdges(10);
        }
        // {L, R} preserves edge orientation but mis orients corners
        private void L(int times)
        {
            // corners
            Cubie c = corners[1];
            corners[1] = corners[2];
            SwapCorner12(1);
            corners[1].orientation = (corners[1].orientation + 1) % cLimit;
            corners[2] = corners[6];
            SwapCorner12(2);
            corners[2].orientation = (corners[2].orientation + 2) % cLimit;
            corners[6] = corners[5];
            SwapCorner12(6);
            corners[6].orientation = (corners[6].orientation + 1) % cLimit;
            corners[5] = c;
            SwapCorner12(5);
            corners[5].orientation = (corners[5].orientation + 2) % cLimit;
            //edges
            Cubie e = edges[2];
            edges[2] = edges[10];
            edges[10] = edges[6];
            edges[6] = edges[9];
            edges[9] = e;
        }
        private void R(int times)
        {
            // corners
            Cubie c = corners[0];
            corners[0] = corners[4];
            SwapCorner12(0);
            corners[0].orientation = (corners[0].orientation + 2) % cLimit;
            corners[4] = corners[7];
            SwapCorner12(4);
            corners[4].orientation = (corners[4].orientation + 1) % cLimit;
            corners[7] = corners[3];
            SwapCorner12(7);
            corners[7].orientation = (corners[7].orientation + 2) % cLimit;
            corners[3] = c;
            SwapCorner12(3);
            corners[3].orientation = (corners[3].orientation + 1) % cLimit;
            //edges
            Cubie e = edges[0];
            edges[0] = edges[8];
            edges[8] = edges[4];
            edges[4] = edges[11];
            edges[11] = e;
        }
        // functions for rotating the states of the colours on each cubie
        private void SwapCorner12(int i)
        {
            // swap colours
            Colour temp = corners[i].colour1;
            corners[i].colour1 = corners[i].colour2;
            corners[i].colour2 = temp;
        }
        private void SwapCorner13(int i)
        {
            Colour temp = corners[i].colour1;
            corners[i].colour1 = corners[i].colour3;
            corners[i].colour3 = temp;
        }
        private void SwapCorner23(int i)
        {
            Colour temp = corners[i].colour2;
            corners[i].colour2 = corners[i].colour3;
            corners[i].colour3 = temp;
        }
        private void SwapEdges(int i)
        {
            Colour temp = edges[i].colour1;
            edges[i].colour1 = edges[i].colour2;
            edges[i].colour2 = temp;
            // as edges are only two pieces we can also change the orientation too
            edges[i].orientation = (edges[i].orientation + 1) % eLimit;
        }
        // initialising the cube state
        private void Initialise()
        {
            for (int i = 0; i < 2; i++)
            {
                Cubie[] temp = new Cubie[8 + (4 * i)];
                for (int j = 0; j < temp.Length; j++)
                {
                    if (i == 0)
                    {
                        temp[j] = new Cubie(0, corner[j], corner[j]);
                    }
                    else
                    {
                        temp[j] = new Cubie(0, edge[j], edge[j]);
                    }
                }
                cube[i] = temp;
            }
            for (int i = 0; i < 2; i++) // using priority of colour orientation we can assign colours with loops
            {
                Cubie[] temp = cube[i];
                if (i == 0)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (temp[j].location.Contains('U'))
                        {
                            temp[j].colour1 = Colour.W;
                        }
                        if (temp[j].location.Contains('D'))
                        {
                            temp[j].colour1 = Colour.Y;
                        }
                        if (temp[j].location.Contains('F'))
                        {
                            temp[j].colour2 = Colour.G;
                        }
                        if (temp[j].location.Contains('B'))
                        {
                            temp[j].colour2 = Colour.B;
                        }
                        if (temp[j].location.Contains('L'))
                        {
                            temp[j].colour3 = Colour.O;
                        }
                        if (temp[j].location.Contains('R'))
                        {
                            temp[j].colour3 = Colour.R;
                        }
                    }
                }
                else if (i == 1)
                {
                    for (int j = 0; j < 12; j++)
                    {
                        int check = 0;
                        temp[j].colour3 = Colour.U; // unknown colour 3 defines an edge piece
                        if (temp[j].location.Contains('U'))
                        {
                            temp[j].colour1 = Colour.W;
                            check++;
                        }
                        if (temp[j].location.Contains('D'))
                        {
                            temp[j].colour1 = Colour.Y;
                            check++;
                        }
                        if (temp[j].location.Contains('F'))
                        {
                            if (check > 0) temp[j].colour2 = Colour.G;
                            else if (check == 0) temp[j].colour1 = Colour.G;
                        }
                        if (temp[j].location.Contains('B'))
                        {
                            if (check > 0) temp[j].colour2 = Colour.B;
                            else if (check == 0) temp[j].colour1 = Colour.B;
                        }
                        if (temp[j].location.Contains('L')) temp[j].colour2 = Colour.O;
                        if (temp[j].location.Contains('R')) temp[j].colour2 = Colour.R;
                    }
                }
                else throw new Exception("Face does not exist.");
                cube[i] = temp;
            }
        }
        // takes a char and calls a function with name corresponding to the char
        private void FunctionAssigner(char face, int num)
        {
            Dictionary<char, Action<int>> faceActions = new Dictionary<char, Action<int>>
            {
                {'U', U},
                {'D', D},
                {'F', F},
                {'B', B},
                {'L', L},
                {'R', R}
            };
            if (faceActions.ContainsKey(face))
            {
                Action<int> action = faceActions[face];
                action(num);
            }
            else
            {
                throw new Exception("Invalid face character.");
            }
        }
    }
}
