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
using System.Configuration;

namespace CRDProdLabel
{
    public partial class Form1 : Form
    {
        string server = "SARV-SQLPROD01";
        string database = "EpicorDB";
        string username = "RailsAppUserP";
        string password = "wA7tA1FaBS1MpLaU";

        public Form1()
        {
            InitializeComponent();
        }

        #region Event Handlers

        private void date_To_ValueChanged(object sender, EventArgs e)
        {
            if (date_To.Value < date_From.Value)
                date_To.Value = date_From.Value;
        }

        private void date_From_ValueChanged(object sender, EventArgs e)
        {
            if (date_From.Value > date_To.Value)
                date_From.Value = date_To.Value;
        }

        #endregion

        private void txt_SO_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '-')
                e.Handled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string checkpoint = "0";
            try
            {
                SqlConnection connection = new SqlConnection("data source=" + server + ";initial catalog=" + database + ";user id=" + username + ";password=" + password);
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "exec [dbo].sp_GetProductionLabelList @Company, @StartDate, @EndDate, @Job, @Partnum, @Asm";
                command.Parameters.AddWithValue("Company", "CRD");
                if (cbox_Date.Checked)
                {
                    command.Parameters.AddWithValue("StartDate", date_From.Value.ToShortDateString());
                    command.Parameters.AddWithValue("EndDate", date_To.Value.ToShortDateString());
                }
                else
                {
                    command.Parameters.AddWithValue("StartDate", DBNull.Value);
                    command.Parameters.AddWithValue("EndDate", DBNull.Value);
                }
                if (cbox_SO.Checked)
                    command.Parameters.AddWithValue("Job", txt_SO.Text);
                else
                    command.Parameters.AddWithValue("Job", DBNull.Value);

                if (cbox_Part.Checked)
                    command.Parameters.AddWithValue("Partnum", txt_Part.Text);
                else
                    command.Parameters.AddWithValue("Partnum", DBNull.Value);

                if (chk_Asm.Checked)
                    command.Parameters.AddWithValue("Asm", txt_Asm.Text);
                else
                    command.Parameters.AddWithValue("Asm", DBNull.Value);

                SqlDataAdapter sda = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                checkpoint = "1";
                sda.Fill(ds);
                checkpoint = "2";

                LabelSelectForm f = new LabelSelectForm();
                f.Data = ds.Tables[0];
                if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    PrintDialog pd = new PrintDialog();
                    if (pd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {

                        #region Set Printer Defaults

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
                            foreach (DataGridViewRow row in f.Selected)
                            {
                                if (Int32.Parse(row.Cells[0].Value.ToString()) > 0)
                                {
                                    command = connection.CreateCommand();
                                    command.CommandText = "exec [dbo].sp_GetProductionLabelData @Company, @Jobnum, @Partnum, @NumLabels";
                                    command.Parameters.AddWithValue("Company", "CRD");
                                    command.Parameters.AddWithValue("Jobnum", row.Cells[1].Value.ToString());
                                    command.Parameters.AddWithValue("Partnum", row.Cells[2].Value.ToString());
                                    command.Parameters.AddWithValue("NumLabels", Int32.Parse(row.Cells[0].Value.ToString()));
                                    sda = new SqlDataAdapter(command);
                                    ds = new DataSet();
                                    sda.Fill(ds);

                                    LocalReport report = new LocalReport();
                                    report.ReportPath = @"Report1.rdlc";
                                    report.DataSources.Add(new ReportDataSource("DataSet1", ds.Tables[0]));
                                    ReportPrintDocument rpd = new ReportPrintDocument(report);
                                    rpd.PrinterSettings = pd.PrinterSettings;

                                    rpd.Print();

                                }
                            }
                        }

                        #region Restore Printer Settings

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
                    }
                }
/*                if (ds.Tables[0].Rows.Count == 0)
                    MessageBox.Show("No results");
                else
                {
                    LabelSelectForm f = new LabelSelectForm();
                    f.Data = ds.Tables[0];
                    if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        checkpoint = "3";
                        LocalReport report = new LocalReport();
                        report.ReportPath = @"Report1.rdlc";
                        report.DataSources.Add(new ReportDataSource("DataSet1", f.Selected));
                        ReportPrintDocument rpd = new ReportPrintDocument(report);

                        PrintPreviewDialog ppd = new PrintPreviewDialog();
                        ppd.Document = rpd;



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
                        ppd.ShowDialog();
                        checkpoint = "4";
                        if (Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Internet Explorer\\PageSetup") != null)
                        {
                            Registry.SetValue(key_root, "margin_bottom", bottom);
                            Registry.SetValue(key_root, "margin_top", top);
                            Registry.SetValue(key_root, "margin_right", right);
                            Registry.SetValue(key_root, "margin_left", left);
                            Registry.SetValue(key_root, "header", "");
                            Registry.SetValue(key_root, "footer", "");
                        }
                        cbox_Date.Checked = false;
                        date_From.Value = DateTime.Today;
                        date_To.Value = DateTime.Today;
                        cbox_SO.Checked = false;
                        txt_SO.Text = "";
                        cbox_Part.Checked = false;
                        txt_Part.Text = "";
                        chk_Asm.Checked = false;
                        txt_Asm.Text = "";
                        num_Labels.Value = 1;
                    }
                }*/
            }
            catch (Exception ex)
            {
                MessageBox.Show(checkpoint + " " + ex.Message + "-" + ex.StackTrace);
            }

        }
    }
}
