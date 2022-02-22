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

namespace CIGSliderLabels
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
            if (String.IsNullOrEmpty(txtPart.Text))
                MessageBox.Show("Missing part #", "Warning");
            else
            {
                SqlConnection connection = new SqlConnection("data source=" + server + ";initial catalog=" + database + ";user id=" + username + ";password=" + password);
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "exec [dbo].sp_GetPartsExactMatch @Company, @Partnum";
                command.Parameters.AddWithValue("Company", "CIG");
                command.Parameters.AddWithValue("Partnum", txtPart.Text);

                SqlDataAdapter sda = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                sda.Fill(ds);

                if (ds.Tables[0].Rows.Count == 0)
                    MessageBox.Show("Invalid part #", "Error");
                else
                {

                    PrintDialog pd = new PrintDialog();
                    if (pd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
/*                        for (int x = 0; x < pd.PrinterSettings.Copies; x++)
                        {*/
                            LocalReport report = new LocalReport();
                            report.ReportPath = @"LabelReport.rdlc";
                            report.SetParameters(new ReportParameter[] { new ReportParameter("PartNum", txtPart.Text), new ReportParameter("Pieces", txtPieces.Text), new ReportParameter("Details", txtDetails.Text), new ReportParameter("Date", DateTime.Now.ToShortDateString()) });

                            ReportPrintDocument rpd = new ReportPrintDocument(report);
                            rpd.PrinterSettings = pd.PrinterSettings;
                            rpd.Print();
/*                        }*/
                    }
                }
            }
            txtPart.Focus();
        }
    }
}
