using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Formats.Asn1;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Resources;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace gk_1
{
    [Serializable]
    internal class CustomPanel : Panel
    {
        public Edge? hoveredEdge;
        public Point? hoveredPoint;
        public bool bezier;
        public List<Edge> edges = [];
        public List<MyPoint> points = [];
        public MyPoint? first, second;
        public bool isPolygonClosed = false;
        public int clickRadius = 6;
        public int pointSize = 12;
        public Bitmap bitmap;
        public bool selected = false;
        public bool bresenham = false;
        public Image vertical, horizontal;
        public Edge? toCurved;
        public MyPoint? draggedPoint = null;
        public bool selectedPoint = false;
        public bool moving = false;

        public CustomPanel()
        {
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.MouseClick += Form_MouseClick;
            this.MouseMove += Form_MouseMove;
            this.MouseUp += Form_MouseUp;
            this.MouseDown += Form_MouseDown;
            bitmap = new Bitmap(this.Width, this.Height);
            first = new MyPoint();
            second = new MyPoint();
            
            vertical = Image.FromFile("C:\\Users\\andrz\\source\\repos\\gk_1\\gk_1\\bin\\Debug\\net8.0-windows" + "\\resources\\_vertical.png");
            horizontal = Image.FromFile("C:\\Users\\andrz\\source\\repos\\gk_1\\gk_1\\bin\\Debug\\net8.0-windows" + "\\resources\\_horizontal.png");
            MakePredefinedScene();
        }
        public void UpdateFirstControlPoint(Point e)
        {
            first.Point = e;
            int index = points.FindIndex(point => point == toCurved.Start);
            points.Insert(index, new MyPoint(first.Point.Value.X, first.Point.Value.Y));
            this.Invalidate();
        }
        public void UpdateSecondControlPoint(Point e)
        {
            second.Point = e;
            int index = points.FindIndex(point => point.Point.Value == first.Point.Value);
            points.Insert(index, new MyPoint(second.Point.Value.X, second.Point.Value.Y));
            MakeBezier();
            this.Invalidate();
        }
        private void Form_MouseClick(object? sender, MouseEventArgs e)
        {
            if (moving)
            {
                moving = false;
            }

            if (bezier)
            {
                if (first.Point == null)
                {
                    UpdateFirstControlPoint(e.Location);
                }
                else if (second.Point == null)
                {
                    UpdateSecondControlPoint(e.Location);
                }
                return;
            }

            selectedPoint = (hoveredPoint != null);

            Edge? ed = GetEdgeNearLocation(e.Location);

            if ((hoveredPoint == null || hoveredPoint == points[0].Point.Value) && e.Button == MouseButtons.Left && !isPolygonClosed && ed == null)
            {
                AddPoint(e.Location);
            }
            else if (e.Button == MouseButtons.Right && hoveredPoint != null)
            {
                DeletePoint();
            }
            else if (ed != null && e.Button == MouseButtons.Right)
            {
                DropHorizontalConstraint(ed);
                DropVerticalConstraint(ed);
                int midX = (ed.Start.Point.Value.X + ed.End.Point.Value.X) / 2;
                int midY = (ed.Start.Point.Value.Y + ed.End.Point.Value.Y) / 2;
                var newPoint = new MyPoint(midX, midY);
                int index = points.IndexOf(ed.Start);
                var tmp = ed.End;
                ed.End = newPoint;
                points.Insert(index + 1, newPoint);
                edges.Add(new Edge(newPoint, tmp));
            }
            else if (ed != null)
            {
                hoveredEdge = ed;
                selected = true;
            }
            else
            {
                hoveredEdge = null;
                selected = false;
            }
            this.Invalidate();
        }
        private void Form_MouseUp(object? sender, MouseEventArgs e)
        {
            draggedPoint = null; // Clear dragged point when mouse is released
            MyPoint? pt = GetPointNearLocation(e.Location);
            Edge? edge = GetEdgeNearLocation(e.Location);

            if (pt != null)
            {
                UpdateHoveredPoint(pt);
            }
            else if (edge != null)
            {
                UpdateHoveredEdge(edge);
            }
            else
            {
                ClearHovered();
            }
        }
        private void UpdateHoveredPoint(MyPoint pt)
        {
            if (draggedPoint == null) // Only update hovered point if we're not dragging
            {
                var prev = points.Find(p => p.Hovered);
                if (prev != null) prev.Hovered = false;

                hoveredPoint = pt.Point;
                pt.Hovered = true;
                hoveredEdge = null;
                this.Invalidate();
            }
        }
        private void MakePredefinedScene()
        {
            AddPoint(new Point { X = 256, Y = 97 });
            AddPoint(new Point { X = 312, Y = 67 });
            AddPoint(new Point { X = 513, Y = 115 });
            AddPoint(new Point { X = 424, Y = 225 });
            AddPoint(new Point { X = 290, Y = 260 });
            AddPoint(new Point { X = 156, Y = 210 });
            AddPoint(new Point { X = 143, Y = 110 });
            AddPoint(new Point { X = 256, Y = 97 });
            first.Point = new Point(270, 120);
            second.Point = new Point(400, 100);
            toCurved = edges[1];
            int index = points.FindIndex(point => point == toCurved.Start);
            points.Insert(index, new MyPoint(first.Point.Value.X, first.Point.Value.Y));
            points.Insert(index, new MyPoint(second.Point.Value.X, second.Point.Value.Y));
            MakeBezier();
            MakeVertical(edges[2]);
            MakeHorizontal(edges[5]);
            LengthConstraint(Distance((Point)edges[4].Start.Point, (Point)edges[4].End.Point), edges[4]);
            this.Invalidate();
        }

        private void UpdateHoveredEdge(Edge edge)
        {
            if (draggedPoint == null) // Only update hovered edge if we're not dragging
            {
                hoveredEdge = edge;
                hoveredPoint = null;
                foreach (var point in points) point.Hovered = false;
                this.Invalidate();
            }
        }

        private void ClearHovered()
        {
            if (draggedPoint == null) // Only clear hovered states if we're not dragging
            {
                hoveredPoint = null;
                hoveredEdge = null;
                foreach (var point in points) point.Hovered = false;
                this.Invalidate();
            }
        }
        private void DeletePoint()
        {
            // Allow right-click to remove the last point added, if any.
            List<Edge> new_edges = [];
            if (points.Count > 0 && hoveredPoint != null)
            {
                if (points.Count == 3)
                    isPolygonClosed = false;

                var tmp = points.Find(pt => pt.Point.Value == hoveredPoint);
                int i = points.IndexOf(tmp);

                if (i > 0 && i < points.Count - 1)
                {
                    new_edges.Add(new Edge(points[i - 1], points[i + 1]));
                }
                else if (i == 0 && points.Count > 3)
                {
                    new_edges.Add(new Edge(points[1], points[^1]));
                }
                else if (i == points.Count - 1 && points.Count > 3 && isPolygonClosed)
                {
                    new_edges.Add(new Edge(points[0], points[^2]));
                }

                points.Remove(tmp);

                foreach (var edge in edges)
                {
                    if (!(edge.Start.Point.Value == hoveredPoint || edge.End.Point.Value == hoveredPoint))
                    {
                        new_edges.Add(edge);
                    }
                }
                edges.Clear();
                edges.AddRange(new_edges);
                this.Invalidate();
            }
        }

        private void AddPoint(Point newPoint)
        {


            // Check if the user clicked near the first point to close the polygon.
            if (points.Count > 2 && IsPointNearLocation((Point)points[0].Point, newPoint))
            {
                // Close the polygon by connecting the last point to the first.
                edges.Add(new Edge(points[^1], points[0]));
                isPolygonClosed = true;
            }
            else
            {
                // Add a new point and create an edge if there is a previous point.
                points.Add(new MyPoint(newPoint.X, newPoint.Y));
                if (points.Count > 1)
                {
                    edges.Add(new Edge(points[^2], points[^1]));
                }
            }
            hoveredPoint = points[^1].Point;
            points[^1].Hovered = true;

            var fs = new StreamWriter(Directory.GetCurrentDirectory() + "\\gowno.txt", true);
            fs.WriteLine(points[^1].Point.ToString());
            fs.Close();
            this.Invalidate(); // Redraw the form to display the updated polygon.
        }
        private void Form_MouseDown(object? sender, MouseEventArgs e)
        {
            MyPoint? pt = GetPointNearLocation(e.Location);
            if (e.Button == MouseButtons.Left && pt != null)
            {
                draggedPoint = pt;
            }
        }

        private bool IsPointNearLocation(Point p1, Point p2)
        {
            double distance = Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
            return distance <= clickRadius;
        }
        private bool IsPointNearEdge(Point p1, Edge e2)
        {
            if (e2 == null) return false;

            double x0 = p1.X,
                y0 = p1.Y,
                x1 = e2.Start.Point.Value.X,
                y1 = e2.Start.Point.Value.Y,
                x2 = e2.End.Point.Value.X,
                y2 = e2.End.Point.Value.Y;
            double numerator = Math.Abs((y2 - y1) * x0 - (x2 - x1) * y0 + x2 * y1 - y2 * x1);
            double denominator = Math.Sqrt(Math.Pow(y2 - y1, 2) + Math.Pow(x2 - x1, 2));

            return numerator / denominator <= 1;
        }

        private void Form_MouseMove(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && draggedPoint != null)
            {
                MovePoint(draggedPoint, new MyPoint(e.Location));
            }
            else if (!selected && !selectedPoint)
            {
                MyPoint? pt = GetPointNearLocation(e.Location);
                Edge? edge = GetEdgeNearLocation(e.Location);

                if (pt != null)
                {
                    UpdateHoveredPoint(pt);
                }
                else if (edge != null)
                {
                    UpdateHoveredEdge(edge);
                }
                else if (moving)
                {
                    var midpoint = CalculateCentroid();
                    int dx = e.Location.X - midpoint.X;
                    int dy = e.Location.Y - midpoint.Y;
                    MovePolygon(dx, dy);
                }
                else
                {
                    ClearHovered();
                }
            }
            this.Invalidate();
        }
        private Point CalculateCentroid()
        {
            float cx = 0, cy = 0;
            float area = 0;
            int count = points.Count;

            for (int i = 0; i < count; i++)
            {
                Point current = (Point)points[i].Point;
                Point next = (Point)points[(i + 1) % count].Point;

                float factor = current.X * next.Y - next.X * current.Y;
                area += factor;
                cx += (current.X + next.X) * factor;
                cy += (current.Y + next.Y) * factor;
            }

            area *= 0.5f;
            cx /= (6 * area);
            cy /= (6 * area);

            return new Point((int)cx, (int)cy);
        }
        private void MovePointWithConstraintsSkewedEdge(List<Edge> adjSkewed, List<Edge> adjacentSkewedEdges, MyPoint prev, MyPoint newPoint, Edge skewedEdge)
        {
            if (adjSkewed.Count < 2 && adjacentSkewedEdges.Count == 0)
            {
                MoveControlPoint(prev, newPoint, (SkewedEdge)skewedEdge);

            }
            else if (adjSkewed.Count != 0)
            {
                var tmp = new List<SkewedEdge>();
                foreach (var edge in adjSkewed)
                {
                    tmp.Add((SkewedEdge)edge);
                }
                MoveDoubleSkewedPoint(prev, newPoint, tmp);
            }
            else
            {
                var tmp = new List<SkewedEdge>();
                foreach (var edge in adjSkewed)
                {
                    tmp.Add((SkewedEdge)edge);
                }
                MoveDoubleSkewedPoint(prev, newPoint, tmp);
            }
            this.Invalidate();
        }

        public void MovePoint(MyPoint prev, MyPoint newPoint)
        {
            int index = points.IndexOf(prev);
            if (index == -1) return;

            var skewedEdge = edges.Find(edge => edge is SkewedEdge ed && (ed.First == prev || ed.Second == prev));
            var adjSkewed = edges.FindAll(edge => edge is SkewedEdge ed && (ed.Start == prev || ed.End == prev));

            List<Edge> adjacentSkewedEdges = new List<Edge>();

            if (skewedEdge != null)
            {
                for (int i = 0; i < edges.Count; i++)
                {
                    if (edges[i] is SkewedEdge currentEdge)
                    {
                        // Check if any of the skewed edges share a Start or End with the current edge
                        var match = (skewedEdge.Start == currentEdge.Start || skewedEdge.End == currentEdge.Start) && edges[i] != skewedEdge;

                        if (match)
                        {
                            adjacentSkewedEdges.Add(currentEdge);
                        }
                    }
                }
            }

            if ((skewedEdge != null || adjSkewed.Count == 2) && (prev.C1 || prev.G1))
            {
                MovePointWithConstraintsSkewedEdge(adjSkewed, adjacentSkewedEdges, prev, newPoint, skewedEdge);
                return;
            }


            if (prev.Direction != null)
            {
                prev.Point = new Point(newPoint.Point.Value.X, (int)(prev.Direction.Value.a * newPoint.Point.Value.X + prev.Direction.Value.b));
            }
            else if (prev.Vertical && !prev.Horizontal && !prev.Fixed)
            {
                // Move vertically only
                prev.Point = new Point(prev.Point.Value.X, newPoint.Point.Value.Y);
            }
            else if (prev.Horizontal && !prev.Vertical && !prev.Fixed)
            {
                // Move horizontally only
                prev.Point = new Point(newPoint.Point.Value.X, prev.Point.Value.Y);
            }
            else if (prev.Fixed && !prev.Horizontal && !prev.Vertical)
            {
                HandleFixedMovement(prev, newPoint);
            }
            else if (prev.Vertical && prev.Horizontal || (prev.Fixed && (prev.Horizontal || prev.Vertical)))
            {
                int dx = newPoint.Point.Value.X - prev.Point.Value.X;
                int dy = newPoint.Point.Value.Y - prev.Point.Value.Y;
                MovePolygon(dx, dy);
            }
            else
            {
                // Default behavior: free movement
                prev.Point = newPoint.Point;
            }

            ApplyContinuityConstraints(prev);
            var adj = FindAdjacentEdges((Point)prev.Point);
            if (prev.G1)
            {
                SkewedEdge skewed = (SkewedEdge)(adj[0] is SkewedEdge ? adj[0] : adj[1]);
                Edge ed = adj[0] is SkewedEdge ? adj[1] : adj[0];
                var controlPoint = prev == skewed.Start ? skewed.First : skewed.Second;
                var point = prev == ed.Start ? ed.End : ed.Start;
                
                MakePointColinear(point, prev, controlPoint, Distance((Point)prev.Point, (Point)controlPoint.Point));
            }
            if (prev.C1)
            {
                SkewedEdge skewed = (SkewedEdge)(adj[0] is SkewedEdge ? adj[0] : adj[1]);
                Edge ed = adj[0] is SkewedEdge ? adj[1] : adj[0];
                var controlPoint = prev == skewed.Start ? skewed.First : skewed.Second;
                var point = prev == ed.Start ? ed.End : ed.Start;
                MakePointColinear(point, prev, controlPoint, Distance((Point)point.Point, (Point)prev.Point) / 3);
            }
            this.Invalidate();
        }

        private void MoveDoubleSkewedPoint(MyPoint prev, MyPoint next, List<SkewedEdge> edges)
        {
            try
            {
                var first = edges[0];
                var second = edges[1];
                var firstPoint = first.First.G1 || first.Second.C1 ? first.First : first.Second;
                var secondPoint = second.First.G1 || second.Second.C1 ? second.First : second.Second;
                prev.Point = next.Point;
                MakePointColinear(secondPoint, prev, firstPoint, Distance((Point)secondPoint.Point, (Point)prev.Point));
            }
            catch (System.ArgumentOutOfRangeException) { }

        }
        private void MoveControlPoint(MyPoint prev, MyPoint next, SkewedEdge edge)
        {

            var start = prev == edge.First ? edge.Start : edge.End;
            var _edge = edges.Find(e => (e.Start == start || e.End == start) && e is not SkewedEdge);
            MyPoint end;
            if (_edge != null)
            {
                end = _edge.Start == start ? _edge.End : _edge.Start;
            }
            else
            {
                end = edge.Start == start ? edge.End : edge.Start;
                _edge = edges.Find(e => (e.Start == start || e.End == start) && e != edge);
            }

            var adj = edges.Find(ed => ed != edge && (ed.End == start || ed.Start == start));

            if (start.C1 || start.G1)
            {
                if (_edge.Horizontal)
                {
                    prev.Point = new Point(next.Point.Value.X, start.Point.Value.Y);
                    AdjustControlPointPositionForContinuity(start, end, prev);
                }
                else if (_edge.Vertical)
                {
                    prev.Point = new Point(start.Point.Value.X, next.Point.Value.Y);
                    AdjustControlPointPositionForContinuity(start, end, prev);
                }
                else if (_edge.FixedLength != null && start.C1)
                {
                    prev.Point = AdjustToFixedDistance((Point)start.Point, (Point)next.Point, (double)edge.FixedLength);
                }
                else if (adj is SkewedEdge skewed)
                {
                    prev.Point = next.Point;
                    var control = skewed.Start == start ? skewed.First : skewed.Second;
                    MakePointColinear(prev, start, control, Distance((Point)prev.Point, (Point)control.Point));
                }
                else
                {
                    prev.Point = next.Point;
                    AdjustControlPointPositionForContinuity(start, end, prev);
                }
            }
            else
            {
                prev.Point = next.Point;
            }
        }

        private Point AdjustToFixedDistance(Point start, Point target, double fixedLength)
        {
            double dx = target.X - start.X;
            double dy = target.Y - start.Y;
            double distance = Math.Sqrt(dx * dx + dy * dy);

            if (distance == 0) return start;

            double scale = fixedLength / distance;
            int newX = (int)(start.X + dx * scale);
            int newY = (int)(start.Y + dy * scale);

            return new Point(newX, newY);
        }

        private void AdjustControlPointPositionForContinuity(MyPoint start, MyPoint end, MyPoint controlPoint)
        {
            // Calculate line equation between start and controlPoint
            (double a, double b) = Line((Point)start.Point, (Point)controlPoint.Point);

            // Determine distance based on continuity type
            double distance = start.C1 ? Distance((Point)start.Point, (Point)controlPoint.Point) * 3
                                       : Distance((Point)start.Point, (Point)end.Point);

            if (a == double.MaxValue) // Handle vertical constraint
            {
                // Ensure end remains vertically aligned with start at the specified distance
                int direction = controlPoint.Point.Value.Y > start.Point.Value.Y ? -1 : 1;
                MovePoint(end, new MyPoint(new Point(start.Point.Value.X, (int)(start.Point.Value.Y + distance * direction))));
            }
            else if (Math.Abs(a) < 1e-10) // Handle horizontal constraint
            {
                // Ensure end remains horizontally aligned with start at the specified distance
                int direction = controlPoint.Point.Value.X > start.Point.Value.X ? -1 : 1;
                MovePoint(end, new MyPoint(new Point((int)(start.Point.Value.X + distance * direction), start.Point.Value.Y)));
            }
            else
            {
                // For general diagonal constraint, adjust end point based on the calculated distance along the line
                int direction = controlPoint.Point.Value.X > start.Point.Value.X ? -1 : 1;
                int newX = (int)(start.Point.Value.X + direction * Math.Cos(Math.Atan(a)) * distance);
                int newY = (int)(a * newX + b);
                MovePoint(end, new MyPoint(new Point(newX, newY)));
            }
        }

        private void ApplyContinuityConstraints(MyPoint prev)
        {
            var adjacentEdges = FindAdjacentEdges((Point)prev.Point);

            foreach (var edge in adjacentEdges)
            {
                if (edge.G1 || edge.C1)
                {
                    var start = prev;
                    var end = edge.End == prev ? edge.Start : edge.End;
                    var skew = edges.OfType<SkewedEdge>().FirstOrDefault(e => (e.Start == end || e.End == end));

                    if (skew == null) continue;

                    var control = end == skew.Start ? skew.First : skew.Second;
                    if (edge.G1)
                        MakePointColinear(start, end, control, Distance((Point)end.Point, (Point)control.Point));
                    if (edge.C1)
                        MakePointColinear(start, end, control, Distance((Point)start.Point, (Point)end.Point) / 3);
                }
            }
        }

        public void MovePolygon(int dx, int dy)
        {
            foreach (var point in points)
            {
                point.Point = new Point(point.Point.Value.X + dx, point.Point.Value.Y + dy);
            }
        }

        public void HandleFixedMovement(MyPoint prev, MyPoint newPoint)
        {
            Point start = (Point)prev.Point;
            Point end = (Point)newPoint.Point;
            int dx = end.X - start.X;
            int dy = end.Y - start.Y;

            var adjEdges = FindAdjacentEdges(start);
            var fixedEdges = adjEdges.FindAll(edge => edge.FixedLength != null);

            if (fixedEdges.Count > 1 || (fixedEdges.Count == 0 && (prev.Horizontal || prev.Vertical)))
            {
                MovePolygon(dx, dy);
            }
            else if (fixedEdges.Count == 1)
            {
                Edge fixedEdge = fixedEdges[0];
                MyPoint adjacentPoint = fixedEdge.Start == prev ? fixedEdge.End : fixedEdge.Start;

                Point adjustedPoint = CalculateFixedPoint((Point)newPoint.Point, adjacentPoint.Point.Value, (double)fixedEdge.FixedLength);
                prev.Point = adjustedPoint;
            }
        }

        public Point CalculateFixedPoint(Point target, Point fixedPoint, double radius)
        {
            double angle = Math.Atan2(target.Y - fixedPoint.Y, target.X - fixedPoint.X);

            int x = fixedPoint.X + (int)(radius * Math.Cos(angle));
            int y = fixedPoint.Y + (int)(radius * Math.Sin(angle));

            return new Point(x, y);
        }
        private MyPoint? GetPointNearLocation(Point location)
        {
            // Iterate through points to find one close to the right-clicked location.
            foreach (MyPoint p in points)
            {
                // Calculate the distance between the click location and the point.
                if (p.Point == null) continue;
                double distance = Math.Sqrt(Math.Pow(p.Point.Value.X - location.X, 2) + Math.Pow(p.Point.Value.Y - location.Y, 2));

                if (distance <= clickRadius)
                {
                    // Return the point if the distance is within the allowed click radius.
                    return p;
                }
            }

            // Return null if no point is close enough.
            return null;
        }

        private Edge? GetEdgeNearLocation(Point location)
        {
            double x0 = location.X, y0 = location.Y;
            Edge? closest = null;
            double minDistance = double.MaxValue;

            foreach (Edge e in edges)
            {
                double distance;

                if (e is SkewedEdge skewedEdge)
                {
                    // If the edge is a Bezier curve, sample points along the curve to approximate the closest point
                    const int SampleCount = 100; // Higher values increase accuracy but may reduce performance
                    distance = double.MaxValue;

                    for (int i = 0; i <= SampleCount; i++)
                    {
                        float t = i / (float)SampleCount;
                        Point bezierPoint = CalculateBezierPoint(skewedEdge, t);

                        double dx = bezierPoint.X - x0;
                        double dy = bezierPoint.Y - y0;
                        double sampleDistance = Math.Sqrt(dx * dx + dy * dy);

                        if (sampleDistance < distance)
                        {
                            distance = sampleDistance;
                        }
                    }
                }
                else
                {
                    // If the edge is a straight line, calculate distance to the line segment
                    double x1 = e.Start.Point.Value.X;
                    double y1 = e.Start.Point.Value.Y;
                    double x2 = e.End.Point.Value.X;
                    double y2 = e.End.Point.Value.Y;

                    double lineDistance = Math.Abs((y2 - y1) * x0 - (x2 - x1) * y0 + x2 * y1 - y2 * x1);
                    double denominator = Math.Sqrt(Math.Pow(y2 - y1, 2) + Math.Pow(x2 - x1, 2));
                    distance = lineDistance / denominator;
                }

                if (distance <= 1.3 && distance < minDistance)
                {
                    // Update closest edge if the current one is closer than previously found edges
                    closest = e;
                    minDistance = distance;
                }
            }

            return closest;
        }

        // Helper function to calculate a point on the Bezier curve at parameter t
        private Point CalculateBezierPoint(SkewedEdge skewedEdge, float t)
        {
            var P0 = skewedEdge.Start.Point.Value;
            var P1 = skewedEdge.First.Point.Value;
            var P2 = skewedEdge.Second.Point.Value;
            var P3 = skewedEdge.End.Point.Value;

            float x = (float)(
                Math.Pow(1 - t, 3) * P0.X +
                3 * Math.Pow(1 - t, 2) * t * P1.X +
                3 * (1 - t) * Math.Pow(t, 2) * P2.X +
                Math.Pow(t, 3) * P3.X
            );

            float y = (float)(
                Math.Pow(1 - t, 3) * P0.Y +
                3 * Math.Pow(1 - t, 2) * t * P1.Y +
                3 * (1 - t) * Math.Pow(t, 2) * P2.Y +
                Math.Pow(t, 3) * P3.Y
            );

            return new Point((int)x, (int)y);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // Resize the bitmap when the panel size changes
            if (this.Width > 0 && this.Height > 0)
            {
                bitmap = new Bitmap(this.Width, this.Height);
                Invalidate(); // Request a redraw to repaint the panel with the new bitmap size
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            // using Graphics g = e.Graphics;

            using (var g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.White);
                g.ResetTransform();

                using (Pen pen = new Pen(Color.Black, 3))
                {
                    using Pen hPen = new Pen(Color.BlueViolet, 3);
                    using Pen dPen = new Pen(Color.LightGray, 2);
                    //if (bezier && first.Point != null && second.Point != null && toCurved != null)
                    //{
                    //    MakeBezier();
                    //}

                    foreach (var edge in edges)
                    {
                        dPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                        if (edge is SkewedEdge skewedEdge)
                        {
                            if (edge == hoveredEdge)
                            {
                                DrawBezier(g, hPen, skewedEdge);
                            }
                            else
                            {
                                DrawBezier(g, pen, skewedEdge);
                            }

                            DrawLine(g, dPen, (Point)skewedEdge.Start.Point, (Point)skewedEdge.First.Point, edge.Horizontal, edge.Vertical);
                            DrawLine(g, dPen, (Point)skewedEdge.First.Point, (Point)skewedEdge.Second.Point, edge.Horizontal, edge.Vertical);
                            DrawLine(g, dPen, (Point)skewedEdge.Second.Point, (Point)skewedEdge.End.Point, edge.Horizontal, edge.Vertical);

                        }
                        else
                        {
                            if (edge == hoveredEdge)
                            {
                                DrawLine(g, hPen, (Point)edge.Start.Point, (Point)edge.End.Point, edge.Horizontal, edge.Vertical, edge.FixedLength);
                            }
                            else
                            {
                                DrawLine(g, pen, (Point)edge.Start.Point, (Point)edge.End.Point, edge.Horizontal, edge.Vertical, edge.FixedLength);
                            }
                        }
                    }
                }


                // Draw all stored points.
                foreach (MyPoint p in points)
                {
                    if (p.Point != hoveredPoint)
                        g.FillEllipse(Brushes.LightBlue, p.Point.Value.X - pointSize / 2, p.Point.Value.Y - pointSize / 2, pointSize, pointSize);
                    else
                        g.FillEllipse(Brushes.Blue, p.Point.Value.X - pointSize / 2, p.Point.Value.Y - pointSize / 2, pointSize, pointSize);
                }
                e.Graphics.DrawImage(bitmap, 0, 0);
            }
        }
        public static double Distance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(Math.Abs(p1.X - p2.X), 2) + Math.Pow(Math.Abs(p1.Y - p2.Y), 2));
        }
        public void MakeBezier()
        {
            var tmp = points.Find(pt => pt.Point.Value == first.Point.Value);
            var tmp2 = points.Find(pt => pt.Point.Value == second.Point.Value);
            SkewedEdge edge = new SkewedEdge(toCurved.Start, toCurved.End, tmp, tmp2);
            edges.Remove(toCurved);
            edges.Add(edge);
            bezier = false;
            toCurved = null;
            first.Point = second.Point = null;
            AddG1Constraint(edge.Start);
            AddG1Constraint(edge.End);
        }

        public void LengthConstraint(double length, Edge edge)
        {
            double distance = Distance((Point)edge.Start.Point, (Point)edge.End.Point);
            if (length != distance)
            {
                double dx = edge.End.Point.Value.X - edge.Start.Point.Value.X;
                double dy = edge.End.Point.Value.Y - edge.Start.Point.Value.Y;
                double unitDx = dx / distance;
                double unitDy = dy / distance;
                double newEndX = edge.Start.Point.Value.X + unitDx * length;
                double newEndY = edge.Start.Point.Value.Y + unitDy * length;
                MyPoint newEnd = new MyPoint((int)newEndX, (int)newEndY);
                MovePoint(edge.End, newEnd);
            }
            edge.FixedLength = length;
            edge.Start.Fixed = true;
            edge.End.Fixed = true;
            this.Invalidate();
        }

        private void DrawBezier(Graphics graphics, Pen pen, SkewedEdge edge)
        {
            // Number of steps determines the curve's smoothness.
            Point p0 = edge.Start.Point.Value;
            Point p1 = edge.First.Point.Value;
            Point p2 = edge.Second.Point.Value;
            Point p3 = edge.End.Point.Value;
            int steps = (int)Math.Sqrt(Math.Pow(Math.Abs(edge.Start.Point.Value.X - edge.End.Point.Value.X), 2)
                + Math.Pow(Math.Abs(edge.Start.Point.Value.Y - edge.End.Point.Value.Y), 2));
            PointF prevPoint = p0;

            for (int i = 1; i <= steps; i++)
            {
                float t = i / (float)steps;

                // Calculate the coordinates of the Bezier point using the formula
                float x = (float)(Math.Pow(1 - t, 3) * p0.X +
                                  3 * Math.Pow(1 - t, 2) * t * p1.X +
                                  3 * (1 - t) * Math.Pow(t, 2) * p2.X +
                                  Math.Pow(t, 3) * p3.X);

                float y = (float)(Math.Pow(1 - t, 3) * p0.Y +
                                  3 * Math.Pow(1 - t, 2) * t * p1.Y +
                                  3 * (1 - t) * Math.Pow(t, 2) * p2.Y +
                                  Math.Pow(t, 3) * p3.Y);

                PointF currentPoint = new PointF(x, y);

                // Draw a line between the previous point and the current point to form the curve

                graphics.DrawLine(pen, prevPoint, currentPoint);

                // Update the previous point for the next segment
                prevPoint = currentPoint;
            }
        }
        private void DrawLine(Graphics graphics, Pen pen, Point start, Point end, bool horizon = false, bool vert = false, double? _fixed = null)
        {
            if (_fixed != null)
            {
                PointF midpoint = new PointF((start.X + end.X) / 2 + 5, (start.Y + end.Y) / 2 + 5);

                // Offset the text position slightly above the line
                PointF textPosition = new PointF(midpoint.X, midpoint.Y - 10);

                // Draw the length above the line
                graphics.DrawString(_fixed.Value.ToString("F2"), this.Font, Brushes.Black, textPosition);
            }
            if (horizon)
            {
                Point imagePosition = new Point((start.X + end.X) / 2 - 10, start.Y);
                graphics.DrawImage(horizontal, new Rectangle(imagePosition.X, imagePosition.Y, 20, 20));
            }
            if (vert)
            {
                Point imagePosition = new Point(start.X, (start.Y + end.Y) / 2 - 10);
                graphics.DrawImage(vertical, new Rectangle(imagePosition.X, imagePosition.Y, 20, 20));
            }
            if (!bresenham)
            {
                graphics.DrawLine(pen, start, end);
                return;
            }

            int x0 = start.X;
            int y0 = start.Y;
            int x1 = end.X;
            int y1 = end.Y;

            // Calculate the differences and step directions
            int dx = Math.Abs(x1 - x0);
            int dy = Math.Abs(y1 - y0);

            int sx = x0 < x1 ? 1 : -1;
            int sy = y0 < y1 ? 1 : -1;
            bool dashed = pen.DashStyle == DashStyle.Dash;
            bool draw = true;
            int err = dx - dy;

            while (true)
            {
                // Draw the pixel at (x0, y0)
                if (dashed)
                {
                    if (draw)
                        //graphics.DrawRectangle(pen, x0, y0, 1, 1);
                        bitmap.SetPixel(x0, y0, pen.Color);
                    draw = !draw;
                }
                else
                    //bitmap.SetPixel(x0, y0, pen.Color);    
                    graphics.DrawRectangle(pen, x0, y0, 1, 1);  // Using 1x1 rectangle to simulate pixel

                // If we've reached the end point, break out of the loop
                if (x0 == x1 && y0 == y1)
                    break;

                // Calculate error and adjust coordinates
                int e2 = 2 * err;

                if (e2 > -dy)
                {
                    err -= dy;
                    x0 += sx;
                }

                if (e2 < dx)
                {
                    err += dx;
                    y0 += sy;
                }
            }
        }
        public void MakeHorizontal(Edge edge)
        {
            var adj = FindAdjacentEdges((Point)edge.Start.Point);
            var adj2 = FindAdjacentEdges((Point)edge.End.Point);
            var hor = adj.Find(ed => ed.Horizontal);
            var hor2 = adj2.Find(ed => ed.Horizontal);
            if (hor != null || hor2 != null)
            {
                MessageBox.Show("Two adjacent edges can't be horizontal at the same time!");
                return;
            }
            MovePoint(edge.End, new MyPoint(edge.End.Point.Value.X, edge.Start.Point.Value.Y));
            if (edge.End.Point.Value.Y != edge.Start.Point.Value.Y)
            {
                MovePoint(edge.Start, new MyPoint(edge.Start.Point.Value.X, edge.End.Point.Value.Y));
                if (edge.End.Point.Value.Y != edge.Start.Point.Value.Y)
                {
                    MessageBox.Show("Can't make edge horizontal due to contraints!");
                    return;
                }

            }
            edge.Horizontal = edge.Start.Horizontal = edge.End.Horizontal = true;
            if (edge.C1 || edge.G1)
            {
                var pt = (edge.Start.G1 || edge.Start.C1) ? edge.Start : edge.End;
                var skewed = (SkewedEdge)edges.Find(ed => (ed.Start == pt || ed.End == pt) && ed is SkewedEdge);
                var control = pt == skewed.Start ? skewed.First : skewed.Second;

            }
            this.Invalidate();
        }
        public void MakeVertical(Edge edge)
        {
            var adj = FindAdjacentEdges((Point)edge.Start.Point);
            var adj2 = FindAdjacentEdges((Point)edge.End.Point);
            var verticals = adj.Find(ed => ed.Vertical);
            var verticals2 = adj2.Find(ed => ed.Vertical);

            if (verticals != null || verticals2 != null)
            {
                MessageBox.Show("Two adjacent edges can't be vertical at the same time!");
                return;
            }

            MovePoint(edge.End, new MyPoint(edge.Start.Point.Value.X, edge.End.Point.Value.Y));
            if (edge.End.Point.Value.X != edge.Start.Point.Value.X)
            {
                MovePoint(edge.Start, new MyPoint(edge.End.Point.Value.X, edge.Start.Point.Value.Y));
                if (edge.End.Point.Value.X != edge.Start.Point.Value.X)
                {
                    MessageBox.Show("Can't make edge vertical due to contraints!");
                    return;
                }
            }
            edge.Vertical = edge.Start.Vertical = edge.End.Vertical = true;
            if (edge.C1 || edge.G1)
            {
                var pt = (edge.Start.G1 || edge.Start.C1) ? edge.Start : edge.End;
                var skewed = (SkewedEdge)edges.Find(ed => (ed.Start == pt || ed.End == pt) && ed is SkewedEdge);
                var control = pt == skewed.Start ? skewed.First : skewed.Second;
            }
            this.Invalidate();
        }
        public void DropHorizontalConstraint(Edge edge)
        {
            edge.Horizontal = edge.Start.Horizontal = edge.End.Horizontal = false;
            this.Invalidate();
        }
        public void DropVerticalConstraint(Edge edge)
        {
            edge.Vertical = edge.Start.Vertical = edge.End.Vertical = false;
            this.Invalidate();
        }
        public List<Edge> FindAdjacentEdges(Point pt)
        {
            var ans = new List<Edge>();
            foreach (Edge e in edges)
            {
                if (e.Start.Point.Value == pt || e.End.Point.Value == pt)
                    ans.Add(e);
            }
            return ans;
        }
        public void DropLengthConstraint(Edge edge)
        {
            edge.FixedLength = null;
            edge.Start.Fixed = edge.End.Fixed = false;
        }
        public void AddG1Constraint(MyPoint point)
        {
            point.G1 = true;
            var adj = FindAdjacentEdges((Point)point.Point);
            if (adj.Count < 2)
            {
                MessageBox.Show("Can't add constraint!");
                return;
            }
            if (adj[0] is SkewedEdge first && adj[1] is SkewedEdge second)
            {
                var control_1 = point == first.Start ? first.First : second.First;
                var control_2 = point == first.End ? first.Second : second.Second;
                MakePointColinear(control_2, point, control_1);
                control_1.G1 = control_2.G1 = true;
                this.Invalidate();
                return;
            }
            Edge ed = adj[0] is SkewedEdge ? adj[1] : adj[0];
            SkewedEdge skewed = (SkewedEdge)(adj[0] is SkewedEdge ? adj[0] : adj[1]);
            MyPoint controlPointToChange = point == skewed.Start ? skewed.First : skewed.Second;
            MyPoint edgePoint = point == ed.Start ? ed.End : ed.Start;
            MakePointColinear(edgePoint, point, controlPointToChange);
            ed.G1 = true;
            controlPointToChange.G1 = true;
            this.Invalidate();

        }
        public static (double a, double b) Line(Point start, Point end)
        {
            double a, b;
            if (end.X == start.X) return (double.MaxValue, start.Y);
            a = (double)(end.Y - start.Y) / (double)(end.X - start.X);
            b = start.Y - a * start.X;
            return (a, b);
        }
        public void MakePointColinear(MyPoint p1, MyPoint p2, MyPoint toChange, double distance = 20)
        {
            (double a, double b) = Line((Point)p1.Point, (Point)p2.Point);
            toChange.Direction = (a, b);

            int directionX = p1.Point.Value.X > p2.Point.Value.X ? -1 : 1;
            int directionY = p1.Point.Value.Y > p2.Point.Value.Y ? 1 : -1;

            if (a == double.MaxValue)
            {
                int newY = p2.Point.Value.Y - (int)(distance * directionY);
                toChange.Point = new Point(p2.Point.Value.X, newY);
            }
            else if (a == 0)
            {
                int newX = p2.Point.Value.X + (int)(distance * directionX);
                toChange.Point = new Point(newX, p2.Point.Value.Y);
            }
            else
            {
                double angle = Math.Atan2(p2.Point.Value.Y - p1.Point.Value.Y, p2.Point.Value.X - p1.Point.Value.X);
                int newX = p2.Point.Value.X + (int)(distance * Math.Cos(angle));
                int newY = p2.Point.Value.Y + (int)(distance * Math.Sin(angle));
                toChange.Point = new Point(newX, newY);
            }
        }

        public void AddC1Constraint(MyPoint point)
        {
            point.C1 = true;
            var adj = FindAdjacentEdges((Point)point.Point);
            if (adj.Count < 2)
            {
                MessageBox.Show("Can't add constraint!");
                return;
            }
            if (adj[0] is SkewedEdge first && adj[1] is SkewedEdge second) // Punkt pomiędzy dwoma segmentami Beziera
            {
                var control_1 = point == first.Start ? first.First : second.First;
                var control_2 = point == first.End ? first.Second : second.Second;
                MakePointColinear(control_2, point, control_1, Distance((Point)control_2.Point, (Point)point.Point));
                control_1.G1 = control_2.G1 = true;
                this.Invalidate();
                return;
            }
            try
            {
                Edge ed = adj[0] is SkewedEdge ? adj[1] : adj[0];
                SkewedEdge skewed = (SkewedEdge)(adj[0] is SkewedEdge ? adj[0] : adj[1]);
                MyPoint controlPointToChange = point == skewed.Start ? skewed.First : skewed.Second;
                MyPoint edgePoint = point == ed.Start ? ed.End : ed.Start;
                MakePointColinear(edgePoint, point, controlPointToChange, Distance((Point)edgePoint.Point, (Point)point.Point) / 3);
                ed.C1 = true;
                controlPointToChange.C1 = true;
                this.Invalidate();
            }
            catch (Exception) { MessageBox.Show("Can't add constraint!"); }

        }
        public void RemoveC1Constraint(MyPoint point)
        {
            point.C1 = false;
            var adj = FindAdjacentEdges((Point)point.Point);
            Edge ed = adj[0] is SkewedEdge ? adj[1] : adj[0];
            ed.C1 = false;
            this.Invalidate();
        }
        public void RemoveG1Constraint(MyPoint point)
        {
            point.G1 = false;
            var adj = FindAdjacentEdges((Point)point.Point);
            Edge ed = adj[0] is SkewedEdge ? adj[1] : adj[0];
            ed.G1 = false;
            this.Invalidate();
        }
        public void DropContinuity(MyPoint point)
        {
            var adj = FindAdjacentEdges((Point)point.Point);
            point.C1 = point.G1 = false;
            foreach (var edge in adj)
            {
                var otherPoint = edge.Start == point ? edge.End : edge.Start;
                if (!otherPoint.C1)
                    edge.C1 = false;
                if (!otherPoint.G1)
                    edge.G1 = false;
                if (edge is SkewedEdge skewed)
                {
                    var control = point == skewed.Start ? skewed.First : skewed.Second;
                    control.Direction = null;
                    control.C1 = control.G1 = false;
                }
            }
        }
        public void DropBezier(Edge edge)
        {
            var tmp = edge as SkewedEdge;
            points.Remove(tmp.First);
            points.Remove(tmp.Second);
            edges.Remove(tmp);
            var _new = new Edge(edge.Start, edge.End);
            _new.G1 = _new.C1 = false;
            edges.Add(_new);
        }
        public string Serialize()
        {
            string ans = string.Empty;
            foreach(var point in points)
            {
                ans += point.Serialize() + '\n';
            }
            foreach(var edge in edges)
            {
                if (edge is SkewedEdge skewed)
                    ans += skewed.Serialize() + "\n";
                else
                    ans += edge.Serialize() + "\n";
            }
            return ans;
        }
        public void MakeDistinctPoints()
        {

        }
    }
}




          

