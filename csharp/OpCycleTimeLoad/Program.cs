using ObjectLibrary;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OpCycleTimeLoad
{
    class Program
    {
        static void CheckPause()
        {
            int dow = (int)DateTime.Now.DayOfWeek;
            int hour = DateTime.Now.Hour;

            if (dow >= 1 && dow <= 5 && hour >= 8 && hour <= 17)
            {
                Console.WriteLine("Sleeping");
                Thread.Sleep(900000);
                CheckPause();

            }
        }

        static void Main(string[] args)
        {
            string E10server = "SARV-APPEPCRP01";
            string E10Sqlserver = "SARV-SQLPROD01";
            string E10database = "EpicorDB";
            string E10username = "CRDService";
            string E10password = "gfd723trajsdc97";

            string username = "RailsAppUserP";
            string password = "wA7tA1FaBS1MpLaU";

            bool cont = true;
            int row_i = 2;

            while (cont)
            {
                CheckPause();
                using (ExcelPackage combPackage = new ExcelPackage(new System.IO.FileInfo("C:\\\\OpTimesPartsUpdate - Full.xlsx")))
                {
                    ExcelWorksheet ws = combPackage.Workbook.Worksheets["Sheet1"];

                    PartInterface pi = new PartInterface();
                    EngWorkbenchInterface ei = new EngWorkbenchInterface();
                    
                    if (ws.Cells[row_i, 1].Value != null)
                    {
                        if (ws.Cells[row_i, 3].Value == null && ws.Cells[row_i, 4].Value == null)
                        {
                            try
                            {
                                string part = ws.Cells[row_i, 1].Value.ToString();
                                string prodcode = ws.Cells[row_i, 2].Value.ToString();
                                string rev = pi.GetPartInfo(E10server, E10database, E10username, E10password, part).RevNum;
                                
                                if (!String.IsNullOrEmpty(rev))
                                {
                                    try
                                    {
                                        bool approved = pi.GetRevStatus(E10server, E10database, E10username, E10password, part, rev);

                                        bool changed = false;

                                        foreach (Operation opr in pi.GetPartOperations(E10Sqlserver, E10database, username, password, part, rev))
                                        {
                                            OprTimeDetail detail = OprTimesInterface.GetDetail(part);
                                            int openings = 0;
                                            string suffix = "";
                                            if (detail != null)
                                            {
                                                openings = detail.Openings;
                                                suffix = detail.Suffix;
                                            }

                                            decimal time = OprTimesInterface.GetOprTime(prodcode, openings, false, opr.OpCode + suffix);
                                            if (time > 0)
                                            {
                                                ei.OperationProductionTime(E10server, E10database, E10username, E10password, part, rev, opr.Seq.ToString(), time, "MP", "oprtimes");
                                                changed = true;
                                            }
                                        }

                                        if (changed && !approved)
                                            pi.UnapproveRevision(E10server, E10database, E10username, E10password, part, rev);
                                        if (changed)
                                            ws.Cells[row_i, 3].Value = DateTime.Now.ToString();
                                        else
                                            ws.Cells[row_i, 3].Value = "No change";
                                    }
                                    catch (Exception ex)
                                    {
                                        ws.Cells[row_i, 4].Value = ex.Message;
                                    }
                                }
                                else
                                {
                                    ws.Cells[row_i, 4].Value = "No approved rev";
                                }
                            }
                            catch (Exception ex)
                            {
                                ws.Cells[row_i, 4].Value = ex.Message;
                            }
                            combPackage.Save();
                        }
                    }
                    else
                    {
                        combPackage.SaveAs(new System.IO.FileInfo(".\\Processed\\OpTimesPartsUpdate - " + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx"));
                        cont = false;
                    }
                }
                Console.WriteLine("Processed row " + row_i.ToString());
                row_i += 1;
            }
        }
    }
}
