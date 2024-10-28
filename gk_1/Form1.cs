using System.Text.Json;

namespace gk_1
{
    public partial class Form1 : Form
    {
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
                customPanel1.bezier = false;
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
            if (!tmp.G1 && !tmp.C1)
                customPanel1.AddG1Constraint(tmp);
            else
                customPanel1.RemoveC1Constraint(tmp);
        }

        private void c0ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var tmp = customPanel1.points.Find(pt => pt.Point == customPanel1.hoveredPoint);
            if (tmp == null) MessageBox.Show("Choose point first!");
            if (!tmp.C1 && !tmp.G1)
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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fs = new StreamWriter(Directory.GetCurrentDirectory() + "\\polygon.json");
            fs.Write(customPanel1.Serialize());
            fs.Close();
            MessageBox.Show("Polygon saved successfully!");
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "json files (*.json)|";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;
                string? filePath;
                string? fileContent;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;
                    usunWielokatToolStripMenuItem_Click(sender, e);

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        while (!reader.EndOfStream)
                        {
                            fileContent = reader.ReadLine();

                            MyPoint? point;
                            var tmp = fileContent.Split(":");
                            var options = new JsonSerializerOptions { IncludeFields = true };
                            if (tmp != null)
                            {
                                if (tmp[0] == "{\"First\"")
                                {
                                    SkewedEdge? edge = JsonSerializer.Deserialize<SkewedEdge>(fileContent, options);
                                    customPanel1.edges.Add(edge);
                                    MyPoint? pt = customPanel1.points.Find(pt => pt.Point.Value == edge.Start.Point);

                                    if (pt != null)
                                    {
                                        edge.Start = pt;
                                    }
                                    else
                                    {
                                        customPanel1.points.Add(edge.Start);
                                    }
                                    pt = customPanel1.points.Find(pt => pt.Point.Value == edge.End.Point);

                                    if (pt != null)
                                    {
                                        edge.End = pt;
                                    }
                                    else
                                    {
                                        customPanel1.points.Add(edge.End);
                                    }

                                    customPanel1.toCurved = edge;
                                    customPanel1.UpdateFirstControlPoint((Point)edge.First.Point);
                                    customPanel1.UpdateSecondControlPoint((Point)edge.Second.Point);

                                }
                            }
                            else if (tmp[0] == "{\"Start\"")
                            {
                                Edge? edge;
                                edge = JsonSerializer.Deserialize<Edge>(fileContent, options);
                                customPanel1.edges.Add(edge);
                                if (customPanel1.points.Find(pt => pt.Point.Value == edge.Start.Point) == null)
                                    customPanel1.points.Add(edge.Start);

                                if (customPanel1.points.Find(pt => pt.Point.Value == edge.End.Point) == null)
                                    customPanel1.points.Add(edge.End);
                            }
                        }
                    }

                    customPanel1.Invalidate();
                }
            }
        }
            
        
    }
}

