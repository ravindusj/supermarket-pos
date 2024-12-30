using LiveCharts.Wpf;
using LiveCharts;
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
using System.Windows.Media;

namespace supermarket_pos
{

    public partial class inven : UserControl
    {
        public inven()
        {
            InitializeComponent();
            LoadTotalWholesaleValue();
            LoadTopSellingItem();
            LoadPieChart();

            Timer refreshTimer = new Timer();
            refreshTimer.Interval = 10000;
            refreshTimer.Tick += RefreshTimer_Tick;
            refreshTimer.Start();
        }
        private void LoadPieChart()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection("Data Source=LAPTOP-G4G46K72\\SQLEXPRESS;Initial Catalog=MINIMART-POS;Integrated Security=True;TrustServerCertificate=True"))
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            i.name,
                            SUM(bd.quantity) as total_quantity
                        FROM inventory i
                        JOIN bill_details bd ON i.productID = bd.productID
                        GROUP BY i.productID, i.name
                        ORDER BY total_quantity DESC";

                    var seriesCollection = new SeriesCollection();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string itemName = reader["name"].ToString();
                                double quantity = Convert.ToDouble(reader["total_quantity"]);

                                var pieSeries = new PieSeries
                                {
                                    Title = itemName,
                                    Values = new ChartValues<double> { quantity },
                                    DataLabels = true,
                                    LabelPoint = point => $"{point.Y:N0}",
                                    StrokeThickness = 2,
                                    Stroke = System.Windows.Media.Brushes.Black,
                                    FontFamily = new System.Windows.Media.FontFamily("Century Gothic"),
                                    FontSize = 10
                                };
                                seriesCollection.Add(pieSeries);

                            }
                        }
                    }
                    pieChart1.Series = seriesCollection;
                    pieChart1.LegendLocation = LegendLocation.Right;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading pie chart: " + ex.Message);
            }
        }

        private void inven_Load(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void amounttotal_Click(object sender, EventArgs e)
        {

        }
        private void LoadTopSellingItem()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection("Data Source=LAPTOP-G4G46K72\\SQLEXPRESS;Initial Catalog=MINIMART-POS;Integrated Security=True;TrustServerCertificate=True"))
                {
                    conn.Open();
                    string query = @"
                        SELECT TOP 1 
                            i.name,
                            SUM(bd.quantity) as total_quantity
                        FROM inventory i
                        JOIN bill_details bd ON i.productID = bd.productID
                        GROUP BY i.productID, i.name
                        ORDER BY total_quantity DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string itemName = reader["name"].ToString();
                                int quantity = Convert.ToInt32(reader["total_quantity"]);
                                topitem.Text = $"{itemName} ({quantity} x)";
                            }
                            else
                            {
                                topitem.Text = "No sales data available";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading top selling item: " + ex.Message);
            }
        }
        private void LoadTotalWholesaleValue()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection("Data Source=LAPTOP-G4G46K72\\SQLEXPRESS;Initial Catalog=MINIMART-POS;Integrated Security=True;TrustServerCertificate=True"))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(@"
                        SELECT SUM(quantity * whole_price) as TotalValue 
                        FROM InventoryView", conn))
                    {
                        object result = cmd.ExecuteScalar();
                        decimal totalValue = result != DBNull.Value ? Convert.ToDecimal(result) : 0;
                        amounttotal.Text = $"{totalValue:C2}";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error calculating inventory value: " + ex.Message);
            }
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            LoadTotalWholesaleValue();
            LoadTopSellingItem();
            LoadPieChart();
        }

        private void topitem_Click(object sender, EventArgs e)
        {

        }

        private void pieChart1_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {

        }
    }
}
