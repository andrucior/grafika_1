using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gk_1
{
    internal class Edge
    {
        public Point Start, End;
        public Edge(Point start, Point end) { Start = start; End = end; }
    }
    internal class SkewedEdge: Edge
    {
        private Point First, Second;
        public SkewedEdge(Point start, Point end, Point first, Point second): base(start, end) 
        { First = first; Second = second; }
    }
}
