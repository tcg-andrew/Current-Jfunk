using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TCGEpicor
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
            Form1 form = new Form1("SARV-APPEPCRP01", "SARV-SQLPROD01", "EpicorDB", "SQL", "MfgSys803");
            //Form1 form = new Form1("SARV-APPEPCRD01", "SARV-SQLDEV01", "EpicorDB_PG", "SQL", "MfgSys803");
            // CRD - SQL
            // NEM - Epicor
            // CRD DEV - sarv-appsql01
            Application.Run(form);
        }
    }
}
