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
            this.SuspendLayout();
            // 
            // lvClients
            // 
            this.lvClients.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.id,
            this.status,
            this.lastUpdate});
            this.lvClients.Location = new System.Drawing.Point(12, 12);
            this.lvClients.Name = "lvClients";
            this.lvClients.Size = new System.Drawing.Size(517, 214);
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 238);
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
    }
}

