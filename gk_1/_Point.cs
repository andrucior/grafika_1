using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gk_1
{
    public class MyPoint
    {
        // Properties for X and Y coordinates.
        public Point? Point { get; set; }
        public bool Vertical { get; set; } 
        public bool Horizontal { get; set; }
        public  bool Fixed { get; set; }
        public bool Hovered { get; set; }
        public bool G1 { get; set; }
        public bool C1 { get; set; }
        public (double a, double b)? Direction { get; set; }
        // Constructor to initialize the point.
        public MyPoint(int x, int y, bool horizontal = false, bool vertical = false, bool _fixed = false, (double a, double b)? direction = null)
        {
            Point = new Point(x, y);
            Horizontal = horizontal;
            Vertical = vertical;
            Fixed = _fixed;
            Direction = direction;
        }
        public MyPoint(Point point)
        {
            this.Point = point;
        }
        public MyPoint() { }        
    }
}
