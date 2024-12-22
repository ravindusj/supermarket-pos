using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Xml.Linq;

namespace supermarket_pos
{
    public partial class inventory : UserControl
    {
        private readonly string connectionString = "Data Source=LAPTOP-G4G46K72\\SQLEXPRESS;Initial Catalog=MINIMART-POS;Integrated Security=True;TrustServerCertificate=True";
        decimal totalValue = 0;

        public inventory()
        {
            InitializeComponent();

            // Load data into DataGridView when the UserControl initializes
            LoadData();
            dataGridView1.CellValueChanged += DataGridView1_CellValueChanged;
            dataGridView1.RowsAdded += DataGridView1_RowsAdded;
            dataGridView1.RowsRemoved += DataGridView1_RowsRemoved;
        }

        // Method to load data into DataGridView
        private void LoadData()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM inventory";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable table = new DataTable();

                    adapter.Fill(table);
                    dataGridView1.DataSource = table;
                    CalculateTotalValue();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading data: " + ex.Message);
                }
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string checkQuery = "SELECT COUNT(*) FROM inventory WHERE productID = @productID";
                    SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                    checkCommand.Parameters.AddWithValue("@productID", textBox1.Text);

                    int count = (int)checkCommand.ExecuteScalar();

                    if (count == 0)
                    {
                        string insertQuery = "INSERT INTO inventory (productID, name, quantity, retail_price, whole_price) VALUES (@productID, @name, @quantity, @retail_price,@whole_price)";
                        SqlCommand insertCommand = new SqlCommand(insertQuery, connection);

                        insertCommand.Parameters.AddWithValue("@productID", textBox1.Text);
                        insertCommand.Parameters.AddWithValue("@name", textBox2.Text);
                        insertCommand.Parameters.AddWithValue("@quantity", textBox3.Text);
                        insertCommand.Parameters.AddWithValue("@retail_price", textBox4.Text);
                        insertCommand.Parameters.AddWithValue("@whole_price", textBox7.Text);

                        insertCommand.ExecuteNonQuery();
                        MessageBox.Show("Product added successfully!");

                        // Refresh the DataGridView
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Product ID already exists.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int productId;

                if (int.TryParse(dataGridView1.SelectedRows[0].Cells["productID"].Value.ToString(), out productId))
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        try
                        {
                            connection.Open();

                            string deleteQuery = "DELETE FROM inventory WHERE productID = @productID";
                            SqlCommand command = new SqlCommand(deleteQuery, connection);
                            command.Parameters.AddWithValue("@productID", productId);

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Product deleted successfully!");
                                LoadData(); // Refresh the DataGridView
                            }
                            else
                            {
                                MessageBox.Show("Error deleting product.");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error: " + ex.Message);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Invalid product ID.");
                }
            }
            else
            {
                MessageBox.Show("Please select a product to delete.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Check if a row is selected
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                // Extract data from the selected row
                textBox1.Text = selectedRow.Cells["productID"].Value.ToString();
                textBox2.Text = selectedRow.Cells["name"].Value.ToString();
                textBox3.Text = selectedRow.Cells["quantity"].Value.ToString();
                textBox4.Text = selectedRow.Cells["retail_price"].Value.ToString();
                textBox7.Text = selectedRow.Cells["whole_price"].Value.ToString();
            }
            else
            {
                MessageBox.Show("Please select a row to edit.");
            }
        }

        private void UpdateRecord()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string updateQuery = "UPDATE inventory SET name = @name, quantity = @quantity, retail_price = @retail_price, whole_price = @whole_price WHERE productID = @productID";
                    SqlCommand updateCommand = new SqlCommand(updateQuery, connection);

                    // Use text box values for the update
                    updateCommand.Parameters.AddWithValue("@productID", textBox1.Text);
                    updateCommand.Parameters.AddWithValue("@name", textBox2.Text);
                    updateCommand.Parameters.AddWithValue("@quantity", textBox3.Text);
                    updateCommand.Parameters.AddWithValue("@retail_price", textBox4.Text);
                    updateCommand.Parameters.AddWithValue("@whole_price", textBox7.Text);

                    int rowsAffected = updateCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Record updated successfully!");

                        // Refresh the DataGridView
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Error updating record.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Recalculate total when a cell value changes
            CalculateTotalValue();
        }

        private void DataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            // Recalculate total when rows are added
            CalculateTotalValue();
        }

        private void DataGridView1_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            // Recalculate total when rows are removed
            CalculateTotalValue();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            UpdateRecord();
            CalculateTotalValue();
        }

        private void label7_Click(object sender, EventArgs e)
        {
        }

        private void label5_Click(object sender, EventArgs e)
        {
        }

        private void CalculateTotalValue()
        {
            try
            {
                // Initialize the total value
                totalValue = 0;

                // Loop through the DataGridView rows
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    // Ensure the row is not a new row
                    if (!row.IsNewRow)
                    {
                        // Access the quantity and price cells directly
                        if (row.Cells["quantity"].Value != null && row.Cells["whole_price"].Value != null)
                        {
                            // Parse quantity and price
                            if (int.TryParse(row.Cells["quantity"].Value.ToString(), out int quantity) &&
                                decimal.TryParse(row.Cells["whole_price"].Value.ToString(), out decimal whole_price))
                            {
                                // Calculate the product and add it to the total
                                totalValue += quantity * whole_price;
                            }
                        }
                    }
                }

                // Update the label with the calculated total value
                label5.Text = totalValue.ToString("C");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error calculating total value: " + ex.Message);
            }
        }


        private void FilterData()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Base query for filtering
                    string query = "SELECT * FROM inventory WHERE 1=1";

                    // Append conditions if search boxes are not empty
                    if (!string.IsNullOrEmpty(textBox5.Text))
                    {
                        query += " AND productID LIKE @productID";
                    }

                    if (!string.IsNullOrEmpty(textBox6.Text))
                    {
                        query += " AND name LIKE @name";
                    }

                    SqlCommand command = new SqlCommand(query, connection);

                    // Add parameters if the search boxes are not empty
                    if (!string.IsNullOrEmpty(textBox5.Text))
                    {
                        command.Parameters.AddWithValue("@productID", "%" + textBox5.Text.Trim() + "%");
                    }

                    if (!string.IsNullOrEmpty(textBox6.Text))
                    {
                        command.Parameters.AddWithValue("@name", "%" + textBox6.Text.Trim() + "%");
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable filteredTable = new DataTable();
                    adapter.Fill(filteredTable);

                    // Update DataGridView with the filtered data
                    dataGridView1.DataSource = filteredTable;

                    // Optionally recalculate the total value
                    CalculateTotalValue();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error filtering data: " + ex.Message);
                }
            }
        }


        private void inventory_Load(object sender, EventArgs e)
        {
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            FilterData();
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            FilterData();
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
