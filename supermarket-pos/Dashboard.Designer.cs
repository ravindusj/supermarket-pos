namespace supermarket_pos
{
    partial class Dashboard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Dashboard));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.logoutBtn = new System.Windows.Forms.Button();
            this.reportsBtn = new System.Windows.Forms.Button();
            this.inventoryBtn = new System.Windows.Forms.Button();
            this.salesBtn = new System.Windows.Forms.Button();
            this.billingBtn = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.billing1 = new supermarket_pos.billing();
            this.inventory1 = new supermarket_pos.inventory();
            this.report1 = new supermarket_pos.report();
            this.sales1 = new supermarket_pos.sales();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(39)))), ((int)(((byte)(40)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.logoutBtn);
            this.panel1.Controls.Add(this.reportsBtn);
            this.panel1.Controls.Add(this.inventoryBtn);
            this.panel1.Controls.Add(this.salesBtn);
            this.panel1.Controls.Add(this.billingBtn);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(199, 700);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label1.Location = new System.Drawing.Point(63, 96);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 32);
            this.label1.TabIndex = 1;
            this.label1.Text = "Mini Mart";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // logoutBtn
            // 
            this.logoutBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.logoutBtn.FlatAppearance.BorderSize = 0;
            this.logoutBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.logoutBtn.Font = new System.Drawing.Font("Century Gothic", 14.25F);
            this.logoutBtn.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.logoutBtn.Image = ((System.Drawing.Image)(resources.GetObject("logoutBtn.Image")));
            this.logoutBtn.Location = new System.Drawing.Point(0, 590);
            this.logoutBtn.Name = "logoutBtn";
            this.logoutBtn.Size = new System.Drawing.Size(193, 45);
            this.logoutBtn.TabIndex = 3;
            this.logoutBtn.Text = "Logout";
            this.logoutBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.logoutBtn.UseVisualStyleBackColor = true;
            this.logoutBtn.Click += new System.EventHandler(this.button5_Click);
            // 
            // reportsBtn
            // 
            this.reportsBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.reportsBtn.FlatAppearance.BorderSize = 0;
            this.reportsBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.reportsBtn.Font = new System.Drawing.Font("Century Gothic", 14.25F);
            this.reportsBtn.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.reportsBtn.Image = ((System.Drawing.Image)(resources.GetObject("reportsBtn.Image")));
            this.reportsBtn.Location = new System.Drawing.Point(0, 376);
            this.reportsBtn.Name = "reportsBtn";
            this.reportsBtn.Size = new System.Drawing.Size(193, 45);
            this.reportsBtn.TabIndex = 2;
            this.reportsBtn.Text = "Reports";
            this.reportsBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.reportsBtn.UseVisualStyleBackColor = true;
            this.reportsBtn.Click += new System.EventHandler(this.button4_Click);
            // 
            // inventoryBtn
            // 
            this.inventoryBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(39)))), ((int)(((byte)(40)))));
            this.inventoryBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.inventoryBtn.FlatAppearance.BorderSize = 0;
            this.inventoryBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.inventoryBtn.Font = new System.Drawing.Font("Century Gothic", 14.25F);
            this.inventoryBtn.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.inventoryBtn.Image = ((System.Drawing.Image)(resources.GetObject("inventoryBtn.Image")));
            this.inventoryBtn.Location = new System.Drawing.Point(0, 269);
            this.inventoryBtn.Name = "inventoryBtn";
            this.inventoryBtn.Size = new System.Drawing.Size(193, 45);
            this.inventoryBtn.TabIndex = 1;
            this.inventoryBtn.Text = "Inventory";
            this.inventoryBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.inventoryBtn.UseVisualStyleBackColor = true;
            this.inventoryBtn.Click += new System.EventHandler(this.button3_Click);
            // 
            // salesBtn
            // 
            this.salesBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.salesBtn.FlatAppearance.BorderSize = 0;
            this.salesBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.salesBtn.Font = new System.Drawing.Font("Century Gothic", 14.25F);
            this.salesBtn.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.salesBtn.Image = ((System.Drawing.Image)(resources.GetObject("salesBtn.Image")));
            this.salesBtn.Location = new System.Drawing.Point(0, 483);
            this.salesBtn.Name = "salesBtn";
            this.salesBtn.Size = new System.Drawing.Size(193, 45);
            this.salesBtn.TabIndex = 1;
            this.salesBtn.Text = "Sales";
            this.salesBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.salesBtn.UseVisualStyleBackColor = true;
            this.salesBtn.Click += new System.EventHandler(this.button2_Click);
            // 
            // billingBtn
            // 
            this.billingBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(39)))), ((int)(((byte)(40)))));
            this.billingBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.billingBtn.FlatAppearance.BorderSize = 0;
            this.billingBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.billingBtn.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.billingBtn.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.billingBtn.Image = ((System.Drawing.Image)(resources.GetObject("billingBtn.Image")));
            this.billingBtn.Location = new System.Drawing.Point(0, 162);
            this.billingBtn.Name = "billingBtn";
            this.billingBtn.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.billingBtn.Size = new System.Drawing.Size(193, 45);
            this.billingBtn.TabIndex = 0;
            this.billingBtn.Text = "Billing";
            this.billingBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.billingBtn.UseVisualStyleBackColor = true;
            this.billingBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::supermarket_pos.Properties.Resources.grocery1;
            this.pictureBox1.Location = new System.Drawing.Point(-33, 8);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(199, 104);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            // 
            // billing1
            // 
            this.billing1.Location = new System.Drawing.Point(205, 12);
            this.billing1.Name = "billing1";
            this.billing1.Size = new System.Drawing.Size(1067, 697);
            this.billing1.TabIndex = 1;
            this.billing1.Load += new System.EventHandler(this.billing1_Load);
            // 
            // inventory1
            // 
            this.inventory1.Location = new System.Drawing.Point(197, 0);
            this.inventory1.Name = "inventory1";
            this.inventory1.Size = new System.Drawing.Size(1067, 697);
            this.inventory1.TabIndex = 2;
            // 
            // report1
            // 
            this.report1.Location = new System.Drawing.Point(197, 0);
            this.report1.Name = "report1";
            this.report1.Size = new System.Drawing.Size(1067, 697);
            this.report1.TabIndex = 3;
            // 
            // sales1
            // 
            this.sales1.Location = new System.Drawing.Point(197, 0);
            this.sales1.Name = "sales1";
            this.sales1.Size = new System.Drawing.Size(1067, 697);
            this.sales1.TabIndex = 4;
            // 
            // Dashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1265, 697);
            this.Controls.Add(this.billing1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.inventory1);
            this.Controls.Add(this.report1);
            this.Controls.Add(this.sales1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Dashboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mini Mart - Dashboard";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button reportsBtn;
        private System.Windows.Forms.Button inventoryBtn;
        private System.Windows.Forms.Button salesBtn;
        private System.Windows.Forms.Button billingBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button logoutBtn;
        private billing billing1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private inventory inventory1;
        private report report1;
        private sales sales1;
    }
}

