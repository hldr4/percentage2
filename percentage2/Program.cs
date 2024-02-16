using System;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace percentage
{
    [SupportedOSPlatform("windows")]
    static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            TrayIcon trayIcon = new TrayIcon();

            Application.Run();
        }
    }
}
