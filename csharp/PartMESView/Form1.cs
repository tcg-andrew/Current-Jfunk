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



namespace MESView
{
    public partial class Form1 : Form
    {
        string server = "SARV-SQLPROD01";
        string database = "EpicorDB";


        const int ERROR_FILE_NOT_FOUND = 2;
        const int ERROR_ACCESS_DENIED = 5;

        SqlConnection conn;
        SqlCommand command;
        SqlDataReader reader;
        string strCompany;
        string strVCBaseLink;
        string strEditLabel;

        public Form1(string[] args)
        {
            InitializeComponent();
            strCompany = args[0];
            strVCBaseLink = args[1];
            strEditLabel = args[2];
        }


        private void refreshShopViewAllMES()
        {
            conn = new SqlConnection("Data Source="+server+";Database="+database+";Integrated Security=SSPI");

            command = new SqlCommand("[dbo].sp_UD03_PartViewAllMES", conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 15).Value = strCompany;
            command.Parameters.Add("@VCBaseLink", SqlDbType.VarChar, 50).Value = strVCBaseLink;
            conn.Open();
            reader = command.ExecuteReader();
            listView1.Items.Clear();
            while (reader.Read())
            {
                string[] strMES = new string[3];
                strMES[0] = reader["mesnum"].ToString();
                if (reader["MESDescription"].ToString().Trim().Length == 0)
                {
                    strMES[1] = ": !!!Missing Data - Please Tell Engineering!!!";
                }
                else
                {
                    strMES[1] = reader["MESDescription"].ToString();
                }
                strMES[2] = reader["MESLink"].ToString();
                ListViewItem itm = new ListViewItem(strMES);
                listView1.Items.Add(itm);
            }
            conn.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (strVCBaseLink.Trim().Length > 0)
            {
                this.Text = "MES View for " + strEditLabel;
                label1.Text = strEditLabel;
                refreshShopViewAllMES();
            }
            else
            {
                this.Text = "MES View - Need to select a Part";
                label1.Text = "MES View - Need to select a Part";
            }
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
