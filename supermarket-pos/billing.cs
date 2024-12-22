using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace supermarket_pos
{
    public partial class billing : UserControl
    {
        private readonly string connectionString = "Data Source=LAPTOP-G4G46K72\\SQLEXPRESS;Initial Catalog=MINIMART-POS;Integrated Security=True;TrustServerCertificate=True";
        private decimal totalBill = 0;

        public billing()
        {
            InitializeComponent();
            InitializeDataGridView();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            string billNo = textBox1.Text;
            string date = label8.Text;
            string totalAmount = label9.Text;
            string payment = textBox2.Text;
            string cashier = textBox4.Text;

        }
        private void InitializeDataGridView()
        {
            // Initialize DataGridView columns if they don't exist
            if (dataGridView1.Columns.Count == 0)
            {
                dataGridView1.Columns.Add("ProductID", "Product ID");
                dataGridView1.Columns.Add("ProductName", "Product Name");
                dataGridView1.Columns.Add("Quantity", "Quantity");
                dataGridView1.Columns.Add("RetailPrice", "Price");
                dataGridView1.Columns.Add("TotalAmount", "Total");
            }

            // Set properties for better display
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }
        private void label7_Click(object sender, EventArgs e)
        {

        }
        private void RetrieveProductData()
        {
            if (string.IsNullOrWhiteSpace(textBox5.Text))
                return;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT productID, name, retail_price FROM inventory WHERE productID = @productID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@productID", textBox5.Text);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // You can optionally show product details in other textboxes here
                                // For example:
                                // txtProductName.Text = reader["name"].ToString();
                                // txtPrice.Text = reader["retail_price"].ToString();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error retrieving product data: " + ex.Message);
                }
            }
        }
        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void AddProductToGrid()
        {
            if (string.IsNullOrWhiteSpace(textBox5.Text))
            {
                MessageBox.Show("Please enter a Product ID");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT productID, name, retail_price FROM inventory WHERE productID = @productID";
                   

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@productID", textBox5.Text);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int quantity;
                                if (!int.TryParse(textBox6.Text, out quantity) || quantity <= 0)
                                {
                                    MessageBox.Show("Please enter a valid quantity");
                                    return;
                                }

                                string productId = reader["productID"].ToString();
                                string productName = reader["name"].ToString();
                                decimal retailPrice = Convert.ToDecimal(reader["retail_price"]);
                                decimal totalPrice = retailPrice * quantity;

                                // Add to DataGridView
                                dataGridView1.Rows.Add(productId, productName, quantity, retailPrice, totalPrice);

                                // Update total bill
                                UpdateTotalBill();

                                // Clear input fields
                                textBox5.Clear();
                                textBox6.Clear();
                                textBox5.Focus();
                            }
                            else
                            {
                                MessageBox.Show("Product not found!");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error adding product: " + ex.Message);
                }
            }
        }

        private void UpdateTotalBill()
        {
            totalBill = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                totalBill += Convert.ToDecimal(row.Cells["TotalAmount"].Value);
            }
            // Update the total bill label/textbox
            // Assuming you have a label named lblTotalBill
            label9.Text = "Total Amount: " + totalBill.ToString("C");  // C format specifier for currency
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void UpdateDateTime()
        {
            label8.Text =  DateTime.Now.ToString("yyyy-MM-dd") + "  "+ DateTime.Now.ToString("HH:mm:ss");
        }

        // Event handlers
        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateDateTime();
        }

        private void billing_Load(object sender, EventArgs e)
        {
            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += timer1_Tick;
            timer.Start();
            UpdateDateTime();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            RetrieveProductData();
        }
        private void label8_Click_1(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddProductToGrid();
        }

        // Add method to handle product deletion
        private void DeleteSelectedProduct()
        {
            if (dataGridView1.CurrentRow != null)
            {
                dataGridView1.Rows.RemoveAt(dataGridView1.CurrentRow.Index);
                UpdateTotalBill();
            }
        }

        // Add method to handle bill printing
        private void PrintBill()
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("No items to print");
                return;
            }
            // Implement your printing logic here
        }

        // Add method to handle payment
        private void ProcessPayment()
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("No items in the bill");
                return;
            }
            // Implement your payment processing logic here
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Remove the selected row
                dataGridView1.Rows.RemoveAt(dataGridView1.CurrentRow.Index);

                // Update the total bill after deletion
                UpdateTotalBill();

                // Optional: Show confirmation message
                MessageBox.Show("Item removed successfully!");
            }
            else
            {
                MessageBox.Show("Please select a row to delete.");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            discount discount = new discount();
            discount.ShowDialog();
        }
    }

}

