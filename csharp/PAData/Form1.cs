using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ObjectLibrary;
using Styleline.WinAnalyzer.DAL.Repositories;
using Styleline.WinAnalyzer.DAL.Entities;

namespace PAData
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string partstring = txt_Parts.Text;
                partstring = partstring.Replace("\r\n", ",");
                string[] parts = partstring.Split(new char[] { ',' });
                DataTable dt = BuildDataTable();


                foreach (string part in parts)
                {
                    DataRow newrow = dt.NewRow();
                    newrow[0] = part;
                    try
                    {
                        newrow[1] = PowerAnalyzerInterface.ParseDoorCountCode(part);
                    }
                    catch (Exception ex)
                    {
                        newrow[1] = "err";
                    }
                    try
                    {
                        newrow[2] = PowerAnalyzerInterface.ParseModelCode(part);
                    }
                    catch (Exception ex)
                    {
                        newrow[2] = "err";
                    }
                    try
                    {
                        newrow[3] = PowerAnalyzerInterface.ParseLineCode(part);
                    }
                    catch (Exception ex)
                    {
                        newrow[3] = "err";
                    }
                    try
                    {
                        newrow[4] = PowerAnalyzerInterface.ParseFrameCode(part);
                    }
                    catch (Exception ex)
                    {
                        newrow[4] = "err";
                    }
                    try
                    {
                        newrow[5] = PowerAnalyzerInterface.ParseVoltage(part);
                    }
                    catch (Exception ex)
                    {
                        newrow[5] = "err";
                    }

                    try
                    {
                        PowerAnalyzerRepository repository = new PowerAnalyzerRepository();
                        IPowerAnalyzerRepository rep = new PowerAnalyzerRepository();
                        PowerTable table = rep.GetPowerTable(repository.GetDoorCounts().First(d => d.CountName == PowerAnalyzerInterface.ParseDoorCountCode(part)).Id,
                            repository.GetModels().First(d => d.ModelCode == PowerAnalyzerInterface.ParseModelCode(part)).Id,
                            repository.GetLines().First(d => d.LineCode == PowerAnalyzerInterface.ParseLineCode(part)).Id,
                            repository.GetFrameTypes().First(d => d.FrameTypeCode == PowerAnalyzerInterface.ParseFrameCode(part)).Id,
                            repository.GetVoltages().First(d => d.VoltageName == PowerAnalyzerInterface.ParseVoltage(part)).Id);
                        if (table != null)
                        {
                            newrow[6] = "YES";
                            newrow[7] = table.drfrwire;
                            newrow[8] = table.drfrohlo;
                            newrow[9] = table.drfrohhi;
                            newrow[10] = table.drglohlo;
                            newrow[11] = table.drglohhi;
                            newrow[12] = table.drdrohlo;
                            newrow[13] = table.drdrohhi;
                            newrow[14] = table.drdramlo;
                            newrow[15] = table.drdramhi;
                            newrow[16] = table.frw1wire;
                            newrow[17] = table.frw1ohlo;
                            newrow[18] = table.frw1ohhi;
                            newrow[19] = table.frw2wire;
                            newrow[20] = table.frw2ohlo;
                            newrow[21] = table.frw2ohhi;
                            newrow[22] = table.frfwohlo;
                            newrow[23] = table.frfwohhi;
                            newrow[24] = table.frmuwire;
                            newrow[25] = table.frmuohlo;
                            newrow[26] = table.frmuohhi;
                            newrow[27] = table.frstwire;
                            newrow[28] = table.frstohlo;
                            newrow[29] = table.frstohhi;
                            newrow[30] = table.frtmohlo;
                            newrow[31] = table.frtmohhi;
                            newrow[32] = table.frtsohlo;
                            newrow[33] = table.frtsohhi;
                            newrow[34] = table.frtfohlo;
                            newrow[35] = table.frtfohhi;
                            newrow[36] = table.frtfamlo;
                            newrow[37] = table.frtfamhi;
                            newrow[38] = table.ltampslo;
                            newrow[39] = table.ltampshi;
                            newrow[40] = table.suflamlo;
                            newrow[41] = table.suflamhi;
                            newrow[42] = table.sudfamlo;
                            newrow[43] = table.sudfamhi;
                            newrow[44] = table.sudlamlo;
                            newrow[45] = table.sudlamhi;
                            newrow[46] = table.sumxamhe;
                            newrow[47] = table.sumxamlt;
                            newrow[48] = table.sumxamto;
                            newrow[49] = table.surtamhe;
                            newrow[50] = table.surtamlt;

                        }
                        else
                            newrow[6] = "NO";

                    }
                    catch (Exception ex)
                    {
                        newrow[6] = "NO";
                    }

                    dt.Rows.Add(newrow);
                }
                dgv_Data.DataSource = dt;
                dgv_Data.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }

        private DataTable BuildDataTable()
        {
            DataTable dt = new DataTable();

            DataColumn c = new DataColumn();
            c.ColumnName = "Part #";
            c.ReadOnly = true;
            c.DataType = typeof(string);
            dt.Columns.Add(c);

            c = new DataColumn();
            c.ColumnName = "Door Count";
            c.ReadOnly = true;
            c.DataType = typeof(string);
            dt.Columns.Add(c);

            c = new DataColumn();
            c.ColumnName = "Model";
            c.ReadOnly = true;
            c.DataType = typeof(string);
            dt.Columns.Add(c);

            c = new DataColumn();
            c.ColumnName = "Line";
            c.ReadOnly = true;
            c.DataType = typeof(string);
            dt.Columns.Add(c);

            c = new DataColumn();
            c.ColumnName = "Frame";
            c.ReadOnly = true;
            c.DataType = typeof(string);
            dt.Columns.Add(c);

            c = new DataColumn();
            c.ColumnName = "Voltage";
            c.ReadOnly = true;
            c.DataType = typeof(string);
            dt.Columns.Add(c);

            c = new DataColumn();
            c.ColumnName = "Found";
            c.ReadOnly = true;
            c.DataType = typeof(string);
            dt.Columns.Add(c);

            c = new DataColumn();
            c.ColumnName = "drfrwire";
            c.ReadOnly = true;
            c.DataType = typeof(string);
            dt.Columns.Add(c);

            c = new DataColumn();
            c.ColumnName = "drfrohlo";
            c.ReadOnly = true;
            c.DataType = typeof(string);
            dt.Columns.Add(c);

            c = new DataColumn();
            c.ColumnName = "drfrohhi";
            c.ReadOnly = true;
            c.DataType = typeof(string);
            dt.Columns.Add(c);

            c = new DataColumn();
            c.ColumnName = "drglohlo";
            c.ReadOnly = true;
            c.DataType = typeof(string);
            dt.Columns.Add(c);

            c = new DataColumn();
            c.ColumnName = "drglohhi";
            c.ReadOnly = true;
            c.DataType = typeof(string);
            dt.Columns.Add(c);

            c = new DataColumn();
            c.ColumnName = "drdrohlo";
            c.ReadOnly = true;
            c.DataType = typeof(string);
            dt.Columns.Add(c);

            c = new DataColumn();
            c.ColumnName = "drdrohhi";
            c.ReadOnly = true;
            c.DataType = typeof(string);
            dt.Columns.Add(c);

            c = new DataColumn();
            c.ColumnName = "drdramlo";
            c.ReadOnly = true;
            c.DataType = typeof(string);
            dt.Columns.Add(c);

            c = new DataColumn();
            c.ColumnName = "drdramhi";
            c.ReadOnly = true;
            c.DataType = typeof(string);
            dt.Columns.Add(c);

            c = new DataColumn();
            c.ColumnName = "frw1wire";
            c.ReadOnly = true;
            c.DataType = typeof(string);
            dt.Columns.Add(c);

            c = new DataColumn();
            c.ColumnName = "frw1ohlo";
            c.ReadOnly = true;
            c.DataType = typeof(string);
            dt.Columns.Add(c);

            c = new DataColumn();
            c.ColumnName = "frw1ohhi";
            c.ReadOnly = true;
            c.DataType = typeof(string);
            dt.Columns.Add(c);

            c = new DataColumn();
            c.ColumnName = "frw2wire";
            c.ReadOnly = true;
            c.DataType = typeof(string);
            dt.Columns.Add(c);

            c = new DataColumn();
            c.ColumnName = "frw2ohlo";
            c.ReadOnly = true;
            c.DataType = typeof(string);
            dt.Columns.Add(c);

            c = new DataColumn();
            c.ColumnName = "frw2ohhi";
            c.ReadOnly = true;
            c.DataType = typeof(string);
            dt.Columns.Add(c);

            c = new DataColumn();
            c.ColumnName = "frfwohlo";
            c.ReadOnly = true;
            c.DataType = typeof(string);
            dt.Columns.Add(c);

            c = new DataColumn();
            c.ColumnName = "frfwohhi";
            c.ReadOnly = true;
            c.DataType = typeof(string);
            dt.Columns.Add(c);

            c = new DataColumn();
            c.ColumnName = "frmuwire";
            c.ReadOnly = true;
            c.DataType = typeof(string);
            dt.Columns.Add(c);

            c = new DataColumn();
            c.ColumnName = "frmuohlo";
            c.ReadOnly = true;
            c.DataType = typeof(string);
            dt.Columns.Add(c);

            c = new DataColumn();
            c.ColumnName = "frmuohhi";
            c.ReadOnly = true;
            c.DataType = typeof(string);
            dt.Columns.Add(c);

            c = new DataColumn();
            c.ColumnName = "frstwire";
            c.ReadOnly = true;
            c.DataType = typeof(string);
            dt.Columns.Add(c);

            c = new DataColumn();
            c.ColumnName = "frstohlo";
            c.ReadOnly = true;
            c.DataType = typeof(string);
            dt.Columns.Add(c);

            c = new DataColumn();
            c.ColumnName = "frstohhi";
            c.ReadOnly = true;
            c.DataType = typeof(string);
            dt.Columns.Add(c);

            c = new DataColumn();
            c.ColumnName = "frtmohlo";
            c.ReadOnly = true;
            c.DataType = typeof(string);
            dt.Columns.Add(c);

            c = new DataColumn();
            c.ColumnName = "frtmohhi";
            c.ReadOnly = true;
            c.DataType = typeof(string);
            dt.Columns.Add(c);

            c = new DataColumn();
            c.ColumnName = "frtsohlo";
            c.ReadOnly = true;
            c.DataType = typeof(string);
            dt.Columns.Add(c);

            c = new DataColumn();
            c.ColumnName = "frtsohhi";
            c.ReadOnly = true;
            c.DataType = typeof(string);
            dt.Columns.Add(c);

            c = new DataColumn();
            c.ColumnName = "frtfohlo";
            c.ReadOnly = true;
            c.DataType = typeof(string);
            dt.Columns.Add(c);

            c = new DataColumn();
            c.ColumnName = "frtfohhi";
            c.ReadOnly = true;
            c.DataType = typeof(string);
            dt.Columns.Add(c);

            c = new DataColumn();
            c.ColumnName = "frtfamlo";
            c.ReadOnly = true;
            c.DataType = typeof(string);
            dt.Columns.Add(c);

            c = new DataColumn();
            c.ColumnName = "frtfamhi";
            c.ReadOnly = true;
            c.DataType = typeof(string);
            dt.Columns.Add(c);

            c = new DataColumn();
            c.ColumnName = "ltampslo";
            c.ReadOnly = true;
            c.DataType = typeof(string);
            dt.Columns.Add(c);

            c = new DataColumn();
            c.ColumnName = "ltampshi";
            c.ReadOnly = true;
            c.DataType = typeof(string);
            dt.Columns.Add(c);

            c = new DataColumn();
            c.ColumnName = "suflamlo";
            c.ReadOnly = true;
            c.DataType = typeof(string);
            dt.Columns.Add(c);

            c = new DataColumn();
            c.ColumnName = "suflamhi";
            c.ReadOnly = true;
            c.DataType = typeof(string);
            dt.Columns.Add(c);

            c = new DataColumn();
            c.ColumnName = "sudfamlo";
            c.ReadOnly = true;
            c.DataType = typeof(string);
            dt.Columns.Add(c);

            c = new DataColumn();
            c.ColumnName = "sudfamhi";
            c.ReadOnly = true;
            c.DataType = typeof(string);
            dt.Columns.Add(c);

            c = new DataColumn();
            c.ColumnName = "sudlamlo";
            c.ReadOnly = true;
            c.DataType = typeof(string);
            dt.Columns.Add(c);

            c = new DataColumn();
            c.ColumnName = "sudlamhi";
            c.ReadOnly = true;
            c.DataType = typeof(string);
            dt.Columns.Add(c);

            c = new DataColumn();
            c.ColumnName = "sumxamhe";
            c.ReadOnly = true;
            c.DataType = typeof(string);
            dt.Columns.Add(c);

            c = new DataColumn();
            c.ColumnName = "sumxamlt";
            c.ReadOnly = true;
            c.DataType = typeof(string);
            dt.Columns.Add(c);

            c = new DataColumn();
            c.ColumnName = "sumxamto";
            c.ReadOnly = true;
            c.DataType = typeof(string);
            dt.Columns.Add(c);

            c = new DataColumn();
            c.ColumnName = "surtamhe";
            c.ReadOnly = true;
            c.DataType = typeof(string);
            dt.Columns.Add(c);

            c = new DataColumn();
            c.ColumnName = "surtamlt";
            c.ReadOnly = true;
            c.DataType = typeof(string);
            dt.Columns.Add(c);

            return dt;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dgv_Data.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            DataObject obj = dgv_Data.GetClipboardContent();
            Clipboard.SetDataObject(obj, true);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            splitContainer1.SplitterDistance = 311;
        }
    }
}
