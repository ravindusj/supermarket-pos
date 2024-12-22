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
    public partial class discount : Form
    {
        public discount()
        {
            InitializeComponent();
            this.Text = "Discount";

            // Populate the dropdown with options
            comboBox1.Items.Add("Amount");
            comboBox1.Items.Add("Percentage");

            // Set default selection
            comboBox1.SelectedIndex = 0; // Default to "Amount"

            // Configure initial state of textboxes
            textBox1.Enabled = true;  // Amount textbox editable
            textBox2.Enabled = false; // Percentage textbox uneditable

            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            button3.Click += button3_Click;
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString() == "Amount")
            {
                textBox1.Enabled = true;  // Enable Amount textbox
                textBox2.Enabled = false; // Disable Percentage textbox
            }
            else if (comboBox1.SelectedItem.ToString() == "Percentage")
            {
                textBox1.Enabled = false; // Disable Amount textbox
                textBox2.Enabled = true;  // Enable Percentage textbox
            }
        }

        private void discount_Load(object sender, EventArgs e)
        {
            // Attach event handler for comboBox selection change
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;


        }

    }
}
