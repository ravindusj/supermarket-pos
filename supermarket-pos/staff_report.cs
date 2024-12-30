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
    public partial class staff_report : UserControl
    {
        private Timer refreshTimer;
        public staff_report()
        {
            InitializeComponent();
            SetupTimer();
            ConfigureDataGridView();
            LoadStaffData();
        }
        private void ConfigureDataGridView()
        {
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.DefaultCellStyle.Padding = new Padding(5);
            dataGridView1.RowHeadersVisible = false;
        }
        private void SetupTimer()
        {
            refreshTimer = new Timer();
            refreshTimer.Interval = 10000;
            refreshTimer.Tick += RefreshTimer_Tick;
            refreshTimer.Start();
        }
        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            LoadStaffData();
        }
        private void LoadStaffData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection("Data Source=LAPTOP-G4G46K72\\SQLEXPRESS;Initial Catalog=MINIMART-POS;Integrated Security=True;TrustServerCertificate=True"))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT StaffName, StaffRole, TotalTimeWorked, LastLogin FROM StaffWorkTimeView ORDER BY StaffName", conn))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dataGridView1.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading staff data: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void staff_report_Load(object sender, EventArgs e)
        {
            LoadStaffData();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (refreshTimer != null)
                {
                    refreshTimer.Stop();
                    refreshTimer.Dispose();
                }
            }
            base.Dispose(disposing);
        }
    }
}
