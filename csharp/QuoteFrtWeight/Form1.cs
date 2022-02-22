using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Diagnostics;



namespace QuoteFrtWeight
{
    public partial class Form1 : Form
    {
        const int ERROR_FILE_NOT_FOUND = 2;
        const int ERROR_ACCESS_DENIED = 5;

        /*        string server = "SARV-SQLPROD01";
                string database = "EpicorDB";*/
        string server = "SARV-SQLDEV01";
        string database = "EpicorDB_PG";


        SqlConnection conn;
        SqlCommand command;
        SqlDataReader reader;
        string strCompany;
        //int intModule;
        //string strVCBaseLink;
        //int intIntBaseLink;
        int intQuoteNum;
        //int intIntLink1;
        //int intIntLink2;
        string strEditLabel;

        public Form1(string[] args)
        {
            InitializeComponent();
            strCompany = args[0];
            intQuoteNum = Convert.ToInt32(args[1]);
            strEditLabel = args[2];
        }


        private void refreshQuoteCalcWeight()
        {
            conn = new SqlConnection("Data Source=" + server + ";Database=" + database + ";Integrated Security=SSPI");
            command = new SqlCommand("[dbo].sp_Quote_CalcWeight", conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 15).Value = strCompany;
            //command.Parameters.Add("@Module", SqlDbType.Int, 15).Value = intModule;
            //command.Parameters.Add("@VCBaseLink", SqlDbType.VarChar, 15).Value = strVCBaseLink;
            //command.Parameters.Add("@IntBaseLink", SqlDbType.Int, 15).Value = intIntBaseLink;
            command.Parameters.Add("@Quotenum", SqlDbType.Int, 15).Value = intQuoteNum;
            //command.Parameters.Add("@IntLink1", SqlDbType.Int, 15).Value = intIntLink1;
            //command.Parameters.Add("@IntLink2", SqlDbType.Int, 15).Value = intIntLink2;
            //command.Parameters.Add("@MESNum", SqlDbType.VarChar, 15).Value = strMESNum;
            conn.Open();
            reader = command.ExecuteReader();
            listView1.Items.Clear();
            int intTotalWeight=0;
            while (reader.Read())
            {
                string[] strFrtWeight = new string[3];
                strFrtWeight[0] = reader["BOLClass"].ToString();
                if (reader["BOLDesc"].ToString().Trim().Length == 0)
                {
                    strFrtWeight[1] = ": !!!Missing Data - Please Tell Engineering!!!";
                }
                else
                {
                    strFrtWeight[1] = reader["BOLDesc"].ToString();
                }
                strFrtWeight[2] = Convert.ToInt32(reader["weight"]).ToString();
                intTotalWeight = intTotalWeight + Convert.ToInt32(reader["weight"]);
                ListViewItem itm = new ListViewItem(strFrtWeight);
                listView1.Items.Add(itm);
            }
            string[] strFrtWeight2 = new string[3];
            strFrtWeight2[0] = "";
            strFrtWeight2[1] = "Total for All Classes";
            strFrtWeight2[2] = intTotalWeight.ToString();
            ListViewItem itm2 = new ListViewItem(strFrtWeight2);
            listView1.Items.Add(itm2);
            conn.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (intQuoteNum.ToString().Trim().Length > 0)
            {
                this.Text = "Freight Weight for " + strEditLabel;
                label1.Text = strEditLabel;
                refreshQuoteCalcWeight();
            }
//            else
//            {
//               this.Text = "MES View - Need to select a Job";
//                label1.Text = "MES View - Need to select a Job";
//            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.CheckedItems)
            {
                if (item.SubItems[2].Text.Trim().Length > 0)
                {
                    OpenDoc(item.SubItems[2].Text.Trim());
                    //MessageBox.Show(item.SubItems[2].ToString());
                }
                item.Checked = false;
            }
        }

        private void OpenDoc(string strFile)
        {
            Process myProcess = new Process();

            try
            {
                // Get the path that stores user documents.
                //string myDocumentsPath =
                    Environment.GetFolderPath(Environment.SpecialFolder.Personal);

                myProcess.StartInfo.FileName = strFile;
                //myProcess.StartInfo.Verb = "Print";
                //myProcess.StartInfo.CreateNoWindow = true;
                myProcess.Start();
            }
            catch (Win32Exception e)
            {
                if (e.NativeErrorCode == ERROR_FILE_NOT_FOUND)
                {
                    MessageBox.Show(e.Message + ". Check the path.");
                }

                else if (e.NativeErrorCode == ERROR_ACCESS_DENIED)
                {
                    // Note that if your word processor might generate exceptions
                    // such as this, which are handled first.
                    MessageBox.Show(e.Message +
                        ". You do not have permission to open this file.");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

    }
}
