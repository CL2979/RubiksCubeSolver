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
                orientation = orientation,
                location = location,
                originalLocation = originalLocation,
                colour1 = colour1,
                colour2 = colour2,
                colour3 = colour3
            };
            return copy;
        }
    }
    public class Node
    {
        public Node parent { get; set; }
        public Cube state { get; set; }
        public string move { get; set; }
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
