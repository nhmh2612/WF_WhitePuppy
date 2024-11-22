using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Windows.Forms;

namespace WF_Whiepuppy
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Kiểm tra quyền Administrator
            if (!IsAdministrator())
            {
                // Yêu cầu chạy lại ứng dụng với quyền Admin
                RequestAdministratorPrivileges();
                return;
            }

            // Cấu hình giao diện người dùng
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Fmain());
        }

        // Kiểm tra quyền Administrator
        static bool IsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        // Yêu cầu quyền Administrator
        static void RequestAdministratorPrivileges()
        {
            ProcessStartInfo procStartInfo = new ProcessStartInfo()
            {
                FileName = Application.ExecutablePath,
                Verb = "runas", // Chạy lại ứng dụng với quyền Admin
                UseShellExecute = true
            };

            try
            {
                Process.Start(procStartInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể yêu cầu quyền Administrator: {ex.Message}");
            }
        }
    }
}
