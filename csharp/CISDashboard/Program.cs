using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CISDashboard
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (args.Length < 3)
                MessageBox.Show("Application Arguments missing for Vantage Server, Port, and Database.  Application target should be similar to ~\\TCGEpicor.exe {SERVER} {PORT} {DATABASE}", "No Connection Arguments", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                Form1 form = new Form1(args[0], args[1], args[2]);
                Application.Run(form);
            }
        }
    }
}
