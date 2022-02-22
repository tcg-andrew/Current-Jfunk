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

namespace DatabaseExplorer
{
    public partial class DetailsForm : Form
    {
        public string table;
        public string column;
        public string server;
        public string database;
        public string roottable;
        public string rootcolumn;
        public string filter;
        public string title;
        public string query;

        public DetailsForm()
        {
            InitializeComponent();
        }

        private void DetailsForm_Shown(object sender, EventArgs e)
        {
            this.Text = title;
            SqlCommand command = new SqlCommand(query);
            DataSet ds = SQLAccess.SQLAccess.GetDataSetSSPI(server, database, command);
            dataGridView1.DataSource = ds.Tables[0];
        }
    }
}
