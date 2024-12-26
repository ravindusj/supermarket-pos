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
    public partial class reven : UserControl


    {
        private readonly string connectionString = "Data Source=LAPTOP-G4G46K72\\SQLEXPRESS;Initial Catalog=MINIMART-POS;Integrated Security=True;TrustServerCertificate=True";
        public reven()
        {
            InitializeComponent();
            SetupDatePicker();
        }

        private void SetupDatePicker()
        {
            // Initialize DateTimePicker with current date
            dateTimePicker2.Value = DateTime.Now;
            // Set the format to show date only
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "yyyy-MM-dd";

            LoadRevenueData(dateTimePicker2.Value);
        }

        private void LoadRevenueData(DateTime selectedDate)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Get sales data
                    decimal totalSales = 0;
                    string salesQuery = "SELECT total_amount_of_sale FROM sales WHERE CAST(date AS DATE) = @date";
                    using (SqlCommand command = new SqlCommand(salesQuery, connection))
                    {
                        command.Parameters.AddWithValue("@date", selectedDate.Date);
                        object salesResult = command.ExecuteScalar();
                        if (salesResult != null && salesResult != DBNull.Value)
                        {
                            totalSales = Convert.ToDecimal(salesResult);
                        }
                    }

                    // Get profit data
                    decimal totalProfit = 0;
                    string profitQuery = "SELECT total_profit FROM daily_profits WHERE CAST(date AS DATE) = @date";
                    using (SqlCommand command = new SqlCommand(profitQuery, connection))
                    {
                        command.Parameters.AddWithValue("@date", selectedDate.Date);
                        object profitResult = command.ExecuteScalar();
                        if (profitResult != null && profitResult != DBNull.Value)
                        {
                            totalProfit = Convert.ToDecimal(profitResult);
                        }
                    }

                    // Update UI
                    label5.Text = totalSales.ToString("C2");
                    label2.Text = totalProfit.ToString("C2");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading revenue data: " + ex.Message);
                    label5.Text = "$0.00";
                    label2.Text = "$0.00";
                }
            }
        }
        private void reven_Load(object sender, EventArgs e)
        {

            // Remove the duplicate InitializeComponent call
            SetupDatePicker();

        }





        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            LoadRevenueData(dateTimePicker2.Value);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
