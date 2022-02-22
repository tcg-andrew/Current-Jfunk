using ObjectLibrary;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialQtyUpdate
{
    class Program
    {
        //private static string company = "CRD";

        /*static string server = "SARV-SQLDEV01";
        static string database = "EpicorDB_PG";
        static string username = "RailsAppUserP";
        static string password = "wA7tA1FaBS1MpLaU";*/

        private static string vantage_server = "SARV-APPEPCRP01";
        private static string vantage_database = "EpicorDB";
        private static string vantage_user = "CIGFLService";
        private static string vantage_pass = "gfd723trajsdc97";


        /*static string server = "SARV-SQLDEV01";
        
        static string database = "EpicorDB_PG";
        static string username = "RailsAppUser";
        static string password = "2fe8wJcH";
             
        private static string vantage_server = "SARV-APPEPCRD01";
        private static string vantage_database = "EpicorDB_PG";
        private static string vantage_user = "CIGFLService";
        private static string vantage_pass = "gfd723trajsdc97";*/

        static void Main(string[] args)
        {
            EngWorkbenchInterface enginterface = new EngWorkbenchInterface();

            string dir = "C: \\Users\\jfunk\\Documents\\GitHub\\csharp\\MaterialQtyUpdate\\";
            ExcelPackage pck = null;
            string filename = dir + "Sealant Quantity Update.xlsx";
            FileInfo newFile = new FileInfo(filename);

            pck = new ExcelPackage(newFile);

            var ws = pck.Workbook.Worksheets[1];

            int row = 1;
            bool cont = true;
            while (cont)
            {
                try
                {
                    Console.WriteLine("Updating row " + row.ToString() + "....");

                    string part = ws.Cells[row, 1].Value.ToString();
                    string rev = "FL";
                    string seq = ws.Cells[row, 2].Value.ToString();
                    string mtl = ws.Cells[row, 3].Value.ToString();
                    string qty = ws.Cells[row, 5].Value.ToString();

                    try
                    {
                        enginterface.ReviseMaterial(vantage_server, vantage_database, vantage_user, vantage_pass, part, rev, seq, mtl, qty, "", false);
                        ws.Cells[row, 6].Value = "true";
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error row " + row.ToString() + ": " + ex.Message);
                        ws.Cells[row, 6].Value = "false";
                    }

                    row++;
                }
                catch (Exception ex)
                {
                    cont = false;
                }
            }

            pck.Save();
        }
    }
}
