using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace MESEdit
{
    public partial class Form1 : Form
    {
        SqlConnection conn;
        SqlCommand command;
        SqlDataReader reader;
        string strCompany;
        int intModule;
        string strVCBaseLink;
        int intIntBaseLink;
        string strVCLink;
        int intIntLink1; // Quotenum
        int intIntLink2; // Quoteline
        string strEditLabel;
        //string strMESNum;

        public Form1(string[] args)
        {
            InitializeComponent();
            conn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["Server"].ToString() + ";Database=" + ConfigurationManager.AppSettings["Database"].ToString() + ";Integrated Security=SSPI");
            strCompany = args[0];
            intModule = Convert.ToInt32(args[1]);
            strVCBaseLink = args[2];
            intIntBaseLink = Convert.ToInt32(args[3]);
            strVCLink = args[4];
            intIntLink1 = Convert.ToInt32(args[5]);
            intIntLink2 = Convert.ToInt32(args[6]);
            strEditLabel = args[7];
            PopulateHelperInfo();
        }

        private void PopulateHelperInfo()
        {
            try
            {
                conn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["Server"].ToString() + ";Database=" + ConfigurationManager.AppSettings["Database"].ToString() + ";Integrated Security=SSPI");
                command = conn.CreateCommand();
                command.CommandText = "exec [dbo].sp_GetMESHelperInfo @Company, @Quotenum, @Quoteline";
                command.Parameters.AddWithValue("Company", strCompany);
                command.Parameters.AddWithValue("Quotenum", intIntLink1);
                command.Parameters.AddWithValue("Quoteline", intIntLink2);

                DataTable dt = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(command);
                sda.Fill(dt);

                tbHelperInfo.Text = "";
                tbHelperInfo.Text = "Customer: " + dt.Rows[0]["Customer"].ToString() + Environment.NewLine + Environment.NewLine;
                tbHelperInfo.Text += "Carrier: " + dt.Rows[0]["Carrier"].ToString() + Environment.NewLine + Environment.NewLine;
                tbHelperInfo.Text += "Line Desc: " + dt.Rows[0]["Description"].ToString() + Environment.NewLine + Environment.NewLine;
                tbHelperInfo.Text += "Special Instructions: " + dt.Rows[0]["Special Instructions"].ToString() + Environment.NewLine + Environment.NewLine;
                tbHelperInfo.Visible = true;
            }
            catch (Exception ex)
            {
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "MES Edit for " + strEditLabel;
            label3.Text = strEditLabel;
            getAllMES();
            if (intModule>=10)
            {
                autoPopulateSelectedMES();
            }
            refreshSelectedMES();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            { 
                MessageBox.Show("Select an MES to add"); 
            }
            else
            {
                int found; 
                found = listBox2.FindStringExact(listBox1.SelectedItem.ToString());
                if (found == -1)
                { 
                    // Code for update MES Table
                    addMES(listBox1.SelectedItem.ToString());
                    refreshSelectedMES();
                    //listBox2.Items.Add(listBox1.SelectedItem); 
                }
                else 
                { 
                    MessageBox.Show("MES already added"); 
                }
            }
        }

        private void getAllMES()
        {
            conn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["Server"].ToString() + ";Database=" + ConfigurationManager.AppSettings["Database"].ToString() + ";Integrated Security=SSPI");
            command = new SqlCommand("[dbo].sp_UD03_ListAllMESforCompany", conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 15).Value = strCompany;
            conn.Open();
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                listBox1.Items.Add(reader["MES"].ToString() + ": " + reader["MESDescription"].ToString());
            }
            conn.Close();
        }

        private void refreshSelectedMES()
        {
            conn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["Server"].ToString() + ";Database=" + ConfigurationManager.AppSettings["Database"].ToString() + ";Integrated Security=SSPI");
            command = new SqlCommand("[dbo].sp_UD03_ListAllMESforModule", conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 15).Value = strCompany;
            command.Parameters.Add("@Module", SqlDbType.Int, 15).Value = intModule;
            command.Parameters.Add("@VCBaseLink", SqlDbType.VarChar, 50).Value = strVCBaseLink;
            command.Parameters.Add("@IntBaseLink", SqlDbType.Int, 15).Value = intIntBaseLink;
            command.Parameters.Add("@VCLink", SqlDbType.VarChar, 50).Value = strVCLink;
            command.Parameters.Add("@IntLink1", SqlDbType.Int, 15).Value = intIntLink1;
            command.Parameters.Add("@IntLink2", SqlDbType.Int, 15).Value = intIntLink2;
            //command.Parameters.Add("@MESNum", SqlDbType.VarChar, 15).Value = strMESNum;
            conn.Open();
            reader = command.ExecuteReader();
            listBox2.Items.Clear();
            while (reader.Read())
            {
                string strMESandDescription;
                if (reader["MESDescription"].ToString().Trim().Length == 0)
                {
                    strMESandDescription = reader["MES"].ToString() + ": !!!Missing Data - Please Tell Engineering!!!";
                }
                else
                {
                    strMESandDescription = reader["MES"].ToString() + ": " + reader["MESDescription"].ToString();
                }
                listBox2.Items.Add(strMESandDescription);
            }
            conn.Close();
        }

        private void addMES(string MESNumandDescr)
        {
            conn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["Server"].ToString() + ";Database=" + ConfigurationManager.AppSettings["Database"].ToString() + ";Integrated Security=SSPI");
            command = new SqlCommand("[dbo].sp_UD03_AddMES", conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 15).Value = strCompany;
            command.Parameters.Add("@Module", SqlDbType.Int, 15).Value = intModule;
            command.Parameters.Add("@VCBaseLink", SqlDbType.VarChar, 50).Value = strVCBaseLink;
            command.Parameters.Add("@IntBaseLink", SqlDbType.Int, 15).Value = intIntBaseLink;
            command.Parameters.Add("@VCLink", SqlDbType.VarChar, 50).Value = strVCLink;
            command.Parameters.Add("@IntLink1", SqlDbType.Int, 15).Value = intIntLink1;
            command.Parameters.Add("@IntLink2", SqlDbType.Int, 15).Value = intIntLink2;
            command.Parameters.Add("@MESNum", SqlDbType.VarChar, 15).Value = getMESNum(MESNumandDescr);
            //command.Parameters.Add("@MESNum", SqlDbType.VarChar, 15).Value = strMESNum;
            conn.Open();
            reader = command.ExecuteReader();
        }

        private string getMESNum(string strMESandDescr)
        {
            string[] strSepPcs = strMESandDescr.Split(':');
            return strSepPcs[0].Trim();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem == null)
            {
                MessageBox.Show("Select an MES to remove");
            }
            else
            {
                removeMES(listBox2.SelectedItem.ToString());
                refreshSelectedMES();
            }

        }

        private void removeMES(string MESNumandDescr)
        {
            conn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["Server"].ToString() + ";Database=" + ConfigurationManager.AppSettings["Database"].ToString() + ";Integrated Security=SSPI");
            command = new SqlCommand("[dbo].sp_UD03_RemoveMES", conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 15).Value = strCompany;
            command.Parameters.Add("@Module", SqlDbType.Int, 15).Value = intModule;
            command.Parameters.Add("@VCBaseLink", SqlDbType.VarChar, 50).Value = strVCBaseLink;
            command.Parameters.Add("@IntBaseLink", SqlDbType.Int, 15).Value = intIntBaseLink;
            command.Parameters.Add("@VCLink", SqlDbType.VarChar, 50).Value = strVCLink;
            command.Parameters.Add("@IntLink1", SqlDbType.Int, 15).Value = intIntLink1;
            command.Parameters.Add("@IntLink2", SqlDbType.Int, 15).Value = intIntLink2;
            command.Parameters.Add("@MESNum", SqlDbType.VarChar, 15).Value = getMESNum(MESNumandDescr);
            //command.Parameters.Add("@MESNum", SqlDbType.VarChar, 15).Value = strMESNum;
            conn.Open();
            reader = command.ExecuteReader();
        }


        private void autoPopulateSelectedMES()
        {
            conn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["Server"].ToString() + ";Database=" + ConfigurationManager.AppSettings["Database"].ToString() + ";Integrated Security=SSPI");
            command = new SqlCommand("[dbo].sp_UD03_AddAllLinkedMES", conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 15).Value = strCompany;
            command.Parameters.Add("@Module", SqlDbType.Int, 15).Value = intModule;
            command.Parameters.Add("@VCBaseLink", SqlDbType.VarChar, 50).Value = strVCBaseLink;
            command.Parameters.Add("@IntBaseLink", SqlDbType.Int, 15).Value = intIntBaseLink;
            command.Parameters.Add("@VCLink", SqlDbType.VarChar, 50).Value = strVCLink;
            command.Parameters.Add("@IntLink1", SqlDbType.Int, 15).Value = intIntLink1;
            command.Parameters.Add("@IntLink2", SqlDbType.Int, 15).Value = intIntLink2;
            //command.Parameters.Add("@MESNum", SqlDbType.VarChar, 15).Value = strMESNum;
            conn.Open();
            reader = command.ExecuteReader();
            conn.Close();
        }



        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                conn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["Server"].ToString() + ";Database=" + ConfigurationManager.AppSettings["Database"].ToString() + ";Integrated Security=SSPI");
                command = new SqlCommand("[dbo].sp_UD03_CopyAllLinkedMES", conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Company", SqlDbType.VarChar, 15).Value = strCompany;
                command.Parameters.Add("@OrigPart", SqlDbType.VarChar, 50).Value = textBox1.Text;
                command.Parameters.AddWithValue("NewPart", strVCBaseLink);

                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
    }
}
