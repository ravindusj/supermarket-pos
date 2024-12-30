using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace supermarket_pos
{
    public partial class staff : UserControl
    {
        private readonly string connectionString = "Data Source=LAPTOP-G4G46K72\\SQLEXPRESS;Initial Catalog=MINIMART-POS;Integrated Security=True;TrustServerCertificate=True";

        public staff()
        {
            InitializeComponent();
        }
        private void UpdateDateTime()
        {
            label8.Text = DateTime.Now.ToString("yyyy-MM-dd") + "  " + DateTime.Now.ToString("HH:mm:ss");
        }
        private void LoadStaffData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM vw_StaffDetails";

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
        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void sales_Load(object sender, EventArgs e)
        {
            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += timer1_Tick;
            timer.Start();
            UpdateDateTime();
            LoadStaffData();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateDateTime();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBox7.Text) ||
                    string.IsNullOrWhiteSpace(textBox1.Text) ||
                    string.IsNullOrWhiteSpace(textBox2.Text) ||
                    string.IsNullOrWhiteSpace(comboBox1.Text) ||
                    string.IsNullOrWhiteSpace(textBox4.Text))
                {
                    MessageBox.Show("Please fill in all fields", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"INSERT INTO staff (username, password, name, role, phone_no) 
                                   VALUES (@username, @password, @name, @role, @phone)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", textBox7.Text.Trim());
                        cmd.Parameters.AddWithValue("@password", textBox1.Text.Trim());
                        cmd.Parameters.AddWithValue("@name", textBox2.Text.Trim());
                        cmd.Parameters.AddWithValue("@role", comboBox1.Text.Trim());
                        cmd.Parameters.AddWithValue("@phone", textBox4.Text.Trim());

                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Staff member added successfully!", "Success",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearFields();
                        }
                        else
                        {
                            MessageBox.Show("Failed to add staff member.", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Database Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            LoadStaffData();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a staff member to delete.", "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataGridViewRow row = dataGridView1.SelectedRows[0];

                DialogResult result = MessageBox.Show(
                    $"Are you sure you want to delete staff member: {row.Cells["Full Name"].Value}?",
                    "Confirm Deletion",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = "DELETE FROM staff WHERE username = @username";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@username", row.Cells["username"].Value.ToString());

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Staff member deleted successfully!", "Success",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                                LoadStaffData();
                            }
                            else
                            {
                                MessageBox.Show("Failed to delete staff member.", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Database Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a staff member to update.", "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataGridViewRow row = dataGridView1.SelectedRows[0];

                textBox7.Text = row.Cells["Username"].Value.ToString();
                textBox1.Text = row.Cells["Password"].Value.ToString();
                textBox2.Text = row.Cells["Full Name"].Value.ToString();
                comboBox1.Text = row.Cells["Role"].Value.ToString();
                textBox4.Text = row.Cells["Phone Number"].Value.ToString();

                textBox7.Tag = textBox7.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading staff details: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void save_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBox7.Text) ||
                    string.IsNullOrWhiteSpace(textBox1.Text) ||
                    string.IsNullOrWhiteSpace(textBox2.Text) ||
                    string.IsNullOrWhiteSpace(comboBox1.Text) ||
                    string.IsNullOrWhiteSpace(textBox4.Text))
                {
                    MessageBox.Show("Please fill in all fields", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string originalUsername = textBox7.Tag?.ToString();
                if (string.IsNullOrWhiteSpace(originalUsername))
                {
                    MessageBox.Show("Please select a staff member to update first.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"UPDATE staff 
                           SET username = @newUsername,
                               password = @password,
                               name = @name,
                               role = @role,
                               phone_no = @phone
                           WHERE username = @originalUsername";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@newUsername", textBox7.Text.Trim());
                        cmd.Parameters.AddWithValue("@password", textBox1.Text.Trim());
                        cmd.Parameters.AddWithValue("@name", textBox2.Text.Trim());
                        cmd.Parameters.AddWithValue("@role", comboBox1.Text.Trim());
                        cmd.Parameters.AddWithValue("@phone", textBox4.Text.Trim());
                        cmd.Parameters.AddWithValue("@originalUsername", originalUsername);

                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Staff member updated successfully!", "Success",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearFields();
                            LoadStaffData();
                        }
                        else
                        {
                            MessageBox.Show("Failed to update staff member.", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Database Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ClearFields();
        }
        private void ClearFields()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox7.Clear();
            comboBox1.SelectedIndex = -1;
            textBox7.Tag = null;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void FilterData()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT * FROM vw_StaffDetails WHERE 1=1";

                    if (!string.IsNullOrEmpty(textBox5.Text))
                    {
                        query += " AND username LIKE @username";
                    }

                    if (!string.IsNullOrEmpty(textBox6.Text))
                    {
                        query += " AND role LIKE @role";
                    }

                    SqlCommand command = new SqlCommand(query, connection);

                    if (!string.IsNullOrEmpty(textBox5.Text))
                    {
                        command.Parameters.AddWithValue("@username", "%" + textBox5.Text.Trim() + "%");
                    }

                    if (!string.IsNullOrEmpty(textBox6.Text))
                    {
                        command.Parameters.AddWithValue("@role", "%" + textBox6.Text.Trim() + "%");
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable filteredTable = new DataTable();
                    adapter.Fill(filteredTable);

                    dataGridView1.DataSource = filteredTable;

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error filtering data: " + ex.Message);
                }
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            FilterData();
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            FilterData();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}