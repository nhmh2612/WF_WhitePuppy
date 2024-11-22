namespace WF_Whiepuppy
{
    partial class Fdetails
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelPacketInfo;
        private System.Windows.Forms.Label lblSourceIP;
        private System.Windows.Forms.Label lblDestinationIP;
        private System.Windows.Forms.Label lblProtocol;
        private System.Windows.Forms.Label lblData;

        private void InitializeComponent()
        {
            this.panelPacketInfo = new System.Windows.Forms.Panel();
            this.lblSourceIP = new System.Windows.Forms.Label();
            this.lblDestinationIP = new System.Windows.Forms.Label();
            this.lblProtocol = new System.Windows.Forms.Label();
            this.lblData = new System.Windows.Forms.Label();
            this.panelPacketInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelPacketInfo
            // 
            this.panelPacketInfo.Controls.Add(this.lblSourceIP);
            this.panelPacketInfo.Controls.Add(this.lblDestinationIP);
            this.panelPacketInfo.Controls.Add(this.lblProtocol);
            this.panelPacketInfo.Controls.Add(this.lblData);
            this.panelPacketInfo.Location = new System.Drawing.Point(12, 12);
            this.panelPacketInfo.Name = "panelPacketInfo";
            this.panelPacketInfo.Size = new System.Drawing.Size(668, 373);
            this.panelPacketInfo.TabIndex = 1;
            this.panelPacketInfo.Visible = false;
            this.panelPacketInfo.Paint += new System.Windows.Forms.PaintEventHandler(this.panelPacketInfo_Paint);
            // 
            // lblSourceIP
            // 
            this.lblSourceIP.AutoSize = true;
            this.lblSourceIP.Location = new System.Drawing.Point(10, 10);
            this.lblSourceIP.Name = "lblSourceIP";
            this.lblSourceIP.Size = new System.Drawing.Size(90, 16);
            this.lblSourceIP.TabIndex = 0;
            this.lblSourceIP.Text = "Địa chỉ nguồn:";
            // 
            // lblDestinationIP
            // 
            this.lblDestinationIP.AutoSize = true;
            this.lblDestinationIP.Location = new System.Drawing.Point(10, 40);
            this.lblDestinationIP.Name = "lblDestinationIP";
            this.lblDestinationIP.Size = new System.Drawing.Size(78, 16);
            this.lblDestinationIP.TabIndex = 1;
            this.lblDestinationIP.Text = "Địa chỉ đích:";
            // 
            // lblProtocol
            // 
            this.lblProtocol.AutoSize = true;
            this.lblProtocol.Location = new System.Drawing.Point(10, 70);
            this.lblProtocol.Name = "lblProtocol";
            this.lblProtocol.Size = new System.Drawing.Size(93, 16);
            this.lblProtocol.TabIndex = 2;
            this.lblProtocol.Text = "Loại giao thức:";
            // 
            // lblData
            // 
            this.lblData.AutoSize = true;
            this.lblData.Location = new System.Drawing.Point(10, 100);
            this.lblData.Name = "lblData";
            this.lblData.Size = new System.Drawing.Size(51, 16);
            this.lblData.TabIndex = 3;
            this.lblData.Text = "Dữ liệu:";
            // 
            // Fdetails
            // 
            this.ClientSize = new System.Drawing.Size(696, 397);
            this.Controls.Add(this.panelPacketInfo);
            this.Name = "Fdetails";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Thông tin gói tin";
            this.Load += new System.EventHandler(this.Fdetails_Load);
            this.panelPacketInfo.ResumeLayout(false);
            this.panelPacketInfo.PerformLayout();
            this.ResumeLayout(false);

        }
    }
}
