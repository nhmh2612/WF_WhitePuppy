using System;
using System.Text;
using System.Windows.Forms;

namespace WF_Whiepuppy
{
    public partial class Fdetails : Form
    {
        public Fdetails(string sourceIP, string destinationIP, string protocol, byte[] data)
        {
            InitializeComponent();

            // Hiển thị thông tin trên form
            lblSourceIP.Text = "Địa chỉ nguồn: " + sourceIP;
            lblDestinationIP.Text = "Địa chỉ đích: " + destinationIP;
            lblProtocol.Text = "Loại giao thức: " + protocol;

            // Hiển thị dữ liệu gói tin
            lblData.Text = "Dữ liệu:\n" + ConvertDataToHex(data);

            // Hiển thị panel chứa thông tin
            panelPacketInfo.Visible = true;
        }

        private string ConvertDataToHex(byte[] data)
        {
            if (data == null || data.Length == 0)
                return "Không có dữ liệu để hiển thị.";

            StringBuilder sb = new StringBuilder();
            int bytesPerLine = 16; // Số byte hiển thị mỗi dòng

            for (int i = 0; i < data.Length; i++)
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

                sb.Append($"{data[i]:X2} "); // Dữ liệu byte ở dạng hex

                // Thêm khoảng cách giữa các byte
                if (i % bytesPerLine == bytesPerLine - 1)
                {
                    sb.Append("   "); // Khoảng cách
                }

                // Thêm phần hiển thị ký tự ASCII
                if (i % bytesPerLine == bytesPerLine - 1 || i == data.Length - 1)
                {
                    sb.Append("  | ");
                    for (int j = i - (bytesPerLine - 1); j <= i; j++)
                    {
                        if (j < data.Length)
                        {
                            char c = (char)data[j];
                            sb.Append(char.IsControl(c) ? '.' : c); // Thay thế ký tự điều khiển bằng dấu chấm
                        }
                    }
                }
            }

            return sb.ToString();
        }

        private void Fdetails_Load(object sender, EventArgs e)
        {
        }

        private void panelPacketInfo_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}
