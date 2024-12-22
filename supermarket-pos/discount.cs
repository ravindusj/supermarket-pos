using System;
using System.Windows.Forms;

namespace supermarket_pos
{
    public partial class discount : Form
    {
        public decimal DiscountValue { get; private set; }
        public bool IsPercentage { get; private set; }

        public discount()
        {
            InitializeComponent();
            this.Text = "Discount";

            comboBox1.Items.Add("Amount");
            comboBox1.Items.Add("Percentage");
            comboBox1.SelectedIndex = 0;

            textBox1.Enabled = true;
            textBox2.Enabled = false;

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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString() == "Amount")
            {
                textBox1.Enabled = true;
                textBox2.Enabled = false;
                textBox2.Clear(); // Clear percentage textbox
            }
            else if (comboBox1.SelectedItem.ToString() == "Percentage")
            {
                textBox1.Enabled = false;
                textBox2.Enabled = true;
                textBox1.Clear(); // Clear amount textbox
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString() == "Amount")
            {
                if (decimal.TryParse(textBox1.Text, out decimal amount))
                {
                    DiscountValue = amount;
                    IsPercentage = false;
                }
                else
                {
                    MessageBox.Show("Please enter a valid Amount.");
                    return;
                }
            }
            else if (comboBox1.SelectedItem.ToString() == "Percentage")
            {
                if (decimal.TryParse(textBox2.Text, out decimal percentage))
                {
                    if (percentage <= 0 || percentage > 100)
                    {
                        MessageBox.Show("Please enter a percentage between 0 and 100.");
                        return;
                    }
                    DiscountValue = percentage;
                    IsPercentage = true;
                }
                else
                {
                    MessageBox.Show("Please enter a valid Percentage.");
                    return;
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void discount_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
        }
    }
}