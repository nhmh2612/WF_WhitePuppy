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
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPackets)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewPackets
            // 
            this.dataGridViewPackets.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewPackets.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPackets.Location = new System.Drawing.Point(12, 49);
            this.dataGridViewPackets.Name = "dataGridViewPackets";
            this.dataGridViewPackets.RowHeadersWidth = 51;
            this.dataGridViewPackets.RowTemplate.Height = 24;
            this.dataGridViewPackets.Size = new System.Drawing.Size(1105, 172);
            this.dataGridViewPackets.TabIndex = 3;
            this.dataGridViewPackets.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewPackets_CellContentClick);
            // 
            // btn_stop
            // 
            this.btn_stop.Location = new System.Drawing.Point(12, 12);
            this.btn_stop.Name = "btn_stop";
            this.btn_stop.Size = new System.Drawing.Size(75, 31);
            this.btn_stop.TabIndex = 4;
            this.btn_stop.Text = "Dừng";
            this.btn_stop.UseVisualStyleBackColor = true;
            this.btn_stop.Click += new System.EventHandler(this.btn_stop_Click);
            // 
            // packetDetailsTextBox
            // 
            this.packetDetailsTextBox.Location = new System.Drawing.Point(564, 227);
            this.packetDetailsTextBox.Multiline = true;
            this.packetDetailsTextBox.Name = "packetDetailsTextBox";
            this.packetDetailsTextBox.Size = new System.Drawing.Size(553, 211);
            this.packetDetailsTextBox.TabIndex = 5;
            this.packetDetailsTextBox.TextChanged += new System.EventHandler(this.packetDetailsTextBox_TextChanged);
            // 
            // PacketViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1129, 450);
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
    }
}