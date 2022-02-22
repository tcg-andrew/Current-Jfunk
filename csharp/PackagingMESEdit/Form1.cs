using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PackagingMESEdit
{
    public partial class Form1 : Form
    {
        /*        string server = "SARV-SQLPROD01";
                string database = "EpicorDB";*/
        string server = "SARV-SQLDEV01";
        string database = "EpicorDB_PG";

        string strCompany;
        int orderNum;

        public Form1(string[] args)
        {
            InitializeComponent();
            strCompany = args[0];
            orderNum = Int32.Parse(args[1]);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "Packaging MES for Order # " + orderNum.ToString();
            getPackagingMES();
            refreshSelectedMES();
        }

        private void getPackagingMES()
        {
            SqlConnection connection = new SqlConnection("Data Source="+server+";Database="+database+";Integrated Security=SSPI");
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "exec [dbo].sp_UD03_ListAllPackagingMESforCompany @Company";
            command.Parameters.AddWithValue("@Company", strCompany);

            lbAvailableMES.Items.Clear();
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                lbAvailableMES.Items.Add(reader["MES"].ToString() + ": " + reader["MESDescription"].ToString());
            }
            connection.Close();
        }

        private void refreshSelectedMES()
        {
            SqlConnection connection = new SqlConnection("Data Source=" + server + ";Database=" + database + ";Integrated Security=SSPI");
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "exec [dbo].sp_UD03_ListAllPackagingMESforOrder @Company, @Ordernum";
            command.Parameters.AddWithValue("@Company", strCompany);
            command.Parameters.AddWithValue("@Ordernum", orderNum);

            lbSelectedMES.Items.Clear();
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                lbSelectedMES.Items.Add(reader["MES"].ToString() + ": " + reader["MESDescription"].ToString());
            }
            connection.Close();
        }

        private void butAdd_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection("Data Source=" + server + ";Database=" + database + ";Integrated Security=SSPI");
            SqlCommand command = connection.CreateCommand();

            command.CommandText = "exec [dbo].sp_UD03_AddMESToAllLines @Company, @Ordernum, @MES";

            foreach (string str in lbAvailableMES.SelectedItems)
            {
                string mesnum = str.Split(':')[0].Trim();

                command.Parameters.Clear();
                command.Parameters.AddWithValue("@Company", strCompany);
                command.Parameters.AddWithValue("@Ordernum", orderNum);
                command.Parameters.AddWithValue("@MES", mesnum);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }

            refreshSelectedMES();
        }

        private void butRemove_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection("Data Source=" + server + ";Database=" + database + ";Integrated Security=SSPI");
            SqlCommand command = connection.CreateCommand();

            command.CommandText = "exec [dbo].sp_UD03_RemoveMESFromAllLines @Company, @Ordernum, @MES";

            foreach (string str in lbSelectedMES.SelectedItems)
            {
                string mesnum = str.Split(':')[0].Trim();

                command.Parameters.Clear();
                command.Parameters.AddWithValue("@Company", strCompany);
                command.Parameters.AddWithValue("@Ordernum", orderNum);
                command.Parameters.AddWithValue("@MES", mesnum);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }

            refreshSelectedMES();

        }

        private void butDone_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
