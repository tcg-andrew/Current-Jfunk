namespace Styleline.WinAnalyzer.WinClient
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;

    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            try
            {
                bool createdNew = true;
                using (new Mutex(true, "WindowsAnalyzer", out createdNew))
                {
                    if (createdNew)
                    {
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        Application.Run(new MainForm());
                    }
                    else
                    {
                        Process currentProcess = Process.GetCurrentProcess();
                        foreach (Process process2 in Process.GetProcessesByName(currentProcess.ProcessName))
                        {
                            if (process2.Id != currentProcess.Id)
                            {
                                SetForegroundWindow(process2.MainWindowHandle);
                                return;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " - " + ex.StackTrace);
            }
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
    }
}

