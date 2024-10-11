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
            dodajKrawedzToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { pomocToolStripMenuItem, usunWielokatToolStripMenuItem, dodajKrawedzToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 28);
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
            // dodajKrawedzToolStripMenuItem
            // 
            dodajKrawedzToolStripMenuItem.Name = "dodajKrawedzToolStripMenuItem";
            dodajKrawedzToolStripMenuItem.Size = new Size(89, 24);
            dodajKrawedzToolStripMenuItem.Text = "Add edge";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem pomocToolStripMenuItem;
        private ToolStripMenuItem usunWielokatToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem dodajKrawedzToolStripMenuItem;
    }
}
