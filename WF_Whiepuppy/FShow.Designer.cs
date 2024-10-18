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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dataGridViewPacketDetails = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPackets)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPacketDetails)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewPackets
            // 
            this.dataGridViewPackets.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewPackets.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPackets.Location = new System.Drawing.Point(12, 38);
            this.dataGridViewPackets.Name = "dataGridViewPackets";
            this.dataGridViewPackets.RowHeadersWidth = 51;
            this.dataGridViewPackets.RowTemplate.Height = 24;
            this.dataGridViewPackets.Size = new System.Drawing.Size(1105, 201);
            this.dataGridViewPackets.TabIndex = 3;
            // 
            // btn_stop
            // 
            this.btn_stop.Location = new System.Drawing.Point(12, 9);
            this.btn_stop.Name = "btn_stop";
            this.btn_stop.Size = new System.Drawing.Size(75, 23);
            this.btn_stop.TabIndex = 4;
            this.btn_stop.Text = "Dừng";
            this.btn_stop.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 245);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(552, 193);
            this.dataGridView1.TabIndex = 5;
            // 
            // dataGridViewPacketDetails
            // 
            this.dataGridViewPacketDetails.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewPacketDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPacketDetails.Location = new System.Drawing.Point(570, 245);
            this.dataGridViewPacketDetails.Name = "dataGridViewPacketDetails";
            this.dataGridViewPacketDetails.RowHeadersWidth = 51;
            this.dataGridViewPacketDetails.RowTemplate.Height = 24;
            this.dataGridViewPacketDetails.Size = new System.Drawing.Size(547, 193);
            this.dataGridViewPacketDetails.TabIndex = 6;
            // 
            // PacketViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1129, 450);
            this.Controls.Add(this.dataGridViewPacketDetails);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btn_stop);
            this.Controls.Add(this.dataGridViewPackets);
            this.Name = "PacketViewerForm";
            this.Text = "Xem";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPackets)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPacketDetails)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView dataGridViewPackets;
        private System.Windows.Forms.Button btn_stop;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridView dataGridViewPacketDetails;
    }
}