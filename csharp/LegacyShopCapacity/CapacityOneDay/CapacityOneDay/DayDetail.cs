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



namespace CapacityOneDay
{
    public partial class DayDetail : Form
    {
        string server = "SAR-SQLPROD01";
        string database = "EpicorDB";
        string company = "CRD";
        string exepath = @"\\sarv-appepcrp03\Epicor_Apps\MESView\MESView.exe";

        private DataTable dt;
        public DayDetail(string strDate, string strPrior)
        {
            InitializeComponent();
            if (strPrior == "Yes")
            {
                this.Text = "Day Detail for: " + strDate + " and Prior";
            }
            else
            {
                this.Text = "Day Detail for: " + strDate;
            }

            SqlConnection conn;
            SqlCommand command;
            //SqlDataReader reader;
            conn = new SqlConnection("Data Source=" + server + ";Database=" + database + ";Integrated Security=SSPI");
            command = new SqlCommand("[dbo].sp_JobAsmbl_FrameandDoor_Counts_OneDay_Detail", conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 15).Value = company;
            command.Parameters.Add("@LookupDate", SqlDbType.VarChar, 15).Value = strDate;
            command.Parameters.Add("@Prior", SqlDbType.VarChar, 15).Value = strPrior;
            conn.Open();
            //reader = command.ExecuteReader();
            //listView1.Items.Clear();
            //while (reader.Read())
            //{
            //    string[] strQtys = new string[6];
            //    strQtys[0] = reader["jobnum"].ToString();
            //    strQtys[1] = System.Convert.ToDecimal((reader["doorqty"].ToString())).ToString("N0");
            //    strQtys[2] = System.Convert.ToDecimal((reader["frameqty"].ToString())).ToString("N0");
            //    strQtys[4] = System.Convert.ToDecimal((reader["frmdoorqty"].ToString())).ToString("N0");
            //    strQtys[3] = reader["partnum2"].ToString();
            //    strQtys[5] = reader["partnum"].ToString();
            //    ListViewItem itm = new ListViewItem(strQtys);
            //    listView1.Items.Add(itm);
            //}
            //reader.Dispose();
            dt = new DataTable();
            dt.TableName = "DayDetail";
            SqlDataAdapter da = new SqlDataAdapter(command);
            dt.Clear();
            da.Fill(dt);


            //System.Type myDataType = System.Type.GetType("System.Uri");
            System.Type myDataType = typeof(Uri);
            DataColumn colUri = new DataColumn("PicUri",myDataType);
            dt.Columns.Add(colUri);
            myDataType = typeof(string);
            DataColumn colCalc_Date = new DataColumn("Calc_Date", myDataType);
            dt.Columns.Add(colCalc_Date);

            foreach (DataRow row in dt.Rows)
            {
                if (row["Picture URL"].ToString().Trim().Length > 0)
                {
                    row["PicUri"] = new Uri(row["Picture URL"].ToString().Replace("doorsjpg.php","doors.php"));
                }
                else
                {
                    row["PicUri"] = new Uri("about:blank");
                }
                row["Description"] = row["Description"].ToString().Replace("\r\r[*]", Environment.NewLine + Environment.NewLine);
                row["Special Instr"] = row["Special Instr"].ToString().Replace("\r\r[*]", Environment.NewLine + Environment.NewLine);
                DateTime date = (DateTime)row["CRD Date"];
                row["Calc_Date"] = date.ToString("d");
            }


            da.Dispose();
            command.Dispose();
            conn.Close();
            //listView1.Items.Clear();
            dataGridViewJobs.DataSource = dt.DefaultView;
            dataGridViewJobs.Columns["Doors"].Width = 60;
            dataGridViewJobs.Columns["Frames"].Width = 60;
            dataGridViewJobs.Columns["Assembly"].Width = 60;
            dataGridViewJobs.Columns["Frame-Door Openings"].Width = 70;
            txtJobNum.DataBindings.Add("Text", dt.DefaultView, "Job Number");
            txtAsmbl.DataBindings.Add("Text", dt.DefaultView, "Assembly");
            txtDescription.DataBindings.Add("Text", dt.DefaultView, "Description");
            txtConfigPart.DataBindings.Add("Text", dt.DefaultView, "Config Part");
            txtPart.DataBindings.Add("Text", dt.DefaultView, "Part");
            webBrowser1.DataBindings.Add("Url", dt.DefaultView, "PicUri");
            txtDoors.DataBindings.Add("Text", dt.DefaultView, "Doors");
            txtFrames.DataBindings.Add("Text", dt.DefaultView, "Frames");
            txtCRDDate.DataBindings.Add("Text", dt.DefaultView, "Calc_Date");
            txtSpecialInstr.DataBindings.Add("Text", dt.DefaultView, "Special Instr");
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //dt.WriteXml("c:\\temp\\report.xml");
            //CrystalDecisions.CrystalReports.Engine.ReportDocument rd = new ReportDocument();
            //ReportDocument cryRpt = new ReportDocument();
            //cryRpt.Load("I:\\Capacity\\ShopCap.rpt");

            //foreach (Table t in cryRpt.Database.Tables)
            //{
            //    t.SetDataSource(dt);
            //}

            try
            {
                PrintDialog dlg = new PrintDialog();
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    ReportDocument cryRpt = new ReportDocument();
                    cryRpt.Load(@"\\sarv-appepcrp03\Epicor_Apps\" + company + @"\Capacity\ShopCap.rpt");

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

            
            //cryRpt.PrintToPrinter(1, false,0,0);
            //cryRpt.Close();
            //CrystalReportViewer crystalReportViewer1 = new CrystalReportViewer();
            //crystalReportViewer1.ReportSource = cryRpt;
            //crystalReportViewer1.Show();
            //crystalReportViewer1.Dispose();
            //crystalReportViewer1.ReportSource = cryRpt;
            //crystalReportViewer1.Refresh();

        }

        private void btnMES_Click(object sender, EventArgs e)
        {
            try
            {
                string strJobNum = txtJobNum.Text.ToString();
                string strParams = "CRD " + strJobNum + " \"Job Number: " + strJobNum + "\"";
                
                System.Diagnostics.Process.Start(exepath,strParams);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: "+ex.ToString());
            }
        }

        private void txtSpecialInstr_TextChanged(object sender, EventArgs e)
        {
            if (this.txtSpecialInstr.Text.ToString().Trim().Length != 0)
            {
                this.txtSpecialInstr.BackColor = Color.Yellow;
            }
            else
            {
                this.txtSpecialInstr.BackColor = Color.FromKnownColor(KnownColor.Control) ;
            }
        }






    }
}
