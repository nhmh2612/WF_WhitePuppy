using SharpPcap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using PacketDotNet;
using PacketDotNet.Ieee80211;
using System.Net.NetworkInformation;

namespace WF_Whiepuppy
{
    public partial class Fmain : Form
    {
        private ICaptureDevice currentDevice;

        public Fmain()
        {
            InitializeComponent();
            LoadNetworkDevices();

            // Thiết lập ComboBox 
            comboBoxFilter.Items.Add("Show All");
            comboBoxFilter.Items.Add("Wired");
            comboBoxFilter.Items.Add("Wireless");
            comboBoxFilter.Items.Add("Virtual");
            comboBoxFilter.SelectedIndex = 0; // Chọn "Show All" là giá trị mặc định
        }

        private void Fmain_Load(object sender, EventArgs e)
        {
            LoadNetworkDevices();
        }

        // Hàm lấy danh sách thiết bị mạng và hiển thị trong ListBox
        private void LoadNetworkDevices()
        {
            try
            {
                // Lấy danh sách các thiết bị mạng
                var devices = CaptureDeviceList.Instance;

                // Kiểm tra nếu không có thiết bị nào
                if (devices.Count < 1)
                {
                    MessageBox.Show("Không tìm thấy thiết bị nào.");
                    return;
                }

                // Hiển thị danh sách các thiết bị mạng trong ListBox
                listBoxDevices.Items.Clear(); // Xóa danh sách cũ
                foreach (var device in devices)
                {
                    listBoxDevices.Items.Add(device.Description);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }

        private void StartPacketCapture(string deviceDescription)
        {
            var device = CaptureDeviceList.Instance.FirstOrDefault(d => d.Description == deviceDescription);
            if (device == null)
            {
                MessageBox.Show("Không thể tìm thấy thiết bị đã chọn.");
                return;
            }

            // Mở form PacketViewerForm và chuyển thiết bị cho form này
            PacketViewerForm packetViewerForm = new PacketViewerForm(device);
            packetViewerForm.Show();
        }

        // Sự kiện khi nhấn nút "F5" để làm mới danh sách
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadNetworkDevices();
        }

        private void listBoxDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxDevices.SelectedItem != null)
            {
                var selectedDevice = listBoxDevices.SelectedItem.ToString();

                // Nếu thiết bị hiện tại khác với thiết bị đã chọn
                if (currentDevice == null || currentDevice.Description != selectedDevice)
                {
                    // Dừng bắt gói tin nếu thiết bị hiện tại đã được mở
                    if (currentDevice != null)
                    {
                        currentDevice.StopCapture();
                        currentDevice.Close();
                    }

                    // Chuyển sang form mới và bắt đầu bắt gói tin
                    StartPacketCapture(selectedDevice);
                }
            }
        }

        private static void OnPacketArrival(object sender, PacketCapture e)
        {
            var rawPacket = e.GetPacket();
            var packet = Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);

            // Kiểm tra gói tin IPv4 và IPv6
            var ipPacket = packet.Extract<IPPacket>();
            string ipvVersion = "IPv4"; // Mặc định là IPv4

            // Kiểm tra nếu gói tin là IPv6
            if (ipPacket == null)
            {
                var ipv6Packet = packet.Extract<IPv6Packet>();
                if (ipv6Packet != null)
                {
                    ipvVersion = "IPv6";
                    var sourceIp = ipv6Packet.SourceAddress.ToString();
                    var destinationIp = ipv6Packet.DestinationAddress.ToString();
                    var protocol = ipv6Packet.Protocol.ToString();

                    // Cập nhật thông tin gói tin trong DataGridView
                    UpdatePacketViewerForm(sourceIp, destinationIp, protocol, rawPacket.Data, ipvVersion);
                }
            }
            else
            {
                var sourceIp = ipPacket.SourceAddress.ToString();
                var destinationIp = ipPacket.DestinationAddress.ToString();
                var protocol = ipPacket.Protocol.ToString(); // Lấy giao thức từ IP

                // Cập nhật thông tin gói tin trong DataGridView
                UpdatePacketViewerForm(sourceIp, destinationIp, protocol, rawPacket.Data, ipvVersion);
            }
        }

        // Phương thức hỗ trợ để cập nhật PacketViewerForm
        private static void UpdatePacketViewerForm(string sourceIp, string destinationIp, string protocol, byte[] packetData, string ipvVersion)
        {
            // Kiểm tra xem form có mở hay không và cập nhật nó
            if (Application.OpenForms.OfType<PacketViewerForm>().FirstOrDefault() is PacketViewerForm packetViewerForm)
            {
                if (packetViewerForm.InvokeRequired)
                {
                    packetViewerForm.Invoke(new Action(() =>
                        packetViewerForm.AddPacketInfo(sourceIp, destinationIp, protocol, packetData, ipvVersion)));
                }
                else
                {
                    packetViewerForm.AddPacketInfo(sourceIp, destinationIp, protocol, packetData, ipvVersion);
                }
            }
        }

        private void comboBoxFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            string filter = comboBoxFilter.SelectedItem.ToString();
            List<ICaptureDevice> filteredDevices = new List<ICaptureDevice>();

            foreach (var device in CaptureDeviceList.Instance)
            {
                // Lấy danh sách các giao diện mạng
                var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
                var matchingInterface = networkInterfaces.FirstOrDefault(ni => ni.Description == device.Description);

                // Nếu tìm thấy giao diện mạng phù hợp, áp dụng bộ lọc
                if (matchingInterface != null)
                {
                    if (filter == "Show All" ||
                        (filter == "Wired" && matchingInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet) ||
                        (filter == "Wireless" && matchingInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211) ||
                        (filter == "Virtual" && matchingInterface.NetworkInterfaceType == NetworkInterfaceType.Loopback))
                    {
                        filteredDevices.Add(device);
                    }
                }
            }

            // Cập nhật ListBox với các thiết bị đã lọc
            listBoxDevices.Items.Clear();
            foreach (var device in filteredDevices)
            {
                listBoxDevices.Items.Add(device.Description);
            }
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "https://web.facebook.com/HoangluvTrang2426?locale=vi_VN";

            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true // Đảm bảo rằng trình duyệt được mở
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "https://web.facebook.com/HoangluvTrang2426?locale=vi_VN";

            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true // Đảm bảo rằng trình duyệt được mở
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "https://github.com/nhmh2612";

            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true // Đảm bảo rằng trình duyệt được mở
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }
    }
}
