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

            LoadData();
            dataGridView1.CellValueChanged += DataGridView1_CellValueChanged;
            dataGridView1.RowsAdded += DataGridView1_RowsAdded;
            dataGridView1.RowsRemoved += DataGridView1_RowsRemoved;
        }

        private void LoadData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM InventoryView";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, conn))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);


                        dataGridView1.DataSource = dataTable;

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading staff data: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                                LoadData();
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
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

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

                    updateCommand.Parameters.AddWithValue("@productID", textBox1.Text);
                    updateCommand.Parameters.AddWithValue("@name", textBox2.Text);
                    updateCommand.Parameters.AddWithValue("@quantity", textBox3.Text);
                    updateCommand.Parameters.AddWithValue("@retail_price", textBox4.Text);
                    updateCommand.Parameters.AddWithValue("@whole_price", textBox7.Text);

                    int rowsAffected = updateCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Record updated successfully!");

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
            CalculateTotalValue();
        }

        private void DataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            CalculateTotalValue();
        }

        private void DataGridView1_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
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
                totalValue = 0;

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        if (row.Cells["quantity"].Value != null && row.Cells["whole_price"].Value != null)
                        {
                            if (int.TryParse(row.Cells["quantity"].Value.ToString(), out int quantity) &&
                                decimal.TryParse(row.Cells["whole_price"].Value.ToString(), out decimal whole_price))
                            {
                                totalValue += quantity * whole_price;
                            }
                        }
                    }
                }

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

                    string query = "SELECT * FROM InventoryView WHERE 1=1";

                    if (!string.IsNullOrEmpty(textBox5.Text))
                    {
                        query += " AND productID LIKE @productID";
                    }

                    if (!string.IsNullOrEmpty(textBox6.Text))
                    {
                        query += " AND name LIKE @name";
                    }

                    SqlCommand command = new SqlCommand(query, connection);

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

                    dataGridView1.DataSource = filteredTable;

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
            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += timer1_Tick;
            timer.Start();
            UpdateDateTime();
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

        private void label11_Click(object sender, EventArgs e)
        {

        }
        private void UpdateDateTime()
        {
            label11.Text = DateTime.Now.ToString("yyyy-MM-dd") + "  " + DateTime.Now.ToString("HH:mm:ss");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateDateTime();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void ResetForm()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox7.Clear();
        }
    }
}
