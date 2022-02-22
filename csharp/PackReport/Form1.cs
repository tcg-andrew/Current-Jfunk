using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.Reporting.WinForms;

namespace PackReport
{
    public partial class Form1 : Form
    {
        string server = "SARV-SQLPROD01";
        string database = "EpicorDB";

        public string Filter
        {
            get
            {
                if (rb_ShopCap.Checked)
                    return "Shop Cap";
                else
                    return "Ship Date";
            }
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_Done_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            endDate.Value = startDate.Value.AddDays(7);

        }

        private void btn_Report_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection("Data Source=" + server + ";Database=" + database + ";Integrated Security=SSPI");
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "exec [dbo].sp_JobAsmbl_FrameandDoor_Counts_Pack_Report @Company, @StartDate, @EndDate, @Filter";
            command.Parameters.AddWithValue("Company", "CRD");
            command.Parameters.AddWithValue("StartDate", startDate.Value.ToShortDateString());
            command.Parameters.AddWithValue("EndDate", endDate.Value.ToShortDateString());
            command.Parameters.AddWithValue("Filter", Filter);

            SqlDataAdapter sda = new SqlDataAdapter(command);
            DataSet ds = new DataSet();
            sda.Fill(ds);

            if (ds.Tables[0].Rows.Count == 0)
                MessageBox.Show("No Results");
            else
            {
                PrintDialog pd = new PrintDialog();
                if (pd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    for (int x = 0; x < pd.PrinterSettings.Copies; x++)
                    {
                        LocalReport report = new LocalReport();
                        report.ReportPath = @"PackReport.rdlc";
                        report.SetParameters(new ReportParameter[] { new ReportParameter("StartDate", startDate.Value.ToShortDateString()), new ReportParameter("EndDate", endDate.Value.ToShortDateString()), new ReportParameter("Filter", Filter) });

                        report.DataSources.Add(new ReportDataSource("PackReportDataSet", ds.Tables[0]));

                        ReportPrintDocument rpd = new ReportPrintDocument(report);
                        rpd.PrinterSettings = pd.PrinterSettings;
                        rpd.Print();
                    }
                }
            }
          }
    }
}
