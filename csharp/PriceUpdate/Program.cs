using ObjectLibrary;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceUpdate
{
    class Program
    {

        //private static string company = "CRD";

        static string server = "SARV-SQLPROD01";
        static string database = "EpicorDB";
        static string username = "RailsAppUserP";
        static string password = "wA7tA1FaBS1MpLaU";

        private static string vantage_server = "SARV-APPEPCRP01";
        private static string vantage_database = "EpicorDB";
        //private static string vantage_user = "CRDService";
        private static string vantage_user = "CIGFLService";
        private static string vantage_pass = "gfd723trajsdc97";


        /*static string server = "SARV-SQLDEV01";
        
        static string database = "EpicorDB_D";
        static string username = "RailsAppUser";
        static string password = "2fe8wJcH";
             
        private static string vantage_server = "SARV-APPEPCRD01";
        private static string vantage_database = "EpicorDB_D";
        private static string vantage_user = "CIGFLService";
        private static string vantage_pass = "gfd723trajsdc97";*/

        static void Main(string[] args)
        {


            PartInterface partInterface = new PartInterface();
            CustomerInterface custInterface = new CustomerInterface();

            string dir = "C: \\Users\\jfunk\\Documents\\GitHub\\csharp\\PriceUpdate\\";
            ExcelPackage pck = null;
            string filename = dir + "CIG_2021-10-18.xlsx";
            FileInfo newFile = new FileInfo(filename);

            pck = new ExcelPackage(newFile);

            var ws = pck.Workbook.Worksheets[1];

            int row = 2;
            bool cont = true;
            while (cont) 
            {
                try
                {
                    Console.WriteLine("Updating row " + row.ToString() + "....");
                    pck = new ExcelPackage(newFile);

                    ws = pck.Workbook.Worksheets[1];

                    if (ws.Cells[row, 1].Value == null)
                        cont = false;
                    else
                    {

                        if (ws.Cells[row, 7].ToString() != "true")
                        {
                            try
                            {

                                string part = ws.Cells[row, 1].Value.ToString();
                                decimal price = Decimal.Parse(ws.Cells[row, 6].Value.ToString());
                                if (partInterface.UpdateUnitPrice(vantage_server, vantage_database, vantage_user, vantage_pass, part, price))
                                {
                                    ws.Cells[row, 7].Value = "true";
                                }
                                else
                                    ws.Cells[row, 7].Value = "false";
                            }
                            catch (Exception ex)
                            {
                                ws.Cells[row, 7].Value = "false";
                                ws.Cells[row, 8].Value = ex.Message;
                            }

                            pck.Save();
                        }
                    }
                    row++;
                }
                catch (Exception ex)
                {
                    cont = false;
                }
            }

        }
    }
}
