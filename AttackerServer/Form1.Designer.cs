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
            this.message = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lastUpdate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.btnAttack = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lvClients
            // 
            this.lvClients.AllowColumnReorder = true;
            this.lvClients.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.id,
            this.status,
            this.message,
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
            // message
            // 
            this.message.Tag = "message";
            this.message.Text = "Message";
            // 
            // lastUpdate
            // 
            this.lastUpdate.Tag = "lastUpdate";
            this.lastUpdate.Text = "Last Update";
            this.lastUpdate.Width = 148;
            // 
            // txtUrl
            // 
            this.txtUrl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUrl.Location = new System.Drawing.Point(12, 12);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(509, 23);
            this.txtUrl.TabIndex = 1;
            // 
            // btnAttack
            // 
            this.btnAttack.Location = new System.Drawing.Point(527, 12);
            this.btnAttack.Name = "btnAttack";
            this.btnAttack.Size = new System.Drawing.Size(111, 23);
            this.btnAttack.TabIndex = 2;
            this.btnAttack.Text = "ATTACK";
            this.btnAttack.UseVisualStyleBackColor = true;
            this.btnAttack.Click += new System.EventHandler(this.BtnAttack_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(644, 12);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(111, 23);
            this.btnStop.TabIndex = 3;
            this.btnStop.Text = "STOP";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(767, 264);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnAttack);
            this.Controls.Add(this.txtUrl);
            this.Controls.Add(this.lvClients);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.ShowIcon = false;
            this.Text = "ddosAttacker";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvClients;
        private System.Windows.Forms.ColumnHeader id;
        private System.Windows.Forms.ColumnHeader status;
        private System.Windows.Forms.ColumnHeader lastUpdate;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.Button btnAttack;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.ColumnHeader message;
    }
}

