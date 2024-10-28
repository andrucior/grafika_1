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
        public Form1()
        {
            InitializeComponent();
        }
        private void pomocToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, Directory.GetCurrentDirectory() + "\\help.html");
        }
        private void usunWielokatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            customPanel1.points.Clear();
            customPanel1.isPolygonClosed = false;
            customPanel1.edges.Clear();
            customPanel1.hoveredPoint = null;
            customPanel1.hoveredEdge = null;
            customPanel1.Invalidate();
        }
        private void drawBezierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //drawBezierToolStripMenuItem.BackColor = SystemColors.ActiveBorder;
            if (customPanel1.hoveredEdge == null)
            {
                MessageBox.Show("Must select edge first!");
                return;
            }
            if (customPanel1.hoveredEdge.Horizontal || customPanel1.hoveredEdge.Vertical)
            {
                MessageBox.Show("Unable to perform due to constraints!");
                return;
            }
            if (customPanel1.hoveredEdge is not SkewedEdge)
            {
                customPanel1.bezier = true;
                customPanel1.toCurved = customPanel1.hoveredEdge;
            }
            else
            {
                customPanel1.DropBezier(customPanel1.hoveredEdge);
            }
        }

        private void addConstraintToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (customPanel1.hoveredEdge == null && customPanel1.hoveredPoint == null)
            {
                addConstraintToolStripMenuItem.HideDropDown();
                MessageBox.Show("Must select edge or point first!");
                return;
            }

            //horizontalToolStripMenuItem1.Checked = customPanel1.hoveredEdge.Horizontal;
            //verticalToolStripMenuItem1.Checked = customPanel1.hoveredEdge.Vertical;
            //lengthToolStripMenuItem1.Checked = customPanel1.hoveredEdge.FixedLength != null;

        }

        private void customPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void customPanel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                customPanel1.bresenham = true;
                customPanel1.Invalidate();
            }
            else
            {
                customPanel1.bresenham = false;
                customPanel1.Invalidate();
            }
        }

        private void radioButton1_MouseClick(object sender, MouseEventArgs e)
        {
            radioButton1.Checked = !radioButton1.Checked;
        }

        private void lengthToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (customPanel1.hoveredEdge.FixedLength == null)
            {
                string? length = ShowTextInsertionWindow();
                customPanel1.LengthConstraint(Convert.ToDouble(length), customPanel1.hoveredEdge);
            }
            else
            {
                customPanel1.DropLengthConstraint(customPanel1.hoveredEdge);
            }
        }

        // Function to show the temporary window for text insertion
        private string? ShowTextInsertionWindow()
        {
            // Create a new temporary form
            Form tempForm = new Form();
            tempForm.Text = "Enter new length";
            tempForm.Size = new Size(300, 150);
            tempForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            tempForm.StartPosition = FormStartPosition.CenterScreen;
            tempForm.MinimizeBox = false;
            tempForm.MaximizeBox = false;
            tempForm.ShowInTaskbar = false;


            // Create a TextBox for text input
            TextBox textBox = new TextBox();
            textBox.Location = new Point(10, 10);
            textBox.Width = 260;
            textBox.Text = CustomPanel.Distance((Point)customPanel1.hoveredEdge.Start.Point, (Point)customPanel1.hoveredEdge.End.Point).ToString("F2");

            // Create an "OK" button
            Button okButton = new Button();
            okButton.Text = "OK";
            okButton.Location = new Point(100, 50);
            okButton.DialogResult = DialogResult.OK; // Set the result for this button

            // Create a "Cancel" button
            Button cancelButton = new Button();
            cancelButton.Text = "Cancel";
            cancelButton.Location = new Point(180, 50);
            cancelButton.DialogResult = DialogResult.Cancel;

            // Add controls to the form
            tempForm.Controls.Add(textBox);
            tempForm.Controls.Add(okButton);
            tempForm.Controls.Add(cancelButton);

            // Set the default button behavior
            tempForm.AcceptButton = okButton;  // 'Enter' key will trigger the OK button
            tempForm.CancelButton = cancelButton;  // 'Escape' key will trigger the Cancel button

            // Show the form as a dialog and check the result
            if (tempForm.ShowDialog() == DialogResult.OK)
            {
                return textBox.Text; // Return the text entered by the user
            }
            else
            {
                return null; // User canceled, return null
            }
        }

        private void verticalToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!customPanel1.hoveredEdge.Vertical)
                customPanel1.MakeVertical(customPanel1.hoveredEdge);
            else
                customPanel1.DropVerticalConstraint(customPanel1.hoveredEdge);

        }

        private void horizontalToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!customPanel1.hoveredEdge.Horizontal)
                customPanel1.MakeHorizontal(customPanel1.hoveredEdge);
            else
                customPanel1.DropHorizontalConstraint(customPanel1.hoveredEdge);
        }

        private void g1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var tmp = customPanel1.points.Find(pt => pt.Point == customPanel1.hoveredPoint);
            if (tmp == null) MessageBox.Show("Choose point first!");
            if (!tmp.G1)
                customPanel1.AddG1Constraint(tmp);
            else
                customPanel1.RemoveC1Constraint(tmp);
        }

        private void c0ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var tmp = customPanel1.points.Find(pt => pt.Point == customPanel1.hoveredPoint);
            if (tmp == null) MessageBox.Show("Choose point first!");
            if (!tmp.C1)
                customPanel1.AddC1Constraint(tmp);
            else
                customPanel1.RemoveC1Constraint(tmp);
        }

        private void g0ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var tmp = customPanel1.points.Find(pt => pt.Point == customPanel1.hoveredPoint);
            if (tmp == null) MessageBox.Show("Choose point first!");
            customPanel1.DropContinuity(tmp);
        }

        private void movePolygonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            customPanel1.moving = true;
        }

        private void algorithmExplanationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, Directory.GetCurrentDirectory() + "\\algorithm.html");
        }
    }
}
