namespace A_star
{
    partial class GraphLayout
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.addNodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NodeVal = new System.Windows.Forms.ToolStripTextBox();
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addEdgeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FirstNodeComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.SecondNodeComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.WeightTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.addEdge = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteNodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DeleteNodeComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteEdgeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DeleteEdgeComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.deleteToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.setStartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setEndToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findShortestPathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.traverseGraphToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dFSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bFSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeLayoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Result = new System.Windows.Forms.RichTextBox();
            this.Canvas = new A_star.DoubleBufferedPanel();
            this.dijksraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.astarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addNodeToolStripMenuItem,
            this.addEdgeToolStripMenuItem,
            this.deleteNodeToolStripMenuItem,
            this.deleteEdgeToolStripMenuItem,
            this.setStartToolStripMenuItem,
            this.setEndToolStripMenuItem,
            this.findShortestPathToolStripMenuItem,
            this.traverseGraphToolStripMenuItem,
            this.changeLayoutToolStripMenuItem,
            this.clearToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(888, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // addNodeToolStripMenuItem
            // 
            this.addNodeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NodeVal,
            this.addToolStripMenuItem});
            this.addNodeToolStripMenuItem.Name = "addNodeToolStripMenuItem";
            this.addNodeToolStripMenuItem.Size = new System.Drawing.Size(73, 20);
            this.addNodeToolStripMenuItem.Text = "Add Node";
            this.addNodeToolStripMenuItem.ToolTipText = "Enter value";
            // 
            // NodeVal
            // 
            this.NodeVal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.NodeVal.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.NodeVal.Name = "NodeVal";
            this.NodeVal.Size = new System.Drawing.Size(100, 23);
            this.NodeVal.ToolTipText = "Enter value";
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.addToolStripMenuItem.Text = "Add";
            this.addToolStripMenuItem.Click += new System.EventHandler(this.AddNode_Click);
            // 
            // addEdgeToolStripMenuItem
            // 
            this.addEdgeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FirstNodeComboBox,
            this.SecondNodeComboBox,
            this.WeightTextBox,
            this.addEdge});
            this.addEdgeToolStripMenuItem.Name = "addEdgeToolStripMenuItem";
            this.addEdgeToolStripMenuItem.Size = new System.Drawing.Size(70, 20);
            this.addEdgeToolStripMenuItem.Text = "Add Edge";
            // 
            // FirstNodeComboBox
            // 
            this.FirstNodeComboBox.Name = "FirstNodeComboBox";
            this.FirstNodeComboBox.Size = new System.Drawing.Size(121, 23);
            this.FirstNodeComboBox.Text = "First Node";
            // 
            // SecondNodeComboBox
            // 
            this.SecondNodeComboBox.Name = "SecondNodeComboBox";
            this.SecondNodeComboBox.Size = new System.Drawing.Size(121, 23);
            this.SecondNodeComboBox.Text = "Second Node";
            // 
            // WeightTextBox
            // 
            this.WeightTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.WeightTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.WeightTextBox.Name = "WeightTextBox";
            this.WeightTextBox.Size = new System.Drawing.Size(100, 23);
            this.WeightTextBox.ToolTipText = "Weight";
            // 
            // addEdge
            // 
            this.addEdge.Name = "addEdge";
            this.addEdge.Size = new System.Drawing.Size(181, 22);
            this.addEdge.Text = "Add";
            this.addEdge.Click += new System.EventHandler(this.addEdge_Click);
            // 
            // deleteNodeToolStripMenuItem
            // 
            this.deleteNodeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DeleteNodeComboBox,
            this.deleteToolStripMenuItem});
            this.deleteNodeToolStripMenuItem.Name = "deleteNodeToolStripMenuItem";
            this.deleteNodeToolStripMenuItem.Size = new System.Drawing.Size(84, 20);
            this.deleteNodeToolStripMenuItem.Text = "Delete Node";
            // 
            // DeleteNodeComboBox
            // 
            this.DeleteNodeComboBox.Name = "DeleteNodeComboBox";
            this.DeleteNodeComboBox.Size = new System.Drawing.Size(100, 23);
            this.DeleteNodeComboBox.Text = "Choose Node";
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.DeleteNode_Click);
            // 
            // deleteEdgeToolStripMenuItem
            // 
            this.deleteEdgeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DeleteEdgeComboBox,
            this.deleteToolStripMenuItem1});
            this.deleteEdgeToolStripMenuItem.Name = "deleteEdgeToolStripMenuItem";
            this.deleteEdgeToolStripMenuItem.Size = new System.Drawing.Size(81, 20);
            this.deleteEdgeToolStripMenuItem.Text = "Delete Edge";
            // 
            // DeleteEdgeComboBox
            // 
            this.DeleteEdgeComboBox.Name = "DeleteEdgeComboBox";
            this.DeleteEdgeComboBox.Size = new System.Drawing.Size(121, 23);
            this.DeleteEdgeComboBox.Text = "Choose Edge";
            // 
            // deleteToolStripMenuItem1
            // 
            this.deleteToolStripMenuItem1.Name = "deleteToolStripMenuItem1";
            this.deleteToolStripMenuItem1.Size = new System.Drawing.Size(181, 22);
            this.deleteToolStripMenuItem1.Text = "Delete";
            this.deleteToolStripMenuItem1.Click += new System.EventHandler(this.DeleteEdge_Click);
            // 
            // setStartToolStripMenuItem
            // 
            this.setStartToolStripMenuItem.Name = "setStartToolStripMenuItem";
            this.setStartToolStripMenuItem.Size = new System.Drawing.Size(62, 20);
            this.setStartToolStripMenuItem.Text = "Set Start";
            this.setStartToolStripMenuItem.Click += new System.EventHandler(this.setStartToolStripMenuItem_Click);
            // 
            // setEndToolStripMenuItem
            // 
            this.setEndToolStripMenuItem.Name = "setEndToolStripMenuItem";
            this.setEndToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
            this.setEndToolStripMenuItem.Text = "Set End";
            this.setEndToolStripMenuItem.Click += new System.EventHandler(this.setEndToolStripMenuItem_Click);
            // 
            // findShortestPathToolStripMenuItem
            // 
            this.findShortestPathToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dijksraToolStripMenuItem,
            this.astarToolStripMenuItem});
            this.findShortestPathToolStripMenuItem.Name = "findShortestPathToolStripMenuItem";
            this.findShortestPathToolStripMenuItem.Size = new System.Drawing.Size(115, 20);
            this.findShortestPathToolStripMenuItem.Text = "Find Shortest path";
            // 
            // traverseGraphToolStripMenuItem
            // 
            this.traverseGraphToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dFSToolStripMenuItem,
            this.bFSToolStripMenuItem});
            this.traverseGraphToolStripMenuItem.Name = "traverseGraphToolStripMenuItem";
            this.traverseGraphToolStripMenuItem.Size = new System.Drawing.Size(96, 20);
            this.traverseGraphToolStripMenuItem.Text = "Traverse Graph";
            // 
            // dFSToolStripMenuItem
            // 
            this.dFSToolStripMenuItem.Name = "dFSToolStripMenuItem";
            this.dFSToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.dFSToolStripMenuItem.Text = "DFS";
            this.dFSToolStripMenuItem.Click += new System.EventHandler(this.dFSToolStripMenuItem_Click);
            // 
            // bFSToolStripMenuItem
            // 
            this.bFSToolStripMenuItem.Name = "bFSToolStripMenuItem";
            this.bFSToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.bFSToolStripMenuItem.Text = "BFS";
            this.bFSToolStripMenuItem.Click += new System.EventHandler(this.bFSToolStripMenuItem_Click);
            // 
            // changeLayoutToolStripMenuItem
            // 
            this.changeLayoutToolStripMenuItem.Name = "changeLayoutToolStripMenuItem";
            this.changeLayoutToolStripMenuItem.Size = new System.Drawing.Size(99, 20);
            this.changeLayoutToolStripMenuItem.Text = "Change Layout";
            this.changeLayoutToolStripMenuItem.Click += new System.EventHandler(this.changeLayoutToolStripMenuItem_Click);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.clearToolStripMenuItem.Text = "Clear";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // Result
            // 
            this.Result.Location = new System.Drawing.Point(624, 44);
            this.Result.Name = "Result";
            this.Result.ReadOnly = true;
            this.Result.Size = new System.Drawing.Size(219, 572);
            this.Result.TabIndex = 2;
            this.Result.Text = "";
            // 
            // Canvas
            // 
            this.Canvas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Canvas.Location = new System.Drawing.Point(34, 44);
            this.Canvas.Name = "Canvas";
            this.Canvas.Size = new System.Drawing.Size(583, 572);
            this.Canvas.TabIndex = 0;
            this.Canvas.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseClick);
            this.Canvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseMove);
            // 
            // dijksraToolStripMenuItem
            // 
            this.dijksraToolStripMenuItem.Name = "dijksraToolStripMenuItem";
            this.dijksraToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.dijksraToolStripMenuItem.Text = "Dijksra";
            this.dijksraToolStripMenuItem.Click += new System.EventHandler(this.dijksraToolStripMenuItem_Click);
            // 
            // astarToolStripMenuItem
            // 
            this.astarToolStripMenuItem.Name = "astarToolStripMenuItem";
            this.astarToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.astarToolStripMenuItem.Text = "A-star";
            this.astarToolStripMenuItem.Click += new System.EventHandler(this.astarToolStripMenuItem_Click);
            // 
            // GraphLayout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(888, 682);
            this.Controls.Add(this.Result);
            this.Controls.Add(this.Canvas);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "GraphLayout";
            this.Text = "Form2";
            this.Resize += new System.EventHandler(this.GraphLayout_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DoubleBufferedPanel Canvas;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem addNodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addEdgeToolStripMenuItem;
        private System.Windows.Forms.ToolStripComboBox FirstNodeComboBox;
        private System.Windows.Forms.ToolStripComboBox SecondNodeComboBox;
        private System.Windows.Forms.ToolStripTextBox NodeVal;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteNodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripComboBox DeleteNodeComboBox;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteEdgeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addEdge;
        private System.Windows.Forms.ToolStripMenuItem changeLayoutToolStripMenuItem;
        private System.Windows.Forms.ToolStripComboBox DeleteEdgeComboBox;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem1;
        private System.Windows.Forms.ToolStripTextBox WeightTextBox;
        private System.Windows.Forms.ToolStripMenuItem findShortestPathToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setStartToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setEndToolStripMenuItem;
        private System.Windows.Forms.RichTextBox Result;
        private System.Windows.Forms.ToolStripMenuItem traverseGraphToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dFSToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bFSToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dijksraToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem astarToolStripMenuItem;
    }
}