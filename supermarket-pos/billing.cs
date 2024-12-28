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

            if (!string.IsNullOrEmpty(UserSession.StaffName))
            {
                textBox4.Text = UserSession.StaffName;
                textBox4.ReadOnly = true; // Make it read-only
            }
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
 
        private void button3_Click(object sender, EventArgs e)
        {
            string billNo = textBox1.Text;
            string date = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string payment = comboBox1.Text;
            string cashier = textBox4.Text;
            string customername = customerno.Text;

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

                    // Save main bill
                    using (SqlCommand command = new SqlCommand("sp_SaveCompleteBill", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@billing_no", billNo);
                        command.Parameters.AddWithValue("@date", date);
                        command.Parameters.AddWithValue("@payment", payment);
                        command.Parameters.AddWithValue("@cashier", cashier);
                        command.Parameters.AddWithValue("@total", totalAmount);
                        command.Parameters.AddWithValue("@customer_name", customername);
                        command.ExecuteNonQuery();
                    }

                    // Save bill details and update inventory for each product
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.Cells["ProductID"].Value == null) continue;

                        string productId = row.Cells["ProductID"].Value.ToString();
                        int quantity = Convert.ToInt32(row.Cells["Quantity"].Value);
                        decimal price = Convert.ToDecimal(row.Cells["RetailPrice"].Value);
                        decimal rowTotal = Convert.ToDecimal(row.Cells["TotalAmount"].Value);

                        using (SqlCommand command = new SqlCommand("sp_SaveBillDetailsAndUpdateInventory", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@billing_no", billNo);
                            command.Parameters.AddWithValue("@productID", productId);
                            command.Parameters.AddWithValue("@quantity", quantity);
                            command.Parameters.AddWithValue("@price", price);
                            command.Parameters.AddWithValue("@total", totalAmount);
                            command.Parameters.AddWithValue("@customer_name", customername);
                            command.ExecuteNonQuery();
                        }
                    }

                    // Update daily sales and profits
                    using (SqlCommand command = new SqlCommand("sp_UpdateDailySalesAndProfits", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@date", dateTimePicker1.Value.Date);
                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Bill saved successfully!");
                    GenerateUniqueBillNumber();
                    dataGridView1.Rows.Clear();
                    totalBill = 0;
                    discountAmount = 0;
                    label9.Text = "Total Amount: 0.00";
                    label10.Text = "Discount: 0.00";
                    label11.Text = "$0.00";
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

        private void button4_Click(object sender, EventArgs e)
        {
            customer_detect customerDetectForm = new customer_detect();
            if (customerDetectForm.ShowDialog() == DialogResult.OK)
            {
                customerno.Text = customerDetectForm.CustomerName;
            }


        }
        private void customerno_TextChanged(object sender, EventArgs e)
        {

        }
    }



}
