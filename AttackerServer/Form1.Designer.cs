namespace AttackerServer
{
    partial class Form1
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
            this.lvClients = new System.Windows.Forms.ListView();
            this.id = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.status = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lastUpdate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnRefreshClients = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lvClients
            // 
            this.lvClients.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.id,
            this.status,
            this.lastUpdate});
            this.lvClients.Location = new System.Drawing.Point(12, 41);
            this.lvClients.Name = "lvClients";
            this.lvClients.Size = new System.Drawing.Size(743, 211);
            this.lvClients.TabIndex = 0;
            this.lvClients.UseCompatibleStateImageBehavior = false;
            this.lvClients.View = System.Windows.Forms.View.Details;
            // 
            // id
            // 
            this.id.Tag = "id";
            this.id.Text = "ClientID";
            this.id.Width = 143;
            // 
            // status
            // 
            this.status.Tag = "status";
            this.status.Text = "Status";
            this.status.Width = 205;
            // 
            // lastUpdate
            // 
            this.lastUpdate.Tag = "lastUpdate";
            this.lastUpdate.Text = "Last Update";
            this.lastUpdate.Width = 148;
            // 
            // btnRefreshClients
            // 
            this.btnRefreshClients.Location = new System.Drawing.Point(12, 12);
            this.btnRefreshClients.Name = "btnRefreshClients";
            this.btnRefreshClients.Size = new System.Drawing.Size(75, 23);
            this.btnRefreshClients.TabIndex = 1;
            this.btnRefreshClients.Text = "Refresh Clients";
            this.btnRefreshClients.UseVisualStyleBackColor = true;
            this.btnRefreshClients.Click += new System.EventHandler(this.BtnRefreshClients_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(767, 264);
            this.Controls.Add(this.btnRefreshClients);
            this.Controls.Add(this.lvClients);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvClients;
        private System.Windows.Forms.ColumnHeader id;
        private System.Windows.Forms.ColumnHeader status;
        private System.Windows.Forms.ColumnHeader lastUpdate;
        private System.Windows.Forms.Button btnRefreshClients;
    }
}

