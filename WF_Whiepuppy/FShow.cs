using PacketDotNet;
using SharpPcap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace WF_Whiepuppy
{
    public partial class PacketViewerForm : Form
    {
        private ICaptureDevice _captureDevice;
        private bool isCapturing = false;   // Biến để theo dõi trạng thái bắt gói tin
        private byte[] packetData;          // Biến để lưu dữ liệu gói tin
        private bool packetSelected = false; // Biến để theo dõi trạng thái gói tin đã được chọn
        private List<byte[]> capturedPackets = new List<byte[]>(); // Danh sách lưu các gói tin đã bắt

        public PacketViewerForm(ICaptureDevice captureDevice)
        {
            InitializeComponent();
            InitializeDataGridViews();
            this._captureDevice = captureDevice;
            this._captureDevice.OnPacketArrival += new PacketArrivalEventHandler(OnPacketArrival);
        }
        public class PacketInfo
        {
            public string IPv { get; set; }
            public string SourceIP { get; set; }
            public string DestinationIP { get; set; }
            public string Protocol { get; set; }
            public byte[] Data { get; set; }
        }

        private void InitializeDataGridViews()
        {
            // Thiết lập DataGridView cho danh sách gói tin
            dataGridViewPackets.Columns.Add("IPv", "Phiên bản IP");
            dataGridViewPackets.Columns.Add("SourceIP", "Địa chỉ IP nguồn");
            dataGridViewPackets.Columns.Add("DestinationIP", "Địa chỉ IP đích");
            dataGridViewPackets.Columns.Add("Protocol", "Giao thức");
            dataGridViewPackets.ReadOnly = true;
            dataGridViewPackets.AllowUserToAddRows = false;

        }

        private void OnPacketArrival(object sender, PacketCapture e)
        {
            var rawPacket = e.GetPacket();
            var packet = Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);
            capturedPackets.Add(rawPacket.Data);

            string ipvVersion = "N/A";
            string sourceIp = "N/A";
            string destinationIp = "N/A";
            string protocol = "N/A";

            // Kiểm tra gói tin IPv4
            var ipv4Packet = packet.Extract<IPv4Packet>();
            if (ipv4Packet != null)
            {
                ipvVersion = "IPv4";
                sourceIp = ipv4Packet.SourceAddress.ToString();
                destinationIp = ipv4Packet.DestinationAddress.ToString();
                protocol = ipv4Packet.Protocol.ToString();
            }

            // Kiểm tra gói tin IPv6
            var ipv6Packet = packet.Extract<IPv6Packet>();
            if (ipv6Packet != null)
            {
                ipvVersion = "IPv6";
                sourceIp = ipv6Packet.SourceAddress.ToString();
                destinationIp = ipv6Packet.DestinationAddress.ToString();
                protocol = ipv6Packet.NextHeader.ToString(); // IPv6 dùng NextHeader
            }

            // Thêm vào DataGridView
            UpdatePacketViewerForm(ipvVersion, sourceIp, destinationIp, protocol, rawPacket.Data);
        }

        // Lưu lại file chứa các gói tin
        private void SaveAllCapturedPacketsToFile()
        {
            if (capturedPackets.Count == 0)
            {
                MessageBox.Show("Không có gói tin nào để lưu.");
                return;
            }

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Packet Files (*.pkt)|*.pkt|All Files (*.*)|*.*";
                saveFileDialog.Title = "Save Captured Packets";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (FileStream fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.Write))
                        {
                            foreach (var packetData in capturedPackets)
                            {
                                // Ghi mỗi gói tin vào file
                                fileStream.Write(packetData, 0, packetData.Length);
                                fileStream.Write(new byte[] { 0x0D, 0x0A }, 0, 2); // Thêm một dòng mới sau mỗi gói tin (nếu cần)
                            }
                        }

                        MessageBox.Show("Tất cả gói tin đã được lưu thành công.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi lưu gói tin: {ex.Message}");
                    }
                }
            }
        }

        public void UpdatePacketViewerForm(string ipvVersion, string sourceIp, string destinationIp, string protocol, byte[] packetData)
        {
            // Đảm bảo gọi từ giao diện chính nếu cần
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdatePacketViewerForm(ipvVersion, sourceIp, destinationIp, protocol, packetData)));
            }
            else
            {
                // Thêm thông tin vào DataGridView
                dataGridViewPackets.Rows.Add(ipvVersion, sourceIp, destinationIp, protocol);
                this.packetData = packetData; // Lưu dữ liệu gói tin vào biến toàn cục
                                              // Lưu gói tin vào Tag của hàng DataGridView
                dataGridViewPackets.Rows[dataGridViewPackets.Rows.Count - 1].Tag = packetData;
            }
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

        public void ProcessPacket(Packet packet)
        {
            // Kiểm tra nếu đây là gói tin UDP
            var udpPacket = packet.Extract<UdpPacket>();
            if (udpPacket == null) return;

            // Kiểm tra nếu gói tin đến từ cổng DNS (53)
            if (udpPacket.DestinationPort == 53 || udpPacket.SourcePort == 53)
            {
                // Phân tích payload DNS
                var payload = udpPacket.PayloadData;
                string domainName = ParseDnsQuery(payload);
                if (!string.IsNullOrEmpty(domainName))
                {
                    // Hiển thị tên miền trong packetDetailsTextBox
                    packetDetailsTextBox.AppendText($"Domain: {domainName}\n");
                }
            }
        }

        private string ParseDnsQuery(byte[] payload)
        {
            try
            {
                int position = 12; // DNS header dài 12 byte
                StringBuilder domainName = new StringBuilder();

                while (position < payload.Length)
                {
                    byte length = payload[position++];
                    if (length == 0) break;

                    if (position + length > payload.Length) return null;

                    domainName.Append(Encoding.ASCII.GetString(payload, position, length));
                    domainName.Append(".");
                    position += length;
                }

                // Loại bỏ dấu chấm cuối cùng
                return domainName.ToString().TrimEnd('.');
            }
            catch
            {
                return null;
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
                // Dòng mới cho mỗi bytesPerLine
                if (i % bytesPerLine == 0)
                {
                    if (i > 0)
                    {
                        // Thêm ASCII của dòng trước
                        sb.Append("  | ");
                        for (int j = i - bytesPerLine; j < i; j++)
                        {
                            char c = (char)packetData[j]; // Ép kiểu byte sang char
                            sb.Append(c >= 0x20 && c <= 0x7E ? c : '.'); // Sử dụng giá trị ép kiểu
                        }
                        sb.AppendLine(); // Xuống dòng
                    }
                    sb.Append($"{i:X4} "); // Địa chỉ offset
                }

                // Thêm byte dạng Hex
                sb.Append($"{packetData[i]:X2} ");

                // Xử lý dòng cuối cùng
                if (i == packetData.Length - 1)
                {
                    int padding = (bytesPerLine - (i % bytesPerLine) - 1) * 3;
                    sb.Append(new string(' ', padding)); // Đệm khoảng trống cho phần Hex

                    sb.Append("  | ");
                    for (int j = i - (i % bytesPerLine); j <= i; j++)
                    {
                        char c = (char)packetData[j]; // Ép kiểu byte sang char
                        sb.Append(c >= 0x20 && c <= 0x7E ? c : '.'); // Sử dụng giá trị ép kiểu
                    }
                }
            }

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
            if (_captureDevice != null && isCapturing)
            {
                try
                {
                    _captureDevice.StopCapture();
                    _captureDevice.Close();
                    isCapturing = false;
                    MessageBox.Show("Đã dừng việc bắt gói tin.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi dừng bắt gói tin: {ex.Message}");
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

                if (packetData != null)
                {
                    // Lấy thông tin gói tin
                    var sourceIp = row.Cells["SourceIP"].Value.ToString();
                    var destinationIp = row.Cells["DestinationIP"].Value.ToString();
                    var protocol = row.Cells["Protocol"].Value.ToString();

                    // Tạo và hiển thị form Fdetails
                    Fdetails detailsForm = new Fdetails(sourceIp, destinationIp, protocol, packetData); // Đảm bảo packetData là byte[]
                    detailsForm.ShowDialog(); // Hiển thị form như một hộp thoại

                    // Hiển thị nội dung gói tin trong packetDetailsTextBox
                    DisplayPacketData(packetData);  // Gọi phương thức DisplayPacketData để hiển thị gói tin

                    // Hiển thị thông tin gói tin trong treeViewPacketDetails
                    DisplayPacketInfoInTreeView(packetData);  // Gọi phương thức DisplayPacketInfoInTreeView để hiển thị trong TreeView

                    // Đánh dấu rằng gói tin đã được chọn
                    packetSelected = true;
                }
                else
                {
                    MessageBox.Show("Không có dữ liệu gói tin để hiển thị."); // Nếu không có dữ liệu
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
            if (_captureDevice != null && isCapturing)
            {
                try
                {
                    _captureDevice.StopCapture();
                    _captureDevice.Close();
                    isCapturing = false;
                    MessageBox.Show("Đã dừng việc bắt gói tin.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi dừng bắt gói tin: {ex.Message}");
                }
            }
        }

        private void DisplayPacketInfoInTreeView(byte[] packetData)
        {
            treeViewPacketDetails.Nodes.Clear();

            try
            {
                // Phân tích gói tin
                var packet = PacketDotNet.Packet.ParsePacket(PacketDotNet.LinkLayers.Ethernet, packetData);

                // Frame Information
                TreeNode frameNode = new TreeNode($"Frame: {packetData.Length} bytes");
                treeViewPacketDetails.Nodes.Add(frameNode);

                // Ethernet Information
                var ethernetPacket = packet.Extract<PacketDotNet.EthernetPacket>();
                if (ethernetPacket != null)
                {
                    TreeNode ethernetNode = new TreeNode("Ethernet II");
                    ethernetNode.Nodes.Add($"Source: {ethernetPacket.SourceHardwareAddress}");
                    ethernetNode.Nodes.Add($"Destination: {ethernetPacket.DestinationHardwareAddress}");

                    // Kiểm tra loại Ethernet
                    if (ethernetPacket.Type == PacketDotNet.EthernetType.Arp)
                    {
                        ethernetNode.Nodes.Add("Ethernet Type: ARP");
                    }
                    else if (ethernetPacket.Type == PacketDotNet.EthernetType.IPv4)
                    {
                        ethernetNode.Nodes.Add("Ethernet Type: IPv4");
                    }
                    else
                    {
                        ethernetNode.Nodes.Add($"Ethernet Type: {ethernetPacket.Type}");
                    }

                    treeViewPacketDetails.Nodes.Add(ethernetNode);
                }

                // IP Information
                var ipPacket = packet.Extract<PacketDotNet.IPPacket>();
                if (ipPacket != null)
                {
                    TreeNode ipNode;
                    if (ipPacket is PacketDotNet.IPv4Packet ipv4Packet)
                    {
                        ipNode = new TreeNode("IPv4");
                        ipNode.Nodes.Add($"Source: {ipv4Packet.SourceAddress}");
                        ipNode.Nodes.Add($"Destination: {ipv4Packet.DestinationAddress}");
                    }
                    else if (ipPacket is PacketDotNet.IPv6Packet ipv6Packet)
                    {
                        ipNode = new TreeNode("IPv6");
                        ipNode.Nodes.Add($"Source: {ipv6Packet.SourceAddress}");
                        ipNode.Nodes.Add($"Destination: {ipv6Packet.DestinationAddress}");
                    }
                    else
                    {
                        ipNode = new TreeNode("Unknown IP Version");
                    }
                    ipNode.Nodes.Add($"Protocol: {ipPacket.Protocol}");
                    treeViewPacketDetails.Nodes.Add(ipNode);
                }

                // Protocol Information - TCP
                var tcpPacket = packet.Extract<PacketDotNet.TcpPacket>();
                if (tcpPacket != null)
                {
                    TreeNode tcpNode = new TreeNode("TCP");
                    tcpNode.Nodes.Add($"Source Port: {tcpPacket.SourcePort}");
                    tcpNode.Nodes.Add($"Destination Port: {tcpPacket.DestinationPort}");
                    tcpNode.Nodes.Add($"Sequence Number: {tcpPacket.SequenceNumber}");
                    tcpNode.Nodes.Add($"Acknowledgment Number: {tcpPacket.AcknowledgmentNumber}");

                    // Lấy dữ liệu từ TCP Header
                    byte[] tcpHeader = tcpPacket.HeaderData;

                    // Byte thứ 13 trong TCP header chứa Flags
                    byte tcpFlags = tcpHeader[13];

                    // Phân tích Flags
                    bool fin = (tcpFlags & 0x01) != 0;
                    bool syn = (tcpFlags & 0x02) != 0;
                    bool rst = (tcpFlags & 0x04) != 0;
                    bool psh = (tcpFlags & 0x08) != 0;
                    bool ack = (tcpFlags & 0x10) != 0;
                    bool urg = (tcpFlags & 0x20) != 0;

                    // Tạo chuỗi cờ
                    string flags = $"{(fin ? "FIN " : "")}" +
                                   $"{(syn ? "SYN " : "")}" +
                                   $"{(rst ? "RST " : "")}" +
                                   $"{(psh ? "PSH " : "")}" +
                                   $"{(ack ? "ACK " : "")}" +
                                   $"{(urg ? "URG " : "")}".Trim();

                    // Thêm vào TreeNode
                    tcpNode.Nodes.Add($"Flags: {flags}");

                    // Hiển thị Payload nếu có
                    if (tcpPacket.PayloadData != null && tcpPacket.PayloadData.Length > 0)
                    {
                        TreeNode payloadNode = new TreeNode("TCP Payload:");

                        // Chuyển đổi payload thành Hex hoặc ASCII
                        string tcpPayloadHex = BitConverter.ToString(tcpPacket.PayloadData).Replace("-", " ");
                        payloadNode.Nodes.Add($"Hex: {tcpPayloadHex}");

                        // Hoặc nếu bạn muốn hiển thị dữ liệu ASCII của Payload
                        string tcpPayloadAscii = Encoding.ASCII.GetString(tcpPacket.PayloadData);
                        payloadNode.Nodes.Add($"ASCII: {tcpPayloadAscii}");

                        tcpNode.Nodes.Add(payloadNode);
                    }

                    treeViewPacketDetails.Nodes.Add(tcpNode);
                }

                // Protocol Information - UDP
                var udpPacket = packet.Extract<PacketDotNet.UdpPacket>();
                if (udpPacket != null)
                {
                    TreeNode udpNode = new TreeNode("UDP");
                    udpNode.Nodes.Add($"Source Port: {udpPacket.SourcePort}");
                    udpNode.Nodes.Add($"Destination Port: {udpPacket.DestinationPort}");

                    // Hiển thị Payload nếu có
                    if (udpPacket.PayloadData != null && udpPacket.PayloadData.Length > 0)
                    {
                        TreeNode payloadNode = new TreeNode("UDP Payload:");

                        // Chuyển đổi payload thành Hex hoặc ASCII
                        string udpPayloadHex = BitConverter.ToString(udpPacket.PayloadData).Replace("-", " ");
                        payloadNode.Nodes.Add($"Hex: {udpPayloadHex}");

                        // Hoặc nếu bạn muốn hiển thị dữ liệu ASCII của Payload
                        string udpPayloadAscii = Encoding.ASCII.GetString(udpPacket.PayloadData);
                        payloadNode.Nodes.Add($"ASCII: {udpPayloadAscii}");

                        udpNode.Nodes.Add(payloadNode);
                    }

                    treeViewPacketDetails.Nodes.Add(udpNode);
                }

                // Protocol Information - ICMP
                var icmpPacket = packet.Extract<PacketDotNet.IcmpV4Packet>();
                if (icmpPacket != null)
                {
                    TreeNode icmpNode = new TreeNode("ICMP");
                    icmpNode.Nodes.Add($"Type: {icmpPacket.TypeCode}");
                    icmpNode.Nodes.Add($"Checksum: {icmpPacket.Checksum}");

                    // Hiển thị Payload nếu có
                    if (icmpPacket.PayloadData != null && icmpPacket.PayloadData.Length > 0)
                    {
                        TreeNode payloadNode = new TreeNode("ICMP Payload:");

                        // Chuyển đổi payload thành Hex hoặc ASCII
                        string icmpPayloadHex = BitConverter.ToString(icmpPacket.PayloadData).Replace("-", " ");
                        payloadNode.Nodes.Add($"Hex: {icmpPayloadHex}");

                        // Hoặc nếu bạn muốn hiển thị dữ liệu ASCII của Payload
                        string icmpPayloadAscii = Encoding.ASCII.GetString(icmpPacket.PayloadData);
                        payloadNode.Nodes.Add($"ASCII: {icmpPayloadAscii}");

                        icmpNode.Nodes.Add(payloadNode);
                    }

                    treeViewPacketDetails.Nodes.Add(icmpNode);
                }

                // Mở rộng tất cả các node
                treeViewPacketDetails.ExpandAll();
            }
            catch (Exception ex)
            {
                // Xử lý lỗi khi phân tích gói tin
                TreeNode errorNode = new TreeNode($"Error parsing packet: {ex.Message}");
                treeViewPacketDetails.Nodes.Add(errorNode);
            }
        }


        private string GetFrameInfo(byte[] packetData)
        {
            return $"Frame: {packetData.Length} bytes";
        }

        private string GetEthernetInfo(byte[] packetData)
        {
            var packet = PacketDotNet.Packet.ParsePacket(PacketDotNet.LinkLayers.Ethernet, packetData);
            var ethernetPacket = packet.Extract<PacketDotNet.EthernetPacket>();
            if (ethernetPacket != null)
            {
                return $"Ethernet II, Src: {ethernetPacket.SourceHardwareAddress}, Dst: {ethernetPacket.DestinationHardwareAddress}";
            }
            return "Ethernet II: N/A";
        }

        private string GetIPInfo(byte[] packetData)
        {
            var packet = PacketDotNet.Packet.ParsePacket(PacketDotNet.LinkLayers.Ethernet, packetData);
            var ipPacket = packet.Extract<PacketDotNet.IPv4Packet>();
            if (ipPacket != null)
            {
                return $"Internet Protocol Version 4, Src: {ipPacket.SourceAddress}, Dst: {ipPacket.DestinationAddress}";
            }
            return "IP: N/A";
        }

        private string GetProtocolInfo(byte[] packetData)
        {
            var packet = PacketDotNet.Packet.ParsePacket(PacketDotNet.LinkLayers.Ethernet, packetData);
            var ipPacket = packet.Extract<PacketDotNet.IPv4Packet>();
            if (ipPacket != null)
            {
                return $"Protocol: {ipPacket.Protocol}";
            }
            return "Protocol: N/A";
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // Nếu dữ liệu gói tin đã được chọn, hiển thị chi tiết gói tin
            if (packetData != null)
            {
                DisplayPacketInfoInTreeView(packetData);
            }
        }

        private void FilterPackets_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnApplyFilter_Click(object sender, EventArgs e)
        {

        }

        private void btnStartCapture_Click(object sender, EventArgs e)
        {
            if (_captureDevice != null && !isCapturing)
            {
                try
                {
                    // Mở thiết bị và bắt đầu bắt gói tin
                    _captureDevice.Open();
                    _captureDevice.StartCapture();
                    isCapturing = true;
                    MessageBox.Show("Bắt đầu bắt gói tin.");

                    // Cập nhật giao diện ngay lập tức để hiển thị gói tin đầu tiên
                    _captureDevice.OnPacketArrival += new PacketArrivalEventHandler(OnPacketArrival);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi bắt đầu bắt gói tin: {ex.Message}");
                }
            }
        }

        // Lưu lại file
        private void SavePacketToFile(byte[] packetData)
        {
            if (packetData == null || packetData.Length == 0)
            {
                MessageBox.Show("Không có dữ liệu gói tin để lưu.");
                return;
            }

            // Mở hộp thoại lưu file để chọn tên và đường dẫn lưu file
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Packet Files (*.pkt)|*.pkt|All Files (*.*)|*.*";
                saveFileDialog.Title = "Save Packet File";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Ghi dữ liệu gói tin vào file
                        using (FileStream fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.Write))
                        {
                            fileStream.Write(packetData, 0, packetData.Length);
                        }

                        MessageBox.Show("Gói tin đã được lưu thành công.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi lưu gói tin: {ex.Message}");
                    }
                }
            }
        }

        private void treeViewPacketDetails_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void btn_stop_Click_1(object sender, EventArgs e)
        {
            if (_captureDevice != null && isCapturing)
            {
                try
                {
                    _captureDevice.StopCapture();
                    _captureDevice.Close();
                    isCapturing = false;
                    MessageBox.Show("Đã dừng việc bắt gói tin.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi dừng bắt gói tin: {ex.Message}");
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnApplyFilter_Click_1(object sender, EventArgs e)
        {
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra xem có gói tin nào đã được bắt hay chưa
                if (capturedPackets.Count == 0)
                {
                    MessageBox.Show("Không có gói tin nào để lưu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Mở hộp thoại lưu tệp
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Packet Files (*.pkt)|*.pkt|All Files (*.*)|*.*";
                    saveFileDialog.Title = "Save Captured Packets";

                    // Nếu người dùng chọn tệp và nhấn "Lưu"
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            // Tạo tệp mới và ghi dữ liệu vào
                            using (FileStream fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.Write))
                            {
                                foreach (var packetData in capturedPackets)
                                {
                                    // Kiểm tra gói tin có dữ liệu hợp lệ không
                                    if (packetData == null || packetData.Length == 0)
                                    {
                                        MessageBox.Show("Gói tin bị lỗi hoặc không có dữ liệu để lưu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }

                                    // Ghi gói tin vào file
                                    fileStream.Write(packetData, 0, packetData.Length);
                                    fileStream.Write(new byte[] { 0x0D, 0x0A }, 0, 2); // Thêm một dòng mới sau mỗi gói tin (nếu cần)
                                }
                            }

                            MessageBox.Show("Tất cả gói tin đã được lưu thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (UnauthorizedAccessException)
                        {
                            MessageBox.Show("Không có quyền truy cập để lưu tệp này. Vui lòng kiểm tra quyền truy cập của thư mục đích.", "Lỗi Quyền", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        catch (IOException ioEx)
                        {
                            MessageBox.Show($"Lỗi trong quá trình ghi tệp: {ioEx.Message}", "Lỗi I/O", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Lỗi không xác định khi lưu gói tin: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Quá trình lưu bị hủy. Không có tệp nào được lưu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                // Bắt lỗi tổng thể và thông báo cho người dùng
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
