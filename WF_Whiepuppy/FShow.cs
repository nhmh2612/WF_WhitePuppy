using PacketDotNet;
using SharpPcap;
using System;
using System.Text;
using System.Windows.Forms;

namespace WF_Whiepuppy
{
    public partial class PacketViewerForm : Form
    {
        private bool isCapturing = false;   // Biến để theo dõi trạng thái bắt gói tin
        private ICaptureDevice capture;     // Biến để lưu đối tượng bắt gói tin
        private byte[] packetData;          // Biến để lưu dữ liệu gói tin
        private bool packetSelected = false; // Biến để theo dõi trạng thái gói tin đã được chọn

        public PacketViewerForm()
        {
            InitializeComponent();
            InitializeDataGridViews();
            dataGridViewPackets.CellDoubleClick += dataGridViewPackets_CellDoubleClick;
        }

        private void InitializeDataGridViews()
        {
            // Thiết lập DataGridView cho danh sách gói tin
            dataGridViewPackets.Columns.Add("IPv", "Phiên bản IP");
            dataGridViewPackets.Columns.Add("SourceIP", "Địa chỉ IP nguồn");
            dataGridViewPackets.Columns.Add("DestinationIP", "Địa chỉ IP đích");
            dataGridViewPackets.Columns.Add("Protocol", "Giao thức");
            dataGridViewPackets.ReadOnly = true;
            dataGridViewPackets.AllowUserToAddRows = false; // Không cho phép thêm hàng mới
        }

        public void AddPacketInfo(string sourceIp, string destinationIp, string protocol, byte[] packetData, string ipvVersion)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => AddPacketInfo(sourceIp, destinationIp, protocol, packetData, ipvVersion)));
            }
            else
            {
                dataGridViewPackets.Rows.Add(ipvVersion, sourceIp, destinationIp, protocol);
                this.packetData = packetData; // Lưu dữ liệu gói tin vào biến global
                // Lưu dữ liệu gói tin vào Tag của hàng
                dataGridViewPackets.Rows[dataGridViewPackets.Rows.Count - 1].Tag = packetData;
            }
        }

        private void DisplayPacketData(byte[] packetData)
        {
            if (packetData == null || packetData.Length == 0)
            {
                packetDetailsTextBox.Text = "Không có dữ liệu gói tin để hiển thị.";
                return;
            }

            StringBuilder sb = new StringBuilder();
            int bytesPerLine = 16; // Số byte hiển thị mỗi dòng

            for (int i = 0; i < packetData.Length; i++)
            {
                // Mỗi dòng bắt đầu bằng địa chỉ (offset)
                if (i % bytesPerLine == 0)
                {
                    if (i > 0)
                    {
                        sb.AppendLine(); // Thêm dòng mới sau mỗi dòng dữ liệu
                    }

                    sb.Append($"{i:X4} "); // Địa chỉ (offset) ở dạng hex
                }

                sb.Append($"{packetData[i]:X2} "); // Dữ liệu byte ở dạng hex

                // Thêm khoảng cách giữa các byte
                if (i % bytesPerLine == bytesPerLine - 1)
                {
                    sb.Append("   "); // Khoảng cách
                }

                // Thêm phần hiển thị ký tự ASCII
                if (i % bytesPerLine == bytesPerLine - 1 || i == packetData.Length - 1)
                {
                    sb.Append("  | ");
                    for (int j = i - (bytesPerLine - 1); j <= i; j++)
                    {
                        if (j < packetData.Length)
                        {
                            char c = (char)packetData[j];
                            sb.Append(char.IsControl(c) ? '.' : c); // Thay thế ký tự điều khiển bằng dấu chấm
                        }
                    }
                }
            }

            // Hiển thị dữ liệu trong TextBox
            packetDetailsTextBox.Text = sb.ToString();
        }

        private void DisplayPacketDetails(byte[] packetData)
        {
            if (packetData == null || packetData.Length == 0)
            {
                packetDetailsTextBox.Text = "Không có dữ liệu gói tin để hiển thị.";
                return;
            }

            StringBuilder sb = new StringBuilder();
            foreach (byte b in packetData)
            {
                sb.Append(b.ToString("X2") + " ");
            }

            // Cập nhật nội dung TextBox với thông tin gói tin
            packetDetailsTextBox.Text = sb.ToString();
        }

        private void StopCapturing()
        {
            if (isCapturing)
            {
                try
                {
                    if (capture != null)
                    {
                        capture.StopCapture();
                        capture.Close();
                        capture = null;
                    }
                    isCapturing = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error stopping capture: {ex.Message}");
                }
            }
        }

        private void dataGridViewPackets_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Đảm bảo là hàng hợp lệ
            {
                var row = dataGridViewPackets.Rows[e.RowIndex];
                var packetData = row.Tag as byte[]; // Lấy dữ liệu gói tin từ Tag

                if (packetData != null)
                {
                    // Lấy thông tin gói tin
                    var sourceIp = row.Cells["SourceIP"].Value.ToString();
                    var destinationIp = row.Cells["DestinationIP"].Value.ToString();
                    var protocol = row.Cells["Protocol"].Value.ToString();

                    // Tạo và hiển thị form Fdetails
                    Fdetails detailsForm = new Fdetails(sourceIp, destinationIp, protocol, packetData); // Đảm bảo packetData là byte[]
                    detailsForm.ShowDialog(); // Hiển thị form như một hộp thoại

                    // Đánh dấu rằng gói tin đã được chọn
                    packetSelected = true;
                }
                else
                {
                    MessageBox.Show("Không có dữ liệu gói tin để hiển thị."); // Nếu không có dữ liệu
                }
            }
        }

        private void dataGridViewPackets_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Đảm bảo là hàng hợp lệ
            {
                var row = dataGridViewPackets.Rows[e.RowIndex];
                var packetData = row.Tag as byte[]; // Lấy dữ liệu gói tin từ Tag

                if (packetData != null && !packetSelected) // Chỉ hiển thị nếu chưa chọn gói tin
                {
                    DisplayPacketDetails(packetData); // Hiển thị thông tin gói tin
                }
                else
                {
                    packetDetailsTextBox.Text = "Không có dữ liệu gói tin để hiển thị."; // Nếu không có dữ liệu
                }
            }
        }

        private void packetDetailsTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!packetSelected) // Chỉ hiển thị nếu chưa chọn gói tin
            {
                DisplayPacketData(packetData);
            }
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            StopCapturing();

            if (!isCapturing)
            {
                MessageBox.Show("Đã dừng việc bắt gói tin.");
            }
            else
            {
                MessageBox.Show("Không thể dừng bắt gói tin.");
            }
        }
    }
}
