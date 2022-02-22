using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CRDProdLabel
{
    public partial class LabelSelectForm : Form
    {
        public DataTable Data { get; set; }
        private DataTable DisplayData { get; set; }
        public DataGridViewRowCollection Selected { get; set; }

        public LabelSelectForm()
        {
            InitializeComponent();
        }

        private void LabelSelectForm_Shown(object sender, EventArgs e)
        {

            string jobnum = "";
            string assemblyseq = "";
            dataGridView1.AllowUserToResizeColumns = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AllowUserToOrderColumns = false;
            dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;

            DisplayData = new DataTable();

            {
                System.Data.DataColumn c = new DataColumn();
                c.ColumnName = "# Labels";
                c.ReadOnly = false;
                c.DataType = typeof(String);
                DisplayData.Columns.Add(c);
            }
            {
                System.Data.DataColumn c = new DataColumn();
                c.ColumnName = "Job #";
                c.ReadOnly = true;
                c.DataType = typeof(String);
                DisplayData.Columns.Add(c);
            }

            {
                System.Data.DataColumn c = new DataColumn();
                c.ColumnName = "Part #";
                c.ReadOnly = true;
                c.DataType = typeof(String);
                DisplayData.Columns.Add(c);
            }

            {
                System.Data.DataColumn c = new DataColumn();
                c.ColumnName = "Desc";
                c.ReadOnly = true;
                c.DataType = typeof(String);
                DisplayData.Columns.Add(c);
            }

            foreach (DataRow row in Data.Rows)
            {
                DataRow newRow = DisplayData.NewRow();
                newRow["Job #"] = row[4];
                newRow["Part #"] = row[9];
                newRow["Desc"] = row[3];
                newRow["# Labels"] = 0;
                DisplayData.Rows.Add(newRow);
            }
            dataGridView1.DataSource = DisplayData;
            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            /*            foreach (DataRow row in Data.Rows)
                        {
                            if (row["jobnum"].ToString() != jobnum || row["asm"].ToString() != assemblyseq)
                            {
                                jobnum = row["jobnum"].ToString();
                                assemblyseq = row["asm"].ToString();
                                listView1.Items.Add(new ListViewItem(new string[] { jobnum, assemblyseq, row["part"].ToString() }));
                            }
                        }*/
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Selected = dataGridView1.Rows;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}
