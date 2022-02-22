using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DatabaseExplorer
{
    public partial class ChildForm : Form
    {
        const int maxdepth = 1;
        public string server = "";
        public string database = "";
        public string rootquery = "";
        public string rootcolumn = "";
        public string roottable = "";
        public string keycolumn = "";
        public string filter = "";
        public string title = "";
        public string mode = "JOIN";
        public ChildForm parent = null;
        List<string> layer = new List<string>();
        Dictionary<string, List<int>> tree = new Dictionary<string, List<int>>();

        public ChildForm()
        {
            InitializeComponent();
        }

        public void AddLayer(List<string> l)
        {
            foreach (string s in l)
                layer.Add(s);
        }

        private void ChildForm_Shown(object sender, EventArgs e)
        {
            this.Text = title;
            SqlCommand command = new SqlCommand(rootquery);
            DataSet ds = SQLAccess.SQLAccess.GetDataSetSSPI(server, database, command);
            int topcount = ds.Tables[0].Rows.Count;
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                try
                {
                    layer.Add(row[1].ToString());
                    SqlCommand command2;
                 //   if (parent == null)
                   //     command2 = new SqlCommand("select count(*) from " + row[1] + " as t inner join " + roottable + " on t.company = " + roottable + ".company and t." + row[0] + " = " + roottable + "." + rootcolumn + " " + filter + " and convert(varchar,t." + row[0] + ") != '0'");
                    //else
                        command2 = new SqlCommand("select count(*) from " + row[1] + " (nolock) " + BuildFilter(row[0].ToString(), filter, row[1].ToString()));
                        //command2 = new SqlCommand("select count(*) from " + row[1] + " " + BuildFilter(row[0].ToString(), filter));
                    //SqlCommand command2 = new SqlCommand("select count(*) from " + row[1] + " as t  + filter);
                    int count = Int32.Parse(SQLAccess.SQLAccess.GetScalarSSPI(server, database, command2));
                    if (count > 0)
                    {
                        /*                                SqlCommand command3 = new SqlCommand(@"SELECT      cc.column_name, c.name  AS 'ColumnName',t.name AS 'TableName'
                                                            FROM        sys.columns c
                                                            JOIN        sys.tables  t   ON c.object_id = t.object_id
                                                            join INFORMATION_SCHEMA.COLUMNS as cc on c.name like '%'+ cc.column_name+'%'
                                                            where  cc.table_name = '" + row[1] + @"'
                                                            and cc.column_name not in (" + ParseExclusions() + @")
                                                            ORDER BY    TableName,ColumnName;");
                                                        DataSet ds1 = SQLAccess.SQLAccess.GetDataSetSSPI(server, database, command3);*/

                        SqlCommand command3 = new SqlCommand("select c.name from sys.columns as c inner join sys.tables as t on c.object_id = t.object_id where t.name = '" + row[1] + "'");
                        DataSet ds2 = SQLAccess.SQLAccess.GetDataSetSSPI(server, database, command3);
                        DataGridViewRow newrow = new DataGridViewRow();
                        newrow.CreateCells(dataGridView1);
                        newrow.Cells[0].Value = "select * from " + row[1] + " " + BuildFilter(row[0].ToString(), filter, row[1].ToString());
                        newrow.Cells[1].Value = row[1];
                        newrow.Cells[2].Value = row[0];
                        newrow.Cells[3].Value = count.ToString();
                        ((DataGridViewComboBoxCell)newrow.Cells[4]).DataSource = ds2.Tables[0];
                        ((DataGridViewComboBoxCell)newrow.Cells[4]).DisplayMember = "name";
                        ((DataGridViewComboBoxCell)newrow.Cells[4]).ValueMember = "name";
                        newrow.Cells[5].Value = "Find";
                        System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                        if (!Captured(row[1].ToString(), count))
                        {
                            AddCaptured(row[1].ToString(), count);
                            Output.Write(newrow.Cells[0].Value.ToString());
                            if (config.AppSettings.Settings[row[1].ToString()] != null)
                            {
                                List<string> processed = new List<string>();
                                List<string> toremove = new List<string>();
                                foreach (string s in config.AppSettings.Settings[row[1].ToString()].Value.Split(','))
                                {
                                    if (processed.IndexOf(s) < 0)
                                    {
                                        processed.Add(s);
                                      //  if (s != keycolumn)
                                       //     SpawnChild(row[1].ToString(), row[0].ToString(), s);
                                    }
                                    else
                                        toremove.Add(s);
                                }
                                if (toremove.Count > 0)
                                {
                                    foreach (string s in toremove)
                                        config.AppSettings.Settings[row[1].ToString()].Value = config.AppSettings.Settings[row[1].ToString()].Value.Replace("," + s, "");
                                    ConfigurationManager.RefreshSection("appSettings");
                                }
                            }
                            if (config.AppSettings.Settings[row[1].ToString()] != null)
                                newrow.Cells[6].Value = config.AppSettings.Settings[row[1].ToString()].Value;
                            //                                newrow.Cells[3].Value = ds1.Tables[0].Rows.Count;
                            dataGridView1.Rows.Add(newrow);
                        }
                    }
                }
                catch (Exception ex)
                {
                    //string error = ex.Message;

         //           MessageBox.Show(row[1].ToString() + ":" + ex.Message);
                }
            }
            if (dataGridView1.Rows.Count == 0)
                this.Close();
        }

        public void AddCaptured(string table, int count)
        {
            if (!tree.ContainsKey(table))
                tree[table] = new List<int>();
            tree[table].Add(count);
            if (parent != null)
                parent.AddCaptured(table, count);
        }
        public bool Captured(string table, int count)
        {
            if (tree.ContainsKey(table) && tree[table].IndexOf(count) >= 0)
                return true;
            else
            {
                if (parent != null)
                    return parent.Captured(table, count);
                else
                    return false;
            }
        }

        private int Depth()
        {
            if (parent == null)
                return 0;
            else
                return 1 + parent.Depth();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 0)
                {
                    DetailsForm form = new DetailsForm();
                    form.server = server;
                    form.database = database;
                    form.table = senderGrid.Rows[e.RowIndex].Cells[1].Value.ToString();
                    form.column = senderGrid.Rows[e.RowIndex].Cells[2].Value.ToString();
                    form.roottable = roottable;
                    form.rootcolumn = rootcolumn;
                    form.filter = filter;
                    form.title = this.title + " " + senderGrid.Rows[e.RowIndex].Cells[1].Value.ToString() + " Records";
                    form.query = "select * from " + senderGrid.Rows[e.RowIndex].Cells[1].Value.ToString() + " join " + roottable + " on " 
                        + senderGrid.Rows[e.RowIndex].Cells[1].Value.ToString() + ".company = " + roottable + ".company and " 
                        + senderGrid.Rows[e.RowIndex].Cells[1].Value.ToString() + "." + senderGrid.Rows[e.RowIndex].Cells[2].Value.ToString() 
                        + " = " + roottable + "." + rootcolumn + " " + filter;
                    form.Show();
                }
                else if (e.ColumnIndex == 5)
                {
                    System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                    if (config.AppSettings.Settings[senderGrid.Rows[e.RowIndex].Cells[1].Value.ToString()] == null)
                    {
                        config.AppSettings.Settings.Add(senderGrid.Rows[e.RowIndex].Cells[1].Value.ToString(), senderGrid.Rows[e.RowIndex].Cells[4].Value.ToString());
                    }
                    else
                    {
                        if (!config.AppSettings.Settings[senderGrid.Rows[e.RowIndex].Cells[1].Value.ToString()].Value.Split(',').Contains(senderGrid.Rows[e.RowIndex].Cells[4].Value.ToString()))
                            config.AppSettings.Settings[senderGrid.Rows[e.RowIndex].Cells[1].Value.ToString()].Value += "," + senderGrid.Rows[e.RowIndex].Cells[4].Value.ToString();
                    }


                    config.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection("appSettings");

                    SpawnChild(senderGrid.Rows[e.RowIndex].Cells[1].Value.ToString(), senderGrid.Rows[e.RowIndex].Cells[2].Value.ToString(), senderGrid.Rows[e.RowIndex].Cells[4].Value.ToString());
                }
                else if (e.ColumnIndex == 6)
                {
                    foreach (string s in senderGrid.Rows[e.RowIndex].Cells[6].Value.ToString().Split(','))
                    {
                        if (s != keycolumn)
                            SpawnChild(senderGrid.Rows[e.RowIndex].Cells[1].Value.ToString(), senderGrid.Rows[e.RowIndex].Cells[2].Value.ToString(), s);
                    }
                }
            }
        }

        private void SpawnChild(string table, string column, string key)
        {
            ChildForm form = new ChildForm();
            form.server = server;
            form.database = database;
            form.rootquery = @"SELECT      c.name  AS 'ColumnName',t.name AS 'TableName'
                        FROM sys.columns c
                        JOIN sys.tables t   ON c.object_id = t.object_id
                        join INFORMATION_SCHEMA.COLUMNS as cc on c.name like '%' + cc.column_name + '%'
						left join INFORMATION_SCHEMA.COLUMNS as cc2 on cc.column_name = cc2.column_name and cc2.table_name = '" + roottable + @"'
                        where cc.table_name = '" + table + @"'
                        and c.name = '" + key + @"'
                        ORDER BY    TableName,ColumnName;";

            //                    and cc2.column_name is null

            //                    and t.name != '" + roottable + @"'

            //                    and t.name not in (" + MakeLayerFilter() + @")
            form.roottable = table;
            form.rootcolumn = column;
            form.keycolumn = key;
            form.mode = "WHERE";
            form.parent = this;
            //                    form.filter = "inner join " + roottable + " on " + senderGrid.Rows[e.RowIndex].Cells[1].Value.ToString() + ".company = " + roottable + ".company and " + senderGrid.Rows[e.RowIndex].Cells[1].Value.ToString() + "." + senderGrid.Rows[e.RowIndex].Cells[2].Value.ToString() + " = " + roottable + "." + rootcolumn + " " + filter;
            form.filter = filter;
            form.title = this.title + " relation " + column + " on " + table + " for " + key;
            form.AddLayer(layer);
            form.Show();

        }

        private string MakeLayerFilter()
        {
            string r = "";
            foreach (string f in layer)
            {
                if (r.Length > 0)
                    r += ", ";
                r += "'" + f + "'";
            }
            return r;
        }

        private string BuildFilter(string column, string f, string table = "")
        {
            string result = "";
            if (parent != null)
            {
                //                result = " where " + table + "." + column + " in (select " + roottable + "." + column + " from " + roottable + parent.Join(roottable, rootcolumn) + " " + f + ") and convert(varchar," + table + "." + column + ") != '0' and convert(varchar," + table + "." + column + ") != ''";
                result = Join(table, column) + " " + f;
            }
            else
            {
                if (table == roottable)
                    result = f;
                else
                    result = Join(table, column) + " " + f;
            }
            return result;
        }

        private string Join(string child, string column)
        {
            string result = " inner join " + roottable + " (nolock) on " + child + ".company = " + roottable + ".company and " + child + "." + column + " = " + roottable + "." + keycolumn + " and convert(varchar," + child+"."+column+") != '0' and convert(varchar," + child+"."+column+") != ''";
            if (parent != null && parent.roottable != roottable)
                result += parent.Join(roottable, rootcolumn);
            return result;
        }
    }
}
