namespace gk_1
{
    public partial class Form1 : Form
    {
        /*
            Architektura:
                -klasy: punkt, krawêdŸ (pozioma, pionowa, krzywa), wielok¹t
                -wielok¹t: lista krawêdzi 
            UX/UI:
                -predefiniowana scena wstêpna
            Research:
                -G0, G1, algorytm Bresenhama, algorytm przyrostowy
         
         */
        private List<Point> points;
        private List<Edge> edges;
        private const int pointSize = 12;
        private const int clickRadius = 6;
        private Point? hoveredPoint;
        private bool isPolygonClosed = false;

        public Form1()
        {
            InitializeComponent();
            
            points = new List<Point>();
            edges = new List<Edge>();
            
            this.MouseDown += Form_MouseClick;
            this.MouseMove += Form_MouseMove;
        }
      
        private void Form_MouseClick(object sender, MouseEventArgs e)
        {
            Point? p = GetPointNearLocation(e.Location);

            if (e.Button == MouseButtons.Left && !isPolygonClosed)
            {
                Point newPoint = e.Location;

                // Check if the user clicked near the first point to close the polygon.
                if (points.Count > 2 && IsPointNearLocation(points[0], newPoint))
                {
                    // Close the polygon by connecting the last point to the first.
                    edges.Add(new Edge(points[^1], points[0]));
                    isPolygonClosed = true;
                }
                else
                {
                    // Add a new point and create an edge if there is a previous point.
                    points.Add(newPoint);
                    if (points.Count > 1)
                    {
                        edges.Add(new Edge(points[^2], points[^1]));
                    }
                }

                this.Invalidate(); // Redraw the form to display the updated polygon.
            }
            else if (e.Button == MouseButtons.Right)
            {
                // Allow right-click to remove the last point added, if any.
                List<Edge> new_edges = new List<Edge>();
                if (points.Count > 0)
                {
                    isPolygonClosed = false;
                    if (hoveredPoint != null)
                    {
                        points.Remove((Point)hoveredPoint);
                    }
                    
                    foreach (var edge in edges)
                    {
                        if (!(edge.Start == hoveredPoint || edge.End == hoveredPoint))
                        {
                            new_edges.Add(edge);
                        }
                    }
                    edges.Clear();
                    edges.AddRange(new_edges);
                    this.Invalidate();
                }
            }
            //else if (e.Button == MouseButtons.Left && p != null)
            //{
            //    points.Remove((Point)p);
            //    hoveredPoint = p;
            //}
        }
        private void Form_MouseUp(object sender, MouseEventArgs e) 
        {
            if (hoveredPoint != null)
            {
                hoveredPoint = new Point(e.X, e.Y);
                
            }
        }
        private bool IsPointNearLocation(Point p1, Point p2)
        {
            double distance = Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
            return distance <= clickRadius;
        }
        private void Form_MouseMove(object sender, MouseEventArgs e)
        {
            // Check if the mouse is hovering near any point.
            Point? newHoveredPoint = GetPointNearLocation(e.Location);

            // Only trigger a redraw if the hovered point has changed.
            if (newHoveredPoint != hoveredPoint)
            {
                hoveredPoint = newHoveredPoint;
                this.Invalidate(); // Redraw to update the hover effect.
            }
            if (e.Button == MouseButtons.Left && hoveredPoint != null)
            {
                points.Add(new Point(e.X, e.Y));
                var new_edges = new List<Edge>();
                foreach (var edge in edges)
                {
                    if (edge.Start == hoveredPoint)
                    {
                        new_edges.Add(new Edge(new Point(e.X, e.Y), edge.End));
                    }
                    else if (edge.End == hoveredPoint)
                    {
                        new_edges.Add(new Edge(edge.Start, new Point(e.X, e.Y)));
                    }
                    else
                    {
                        new_edges.Add((Edge)edge);
                    }
                }
                points.Remove((Point)hoveredPoint);
                edges.Clear();
                new_edges.AddRange(new_edges);
                this.Invalidate();
            }
        }

        private Point? GetPointNearLocation(Point location)
        {
            // Iterate through points to find one close to the right-clicked location.
            foreach (Point p in points)
            {
                // Calculate the distance between the click location and the point.
                double distance = Math.Sqrt(Math.Pow(p.X - location.X, 2) + Math.Pow(p.Y - location.Y, 2));

                if (distance <= clickRadius)
                {
                    // Return the point if the distance is within the allowed click radius.
                    return p;
                }
            }

            // Return null if no point is close enough.
            return null;
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            using (Graphics g = e.Graphics)
            {
                g.ResetTransform();
                if (isPolygonClosed)
                    g.FillPolygon(Brushes.LightCyan, points.ToArray());

                using (Pen pen = new Pen(Color.Black, 4))
                {
                    foreach (var edge in edges)
                    {
                        g.DrawLine(pen, edge.Start, edge.End);
                    }
                }
                

                // Draw all stored points.
                foreach (Point p in points)
                {
                    if (p != hoveredPoint)
                        g.FillEllipse(Brushes.LightBlue, p.X - pointSize / 2, p.Y - pointSize / 2, pointSize, pointSize);
                    else
                        g.FillEllipse(Brushes.Blue, p.X - pointSize / 2, p.Y - pointSize / 2, pointSize, pointSize); 
                }
                
            }
        }
        private void Form_RightClick(object sender, MouseEventArgs e)
        {
            // Dodawanie krawêdzi 
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pomocToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Tu prosta dokumentacja
        }

        private void usunWielokatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            points.Clear();
            isPolygonClosed = false;
            edges.Clear();
            this.Invalidate();
        }
    }
}
