using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCubeSolver
{
    public class Cubie : ICloneable
    {
        // pull all of these out seperately and copy using a new constructor
        public int orientation { get; set; }
        public string location { get; set; }
        public string originalLocation { get; set; }
        public Colour colour1 { get; set; }
        public Colour colour2 { get; set; }
        public Colour colour3 { get; set; }
        public Cubie(int orientation, string location, string originalLocation)
        {
            this.orientation = orientation;
            this.location = location;
            this.originalLocation = originalLocation;
        }
        public object Clone()
        {
            Cubie copy = new Cubie(orientation, location, originalLocation)
            {
                orientation = this.orientation,
                location = this.location,
                originalLocation = this.originalLocation,
                colour1 = this.colour1,
                colour2 = this.colour2,
                colour3 = this.colour3
            };
            return copy;
        }
    }
    public class Node
    {
        public Node parent { get; set; }
        public Cube state { get; set; }
        public string move { get; set; }
        public int depth { get; set; }
        // heuristic calculation values
        public double H { get; set; }
        public double G { get; set; }
        public double F { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public Node(Node parent, Cube state, string move)
        {
            this.move = move;
            this.parent = parent;
            this.state = state;
        }
    }
    public enum Colour
    {
        W, G, O, B, R, Y, U
    }
}
