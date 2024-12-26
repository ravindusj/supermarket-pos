using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace supermarket_pos
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            loginpage login = new loginpage();
            this.Hide();
            login.Show();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            inventory1.BringToFront();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sales1.BringToFront();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            billing1.BringToFront();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            report1.BringToFront();
        }

        private void billing1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            customers1.BringToFront();
        }
    }
}
