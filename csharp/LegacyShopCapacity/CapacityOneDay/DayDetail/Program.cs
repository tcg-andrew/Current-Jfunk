using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DayDetail
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string strDate)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new DayDetail(strDate));
        }
    }
}
