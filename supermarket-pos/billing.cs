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
        private decimal discountAmount = 0;
        private Random random = new Random(); // For bill number generation

        public billing()
        {
            InitializeComponent();
            InitializeDataGridView();
            GenerateUniqueBillNumber(); // Generate initial bill number

            dateTimePicker1.Value = DateTime.Now;
            // Set the format to show date only
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy-MM-dd";
        }

        // New methods for bill number generation
        private bool IsBillNumberExists(string billNumber)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM bills WHERE bill_no = @billNo";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@billNo", billNumber);
                        int count = (int)command.ExecuteScalar();
                        return count > 0;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        private void GenerateUniqueBillNumber()
        {
            string billNumber;
            do
            {
                string datepart = DateTime.Now.ToString("yyyyMMdd");
                string randomPart = random.Next(1000, 9999).ToString();
                billNumber = $"BILL-{datepart}-{randomPart}";
            } while (IsBillNumberExists(billNumber));

            textBox1.Text = billNumber;
        }






        private void label7_Click(object sender, EventArgs e)
        {
        }
        private void label9_Click(object sender, EventArgs e)
        {
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
        }



        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }



        private void label8_Click_1(object sender, EventArgs e)
        {
        }



        private void label11_Click(object sender, EventArgs e)
        {
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }


        private void label10_Click(object sender, EventArgs e)
        {
        }
        private void UpdateSalesForDate(DateTime date)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Calculate total sales for the date
                    string getTotalQuery = @"
                        SELECT SUM(total) 
                        FROM billing 
                        WHERE CAST(date AS DATE) = @date";

                    decimal dailyTotal = 0;
                    using (SqlCommand command = new SqlCommand(getTotalQuery, connection))
                    {
                        command.Parameters.AddWithValue("@date", date.Date);
                        object result = command.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            dailyTotal = Convert.ToDecimal(result);
                        }
                    }

                    // Try to update existing record first
                    string updateQuery = @"
                        UPDATE sales 
                        SET total_amount_of_sale = @total 
                        WHERE date = @date";

                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@date", date.Date);
                        command.Parameters.AddWithValue("@total", dailyTotal);
                        int rowsAffected = command.ExecuteNonQuery();

                        // If no record exists, insert new one
                        if (rowsAffected == 0)
                        {
                            string insertQuery = @"
                                INSERT INTO sales (date, total_amount_of_sale) 
                                VALUES (@date, @total)";

                            command.CommandText = insertQuery;
                            command.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating sales record: " + ex.Message);
                }
            }
        }

        private void UpdateProfitsForDate(DateTime date)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Calculate total profits for the date
                    string getTotalProfitQuery = @"
                SELECT SUM((bd.quantity * (inv.retail_price - inv.whole_price))) as daily_profit
                FROM bill_details bd
                JOIN Billing b ON bd.billing_no = b.billing_no
                JOIN inventory inv ON bd.productID = inv.productID
                WHERE CAST(b.date AS DATE) = @date";

                    decimal dailyProfit = 0;
                    using (SqlCommand command = new SqlCommand(getTotalProfitQuery, connection))
                    {
                        command.Parameters.AddWithValue("@date", date.Date);
                        object result = command.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            dailyProfit = Convert.ToDecimal(result);
                        }
                    }

                    // Try to update existing record first
                    string updateQuery = @"
                UPDATE daily_profits 
                SET total_profit = @profit 
                WHERE date = @date";

                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@date", date.Date);
                        command.Parameters.AddWithValue("@profit", dailyProfit);
                        int rowsAffected = command.ExecuteNonQuery();

                        // If no record exists, insert new one
                        if (rowsAffected == 0)
                        {
                            string insertQuery = @"
                        INSERT INTO daily_profits (date, total_profit) 
                        VALUES (@date, @profit)";

                            command.CommandText = insertQuery;
                            command.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating profits record: " + ex.Message);
                }
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            string billNo = textBox1.Text;
            string date = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string payment = comboBox1.Text;
            string cashier = textBox4.Text;

            decimal totalAmount;
            if (!decimal.TryParse(label11.Text.Replace("$", "").Replace(",", ""), out totalAmount))
            {
                MessageBox.Show("Invalid total amount");
                return;
            }

            if (string.IsNullOrWhiteSpace(billNo) || string.IsNullOrWhiteSpace(date) ||
                string.IsNullOrWhiteSpace(payment) || string.IsNullOrWhiteSpace(cashier))
            {
                MessageBox.Show("Please fill all the fields");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            string query = "INSERT INTO billing (billing_no, date, payment, cashier, total) VALUES (@billing_no, @date, @payment, @cashier, @total)";
                            using (SqlCommand command = new SqlCommand(query, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@billing_no", billNo);
                                command.Parameters.AddWithValue("@date", date);
                                command.Parameters.AddWithValue("@payment", payment);
                                command.Parameters.AddWithValue("@cashier", cashier);
                                command.Parameters.AddWithValue("@total", totalAmount);

                                command.ExecuteNonQuery();
                            }

                            foreach (DataGridViewRow row in dataGridView1.Rows)
                            {
                                if (row.Cells["ProductID"].Value == null) continue;

                                string productId = row.Cells["ProductID"].Value.ToString();
                                int quantity = Convert.ToInt32(row.Cells["Quantity"].Value);
                                decimal price = Convert.ToDecimal(row.Cells["RetailPrice"].Value);
                                decimal rowTotal = Convert.ToDecimal(row.Cells["TotalAmount"].Value);

                                query = "INSERT INTO bill_details (billing_no, productID, quantity, price, total) VALUES (@billing_no, @productID, @quantity, @price, @total)";
                                using (SqlCommand command = new SqlCommand(query, connection, transaction))
                                {
                                    command.Parameters.AddWithValue("@billing_no", billNo);
                                    command.Parameters.AddWithValue("@productId", productId);
                                    command.Parameters.AddWithValue("@quantity", quantity);
                                    command.Parameters.AddWithValue("@price", price);
                                    command.Parameters.AddWithValue("@total", totalAmount);
                                    command.ExecuteNonQuery();
                                }

                                // Update inventory quantity
                                query = "UPDATE inventory SET quantity = quantity - @quantity WHERE productID = @productId";
                                using (SqlCommand command = new SqlCommand(query, connection, transaction))
                                {
                                    command.Parameters.AddWithValue("@quantity", quantity);
                                    command.Parameters.AddWithValue("@productId", productId);
                                    command.ExecuteNonQuery();
                                }
                            }

                            transaction.Commit();

                            UpdateSalesForDate(dateTimePicker1.Value);
                            UpdateProfitsForDate(dateTimePicker1.Value);

                            MessageBox.Show("Bill saved successfully!");
                            GenerateUniqueBillNumber();
                            dataGridView1.Rows.Clear();
                            totalBill = 0;
                            discountAmount = 0;
                            label9.Text = "Total Amount: 0.00";
                            label10.Text = "Discount: 0.00";
                            label11.Text = "$0.00";
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving bill: " + ex.Message);
                }
            }
        }

        private void InitializeDataGridView()
        {
            if (dataGridView1.Columns.Count == 0)
            {
                dataGridView1.Columns.Add("ProductID", "Product ID");
                dataGridView1.Columns.Add("ProductName", "Product Name");
                dataGridView1.Columns.Add("Quantity", "Quantity");
                dataGridView1.Columns.Add("RetailPrice", "Price");
                dataGridView1.Columns.Add("TotalAmount", "Total");
            }

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
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
                                // Product details logic remains the same
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

                                dataGridView1.Rows.Add(productId, productName, quantity, retailPrice, totalPrice);
                                UpdateTotalBill();

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
            label9.Text = "Total Amount: " + totalBill.ToString("C");

            // Update final amount after discount
            decimal finalAmount = totalBill - discountAmount;
            label11.Text = finalAmount.ToString("C");
        }

        private void UpdateDateTime()
        {
            label8.Text = DateTime.Now.ToString("yyyy-MM-dd") + "  " + DateTime.Now.ToString("HH:mm:ss");
        }

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
            GenerateUniqueBillNumber(); // Generate unique bill number when form loads
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            RetrieveProductData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddProductToGrid();
        }

        private void DeleteSelectedProduct()
        {
            if (dataGridView1.CurrentRow != null)
            {
                dataGridView1.Rows.RemoveAt(dataGridView1.CurrentRow.Index);
                UpdateTotalBill();
            }
        }

        private void PrintBill()
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("No items to print");
                return;
            }
        }

        private void ProcessPayment()
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("No items in the bill");
                return;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                dataGridView1.Rows.RemoveAt(dataGridView1.CurrentRow.Index);
                UpdateTotalBill();
                MessageBox.Show("Item removed successfully!");
            }
            else
            {
                MessageBox.Show("Please select a row to delete.");
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            discount discountForm = new discount();
            if (discountForm.ShowDialog() == DialogResult.OK)
            {
                decimal discountValue = discountForm.DiscountValue;
                bool isPercentage = discountForm.IsPercentage;

                if (isPercentage)
                {
                    label10.Text = "Discount: " + discountValue.ToString("0.##") + "%";
                    discountAmount = totalBill * (discountValue / 100);
                }
                else
                {
                    label10.Text = "Discount: " + discountValue.ToString("C");
                    discountAmount = discountValue;
                }

                decimal finalAmount = totalBill - discountAmount;
                label11.Text = finalAmount.ToString("C");
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }
    }



}
