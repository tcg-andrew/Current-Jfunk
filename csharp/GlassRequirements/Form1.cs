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

namespace GlassRequirements
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dtpEndDate.Value = dtpStartDate.Value.AddDays(7);
        }

        private void btnRunReport_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection conn;
                SqlCommand command;
                //SqlDataReader reader;
                conn = new SqlConnection("Data Source=sql;Database=MfgSys803;Integrated Security=SSPI");

                command = new SqlCommand("[dbo].sp_GetGlassDemand", conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Company", SqlDbType.VarChar, 15).Value = "CRD";
                command.Parameters.Add("@StartDate", SqlDbType.VarChar, 15).Value = dtpStartDate.Value.ToShortDateString();
                command.Parameters.Add("@EndDate", SqlDbType.VarChar, 15).Value = dtpEndDate.Value.ToShortDateString();
                conn.Open();
                DataTable dt = new DataTable();
                dt.TableName = "GlassDemand";
                SqlDataAdapter da = new SqlDataAdapter(command);
                dt.Clear();
                da.Fill(dt);
                //dt.WriteXml("c:\\temp\\glassdemand.xml");
                PrintDialog dlg = new PrintDialog();
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    ReportDocument cryRpt = new ReportDocument();
                    cryRpt.Load("I:\\Capacity\\GlassDemand.rpt");

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
