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
    public partial class loginpage : Form
    {
        private readonly string connectionString = "Data Source=LAPTOP-G4G46K72\\SQLEXPRESS;Initial Catalog=MINIMART-POS;Integrated Security=True;TrustServerCertificate=True";

        public loginpage()
        {
            InitializeComponent();
            this.Text = "Login - Mini Mart";
            UserSession.ClearSession();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();
            string password = textBox2.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password", "Login Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (AuthenticateUser(username, password))
            {
                Dashboard dash = new Dashboard();
                this.Hide();
                dash.Show();
            }

        }
        private bool AuthenticateUser(string username, string password)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string checkLoginQuery = "SELECT is_logged_in, last_login FROM staff WHERE username = @username";
                    using (SqlCommand checkCmd = new SqlCommand(checkLoginQuery, connection))
                    {
                        checkCmd.Parameters.AddWithValue("@username", username);
                        using (SqlDataReader reader = checkCmd.ExecuteReader())
                        {
                            if (reader.Read() && (bool)reader["is_logged_in"])
                            {
                                DateTime lastLogin = reader["last_login"] != DBNull.Value
                                    ? (DateTime)reader["last_login"]
                                    : DateTime.MinValue;

                                string message = $"This account is currently logged in.\n" +
                                               $"Last login: {lastLogin}\n\n" +
                                               "If you believe this is an error, please contact your administrator.";

                                MessageBox.Show(message, "Login Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return false;
                            }
                        }
                    }

                    string staffName = null;
                    string role = null;

                    string query = @"SELECT name, role FROM staff 
                           WHERE username = @username AND password = @password 
                           AND is_logged_in = 0";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@password", password);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                staffName = reader["name"].ToString();
                                role = reader["role"].ToString();
                            }
                        }
                    }

                    if (staffName == null)
                    {
                        LogLoginAttempt(connection, username, false);

                        MessageBox.Show("Invalid username or password", "Login Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    string updateQuery = @"
                UPDATE staff 
                SET is_logged_in = 1, 
                    last_login = GETDATE() 
                WHERE username = @username";

                    using (SqlCommand updateCmd = new SqlCommand(updateQuery, connection))
                    {
                        updateCmd.Parameters.AddWithValue("@username", username);
                        updateCmd.ExecuteNonQuery();
                    }

                    LogLoginAttempt(connection, username, true);

                    UserSession.SetSession(username, staffName, role);

                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error during login: " + ex.Message, "System Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }

        private void LogLoginAttempt(SqlConnection connection, string username, bool success)
        {
            string logQuery = @"
            INSERT INTO staff_login_history (staff_id, login_time, action, success) 
            VALUES (@username, GETDATE(), 'LOGIN', @success)";

            using (SqlCommand logCmd = new SqlCommand(logQuery, connection))
            {
                logCmd.Parameters.AddWithValue("@username", username);
                logCmd.Parameters.AddWithValue("@success", success);
                logCmd.ExecuteNonQuery();
            }
        }
        public static void LogoutUser(string username)
        {
            using (SqlConnection connection = new SqlConnection("Data Source=LAPTOP-G4G46K72\\SQLEXPRESS;Initial Catalog=MINIMART-POS;Integrated Security=True;TrustServerCertificate=True"))
            {
                try
                {
                    connection.Open();

                    string updateQuery = "UPDATE staff SET is_logged_in = 0 WHERE username = @username";
                    using (SqlCommand updateCmd = new SqlCommand(updateQuery, connection))
                    {
                        updateCmd.Parameters.AddWithValue("@username", username);
                        updateCmd.ExecuteNonQuery();
                    }

                    string logQuery = @"
                    INSERT INTO staff_login_history (staff_id, login_time, action, success) 
                    VALUES (@username, GETDATE(), 'LOGOUT', 1)";

                    using (SqlCommand logCmd = new SqlCommand(logQuery, connection))
                    {
                        logCmd.Parameters.AddWithValue("@username", username);
                        logCmd.ExecuteNonQuery();
                    }

                    UserSession.ClearSession();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error during logout: " + ex.Message, "System Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void loginpage_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (UserSession.IsLoggedIn)
            {
                LogoutUser(UserSession.Username);
            }
        }
        private void loginpage_Load(object sender, EventArgs e)
        {


        }
    }

}
