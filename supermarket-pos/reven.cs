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


namespace supermarket_pos
{
    public partial class reven : UserControl


    {
        private readonly string connectionString = "Data Source=LAPTOP-G4G46K72\\SQLEXPRESS;Initial Catalog=MINIMART-POS;Integrated Security=True;TrustServerCertificate=True";
        public reven()
        {
            InitializeComponent();
            SetupDatePicker();
            LoadChartData();
            SetupTimer();
        }

        private void SetupDatePicker()
        {
            dateTimePicker2.Value = DateTime.Now;
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

            SetupDatePicker();
            LoadChartData();


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

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void cartesianChart1_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {

        }

        private void LoadChartData()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var salesData = new List<decimal>();
                    var profitData = new List<decimal>();
                    var dates = new List<string>();

                    string query = @"
                SELECT 
                    CAST(s.date AS DATE) as date,
                    COALESCE(s.total_amount_of_sale, 0) as sales,
                    COALESCE(dp.total_profit, 0) as profit
                FROM 
                    (SELECT DISTINCT CAST(date AS DATE) as date FROM sales 
                     UNION 
                     SELECT DISTINCT CAST(date AS DATE) FROM daily_profits) dates
                LEFT JOIN 
                    (SELECT CAST(date AS DATE) as date, SUM(total_amount_of_sale) as total_amount_of_sale 
                     FROM sales GROUP BY CAST(date AS DATE)) s ON dates.date = s.date
                LEFT JOIN 
                    daily_profits dp ON CAST(dp.date AS DATE) = dates.date
                WHERE 
                    dates.date >= DATEADD(day, -30, GETDATE())
                ORDER BY 
                    dates.date";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                dates.Add(((DateTime)reader["date"]).ToString("MM/dd"));
                                salesData.Add(reader["sales"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["sales"]));
                                profitData.Add(reader["profit"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["profit"]));
                            }
                        }
                    }

                    cartesianChart1.Series.Clear();
                    cartesianChart1.AxisX.Clear();
                    cartesianChart1.AxisY.Clear();

                    cartesianChart1.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Sales",
                    Values = new ChartValues<decimal>(salesData),
                    Fill = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(1, 255, 0, 0)),
                    PointGeometrySize = 10,
                    Stroke = System.Windows.Media.Brushes.Red,
                    StrokeThickness = 3,
                    FontFamily = new System.Windows.Media.FontFamily("Century Gothic"),
                    LineSmoothness = 0.7
                },
                new LineSeries
                {
                    Title = "Profit",
                    Values = new ChartValues<decimal>(profitData),
                    Fill = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(1, 0, 0, 255)),
                    PointGeometrySize = 10,
                    Stroke = System.Windows.Media.Brushes.Blue,
                    StrokeThickness = 3,
                    FontFamily = new System.Windows.Media.FontFamily("Century Gothic"),
                    LineSmoothness = 0.7

                }
            };

                    cartesianChart1.AxisX.Add(new Axis
                    {
                        Title = "Date",
                        Labels = dates,
                        Separator = new Separator { Step = 5 }
                    });

                    cartesianChart1.AxisY.Add(new Axis
                    {
                        Title = "Amount ($)",
                        LabelFormatter = value => value.ToString("C0")
                    });

                    cartesianChart1.LegendLocation = LegendLocation.Right;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading chart data: " + ex.Message);
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            LoadChartData();
        }
        private void SetupTimer()
        {
            timer1 = new System.Windows.Forms.Timer();
            timer1.Interval = 10000;
            timer1.Tick += timer1_Tick;
            timer1.Start();
        }
    }
}
