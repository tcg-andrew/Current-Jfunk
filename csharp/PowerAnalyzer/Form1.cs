#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Styleline.WinAnalyzer.CommPipe;
using Styleline.WinAnalyzer.AnalyzerLib;
using Styleline.WinAnalyzer.DAL.Entities;
using PowerAnalyzer.FoxProServiceReference;
using Microsoft.Reporting.WinForms;
using System.Threading;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Data.Odbc;
using System.Configuration;

#endregion

namespace PowerAnalyzer
{
    public partial class Form1 : Form
    {
        #region Values

        private CurrentPowerReading powerReading = new CurrentPowerReading();
        private Styleline.WinAnalyzer.AnalyzerLib.PowerAnalyzer powerAnalyzer;
        public static readonly string[] COM_PORTS = new string[] { "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8" };
        public string com = "COM1";
        public string unitnum;
        public string currentWire;
        public PrintDialog pd;
        private Wait waitWindow;
        private COMPort c = new COMPort();

        #endregion

        #region Properties

        public Styleline.WinAnalyzer.AnalyzerLib.PowerAnalyzer PowerAnalyzer
        {
            get
            {
                if ((this.powerAnalyzer == null) && COM_PORTS.Contains<string>(SettingsTO.Default.ComPort))
                {
                    this.powerAnalyzer = new Styleline.WinAnalyzer.AnalyzerLib.PowerAnalyzer(SettingsTO.Default.ComPort);
                    this.powerAnalyzer.ReadingsUpdated += new ReadingsUpdateHandler(this.powerAnalyzer_ReadingsUpdated);
                    this.powerAnalyzer.RunStateChanged += new RunStateChangeHandler(this.powerAnalyzer_RunStateChanged);
                }
                return this.powerAnalyzer;
            }
            private set
            {
                if (value != null)
                {
                    throw new InvalidOperationException("Can only set object to null");
                }
                if (this.powerAnalyzer != null)
                {
                    this.powerAnalyzer.ReadingsUpdated -= new ReadingsUpdateHandler(this.powerAnalyzer_ReadingsUpdated);
                    this.powerAnalyzer.RunStateChanged -= new RunStateChangeHandler(this.powerAnalyzer_RunStateChanged);
                }
                this.powerAnalyzer = value;
            }
        }

        #endregion

        #region Constructor

        public Form1()
        {
            InitializeComponent();
            SettingsTO sto = SettingsTO.Default;
            ddl_Plant.SelectedIndex = (sto.Plant == "FL" ? 0 : 1);
            waitWindow = new Wait();
        }

        #endregion

        #region Methods

        private void ChangePowerAnalyzerStart(bool start)
        {
            ShowWait();
            try
            {
                if (start && (this.PowerAnalyzer != null))
                {
                    this.PowerAnalyzer.Start();
                }
                else if (this.powerAnalyzer != null)
                {
                    this.PowerAnalyzer.Stop();
                }
                else
                {
                    start = false;
                }
                SettingsTO sto = SettingsTO.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            HideWait();
        }

        public decimal CalculateOhms(decimal[] readings)
        {
            if (readings[3] == 0M)
            {
                return 0M;
            }
            return Math.Round((decimal)(readings[2] / readings[3]), 2);
        }

        public void CheckReadingSuccess()
        {
            decimal optvolt = Decimal.Parse(lblOptVolt.Text);
            decimal ohmshigh = Decimal.Parse(lblOhmHigh.Text);
            decimal ohmslow = Decimal.Parse(lblOhmLow.Text);

            if (optvolt != 0 && ohmshigh != 0 && ohmslow != 0)
            {
               decimal volt = Decimal.Parse(lblVoltage.Text);
                if (volt != optvolt)
                    pnlVoltage.BackColor = Color.Red;
                else
                    pnlVoltage.BackColor = Color.Green;
                decimal ohms = Decimal.Parse(lblOhms.Text);
                if (ohms < ohmslow || ohms > ohmshigh)
                    pnlOhms.BackColor = Color.Red;
                else
                    pnlOhms.BackColor = Color.Green;
            }
        }

        public void ClearDisplay()
        {
            pnlVoltage.BackColor = Color.Transparent;
            pnlOhms.BackColor = Color.Transparent;
            lblOhmHigh.Text = "0";
            lblOhmLow.Text = "0";
            lblOptVolt.Text = "0";
            textBox1.Text = "";
            textBox1.Focus();
            button2.Enabled = false;
        }

        public void ShowWait()
        {
            waitWindow = new Wait();
            waitWindow.Show();
            waitWindow.Update();
        }

        public void HideWait()
        {
            waitWindow.Close();
        }

        #endregion

        #region Event Handlers

        private void powerAnalyzer_ReadingsUpdated(decimal[] readings)
        {
            try
            {
                base.Invoke((Action)delegate
                {
                    readings[4] = this.CalculateOhms(readings);
                    if (readings[6] == 0M)
                    {
                        lblVoltage.Text = "0";
                        lblOhms.Text = "0";
                        lblAmps.Text = "0";
                        lblWatts.Text = "0";
                        lblPF.Text = "0";
                        CheckReadingSuccess();
                    }
                    else
                    {
                        lblVoltage.Text = readings[2].ToString();
                        lblOhms.Text = readings[4].ToString();
                        lblAmps.Text = readings[3].ToString();
                        lblWatts.Text = readings[1].ToString();
                        lblPF.Text = readings[5].ToString();
                        CheckReadingSuccess();
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating readings");
            }
        }

        private void powerAnalyzer_RunStateChanged(RunState state)
        {
            try
            {
                base.Invoke((Action)delegate
                {
                    switch (state)
                    {
                        case RunState.Started:
                            return;

                        case RunState.Stopped:
                            return;
                    }
                });
            }
            catch (Exception ex)
            {
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
//            c.ShowDialog();
            pd = new PrintDialog();
            if (pd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                button1.Enabled = true;
            else
                button1.Enabled = false;

            ChangePowerAnalyzerStart(true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ShowWait();
            try
            {
                FoxProServiceClient client = new FoxProServiceClient();
                DataSet ds = client.GetCIGUnitInfo(textBox1.Text).ds;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    unitnum = textBox1.Text;
                    lblOptVolt.Text = ds.Tables[0].Rows[0]["Optvolt"].ToString();
                    lblOhmHigh.Text = ds.Tables[0].Rows[0]["Ohm_h"].ToString();
                    lblOhmLow.Text = ds.Tables[0].Rows[0]["Ohm_l"].ToString();
                    currentWire = Boolean.Parse(ds.Tables[0].Rows[0]["Wires"].ToString()) ? "Y" : "N";
                    button2.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            HideWait();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            ChangePowerAnalyzerStart(false);
            this.powerAnalyzer = null;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ShowWait();
            try
            {
                string volts = lblVoltage.Text;
                string amps = lblAmps.Text;
                string watts = lblWatts.Text;
                string pf = lblPF.Text;
                string ohms = lblOhms.Text;

                FoxProServiceClient client = new FoxProServiceClient();
                int autonum = client.InsertCIGPAReading(unitnum, watts, pf, volts, amps, "60", currentWire);

                for (int x = 0; x < pd.PrinterSettings.Copies; x++)
                {
                    LocalReport report = new LocalReport();
                    report.ReportPath = @"Label.rdlc";
                    report.SetParameters(new ReportParameter[] { new ReportParameter("Unitnum", unitnum + "-" + ddl_Plant.SelectedItem.ToString()), new ReportParameter("Autonum", autonum.ToString()), new ReportParameter("Volts", volts), new ReportParameter("Amps", amps), new ReportParameter("Watts", watts), new ReportParameter("Hertz", "60"), new ReportParameter("Ohms", ohms) });

                    ReportPrintDocument rpd = new ReportPrintDocument(report);
                    rpd.PrinterSettings = pd.PrinterSettings;
                    rpd.Print();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
         //   ClearDisplay();
            HideWait();
        }

        #endregion

        private void ddl_Plant_SelectedIndexChanged(object sender, EventArgs e)
        {
            SettingsTO sto = SettingsTO.Default;
            sto.Plant = ddl_Plant.SelectedItem.ToString();
            sto.SaveSettings();
        }
    }
}
