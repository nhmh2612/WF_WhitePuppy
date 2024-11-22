namespace WF_Whiepuppy
{
    partial class PacketViewerForm
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
            this.dataGridViewPackets = new System.Windows.Forms.DataGridView();
            this.btn_stop = new System.Windows.Forms.Button();
            this.packetDetailsTextBox = new System.Windows.Forms.TextBox();
            this.treeViewPacketDetails = new System.Windows.Forms.TreeView();
            this.btnStartCapture = new System.Windows.Forms.Button();
            this.btn_save = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPackets)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewPackets
            // 
            this.dataGridViewPackets.AllowUserToAddRows = false;
            this.dataGridViewPackets.AllowUserToDeleteRows = false;
            this.dataGridViewPackets.AllowUserToResizeColumns = false;
            this.dataGridViewPackets.AllowUserToResizeRows = false;
            this.dataGridViewPackets.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewPackets.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPackets.Dock = System.Windows.Forms.DockStyle.Top;
            this.dataGridViewPackets.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewPackets.Name = "dataGridViewPackets";
            this.dataGridViewPackets.ReadOnly = true;
            this.dataGridViewPackets.RowHeadersVisible = false;
            this.dataGridViewPackets.RowHeadersWidth = 51;
            this.dataGridViewPackets.RowTemplate.Height = 24;
            this.dataGridViewPackets.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewPackets.Size = new System.Drawing.Size(1299, 298);
            this.dataGridViewPackets.TabIndex = 3;
            this.dataGridViewPackets.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewPackets_CellContentClick);
            // 
            // btn_stop
            // 
            this.btn_stop.BackColor = System.Drawing.Color.IndianRed;
            this.btn_stop.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_stop.ForeColor = System.Drawing.Color.Black;
            this.btn_stop.Location = new System.Drawing.Point(93, 304);
            this.btn_stop.Name = "btn_stop";
            this.btn_stop.Size = new System.Drawing.Size(75, 31);
            this.btn_stop.TabIndex = 4;
            this.btn_stop.Text = "Dừng";
            this.btn_stop.UseVisualStyleBackColor = false;
            this.btn_stop.Click += new System.EventHandler(this.btn_stop_Click_1);
            // 
            // packetDetailsTextBox
            // 
            this.packetDetailsTextBox.Location = new System.Drawing.Point(731, 304);
            this.packetDetailsTextBox.Multiline = true;
            this.packetDetailsTextBox.Name = "packetDetailsTextBox";
            this.packetDetailsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.packetDetailsTextBox.Size = new System.Drawing.Size(556, 287);
            this.packetDetailsTextBox.TabIndex = 5;
            this.packetDetailsTextBox.TextChanged += new System.EventHandler(this.packetDetailsTextBox_TextChanged);
            // 
            // treeViewPacketDetails
            // 
            this.treeViewPacketDetails.Location = new System.Drawing.Point(12, 341);
            this.treeViewPacketDetails.Name = "treeViewPacketDetails";
            this.treeViewPacketDetails.Size = new System.Drawing.Size(713, 250);
            this.treeViewPacketDetails.TabIndex = 8;
            this.treeViewPacketDetails.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewPacketDetails_AfterSelect);
            // 
            // btnStartCapture
            // 
            this.btnStartCapture.Location = new System.Drawing.Point(12, 304);
            this.btnStartCapture.Name = "btnStartCapture";
            this.btnStartCapture.Size = new System.Drawing.Size(75, 31);
            this.btnStartCapture.TabIndex = 11;
            this.btnStartCapture.Text = "Bắt đầu";
            this.btnStartCapture.UseVisualStyleBackColor = true;
            this.btnStartCapture.Click += new System.EventHandler(this.btnStartCapture_Click);
            // 
            // btn_save
            // 
            this.btn_save.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btn_save.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_save.Location = new System.Drawing.Point(199, 304);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(96, 31);
            this.btn_save.TabIndex = 12;
            this.btn_save.Text = "Lưu tệp";
            this.btn_save.UseVisualStyleBackColor = false;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // PacketViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1299, 603);
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.btnStartCapture);
            this.Controls.Add(this.treeViewPacketDetails);
            this.Controls.Add(this.packetDetailsTextBox);
            this.Controls.Add(this.btn_stop);
            this.Controls.Add(this.dataGridViewPackets);
            this.Name = "PacketViewerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bắt gói tin";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPackets)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView dataGridViewPackets;
        private System.Windows.Forms.Button btn_stop;
        private System.Windows.Forms.TextBox packetDetailsTextBox;
        private System.Windows.Forms.TreeView treeViewPacketDetails;
        private System.Windows.Forms.TextBox FilterPackets;
        private System.Windows.Forms.Button btnStartCapture;
        private System.Windows.Forms.Button btn_save;
    }
}