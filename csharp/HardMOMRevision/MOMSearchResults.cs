using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HardMOMRevision
{
    public partial class MOMSearchResults : Form
    {
        public DataSet Data { get; set; }
        public Dictionary<string, string> Selected { get; set; }

        public MOMSearchResults()
        {
            InitializeComponent();
            Selected = new Dictionary<string, string>();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void MOMSearchResults_Shown(object sender, EventArgs e)
        {
            dataGridView1.DataSource = Data.Tables[0];
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            Selected.Clear();
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                Selected[row.Cells[0].Value.ToString()] = row.Cells[2].Value.ToString();
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}
