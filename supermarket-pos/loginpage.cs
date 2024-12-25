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
    public partial class loginpage : Form
    {
        public loginpage()
        {
            InitializeComponent();
            this.Text = "Login - Mini Mart";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;

            //if (username == credentials.username && password == credentials.password)
            //{
            //    Dashboard dash = new Dashboard();
            //    this.Hide();
            //    dash.Show();
            //}
            //else
            //{
            //    MessageBox.Show("Invalid username or password");
            //}
            Dashboard dash = new Dashboard();
            this.Hide();
            dash.Show();


        }

        private void loginpage_Load(object sender, EventArgs e)
        {

        }
    }
}
