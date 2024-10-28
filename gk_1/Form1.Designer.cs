namespace gk_1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            pomocToolStripMenuItem = new ToolStripMenuItem();
            usunWielokatToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripMenuItem();
            drawBezierToolStripMenuItem = new ToolStripMenuItem();
            addConstraintToolStripMenuItem = new ToolStripMenuItem();
            continuityToolStripMenuItem = new ToolStripMenuItem();
            g0ToolStripMenuItem = new ToolStripMenuItem();
            c0ToolStripMenuItem = new ToolStripMenuItem();
            c1ToolStripMenuItem = new ToolStripMenuItem();
            edgeToolStripMenuItem = new ToolStripMenuItem();
            lengthToolStripMenuItem1 = new ToolStripMenuItem();
            verticalToolStripMenuItem1 = new ToolStripMenuItem();
            horizontalToolStripMenuItem1 = new ToolStripMenuItem();
            movePolygonToolStripMenuItem = new ToolStripMenuItem();
            algorithmExplanationToolStripMenuItem = new ToolStripMenuItem();
            fileToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            loadToolStripMenuItem = new ToolStripMenuItem();
            radioButton1 = new RadioButton();
            checkBox1 = new CheckBox();
            customPanel1 = new CustomPanel();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { pomocToolStripMenuItem, usunWielokatToolStripMenuItem, toolStripMenuItem2, drawBezierToolStripMenuItem, addConstraintToolStripMenuItem, movePolygonToolStripMenuItem, algorithmExplanationToolStripMenuItem, fileToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(882, 28);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // pomocToolStripMenuItem
            // 
            pomocToolStripMenuItem.Name = "pomocToolStripMenuItem";
            pomocToolStripMenuItem.Size = new Size(55, 24);
            pomocToolStripMenuItem.Text = "Help";
            pomocToolStripMenuItem.Click += pomocToolStripMenuItem_Click;
            // 
            // usunWielokatToolStripMenuItem
            // 
            usunWielokatToolStripMenuItem.Name = "usunWielokatToolStripMenuItem";
            usunWielokatToolStripMenuItem.Size = new Size(126, 24);
            usunWielokatToolStripMenuItem.Text = "Delete polygon";
            usunWielokatToolStripMenuItem.Click += usunWielokatToolStripMenuItem_Click;
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new Size(14, 24);
            // 
            // drawBezierToolStripMenuItem
            // 
            drawBezierToolStripMenuItem.Name = "drawBezierToolStripMenuItem";
            drawBezierToolStripMenuItem.Size = new Size(103, 24);
            drawBezierToolStripMenuItem.Text = "Draw Bezier";
            drawBezierToolStripMenuItem.Click += drawBezierToolStripMenuItem_Click;
            // 
            // addConstraintToolStripMenuItem
            // 
            addConstraintToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { continuityToolStripMenuItem, edgeToolStripMenuItem });
            addConstraintToolStripMenuItem.Name = "addConstraintToolStripMenuItem";
            addConstraintToolStripMenuItem.Size = new Size(122, 24);
            addConstraintToolStripMenuItem.Text = "Add Constraint";
            addConstraintToolStripMenuItem.Click += addConstraintToolStripMenuItem_Click;
            // 
            // continuityToolStripMenuItem
            // 
            continuityToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { g0ToolStripMenuItem, c0ToolStripMenuItem, c1ToolStripMenuItem });
            continuityToolStripMenuItem.Name = "continuityToolStripMenuItem";
            continuityToolStripMenuItem.Size = new Size(159, 26);
            continuityToolStripMenuItem.Text = "Continuity";
            // 
            // g0ToolStripMenuItem
            // 
            g0ToolStripMenuItem.Name = "g0ToolStripMenuItem";
            g0ToolStripMenuItem.Size = new Size(110, 26);
            g0ToolStripMenuItem.Text = "G0";
            g0ToolStripMenuItem.Click += g0ToolStripMenuItem_Click;
            // 
            // c0ToolStripMenuItem
            // 
            c0ToolStripMenuItem.Name = "c0ToolStripMenuItem";
            c0ToolStripMenuItem.Size = new Size(110, 26);
            c0ToolStripMenuItem.Text = "C1";
            c0ToolStripMenuItem.Click += c0ToolStripMenuItem_Click;
            // 
            // c1ToolStripMenuItem
            // 
            c1ToolStripMenuItem.Name = "c1ToolStripMenuItem";
            c1ToolStripMenuItem.Size = new Size(110, 26);
            c1ToolStripMenuItem.Text = "G1";
            c1ToolStripMenuItem.Click += g1ToolStripMenuItem_Click;
            // 
            // edgeToolStripMenuItem
            // 
            edgeToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { lengthToolStripMenuItem1, verticalToolStripMenuItem1, horizontalToolStripMenuItem1 });
            edgeToolStripMenuItem.Name = "edgeToolStripMenuItem";
            edgeToolStripMenuItem.Size = new Size(159, 26);
            edgeToolStripMenuItem.Text = "Edge";
            // 
            // lengthToolStripMenuItem1
            // 
            lengthToolStripMenuItem1.Name = "lengthToolStripMenuItem1";
            lengthToolStripMenuItem1.Size = new Size(162, 26);
            lengthToolStripMenuItem1.Text = "Length";
            lengthToolStripMenuItem1.Click += lengthToolStripMenuItem1_Click;
            // 
            // verticalToolStripMenuItem1
            // 
            verticalToolStripMenuItem1.Name = "verticalToolStripMenuItem1";
            verticalToolStripMenuItem1.Size = new Size(162, 26);
            verticalToolStripMenuItem1.Text = "Vertical";
            verticalToolStripMenuItem1.Click += verticalToolStripMenuItem1_Click;
            // 
            // horizontalToolStripMenuItem1
            // 
            horizontalToolStripMenuItem1.Name = "horizontalToolStripMenuItem1";
            horizontalToolStripMenuItem1.Size = new Size(162, 26);
            horizontalToolStripMenuItem1.Text = "Horizontal";
            horizontalToolStripMenuItem1.Click += horizontalToolStripMenuItem1_Click;
            // 
            // movePolygonToolStripMenuItem
            // 
            movePolygonToolStripMenuItem.Name = "movePolygonToolStripMenuItem";
            movePolygonToolStripMenuItem.Size = new Size(117, 24);
            movePolygonToolStripMenuItem.Text = "Move Polygon";
            movePolygonToolStripMenuItem.Click += movePolygonToolStripMenuItem_Click;
            // 
            // algorithmExplanationToolStripMenuItem
            // 
            algorithmExplanationToolStripMenuItem.Name = "algorithmExplanationToolStripMenuItem";
            algorithmExplanationToolStripMenuItem.Size = new Size(172, 24);
            algorithmExplanationToolStripMenuItem.Text = "Algorithm Explanation";
            algorithmExplanationToolStripMenuItem.Click += algorithmExplanationToolStripMenuItem_Click;
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { saveToolStripMenuItem, loadToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(46, 24);
            fileToolStripMenuItem.Text = "File";
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new Size(224, 26);
            saveToolStripMenuItem.Text = "Save";
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            // 
            // loadToolStripMenuItem
            // 
            loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            loadToolStripMenuItem.Size = new Size(224, 26);
            loadToolStripMenuItem.Text = "Load";
            loadToolStripMenuItem.Click += loadToolStripMenuItem_Click;
            // 
            // radioButton1
            // 
            radioButton1.AutoCheck = false;
            radioButton1.AutoSize = true;
            radioButton1.Location = new Point(29, 518);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new Size(103, 24);
            radioButton1.TabIndex = 3;
            radioButton1.TabStop = true;
            radioButton1.Text = "Bresenham";
            radioButton1.UseVisualStyleBackColor = true;
            radioButton1.CheckedChanged += radioButton1_CheckedChanged;
            radioButton1.MouseClick += radioButton1_MouseClick;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(225, 518);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(161, 24);
            checkBox1.TabIndex = 4;
            checkBox1.Text = "Automatic relations";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // customPanel1
            // 
            customPanel1.Location = new Point(0, 31);
            customPanel1.Name = "customPanel1";
            customPanel1.Size = new Size(870, 481);
            customPanel1.TabIndex = 5;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(882, 553);
            Controls.Add(customPanel1);
            Controls.Add(checkBox1);
            Controls.Add(radioButton1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            MaximumSize = new Size(900, 600);
            MinimumSize = new Size(900, 600);
            Name = "Form1";
            Text = "Form1";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem pomocToolStripMenuItem;
        private ToolStripMenuItem usunWielokatToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem2;
        private ToolStripMenuItem drawBezierToolStripMenuItem;
        private ToolStripMenuItem addConstraintToolStripMenuItem;
        private CustomPanel customPanel1;
        private RadioButton radioButton1;
        private ToolStripMenuItem continuityToolStripMenuItem;
        private ToolStripMenuItem g0ToolStripMenuItem;
        private ToolStripMenuItem c0ToolStripMenuItem;
        private ToolStripMenuItem c1ToolStripMenuItem;
        private ToolStripMenuItem edgeToolStripMenuItem;
        private ToolStripMenuItem lengthToolStripMenuItem1;
        private ToolStripMenuItem verticalToolStripMenuItem1;
        private ToolStripMenuItem horizontalToolStripMenuItem1;
        private ToolStripMenuItem movePolygonToolStripMenuItem;
        private ToolStripMenuItem algorithmExplanationToolStripMenuItem;
        private CheckBox checkBox1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem loadToolStripMenuItem;
    }
}
