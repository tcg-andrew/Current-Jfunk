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
    public partial class Form1 : Form
    {

        string server = "SARV-SQLPROD01";
        string database = "MfgSys803";



        public Form1()
        {
            InitializeComponent();
            Output.Init();
        }


        public string ParseExclusions()
        {
            return "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ChildForm form = new ChildForm();
            form.server = server;
            form.database = database;

            switch (comboBox1.SelectedItem.ToString())
            {
                case "Open POs":
                    form.rootcolumn = "ponum";
                    form.roottable = "poheader";
                    form.keycolumn = "ponum";
                    form.filter = "where (poheader.openorder = 1 and poheader.voidorder = 0) or (poheader.orderdate >= '02/01/2018')";
                    form.title = "Open PO Children";
                    form.rootquery = "SELECT c.name  AS 'ColumnName',t.name AS 'TableName' FROM sys.columns c JOIN sys.tables  t ON c.object_id = t.object_id WHERE c.name LIKE '%ponum%' ORDER BY TableName,ColumnName";
                    break;
                case "Open Orders":
                    form.rootcolumn = "ordernum";
                    form.roottable = "orderhed";
                    form.keycolumn = "ordernum";
                    form.filter = "where (orderhed.openorder = 1 and orderhed.voidorder = 0) or (orderhed.orderdate >= '02/01/2018')";
                    form.title = "Open Order Children";
                    form.rootquery = "SELECT c.name  AS 'ColumnName',t.name AS 'TableName' FROM sys.columns c JOIN sys.tables  t ON c.object_id = t.object_id WHERE c.name LIKE '%ordernum%' ORDER BY TableName,ColumnName";
                    break;
                case "Open Invoices":
                    form.rootcolumn = "invoicenum";
                    form.roottable = "invchead";
                    form.keycolumn = "invoicenum";
                    form.filter = "where (invchead.openinvoice = 1) or (invchead.invoicedate >= '02/01/2018')";
                    form.title = "Open Invoice Children";
                    form.rootquery = "SELECT c.name  AS 'ColumnName',t.name AS 'TableName' FROM sys.columns c JOIN sys.tables  t ON c.object_id = t.object_id WHERE c.name LIKE '%invoicenum%' ORDER BY TableName,ColumnName";
                    break;
            }

            form.Show();

        }

    }
}
