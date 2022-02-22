using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace ULLabelPrinter
{
    public partial class Form1 : Form
    {
        PrintDialog pd;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int x = 0; x < pd.PrinterSettings.Copies; x++)
            {
                LocalReport report = new LocalReport();
                report.ReportPath = @"ULLabel.rdlc";
                report.SetParameters(new ReportParameter[] { new ReportParameter("Description", tb_Part.Text), new ReportParameter("Job", tb_Job.Text), new ReportParameter("HeaterAmps", tb_Heater.Text), new ReportParameter("LightAmps", tb_Light.Text) });

                ReportPrintDocument rpd = new ReportPrintDocument(report);
                rpd.PrinterSettings = pd.PrinterSettings;
                rpd.Print();
            }

            tb_Light.Text = "";
            tb_Heater.Text = "";
            tb_Job.Text = "";
            tb_Part.Text = "";
            tb_Part.Focus();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            pd = new PrintDialog();
            if (pd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                button1.Enabled = true;
        }
    }
}
