using System.Windows.Forms;

namespace A_star
{
    public class DoubleBufferedPanel : Panel
    {
        public DoubleBufferedPanel()
        {
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.UpdateStyles();
        }
    }

    partial class Gridlayout
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new A_star.DoubleBufferedPanel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.GridSizeInp = new System.Windows.Forms.ToolStripTextBox();
            this.resizeGridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findShortestPathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aStToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bFSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dijkstraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearPathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.placeStartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.placeEndToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.layoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(26, 41);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 800);
            this.panel1.TabIndex = 1;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            this.panel1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseClick);
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
            this.panel1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseUp);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.GridSizeInp,
            this.resizeGridToolStripMenuItem,
            this.findShortestPathToolStripMenuItem,
            this.clearPathToolStripMenuItem,
            this.placeStartToolStripMenuItem,
            this.placeEndToolStripMenuItem,
            this.layoutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(894, 27);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // GridSizeInp
            // 
            this.GridSizeInp.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.GridSizeInp.Name = "GridSizeInp";
            this.GridSizeInp.Size = new System.Drawing.Size(100, 23);
            // 
            // resizeGridToolStripMenuItem
            // 
            this.resizeGridToolStripMenuItem.Name = "resizeGridToolStripMenuItem";
            this.resizeGridToolStripMenuItem.Size = new System.Drawing.Size(76, 23);
            this.resizeGridToolStripMenuItem.Text = "Resize Grid";
            this.resizeGridToolStripMenuItem.Click += new System.EventHandler(this.button1_Click);
            // 
            // findShortestPathToolStripMenuItem
            // 
            this.findShortestPathToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aStToolStripMenuItem,
            this.bFSToolStripMenuItem,
            this.dijkstraToolStripMenuItem});
            this.findShortestPathToolStripMenuItem.Name = "findShortestPathToolStripMenuItem";
            this.findShortestPathToolStripMenuItem.Size = new System.Drawing.Size(115, 23);
            this.findShortestPathToolStripMenuItem.Text = "Find Shortest Path";
            // 
            // aStToolStripMenuItem
            // 
            this.aStToolStripMenuItem.Name = "aStToolStripMenuItem";
            this.aStToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.aStToolStripMenuItem.Text = "A-Star";
            this.aStToolStripMenuItem.Click += new System.EventHandler(this.aStToolStripMenuItem_Click);
            // 
            // bFSToolStripMenuItem
            // 
            this.bFSToolStripMenuItem.Name = "bFSToolStripMenuItem";
            this.bFSToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.bFSToolStripMenuItem.Text = "BFS";
            this.bFSToolStripMenuItem.Click += new System.EventHandler(this.bFSToolStripMenuItem_Click);
            // 
            // dijkstraToolStripMenuItem
            // 
            this.dijkstraToolStripMenuItem.Name = "dijkstraToolStripMenuItem";
            this.dijkstraToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.dijkstraToolStripMenuItem.Text = "Dijkstra";
            this.dijkstraToolStripMenuItem.Click += new System.EventHandler(this.dijkstraToolStripMenuItem_Click);
            // 
            // clearPathToolStripMenuItem
            // 
            this.clearPathToolStripMenuItem.Name = "clearPathToolStripMenuItem";
            this.clearPathToolStripMenuItem.Size = new System.Drawing.Size(73, 23);
            this.clearPathToolStripMenuItem.Text = "Clear Path";
            this.clearPathToolStripMenuItem.Click += new System.EventHandler(this.clearPathBtn_Click);
            // 
            // placeStartToolStripMenuItem
            // 
            this.placeStartToolStripMenuItem.Name = "placeStartToolStripMenuItem";
            this.placeStartToolStripMenuItem.Size = new System.Drawing.Size(74, 23);
            this.placeStartToolStripMenuItem.Text = "Place Start";
            this.placeStartToolStripMenuItem.Click += new System.EventHandler(this.placeStartToolStripMenuItem_Click);
            // 
            // placeEndToolStripMenuItem
            // 
            this.placeEndToolStripMenuItem.Name = "placeEndToolStripMenuItem";
            this.placeEndToolStripMenuItem.Size = new System.Drawing.Size(70, 23);
            this.placeEndToolStripMenuItem.Text = "Place End";
            this.placeEndToolStripMenuItem.Click += new System.EventHandler(this.placeEndToolStripMenuItem_Click);
            // 
            // layoutToolStripMenuItem
            // 
            this.layoutToolStripMenuItem.Name = "layoutToolStripMenuItem";
            this.layoutToolStripMenuItem.Size = new System.Drawing.Size(99, 23);
            this.layoutToolStripMenuItem.Text = "Change Layout";
            this.layoutToolStripMenuItem.Click += new System.EventHandler(this.layoutToolStripMenuItem_Click);
            // 
            // Gridlayout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(894, 853);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Gridlayout";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DoubleBufferedPanel panel1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripTextBox GridSizeInp;
        private System.Windows.Forms.ToolStripMenuItem resizeGridToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findShortestPathToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearPathToolStripMenuItem;
        private ToolStripMenuItem placeStartToolStripMenuItem;
        private ToolStripMenuItem placeEndToolStripMenuItem;
        private ToolStripMenuItem aStToolStripMenuItem;
        private ToolStripMenuItem bFSToolStripMenuItem;
        private ToolStripMenuItem dijkstraToolStripMenuItem;
        private ToolStripMenuItem layoutToolStripMenuItem;
    }
}