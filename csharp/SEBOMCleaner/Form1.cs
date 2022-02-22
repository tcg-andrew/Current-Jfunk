using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SEBOMCleaner
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowNewFolderButton = false;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                DirectoryInfo di = new DirectoryInfo(folderBrowserDialog1.SelectedPath);
                using (ExcelPackage combPackage = new ExcelPackage())
                {
                    ExcelWorksheet combworksheet = combPackage.Workbook.Worksheets.Add("Sheet 1");
                    combworksheet.Cells[1, 1].Value = "Parent";
                    combworksheet.Cells[1, 2].Value = "Level";
                    combworksheet.Cells[1, 3].Value = "Document Number";
                    combworksheet.Cells[1, 4].Value = "Revision";
                    combworksheet.Cells[1, 5].Value = "Title";
                    combworksheet.Cells[1, 6].Value = "Quantity";

                    int combrow = 2;
                    Dictionary<int, string> parents = new Dictionary<int, string>();

                    foreach (FileInfo fi in di.GetFiles("*.txt"))
                    {
                        ExcelTextFormat format = new ExcelTextFormat();
                        format.Delimiter = '\t';
                        format.Culture = new CultureInfo(Thread.CurrentThread.CurrentCulture.ToString());
                        format.Culture.DateTimeFormat.ShortDatePattern = "dd-mm-yyyy";
                        format.Encoding = new UTF8Encoding();

                        //create a new Excel package
                        using (ExcelPackage excelPackage = new ExcelPackage())
                        {
                            parents[1] = fi.Name.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[0];

                            //create a WorkSheet
                            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet 1");

                            //load the CSV data into cell A1
                            worksheet.Cells["A1"].LoadFromText(fi, format);

                            bool parse = true;
                            int row = 3;
                            while (parse)
                            {
                                try
                                {
                                    if (String.IsNullOrEmpty(worksheet.Cells[row, 1].Value.ToString().Trim()))
                                    {
                                        if (String.IsNullOrEmpty(worksheet.Cells[row, 4].Value.ToString()))
                                        {
                                            parse = false;
                                        }
                                        else
                                        {
                                            worksheet.Cells[row - 1, 4].Value = worksheet.Cells[row - 1, 4].Value.ToString().Trim() + " " + worksheet.Cells[row, 4].Value.ToString().Trim();
                                            worksheet.DeleteRow(row);
                                        }

                                    }
                                    else
                                    {
                                        int level = Int32.Parse(worksheet.Cells[row, 1].Value.ToString());
                                        parents[level + 1] = worksheet.Cells[row, 2].Value.ToString();
                                        combworksheet.Cells[combrow, 1].Value = parents[level];
                                        combworksheet.Cells[combrow, 2].Value = worksheet.Cells[row, 1].Value;
                                        combworksheet.Cells[combrow, 3].Value = worksheet.Cells[row, 2].Value;
                                        combworksheet.Cells[combrow, 4].Value = worksheet.Cells[row, 3].Value;
                                        combworksheet.Cells[combrow, 5].Value = worksheet.Cells[row, 4].Value;
                                        combworksheet.Cells[combrow, 6].Value = worksheet.Cells[row, 5].Value;
                                        combrow++;
                                        row++;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    parse = false;
                                }
                            }

                            string filename = fi.FullName;

                            FileStream stream = new FileStream(fi.FullName + ".xlsx", FileMode.Create);
                            excelPackage.SaveAs(stream);
                            stream.Close();
                        }

                    }

                    FileStream combstream = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\CombinedBOMSheet" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx", FileMode.Create);
                    combPackage.SaveAs(combstream);
                    combstream.Close();
                }
            }
        }
    }
}
