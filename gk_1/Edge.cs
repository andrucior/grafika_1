using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace gk_1
{
    [Serializable]
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
        public bool G1, C1;
        public virtual string Serialize()
        {
            return JsonSerializer.Serialize(this);
        }
    }
    [Serializable]
    internal class SkewedEdge : Edge
    {
        public MyPoint? First {get; set;}
        public MyPoint? Second { get; set;}
        public SkewedEdge(MyPoint start, MyPoint end, MyPoint first, MyPoint second): base(start, end) 
        { First = first; Second = second; }
        public override string Serialize()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
