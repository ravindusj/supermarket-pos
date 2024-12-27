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
    public partial class customers : UserControl
    {
        private readonly string connectionString = "Data Source=LAPTOP-G4G46K72\\SQLEXPRESS;Initial Catalog=MINIMART-POS;Integrated Security=True;TrustServerCertificate=True";
        private int currentCustomerId = 0;
        public customers()
        {
            InitializeComponent();

        }

        // Method to get the next customer ID
        private int GetNextCustomerId()
        {
            int nextId = 1;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT MAX(customer_id) FROM customer_details";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    var result = cmd.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        nextId = Convert.ToInt32(result) + 1;
                    }
                }
            }
            return nextId;
        }
        // Reset form fields
        private void ResetForm()
        {
            textBox4.Clear(); // Assuming textBox1 is for customer name
            textBox2.Clear(); // Assuming textBox2 is for phone number
            textBox3.Clear(); // Assuming textBox3 is for email
    
            textBox1.Text = GetNextCustomerId().ToString(); // Assuming textBox4 is for customer ID
        }
        // Load event - initialize the form
        private void customers_Load(object sender, EventArgs e)
        {
            textBox1.Text = GetNextCustomerId().ToString();
            textBox1.ReadOnly = true; // Make customer ID read-only
            LoadCustomerData();

            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += timer1_Tick;
            timer.Start();
            UpdateDateTime();
        }
        private void LoadCustomerData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM vw_customer_details";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }
        private void billingBtn_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBox2.Text) ||
                    string.IsNullOrWhiteSpace(textBox3.Text) ||
                    string.IsNullOrWhiteSpace(textBox4.Text))
                {
                    MessageBox.Show("Please fill in all required fields.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string insertQuery = @"INSERT INTO customer_details 
                                     (customer_id, customer_name, phone_number, email) 
                                     VALUES (@customer_id, @customer_name, @phone_number, @email)";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@customer_id", int.Parse(textBox1.Text));
                        cmd.Parameters.AddWithValue("@customer_name", textBox2.Text.Trim());
                        cmd.Parameters.AddWithValue("@phone_number", textBox3.Text.Trim());
                        cmd.Parameters.AddWithValue("@email", textBox4.Text.Trim());

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Customer added successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetForm();
                LoadCustomerData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding customer: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    int customerId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Customer ID"].Value);

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string deleteQuery = "DELETE FROM customer_details WHERE customer_id = @customer_id";
                        using (SqlCommand cmd = new SqlCommand(deleteQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@customer_id", customerId);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    LoadCustomerData();
                    ResetForm();
                    MessageBox.Show("Customer deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting customer: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    DataGridViewRow row = dataGridView1.SelectedRows[0];

                    // Populate textboxes with selected row data
                    textBox1.Text = row.Cells["Customer ID"].Value.ToString();
                    textBox2.Text = row.Cells["Customer Name"].Value.ToString();
                    textBox3.Text = row.Cells["Phone Number"].Value.ToString();
                    textBox4.Text = row.Cells["Email"].Value.ToString();
                }
                else
                {
                    MessageBox.Show("Please select a row to update.", "Selection Required",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error selecting customer: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void save_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                    string.IsNullOrWhiteSpace(textBox2.Text) ||
                    string.IsNullOrWhiteSpace(textBox3.Text) ||
                    string.IsNullOrWhiteSpace(textBox4.Text))
                {
                    MessageBox.Show("Please fill in all required fields.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string updateQuery = @"UPDATE customer_details 
                                 SET customer_name = @customer_name,
                                     phone_number = @phone_number,
                                     email = @email
                                 WHERE customer_id = @customer_id";

                    using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@customer_id", int.Parse(textBox1.Text));
                        cmd.Parameters.AddWithValue("@customer_name", textBox2.Text.Trim());
                        cmd.Parameters.AddWithValue("@phone_number", textBox3.Text.Trim());
                        cmd.Parameters.AddWithValue("@email", textBox4.Text.Trim());

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Customer updated successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadCustomerData();
                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating customer: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void FilterData()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // First, let's verify the column names from the view
                    string baseQuery = "SELECT TOP 1 * FROM vw_customer_details";
                    using (SqlCommand cmd = new SqlCommand(baseQuery, connection))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable schemaTable = reader.GetSchemaTable();
                        reader.Close();
                    }

                    // Base query for filtering - use the column names as they appear in your view
                    string query = "SELECT * FROM vw_customer_details WHERE 1=1";

                    // Append conditions if search boxes are not empty
                    if (!string.IsNullOrEmpty(textBox5.Text))
                    {
                        // Assuming the column name in the view is "Customer Name" (with a space)
                        query += " AND [Customer Name] LIKE @customer_name";
                    }

                    if (!string.IsNullOrEmpty(textBox6.Text))
                    {
                        // Assuming the column name in the view is "Phone Number" (with a space)
                        query += " AND [Phone Number] LIKE @phone_number";
                    }

                    SqlCommand command = new SqlCommand(query, connection);

                    // Add parameters if the search boxes are not empty
                    if (!string.IsNullOrEmpty(textBox5.Text))
                    {
                        command.Parameters.AddWithValue("@customer_name", "%" + textBox5.Text.Trim() + "%");
                    }

                    if (!string.IsNullOrEmpty(textBox6.Text))
                    {
                        command.Parameters.AddWithValue("@phone_number", "%" + textBox6.Text.Trim() + "%");
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable filteredTable = new DataTable();
                    adapter.Fill(filteredTable);

                    // Update DataGridView with the filtered data
                    dataGridView1.DataSource = filteredTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error filtering data: " + ex.Message);
                }
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            FilterData();
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            FilterData();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateDateTime();
        }
        private void UpdateDateTime()
        {
            label11.Text = DateTime.Now.ToString("yyyy-MM-dd") + "  " + DateTime.Now.ToString("HH:mm:ss");
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }

}
