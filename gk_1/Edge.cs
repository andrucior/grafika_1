using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gk_1
{
    internal class Edge
    {
        public MyPoint? Start { get; set; }
        public MyPoint? End { get; set; }
        public Edge(MyPoint start, MyPoint end) 
        { 
            Start = start; 
            End = end;
            Vertical = false;
            Horizontal = false;
        }
        public Edge(MyPoint start, MyPoint end, bool vertical, bool horizontal, double? fixedLength = null) : this(start, end)
        {
            Vertical = vertical;
            Horizontal = horizontal;
            FixedLength = fixedLength;
        }

        public Edge()
        {
        }

        public bool Vertical;
        public bool Horizontal;
        public double? FixedLength;
        public bool G0, G1, C1;
    }
    internal class SkewedEdge: Edge
    {
        public MyPoint? First, Second;
        public SkewedEdge(MyPoint start, MyPoint end, MyPoint first, MyPoint second): base(start, end) 
        { First = first; Second = second; }
    }
}
