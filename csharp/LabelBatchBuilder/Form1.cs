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
using System.Drawing.Printing;

namespace LabelBatchBuilder
{
    public partial class Form1 : Form
    {
        string server = "SARV-SQLPROD01";
        string database = "EpicorDB";
        string username = "RailsAppUserP";
        string password = "wA7tA1FaBS1MpLaU";
        SqlConnection connection;

        public Form1()
        {
            InitializeComponent();

            connection = new SqlConnection("data source=" + server + ";initial catalog=" + database + ";user id=" + username + ";password=" + password);
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "exec [dbo].sp_GetPartClassesForCompany @Company";
            command.Parameters.AddWithValue("Company", "CRD");

            SqlDataAdapter sda = new SqlDataAdapter(command);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            DataRow newRow = ds.Tables[0].NewRow();
            newRow["Class"] = "%";
            newRow["Description"] = "All";
            ds.Tables[0].Rows.InsertAt(newRow, 0);

            cb_PartClass.DataSource = ds.Tables[0];
            cb_PartClass.DisplayMember = "Description";
            cb_PartClass.ValueMember = "Class";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "exec [dbo].sp_POPartLookup @Company, @PONum, @StartsWith, @EndsWith, @Operator, @Desc, @Class";
                command.Parameters.AddWithValue("Company", "CRD");
                if (String.IsNullOrEmpty(tb_PONum.Text))
                    command.Parameters.AddWithValue("PONum", DBNull.Value);
                else
                    command.Parameters.AddWithValue("PONum", Int32.Parse(tb_PONum.Text));

                if (String.IsNullOrEmpty(tb_PartNumStart.Text))
                    command.Parameters.AddWithValue("StartsWith", DBNull.Value);
                else
                    command.Parameters.AddWithValue("StartsWith", tb_PartNumStart.Text);

                if (String.IsNullOrEmpty(tb_PartNumEnd.Text))
                    command.Parameters.AddWithValue("EndsWith", DBNull.Value);
                else
                    command.Parameters.AddWithValue("EndsWith", tb_PartNumEnd.Text);

                if (!String.IsNullOrEmpty(tb_PartNumStart.Text) && !String.IsNullOrEmpty(tb_PartNumEnd.Text))
                    command.Parameters.AddWithValue("Operator", radioButton1.Checked ? "AND" : "OR");
                else
                    command.Parameters.AddWithValue("Operator", DBNull.Value);

                if (String.IsNullOrEmpty(tb_Description.Text))
                    command.Parameters.AddWithValue("Desc", DBNull.Value);
                else
                    command.Parameters.AddWithValue("Desc", tb_Description.Text);

                if (cb_PartClass.SelectedValue.ToString() == "%" || String.IsNullOrEmpty(cb_PartClass.SelectedValue.ToString()))
                    command.Parameters.AddWithValue("Class", DBNull.Value);
                else
                    command.Parameters.AddWithValue("Class", cb_PartClass.SelectedValue);

                SqlDataAdapter sda = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                sda.Fill(ds);

                dataGridView1.DataSource = ds.Tables[0];

                if (dataGridView2.Columns.Count == 0)
                {
                    dataGridView2.Columns.Clear();
                    DataGridViewTextBoxColumn partCol = new DataGridViewTextBoxColumn();
                    partCol.HeaderText = "partnum";
                    partCol.DataPropertyName = "partnum";
                    partCol.ReadOnly = true;
                    partCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dataGridView2.Columns.Add(partCol);

                    DataGridViewTextBoxColumn qtyCol = new DataGridViewTextBoxColumn();
                    qtyCol.HeaderText = "Qty";
                    qtyCol.ReadOnly = false;
                    qtyCol.Width = 25;

                    dataGridView2.Columns.Add(qtyCol);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void b_TakeSelected_Click(object sender, EventArgs e)
        {
            CurrencyManager currencyManager1 = (CurrencyManager)BindingContext[dataGridView1.DataSource];
            currencyManager1.SuspendBinding();

            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                dataGridView2.Rows.Add(((DataRowView)row.DataBoundItem)["partnum"].ToString(), "1");
                row.Visible = false;
            }
            currencyManager1.ResumeBinding();
            dataGridView1.ClearSelection();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Visible)
                {
                    row.Selected = true;
                    break;
                }
            }
        }

        private void b_RemoveSelected_Click(object sender, EventArgs e)
        {
            CurrencyManager currencyManager1 = (CurrencyManager)BindingContext[dataGridView1.DataSource];
            currencyManager1.SuspendBinding();

            List<DataGridViewRow> toRemove = new List<DataGridViewRow>();

            foreach (DataGridViewRow row in dataGridView2.SelectedRows)
            {
                foreach (DataGridViewRow showRow in dataGridView1.Rows)
                {
                    if (showRow.Cells[0].Value.ToString() == row.Cells[0].Value.ToString())
                        showRow.Visible = true;
                }
                toRemove.Add(row);
            }

            foreach (DataGridViewRow row in toRemove)
                dataGridView2.Rows.Remove(row);

            currencyManager1.ResumeBinding();
            dataGridView2.ClearSelection();
        }

        private void b_TakeAll_Click(object sender, EventArgs e)
        {
            CurrencyManager currencyManager1 = (CurrencyManager)BindingContext[dataGridView1.DataSource];
            currencyManager1.SuspendBinding();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                dataGridView2.Rows.Add(((DataRowView)row.DataBoundItem)["partnum"].ToString(), "1");
                row.Visible = false;
            }
            currencyManager1.ResumeBinding();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CurrencyManager currencyManager1 = (CurrencyManager)BindingContext[dataGridView1.DataSource];
            currencyManager1.SuspendBinding();

            List<DataGridViewRow> toRemove = new List<DataGridViewRow>();

            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                foreach (DataGridViewRow showRow in dataGridView1.Rows)
                {
                    if (showRow.Cells[0].Value.ToString() == row.Cells[0].Value.ToString())
                        showRow.Visible = true;
                }
                toRemove.Add(row);
            }

            foreach (DataGridViewRow row in toRemove)
                dataGridView2.Rows.Remove(row);

            currencyManager1.ResumeBinding();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Guid id = Guid.NewGuid();
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "exec [dbo].sp_AddLabelToBatch @Company, @ID, @Part";

            connection.Open();

            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                command.Parameters.Clear();
                command.Parameters.AddWithValue("Company", "CRD");
                command.Parameters.AddWithValue("ID", id);
                command.Parameters.AddWithValue("Part", row.Cells[0].Value.ToString());

                int qty = Int32.Parse(row.Cells[1].Value.ToString());
                for (int i = 0; i < qty; i++)
                {
                    command.ExecuteNonQuery();
                }
            }

            connection.Close();

            PrintDialog pd = new PrintDialog();
            if (pd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                command = connection.CreateCommand();
                command.CommandText = "exec [dbo].sp_GetLabelBatch @Company, @ID";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("Company", "CRD");
                command.Parameters.AddWithValue("ID", id);

                connection.Open();

                SqlDataAdapter sda = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                sda.Fill(ds);

                connection.Close();

                for (int x = 0; x < pd.PrinterSettings.Copies; x++)
                {
                    LocalReport report = new LocalReport();
                    report.ReportPath = @"LabelReport.rdlc";
                    report.SetParameters(new ReportParameter[] { new ReportParameter("Company", "CRD"), new ReportParameter("GUID", id.ToString()) });

                    report.DataSources.Add(new ReportDataSource("DataSet1", ds.Tables[0]));

                    ReportPrintDocument rpd = new ReportPrintDocument(report);
                    rpd.PrinterSettings = pd.PrinterSettings;
                    rpd.Print();
                }
            }
          }
    }
}
