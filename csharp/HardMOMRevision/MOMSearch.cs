using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace HardMOMRevision
{
    public partial class MOMSearch : Form
    {
        public Dictionary<string, string> Selected { get; set; }
        private string Server { get; set; }
        private string Database { get; set; }
        private string Company { get; set; }

        public MOMSearch(string server, string database, string company)
        {
            InitializeComponent();
            Selected = new Dictionary<string, string>();
            Server = server;
            Database = database;
            Company = company;
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection connection = new SqlConnection("data source=" + Server + ";initial catalog=" + Database +";integrated security=SSPI");
                SqlCommand command = connection.CreateCommand();

                if (!String.IsNullOrEmpty(txt_WhereUsed.Text))
                {
                    command.CommandText = "exec [dbo].sp_GetWhereUsed @Company, @Partnum";
                    command.Parameters.AddWithValue("Company", Company);
                    command.Parameters.AddWithValue("Partnum", txt_WhereUsed.Text);
                }
                else
                {
                    command.CommandText = "exec [dbo].sp_FuzzyMOMLookup @Company, @Partnum, @SearchWord, @PartDescription, @Opr";
                    command.Parameters.AddWithValue("Company", Company);
                    if (txt_Partnum.Text.Length > 0)
                        command.Parameters.AddWithValue("Partnum", txt_Partnum.Text);
                    else
                        command.Parameters.AddWithValue("Partnum", DBNull.Value);
                    if (txt_SearchWord.Text.Length > 0)
                        command.Parameters.AddWithValue("SearchWord", txt_SearchWord.Text);
                    else
                        command.Parameters.AddWithValue("SearchWord", DBNull.Value);
                    if (txt_Description.Text.Length > 0)
                        command.Parameters.AddWithValue("PartDescription", txt_Description.Text);
                    else
                        command.Parameters.AddWithValue("PartDescription", DBNull.Value);
                    if (txt_HasOpr.Text.Length > 0)
                        command.Parameters.AddWithValue("Opr", txt_HasOpr.Text);
                    else
                        command.Parameters.AddWithValue("Opr", DBNull.Value);
                }
//                command.CommandText = "select partnum, partdescription from part where company = 'CRD' and substring(partnum, 1,2) ='00' and substring(partnum,11,1) = '1' and substring(partnum, 5, 1) = 'M'";
                SqlDataAdapter sda = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                sda.Fill(ds);

                if (ds.Tables[0].Rows.Count == 0)
                    MessageBox.Show("No results");
                else
                {
                    MOMSearchResults r = new MOMSearchResults();
                    r.Data = ds;
                    if (r.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        Selected.Clear();
                        Selected = r.Selected;
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
    }
}
