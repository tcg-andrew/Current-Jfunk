using Microsoft.Reporting.WinForms;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CIGPackLabel
{
    public partial class Form1 : Form
    {
        private string company = "CIG";

        
        /*
        string server = "SARV-SQLPROD01";
        string database = "EpicorDB";
        string username = "RailsAppUserP";
        string password = "wA7tA1FaBS1MpLaU";
        */
        

            
        string server = "SARV-SQLDEV01";
        string database = "EpicorDB_D";
        string username = "RailsAppUser";
        string password = "2fe8wJcH";
        

        private DataSet result;

        public Form1()
        {
            InitializeComponent();
            result = null;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (txt_OrderNum.Text.Length > 0)
            {
                SqlCommand sqlCommand = new SqlCommand("exec [dbo].sp_GetCIGPackLabels @Company, @Ordernum");
                sqlCommand.Parameters.AddWithValue("Company", "CIG");
                sqlCommand.Parameters.AddWithValue("Ordernum", txt_OrderNum.Text);

                try
                {
                    result = SQLAccess.SQLAccess.GetDataSet(server, database, username, password, sqlCommand);
                    dgv_jobs.DataSource = result.Tables[0];
                    button1.Enabled = true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
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

                        LocalReport report = new LocalReport();
                        report.ReportPath = @"Report1.rdlc";
                        report.DataSources.Add(new ReportDataSource("DataSet1", dgv_jobs.DataSource));

                        ReportPrintDocument rpd = new ReportPrintDocument(report);
                        rpd.PrinterSettings = pd.PrinterSettings;

                        rpd.Print();
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
                dgv_jobs.DataSource = null;
                txt_OrderNum.Text = "";
                button1.Enabled = false;
            }

        }
    }
}
