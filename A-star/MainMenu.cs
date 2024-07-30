using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace A_star
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void MainMenu_Load(object sender, EventArgs e)
        {
            //this.WindowState = FormWindowState.Maximized;
            this.Size = new System.Drawing.Size(800, 800);
        }

        private void GraphLayout_Click(object sender, EventArgs e)
        {
            this.Hide();
            GraphLayout gridLayout = new GraphLayout();
            gridLayout.ShowDialog();
            this.Close();
        }

        private void GridLayout_Click(object sender, EventArgs e)
        {
            this.Hide(); // Hide the current form
            Gridlayout gridLayout = new Gridlayout(); // Show the current form again when GridLayout is closed
            gridLayout.ShowDialog(); // Show the GridLayout form non-modally
            this.Close();
        }

        private void MainMenu_Resize(object sender, EventArgs e)
        {
            label1.Top = this.Height / 3;
            label1.Left = (this.Width - label1.Width) / 2;

            GraphLayoutBtn.Left = this.Width / 4;
            GridLayoutBtn.Left = 3 * this.Width / 4 - GridLayoutBtn.Width;
            GraphLayoutBtn.Top = GridLayoutBtn.Top =(int)(1.5 * this.Height / 3);
        }
    }

    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            System.Windows.Forms.Application.Run(new MainMenu());
        }
    }
}
