using ObjectLibrary;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeComplianceDataLoad
{
    class Program
    {
        /*        static string server = "SARV-SQLPROD01";
                static string database = "EpicorDB";
                static string username = "RailsAppUserP";
                static string password = "wA7tA1FaBS1MpLaU";

                private static string vantage_server = "SARV-APPEPCRP01";
                private static string vantage_database = "EpicorDB";
                private static string vantage_user = "CRDService";
                //private static string vantage_user = "CIGFLService";
                private static string vantage_pass = "gfd723trajsdc97";
                */

        private static string server = "SARV-APPEPCRP01";
        private static string database = "EpicorDB";

        /*private static string server = "SARV-APPEPCRD01";
        private static string database = "EpicorDB_D";*/

        private static string crd_user = "CRDService";
        private static string cig_user = "CIGFLService";
        private static string password = "gfd723trajsdc97";

        static void Main(string[] args)
        {

            PartInterface partInterface = new PartInterface();

            string dir = ".\\Trade Data\\";
            foreach (string file in Directory.GetFiles(dir))
            {
                FileInfo f = new FileInfo(file);

                int row = 2;
                bool cont = true;

                while (cont)
                {
                    try
                    {
                        Console.WriteLine("Updating row " + row.ToString() + " of " + f.Name);
                        ExcelPackage pck = new ExcelPackage(f);
                        var ws = pck.Workbook.Worksheets[1];

                        if (ws.Cells[row, 1].Value == null || String.IsNullOrEmpty(ws.Cells[row, 1].Value.ToString()))
                            cont = false;
                        else
                        {
                            if (ws.Cells[row, 22].Value == null || String.IsNullOrEmpty(ws.Cells[row, 22].Value.ToString()))
                            {
                                try
                                {
                                    string company = ws.Cells[row, 1].Value.ToString();
                                    string part = ws.Cells[row, 2].Value.ToString();

                                    string username = (company == "CRD" ? crd_user : cig_user);

                                    if (partInterface.UpdateTradeData(server, database, username, password, part, ws.Cells[row, 5].Value.ToString().Replace("NULL", "")
                                        , ws.Cells[row, 6].Value.ToString().Replace("NULL", ""), ws.Cells[row, 7].Value.ToString().Replace("NULL", "")
                                        , ws.Cells[row, 8].Value.ToString().Replace("NULL", ""), ws.Cells[row, 9].Value.ToString().Replace("NULL", "")
                                        , ws.Cells[row, 10].Value.ToString().Replace("NULL", ""), ws.Cells[row, 11].Value.ToString().Replace("NULL", "")
                                        , ws.Cells[row, 12].Value.ToString().Replace("NULL", ""), ws.Cells[row, 13].Value.ToString().Replace("NULL", "")
                                        , ws.Cells[row, 14].Value.ToString().Replace("NULL", ""), ws.Cells[row, 15].Value.ToString().Replace("NULL", "")
                                        , ws.Cells[row, 16].Value.ToString().Replace("NULL", ""), ws.Cells[row, 17].Value.ToString().Replace("NULL", "")
                                        , ws.Cells[row, 18].Value.ToString().Replace("NULL", ""), ws.Cells[row, 19].Value.ToString().Replace("NULL", "")))
                                    {
                                        ws.Cells[row, 22].Value = "true";
                                    }
                                    else
                                    {
                                        ws.Cells[row, 22].Value = "false";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ws.Cells[row, 22].Value = "false";
                                    ws.Cells[row, 23].Value = ex.Message;
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
}
