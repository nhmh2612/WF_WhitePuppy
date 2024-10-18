using PacketDotNet;
using System;
using System.Text;
using System.Windows.Forms;

namespace WF_Whiepuppy
{
    public partial class PacketViewerForm : Form
    {
        private bool isCapturing = false; // Biến để theo dõi trạng thái bắt gói tin

        public PacketViewerForm()
        {
            InitializeComponent();
            InitializeDataGridViews();
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

            // Thiết lập DataGridView cho thông tin gói tin chi tiết
            dataGridViewPacketDetails.Columns.Add("Field", "Trường");
            dataGridViewPacketDetails.Columns.Add("Value", "Giá trị");
            dataGridViewPacketDetails.ReadOnly = true;
            dataGridViewPacketDetails.AllowUserToAddRows = false; // Không cho phép thêm hàng mới

            // Thêm sự kiện cho DataGridView gói tin
            dataGridViewPackets.CellClick += DataGridViewPackets_CellClick;
        }

        // Phương thức để thêm thông tin gói tin vào DataGridView
        public void AddPacketInfo(string sourceIp, string destinationIp, string protocol, byte[] packetData, string ipvVersion)
        {
            if (InvokeRequired)
            {
                // Nếu đang ở luồng khác, sử dụng Invoke để gọi lại phương thức trên luồng giao diện
                Invoke(new Action(() => AddPacketInfo(sourceIp, destinationIp, protocol, packetData, ipvVersion)));
            }
            else
            {
                // Thêm hàng mới vào DataGridView
                dataGridViewPackets.Rows.Add(ipvVersion, sourceIp, destinationIp, protocol);
            }
        }


        // Sự kiện khi người dùng nhấp vào một ô trong DataGridView gói tin
        private void DataGridViewPackets_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Đảm bảo là hàng hợp lệ
            {
                var row = dataGridViewPackets.Rows[e.RowIndex];
                var packetData = (byte[])row.Cells[3].Value; // Lấy dữ liệu gói tin

                // Hiển thị nội dung gói tin trong DataGridView bên cạnh
                DisplayPacketDetails(packetData);
            }
        }

        // Phương thức để hiển thị thông tin gói tin chi tiết
        private void DisplayPacketDetails(byte[] packetData)
        {
            dataGridViewPacketDetails.Rows.Clear(); // Xóa dữ liệu cũ trước khi thêm dữ liệu mới

            dataGridViewPacketDetails.Rows.Add("Length", packetData.Length.ToString());
            dataGridViewPacketDetails.Rows.Add("Hex Data", BitConverter.ToString(packetData).Replace("-", " "));
        }


        private void btn_stop_Click(object sender, EventArgs e)
        {

        }

        // Phương thức để bắt đầu bắt gói tin, gọi từ Fmain
        public void StartCapturing()
        {
            isCapturing = true;
        }

        // Phương thức để dừng bắt gói tin, gọi từ Fmain
        public void StopCapturing()
        {
            isCapturing = false;
        }
    }
}
