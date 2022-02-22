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

namespace ECOLabel
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txt_jobnum.Text))
                MessageBox.Show("Missing part #", "Warning");
            else
            {

                SqlConnection connection = new SqlConnection("data source=" + server + ";initial catalog=" + database + ";user id=" + username + ";password=" + password);
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "exec [dbo].sp_GetECOLabel @Company, @Jobnum";
                command.Parameters.AddWithValue("Company", "CIG");
                command.Parameters.AddWithValue("Jobnum", txt_jobnum.Text);

                SqlDataAdapter sda = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                sda.Fill(ds);

                if (ds.Tables[0].Rows.Count == 0)
                    MessageBox.Show("Invalid Job #", "Error");
                else
                {

                    PrintDialog pd = new PrintDialog();
                    pd.PrinterSettings.PrinterName = "CTN ECO ZT230";
                    pd.AllowSomePages = true;
                    pd.PrinterSettings.MaximumPage = Int32.Parse(ds.Tables[0].Rows[0][2].ToString());
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

                            int qty = Int32.Parse(ds.Tables[0].Rows[0][2].ToString());
                            for (int i = 0; i < qty; i++)
                            {
                                if (pd.PrinterSettings.FromPage == 0 || ((i + 1) >= pd.PrinterSettings.FromPage && (i + 1) <= pd.PrinterSettings.ToPage))
                                {
                                    LocalReport report = new LocalReport();
                                    report.ReportPath = @"LabelReport.rdlc";
                                    report.SetParameters(new ReportParameter[] { new ReportParameter("PartNum", ds.Tables[0].Rows[0][0].ToString()), new ReportParameter("JobNum", txt_jobnum.Text), new ReportParameter("Description", ds.Tables[0].Rows[0][1].ToString()), new ReportParameter("Current", (i + 1).ToString()), new ReportParameter("Qty", qty.ToString()) });

                                    ReportPrintDocument rpd = new ReportPrintDocument(report);
                                    rpd.PrinterSettings = pd.PrinterSettings;
                                    rpd.Print();
                                }
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

                    }
                    txt_jobnum.Text = "";
                }
            }
            txt_jobnum.Focus();

        }
    }
}
