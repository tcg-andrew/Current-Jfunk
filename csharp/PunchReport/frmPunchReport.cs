using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Diagnostics;
using System.IO;

namespace PunchReport
{
    public partial class frmPunchReport : Form
    {
        string server = "SARV-SQLDEV01";
        string database = "EpicorDB_PG";

        private DataTable dt;

        public frmPunchReport()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void frmPunchReport_Load(object sender, EventArgs e)
        {
            dtpEndDate.Value = dtpStartDate.Value.AddDays(7);
        }

        private void btnRunReport_Click(object sender, EventArgs e)
        {
            SqlConnection conn;
            SqlCommand command;
            //SqlDataReader reader;
            conn = new SqlConnection("Data Source=" + server + ";Database=" + database + ";Integrated Security=SSPI");

            string proc = "";
            if (checkBox1.Checked)
                proc = "[dbo].sp_JobAsmbl_FrameandDoor_Counts_Cut_NotShipped_Reports";
            else
                proc = "[dbo].sp_JobAsmbl_FrameandDoor_Counts_Cut_Reports";

            command = new SqlCommand(/*"[dbo].sp_JobAsmbl_FrameandDoor_Counts_Cut_Reports"*/proc, conn);
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 5000;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 15).Value = "CRD";
            command.Parameters.Add("@StartDate", SqlDbType.VarChar, 15).Value = dtpStartDate.Value.ToShortDateString();
            command.Parameters.Add("@EndDate", SqlDbType.VarChar, 15).Value = dtpEndDate.Value.ToShortDateString();
            conn.Open();
            dt = new DataTable();
            dt.TableName = "PunchReport";
            SqlDataAdapter da = new SqlDataAdapter(command);
            dt.Clear();
            da.Fill(dt);
            dt.WriteXml("c:\\temp\\punchreport.xml");
            try
            {
                PrintDialog dlg = new PrintDialog();
                dlg.UseEXDialog = true;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    ReportDocument cryRpt = new ReportDocument();
                    if (checkBox1.Checked)
//                        cryRpt.Load("I:\\Capacity\\PunchReport_NotShipped.rpt");
                        cryRpt.Load(@"\\sarv-file01\root\CRD\Capacity\PunchReport_NotShipped.rpt");
                    else
//                        cryRpt.Load("I:\\Capacity\\PunchReport.rpt");
                        cryRpt.Load(@"\\sarv-file01\root\CRD\Capacity\PunchReport.rpt");

                    foreach (Table t in cryRpt.Database.Tables)
                    {
                        t.SetDataSource(dt);
                    }

                    string ps = dlg.PrinterSettings.PrinterName.ToString();

                    cryRpt.PrintOptions.PrinterName = ps;
                    cryRpt.PrintToPrinter(1, false, 0, 0);
                    cryRpt.Close();
                }
            }
            catch (Exception ex)
            {
                // something went wrong
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
