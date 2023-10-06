using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCubeSolver
{
    public class Kociemba
    {
        Cubie[] Corners;
        Cubie[] Edges;
        public Kociemba(Cube cube)
        {
            Corners = cube.GetCube()[0];
            Edges = cube.GetCube()[1];
        }
        public double[] Phase1()
        {
            return new double[] { COCoordinate(), EOCoordinate(), UDSliceCoordinate() };
        }
        public double[] Phase2()
        {
            return new double[] { CPCoordinate(), EPCoordinate(), UDSliceCoordinate2() };
        }
        private double UDSliceCoordinate()
        {
            string[] edges = new string[13];
            bool start = false;
            double s = 0;
            int k = -1;
            int c = 0;
            foreach (Cubie edge in Edges)
            {
                edges[c++] = edge.location;
            }
            string[] UDSliceEdges = { "FR", "FL", "BL", "BR" };
            for (int n = 0; n < 12; n++)
            {
                if (UDSliceEdges.Contains(edges[n]))
                {
                    start = true;
                    k++;
                }
                else if (!UDSliceEdges.Contains(edges[n]) && start)
                {
                    s += Choose(n, k);
                }
            }
            return s;
        }
        private double UDSliceCoordinate2()
        {
            string[] edgesInitial = new string[12];
            string[] edges = new string[4];
            double s = 0;
            int k = -1;
            int c = 0;
            string[] UDSliceEdges = { "FR", "FL", "BL", "BR" };
            foreach (Cubie edge in Edges)
            {
                if (UDSliceEdges.Contains(edge.location))
                {
                    edgesInitial[c++] = edge.location;
                }
            }
            for (int n = 0; n < 12; n++)
            {
                if (UDSliceEdges.Contains(edgesInitial[n]))
                {
                    k++;
                    edges[k] = edgesInitial[n];
                }
                else s += Choose(n, k);
            }
            int x = 0;
            for (int j = 3; j >= 1; j--)
            {
                int sCount = 0;
                for (int i = j - 1; i >= 0; i--)
                {
                    if (Array.IndexOf(edges, UDSliceEdges[i]) > Array.IndexOf(edges, UDSliceEdges[j])) sCount++;
                }
                x = (x + sCount) * j;
            }
            return (UDSliceCoordinate() * 24) + x;
        }
        private double COCoordinate()
        {
            int[] orientations = new int[8];
            int c = 0;
            foreach (Cubie corner in Corners)
            {
                orientations[c++] = corner.orientation;
            }
            string s = "";
            for (int i = 0; i < 7; i++)
            {
                s += orientations[i];
            }
            return Bases(s, 3);
        }
        private double CPCoordinate()
        {
            int[] s = new int[8];
            string[] corners = { "URF", "UFL", "ULB", "UBR", "DFR", "DLF", "DBL", "DRB" };
            string[] cornerCurrent = new string[8];
            int c = 0;
            foreach (Cubie corner in Corners)
            {
                cornerCurrent[c++] = corner.location;
            }
            Dictionary<string, int> naturalOrder = new Dictionary<string, int>();
            for (int i = 0; i < 8; i++)
            {
                naturalOrder.Add(corners[i], i);
            }
            int k = 0;
            foreach (string corner in cornerCurrent)
            {
                int count = 0;
                int naturalIndex = naturalOrder[corner];
                int index = Array.IndexOf(cornerCurrent, corner);
                for (int j = 0; j < Array.IndexOf(cornerCurrent, corner); j++)
                {
                    if (naturalOrder[cornerCurrent[j]] > naturalIndex && Array.IndexOf(cornerCurrent, cornerCurrent[j]) < index)
                    {
                        count++;
                    }
                }
                s[k] = count;
                k++;
            }
            return VariableBase(s);
        }
        private double EOCoordinate()
        {
            int[] orientations = new int[12];
            int c = 0;
            foreach (Cubie edge in Edges)
            {
                orientations[c++] = edge.orientation;
            }
            string s = "";
            for (int i = 0; i < 11; i++)
            {
                s += orientations[i];
            }
            // calculate Y coordinate here
            return Bases(s, 2);
        }
        private double EPCoordinate()
        {
            int[] s = new int[12];
            string[] edges = { "UR", "UF", "UL", "UB", "DR", "DF", "DL", "DB", "FR", "FL", "BL", "BR" };
            string[] edgeCurrent = new string[12];
            int c = 0;
            foreach (Cubie edge in Edges)
            {
                edgeCurrent[c++] = edge.location;
            }
            Dictionary<string, int> naturalOrder = new Dictionary<string, int>();
            for (int i = 0; i < 12; i++)
            {
                naturalOrder.Add(edges[i], i);
            }
            int k = 0;
            foreach (string edge in edgeCurrent)
            {
                int count = 0;
                int naturalIndex = naturalOrder[edge];
                int index = Array.IndexOf(edgeCurrent, edge);
                for (int j = 0; j < Array.IndexOf(edgeCurrent, edge); j++)
                {
                    if (naturalOrder[edgeCurrent[j]] > naturalIndex && Array.IndexOf(edgeCurrent, edgeCurrent[j]) < index)
                    {
                        count++;
                    }
                }
                s[k] = count;
                k++;
            }
            // search alg calulations
            // calculate X here
            return VariableBase(s);
        }
        private double Choose(int n, int k)
        {
            return Factorial(n) / (Factorial(k) * Factorial(n - k));
        }
        private double Factorial(int x)
        {
            if (x == 0 || x == 1) return 1;
            int y = 1;
            for (int i = 1; i < x + 1; i++)
            {
                y *= i;
            }
            return y;
        }
        private double VariableBase(int[] digits)
        {
            double t = 0;
            for (int i = 1; i < digits.Length; i++)
            {
                t += digits[i] * (Factorial(i));
            }
            return t;
        }
        private double Bases(string input, int num)
        {
            double t = 0;
            int[] digits = input.Select(o => Convert.ToInt32(o) - 48).ToArray();
            for (int i = 0; i < digits.Length; i++)
            {
                t += digits[i] * (Math.Pow(num, digits.Length - 1 - i));
            }
            return t;
        }
    }
}
