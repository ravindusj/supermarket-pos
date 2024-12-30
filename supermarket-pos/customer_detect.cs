using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace supermarket_pos
{
    public partial class customer_detect : Form
    {
        private readonly string connectionString = "Data Source=LAPTOP-G4G46K72\\SQLEXPRESS;Initial Catalog=MINIMART-POS;Integrated Security=True;TrustServerCertificate=True";
        public string CustomerName { get; private set; }
        public customer_detect()
        {
            InitializeComponent();
            this.Text = "Customer ?";
        }
        private void customer_detect_Load(object sender, EventArgs e)
        {

        }

        private void phonenum_TextChanged(object sender, EventArgs e)
        {

        }

        private void ok_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(phonenum.Text))
            {
                MessageBox.Show("Please enter a phone number.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (phonenum.Text.Length != 10 || !phonenum.Text.All(char.IsDigit))
            {
                MessageBox.Show("Please enter a valid 10-digit phone number.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT customer_name FROM customer_details WHERE phone_number = @phone";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@phone", phonenum.Text);
                        var result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            CustomerName = result.ToString();
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("No customer found with this phone number.", "Customer Not Found",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error checking customer: " + ex.Message, "Database Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
 }
 

