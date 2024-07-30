namespace A_star
{
    partial class MainMenu
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
            this.label1 = new System.Windows.Forms.Label();
            this.GraphLayoutBtn = new System.Windows.Forms.Button();
            this.GridLayoutBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(187, 103);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(390, 31);
            this.label1.TabIndex = 0;
            this.label1.Text = "Graph theory and Square Grids";
            // 
            // GraphLayoutBtn
            // 
            this.GraphLayoutBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.GraphLayoutBtn.Location = new System.Drawing.Point(153, 220);
            this.GraphLayoutBtn.Name = "GraphLayoutBtn";
            this.GraphLayoutBtn.Size = new System.Drawing.Size(139, 81);
            this.GraphLayoutBtn.TabIndex = 1;
            this.GraphLayoutBtn.Text = "Graph";
            this.GraphLayoutBtn.UseVisualStyleBackColor = true;
            this.GraphLayoutBtn.Click += new System.EventHandler(this.GraphLayout_Click);
            // 
            // GridLayoutBtn
            // 
            this.GridLayoutBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.GridLayoutBtn.Location = new System.Drawing.Point(472, 220);
            this.GridLayoutBtn.Name = "GridLayoutBtn";
            this.GridLayoutBtn.Size = new System.Drawing.Size(139, 81);
            this.GridLayoutBtn.TabIndex = 2;
            this.GridLayoutBtn.Text = "Square Grid";
            this.GridLayoutBtn.UseVisualStyleBackColor = true;
            this.GridLayoutBtn.Click += new System.EventHandler(this.GridLayout_Click);
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(769, 468);
            this.Controls.Add(this.GridLayoutBtn);
            this.Controls.Add(this.GraphLayoutBtn);
            this.Controls.Add(this.label1);
            this.Name = "MainMenu";
            this.Text = "MainMenu";
            this.Load += new System.EventHandler(this.MainMenu_Load);
            this.Resize += new System.EventHandler(this.MainMenu_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button GraphLayoutBtn;
        private System.Windows.Forms.Button GridLayoutBtn;
    }
}