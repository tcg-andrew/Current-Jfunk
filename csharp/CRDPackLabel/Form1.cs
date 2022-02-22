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
using Microsoft.Win32;
using SQLAccess;

namespace CRDPackLabel
{
    public partial class Form1 : Form
    {
        private string company = "CRD";

        string server = "SARV-SQLPROD01";
        string database = "EpicorDB";
        string username = "RailsAppUserP";
        string password = "wA7tA1FaBS1MpLaU";

        private DataSet result;

        public Form1()
        {
            InitializeComponent();
            result = null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txt_jobnum.Text.Length > 0)
            {
                DataSet result = new DataSet();
                SqlCommand sqlCommand = new SqlCommand("exec [dbo].sp_FuzzyJobLookup @Company, @Jobnum");
                sqlCommand.Parameters.AddWithValue("Company", "CRD");
                sqlCommand.Parameters.AddWithValue("Jobnum", txt_jobnum.Text);

                try
                {
                    result = SQLAccess.SQLAccess.GetDataSet(server, database, username, password, sqlCommand);
                    dgv_jobs.DataSource = result.Tables[0];
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (txt_partnum.Text.Length > 0)
            {
                DataSet result = new DataSet();
                SqlCommand sqlCommand = new SqlCommand("exec [dbo].sp_FuzzyPartLookup @Company, @Partnum");
                sqlCommand.Parameters.AddWithValue("Company", "CRD");
                sqlCommand.Parameters.AddWithValue("Partnum", txt_partnum.Text);

                try
                {
                    result = SQLAccess.SQLAccess.GetDataSet(server, database, username, password, sqlCommand);
                    dgv_parts.DataSource = result.Tables[0];
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }

        private void tabControl1_TabIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                if (dgv_jobs.SelectedRows.Count > 0 || dgv_parts.SelectedRows.Count > 0)
                {
                    string jobs = "";
                    foreach (DataGridViewRow row in dgv_jobs.SelectedRows)
                    {
                        jobs += (jobs.Length > 0 ? "," : "") + "'" + row.Cells[0].Value + "'";
                    }
                    string parts = "";
                    foreach (DataGridViewRow row in dgv_parts.SelectedRows)
                    {
                        parts += (parts.Length > 0 ? "," : "") + "'" + row.Cells[0].Value + "'";
                    }

                    
                    SqlCommand sqlCommand = new SqlCommand("exec [dbo].sp_GetCRDPackLabels @Company, @Jobnum, @Partnum");
                    sqlCommand.Parameters.AddWithValue("Company", "CRD");
                    if (!String.IsNullOrEmpty(jobs))
                        sqlCommand.Parameters.AddWithValue("Jobnum", jobs);
                    else
                        sqlCommand.Parameters.AddWithValue("Jobnum", DBNull.Value);

                    if (!String.IsNullOrEmpty(parts))
                        sqlCommand.Parameters.AddWithValue("Partnum", parts);
                    else
                        sqlCommand.Parameters.AddWithValue("Partnum", DBNull.Value);

                    try
                    {
                        result = SQLAccess.SQLAccess.GetDataSet(server, database, username, password, sqlCommand);
                        dgv_Preview.DataSource = result.Tables[0];
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

                PrintDialog pd = new PrintDialog();
                if (pd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    #region Default printer settings

                    string key_root = "HKEY_CURRENT_USER\\Software\\Microsoft\\Internet Explorer\\PageSetup";
                    string bottom = "";
                    string top = "";
                    string left = "";
                    string right = "";
                    string header = "";
                    string footer = "";

                    if (Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Internet Explorer\\PageSetup") != null)
                    {
                        bottom = Registry.GetValue(key_root, "margin_bottom", "").ToString();
                        top = Registry.GetValue(key_root, "margin_top", "").ToString();
                        left = Registry.GetValue(key_root, "margin_left", "").ToString();
                        right = Registry.GetValue(key_root, "margin_right", "").ToString();
                        header = Registry.GetValue(key_root, "header", "").ToString();
                        footer = Registry.GetValue(key_root, "footer", "").ToString();
                        Registry.SetValue(key_root, "margin_bottom", 0);
                        Registry.SetValue(key_root, "margin_top", 0);
                        Registry.SetValue(key_root, "margin_right", 0);
                        Registry.SetValue(key_root, "margin_left", 0);
                        Registry.SetValue(key_root, "header", "");
                        Registry.SetValue(key_root, "footer", "");
                    }

                    #endregion

                    for (int x = 0; x < pd.PrinterSettings.Copies; x++)
                    {

                        foreach (DataRow row in result.Tables[0].Rows)
                        {
                            DataTable toprint = result.Tables[0].Clone();
                            toprint.Clear();
                            for (int i = 0; i < (int)Decimal.Parse(row["qtyper"].ToString()); i++)
                            {
                                toprint.Rows.Add(row.ItemArray);
                            }
                            LocalReport report = new LocalReport();
                            report.ReportPath = @"Report1.rdlc";
                            report.DataSources.Add(new ReportDataSource("DataSet1", toprint));
                            report.SubreportProcessing += new SubreportProcessingEventHandler(report_SubreportProcessing);

                            ReportPrintDocument rpd = new ReportPrintDocument(report);
                            rpd.PrinterSettings = pd.PrinterSettings;

                            rpd.Print();
                        }
                    }

                    #region Restore printer settings

                    if (Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Internet Explorer\\PageSetup") != null)
                    {
                        Registry.SetValue(key_root, "margin_bottom", bottom);
                        Registry.SetValue(key_root, "margin_top", top);
                        Registry.SetValue(key_root, "margin_right", right);
                        Registry.SetValue(key_root, "margin_left", left);
                        Registry.SetValue(key_root, "header", "");
                        Registry.SetValue(key_root, "footer", "");
                    }

                    #endregion
                    dgv_Preview.DataSource = null;
                    dgv_jobs.DataSource = null;
                    dgv_parts.DataSource = null;
                    txt_jobnum.Text = "";
                    txt_partnum.Text = "";
                }
        }

        void report_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            SqlCommand sqlCommand = new SqlCommand("exec [dbo].sp_UD03_ShopViewAllMES @Company, @VCLink");
            sqlCommand.Parameters.AddWithValue("Company", "CRD");
            sqlCommand.Parameters.AddWithValue("VCLink", e.Parameters["Jobnum"].Values[0]);
            e.DataSources.Add(new ReportDataSource("DataSet1", SQLAccess.SQLAccess.GetDataSet(server, database, username, password, sqlCommand).Tables[0]));


        }
    }
}
