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

            // Load initial sales data
            LoadSalesData(dateTimePicker2.Value);
        }

        private void LoadSalesData(DateTime selectedDate)
        {
            using (SqlConnection connection = new SqlConnection("Data Source=LAPTOP-G4G46K72\\SQLEXPRESS;Initial Catalog=MINIMART-POS;Integrated Security=True;TrustServerCertificate=True"))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT total_amount_of_sale FROM sales WHERE CAST(date AS DATE) = @date";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@date", selectedDate.Date);

                        object result = command.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            decimal totalSales = Convert.ToDecimal(result);
                            label5.Text = totalSales.ToString("C2"); // Format as currency
                        }
                        else
                        {
                            label5.Text = "$0.00"; // No sales found for the selected date
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading sales data: " + ex.Message);
                    label5.Text = "$0.00";
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
            LoadSalesData(dateTimePicker2.Value);
        }
    }
}
