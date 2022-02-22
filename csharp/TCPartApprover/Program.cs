using ObjectLibrary;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPartApprover
{
    class Program
    {
        static void Main(string[] args)
        {
            string E10username = "CRDService";
            string E10password = "gfd723trajsdc97";

            string TCusername = "tc_erp";
            string TCpassword = "MtemL7gnJRXVG4t5";

            //SqlCommand command = new SqlCommand("SELECT distinct p.partnum from teamcenter.partinfo as p inner join teamcenter.publishevent as pe on p.publishid = pe.publishid where pe.eventid > 15353 and pe.status = 2 order by p.partnum");
            //DataSet ds = SQLAccess.SQLAccess.GetDataSet("STLV-SQLPROD16", "TC_Epicor_DBP", TCusername, TCpassword, command);

            using (ExcelPackage combPackage = new ExcelPackage(new System.IO.FileInfo("C:\\Material fix.xlsx")))
            {
                ExcelWorksheet ws = combPackage.Workbook.Worksheets["Sheet1"];

                int row_i = 2;
                string processed = DateTime.Now.ToString();
                PartInterface pi = new PartInterface();
                EngWorkbenchInterface ei = new EngWorkbenchInterface();
                while (ws.Cells[row_i, 1].Value != null)
                {
                    try
                    {
                        string part = ws.Cells[row_i, 2].Value.ToString();
                        try
                        {
                            ei.ReviseMaterial("SARV-APPEPCRP01", "EpicorDB", E10username, E10password, part, ws.Cells[row_i, 3].Value.ToString(), ws.Cells[row_i, 4].Value.ToString(), ws.Cells[row_i, 5].Value.ToString(), true);
                            //pi.UnapproveRevision("SARV-APPEPCRP01", "EpicorDB", E10username, E10password, part, ws.Cells[row_i, 2].Value.ToString());
                            ws.Cells[row_i, 9].Value = processed;
                        }
                        catch (Exception ex)
                        {
                            ws.Cells[row_i, 8].Value = ex.Message;
                        }
                    }
                    catch (Exception ex)
                    {
                        ws.Cells[row_i, 9].Value = ex.Message;
                    }
                    row_i += 1;
                }
                combPackage.Save();
            }
        }
    }
}
