using ObjectLibrary;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesRepUpdate
{
    class Program
    {
        static void Main(string[] args)
        {
            string server = "SARV-APPEPCRP01";
            string database = "EpicorDB";
            string E10username = "CRDService";
            string E10password = "gfd723trajsdc97";

            using (ExcelPackage combPackage = new ExcelPackage(new System.IO.FileInfo("C:\\\\OpTimesPartsUpdate.xlsx")))
            {
                ExcelWorksheet ws = combPackage.Workbook.Worksheets["CRD"];

                int row_i = 2;
                string processed = DateTime.Now.ToString();
                CustomerInterface ci = new CustomerInterface();
                while (ws.Cells[row_i, 1].Value != null)
                {
                    try
                    {
                        string custid = ws.Cells[row_i, 2].Value.ToString();
                        string newsalesrepcode = ws.Cells[row_i, 5].Value.ToString();
                        try
                        {
                            ci.UpdateSalesRepCode(server, database, E10username, E10password, custid, newsalesrepcode);
                            ws.Cells[row_i, 8].Value = processed;
                        }
                        catch (Exception ex)
                        {
                            ws.Cells[row_i, 7].Value = ex.Message;
                        }
                    }
                    catch (Exception ex)
                    {
                        ws.Cells[row_i, 7].Value = ex.Message;

                    }
                    row_i += 1;
                }
                combPackage.SaveAs(new System.IO.FileInfo(".\\Processed\\Sales Rep Update - " + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx"));

            }

        }
    }
}
