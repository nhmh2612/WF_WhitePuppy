# WF_WhitePuppy - Phần mềm Bắt và Xem Nội Dung Gói Tin Mạng

WF_WhitePuppy là một phần mềm mạnh mẽ được thiết kế để bắt, phân tích và xem thông tin chi tiết của các gói tin mạng trong hệ thống. Phần mềm sử dụng thư viện **SharpPcap** và **PacketDotNet** để thu thập và phân tích dữ liệu gói tin từ các thiết bị mạng trong môi trường của bạn.

## Tính Năng

- **Bắt Gói Tin Mạng**: Phần mềm hỗ trợ bắt các gói tin qua các thiết bị mạng có sẵn trên hệ thống.
- **Hiển Thị Thông Tin Gói Tin**: Phân tích và hiển thị thông tin chi tiết của các gói tin, bao gồm địa chỉ IP, giao thức, và các dữ liệu tầng thấp khác.
- **Lọc Gói Tin**: Cho phép lọc các gói tin theo các tiêu chí như địa chỉ IP nguồn, địa chỉ IP đích, giao thức, v.v.
- **Lưu Dữ Liệu Gói Tin**: Sau khi bắt gói tin, người dùng có thể lưu thông tin gói tin vào tệp `.pcap` để sử dụng lại sau này.
- **Phân Tích Chi Tiết**: Phân tích thông tin chi tiết của từng gói tin, chẳng hạn như các thông tin liên quan đến Ethernet, IP, và các giao thức khác.

## Cài Đặt

### Yêu Cầu Hệ Thống

- **.NET Framework** 4.5 trở lên.
- **SharpPcap** và **PacketDotNet**: Các thư viện cần thiết để thực hiện việc bắt và phân tích gói tin.
- Thiết bị mạng: Cần có card mạng có thể bắt gói tin. Phần mềm hỗ trợ card mạng ảo và máy ảo nếu chúng được cấu hình đúng.

### Cài Đặt Thư Viện

Để sử dụng phần mềm, bạn cần cài đặt các thư viện phụ thuộc sau:

1. **SharpPcap** và **PacketDotNet**:
   - Mở `NuGet Package Manager Console` trong Visual Studio và chạy lệnh sau:
     ```bash
     Install-Package SharpPcap
     Install-Package PacketDotNet
     ```

2. Thêm các thư viện vào dự án của bạn.

### Cài Đặt và Sử Dụng

1. **Clone repository**:
   - Nếu bạn muốn cài đặt từ mã nguồn, bạn có thể clone repository từ GitHub:
     ```bash
     git clone https://github.com/your-repo/WF_WhitePuppy.git
     ```

2. **Mở dự án trong Visual Studio**:
   - Mở file solution `.sln` trong Visual Studio và biên dịch lại dự án.

3. **Chạy phần mềm**:
   - Sau khi biên dịch thành công, bạn có thể chạy phần mềm trực tiếp từ Visual Studio hoặc sử dụng tệp thực thi (.exe) trong thư mục output.

### Pre-condition (Điều kiện tiên quyết)

- **Card mạng phải hoạt động**: Phần mềm yêu cầu card mạng phải được bật và khả dụng để bắt các gói tin.
- **Máy ảo hoặc card mạng ảo**: Nếu sử dụng card mạng ảo hoặc máy ảo, đảm bảo rằng các thiết bị này đã được cấu hình đúng và có thể bắt gói tin.
- **Quyền truy cập mạng**: Phần mềm yêu cầu quyền truy cập mạng đầy đủ để có thể bắt gói tin từ các thiết bị.
- **Quyền chạy quyển admin**: Phần mềm có thể yêu cầu quyền chạy admin.
- 
### Post-condition (Trạng thái hệ thống sau khi chạy)

- **Hệ thống ổn định**: Sau khi chạy, phần mềm không ảnh hưởng đến các ứng dụng khác đang chạy trên hệ thống.
- **Lưu trữ gói tin**: Dữ liệu gói tin sẽ được lưu vào tệp `.pcap` hoặc tệp định dạng khác mà bạn chọn.
- **Phân tích thành công**: Phần mềm sẽ phân tích và hiển thị chi tiết gói tin ngay sau khi bắt xong.

## Hướng Dẫn Sử Dụng

### Bước 1: Chọn Thiết Bị Mạng

Khi mở phần mềm, danh sách các thiết bị mạng khả dụng trên hệ thống sẽ được hiển thị. Bạn cần chọn thiết bị mạng mà bạn muốn bắt gói tin từ đó.

### Bước 2: Bắt Gói Tin

Nhấn vào nút **Start Capture** để bắt đầu việc thu thập gói tin. Các gói tin sẽ được hiển thị trong bảng với các thông tin như:

- **IPv Version**: Phiên bản giao thức IP (IPv4 hoặc IPv6).
- **Source IP**: Địa chỉ IP nguồn.
- **Destination IP**: Địa chỉ IP đích.
- **Protocol**: Giao thức sử dụng trong gói tin.

### Bước 3: Lọc Gói Tin

Phần mềm hỗ trợ tính năng lọc gói tin theo các tiêu chí sau:

- **Source IP**: Lọc gói tin theo địa chỉ IP nguồn.
- **Destination IP**: Lọc gói tin theo địa chỉ IP đích.
- **Protocol**: Lọc gói tin theo giao thức (ví dụ: TCP, UDP, ICMP).
- **Port**: Lọc gói tin theo cổng.

Sử dụng các tùy chọn lọc để chỉ hiển thị các gói tin mà bạn quan tâm.

### Bước 4: Xem Chi Tiết Gói Tin

Khi nhấp vào một gói tin trong bảng, phần mềm sẽ hiển thị thông tin chi tiết của gói tin đó, bao gồm các lớp (Layer) như:

- **Ethernet**: Thông tin về địa chỉ MAC nguồn và đích.
- **IP**: Thông tin về địa chỉ IP, subnet mask, và các thuộc tính liên quan.
- **Protocol**: Chi tiết về giao thức sử dụng (TCP, UDP, ICMP, v.v.).

### Bước 5: Lưu Gói Tin

Sau khi thu thập và phân tích các gói tin, bạn có thể lưu lại dữ liệu vào tệp `.pcap` hoặc một định dạng khác để xem xét lại sau. Để lưu, nhấn nút **Save** và chọn vị trí lưu tệp.

### Bước 6: Dừng Bắt Gói Tin

Để dừng việc bắt gói tin, nhấn vào nút **Stop Capture**. Sau khi dừng, dữ liệu gói tin sẽ được lưu lại và phần mềm sẽ thông báo về trạng thái kết thúc quá trình bắt gói tin.

## Các Phiên Bản Windows và Hệ Điều Hành Hỗ Trợ

WF_WhitePuppy được phát triển để chạy trên các phiên bản hệ điều hành Windows phổ biến và hỗ trợ nhiều cấu hình khác nhau. Dưới đây là các hệ điều hành mà phần mềm tương thích.

### Phiên Bản Windows Hỗ Trợ:

- **Windows 10** (Tất cả các phiên bản, bao gồm Home, Pro, Enterprise)
- **Windows 11** (Tất cả các phiên bản)
- **Windows Server 2016** và **Windows Server 2019**
- **Windows Server 2022**

### Các Phiên Bản Windows Không Hỗ Trợ:

- **Windows 8** và các phiên bản cũ hơn (Windows 7, Windows Vista, Windows XP): Các phiên bản này có thể gặp khó khăn khi tương thích với các thư viện mới và các tính năng của phần mềm. Bạn có thể cần phải thực hiện thêm các bước cấu hình hoặc nâng cấp lên phiên bản mới.

### Khuyến Nghị:

- Để có trải nghiệm ổn định và hiệu quả, chúng tôi khuyến khích bạn sử dụng **Windows 10** trở lên. Phiên bản Windows này hỗ trợ đầy đủ các công nghệ mạng  và các thư viện phần mềm.
- Các hệ điều hành **Windows Server** cũng hỗ trợ WF_WhitePuppy, nhưng hãy đảm bảo các dịch vụ và cấu hình mạng được thiết lập chính xác.

### Hệ Điều Hành Khác:

- **Linux (Ubuntu, Debian)**: Phần mềm này hiện chỉ hỗ trợ trên hệ điều hành Windows và không tương thích trực tiếp với các hệ điều hành như Linux hoặc macOS. Tuy nhiên, người dùng có thể sử dụng máy ảo hoặc phần mềm giả lập Windows trên các hệ điều hành này để chạy phần mềm.

### Yêu Cầu Hệ Thống:

Đảm bảo hệ thống của bạn đáp ứng các yêu cầu tối thiểu về phần cứng và phần mềm để WF_WhitePuppy hoạt động hiệu quả.
- **Bộ xử lý**: Intel Core i3 hoặc AMD tương đương trở lên.
- **RAM**: 4 GB trở lên.
- **Ổ cứng**: 500 MB dung lượng trống để cài đặt và lưu trữ dữ liệu gói tin.
- **.NET Framework**: Cần cài đặt **.NET Framework 4.5** hoặc phiên bản cao hơn.

## Liên Hệ và Góp Ý

Nếu bạn gặp bất kỳ vấn đề nào trong quá trình sử dụng phần mềm hoặc có bất kỳ câu hỏi nào, vui lòng liên hệ với chúng tôi qua:

- **Email**: nhmh2612@gmail.com


