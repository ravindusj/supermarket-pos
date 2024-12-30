using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace supermarket_pos
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
            ApplyRolePermissions();

        }
        private void ApplyRolePermissions()
        {
            string userRole = UserSession.Role?.ToLower() ?? "";

            billingBtn.Enabled = RolePermissions.HasPermission(userRole, "billing");
            inventoryBtn.Enabled = RolePermissions.HasPermission(userRole, "inventory");
            salesBtn.Enabled = RolePermissions.HasPermission(userRole, "staff");
            customer.Enabled = RolePermissions.HasPermission(userRole, "customers");
            reportsBtn.Enabled = RolePermissions.HasPermission(userRole, "report");

            billingBtn.Enabled = true;
            billingBtn.Visible = true;
            inventoryBtn.Enabled = true;
            inventoryBtn.Visible = true;
            salesBtn.Enabled = true;
            salesBtn.Visible = true;
            customer.Enabled = true;
            customer.Visible = true;
            reportsBtn.Enabled = true;
            reportsBtn.Visible = true;
            logoutBtn.Enabled = true;
            logoutBtn.Visible = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
        if (UserSession.IsLoggedIn)
        {
            loginpage.LogoutUser(UserSession.Username);
        }
            loginpage login = new loginpage();
            this.Hide();
            login.Show();
           
        }

        private void label1_Click(object sender, EventArgs e)
        {
     
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (RolePermissions.HasPermission(UserSession.Role, "inventory"))
                inventory1.BringToFront();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (RolePermissions.HasPermission(UserSession.Role, "staff"))
                sales1.BringToFront();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (RolePermissions.HasPermission(UserSession.Role, "billing"))
                billing1.BringToFront();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (RolePermissions.HasPermission(UserSession.Role, "report"))
            report2.BringToFront();
        }

        private void billing1_Load(object sender, EventArgs e)
        {

        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (UserSession.IsLoggedIn)
                {
                    loginpage.LogoutUser(UserSession.Username);
                }
                Application.Exit();
            }
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {

        }

        private void customer_Click(object sender, EventArgs e)
        {
            if (RolePermissions.HasPermission(UserSession.Role, "customers"))
                customers1.BringToFront();
        }

        private void report2_Load(object sender, EventArgs e)
        {

        }

        private void Dashboard_Load_1(object sender, EventArgs e)
        {

        }
    }
}
