using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace VantagePoints1Emp
{
    public partial class form1 : Form
    {
        string server = "SARV-SQLPROD01";
        string database = "EpicorDB";

        string strCompany;
        string strEmpID;
        public form1(string[] args)
            
        {
            InitializeComponent();
            strCompany = args[0];
            strEmpID = args[1];
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            SqlConnection conn = new SqlConnection("Data Source=" + server + ";Database=" + database + ";Integrated Security=SSPI");
            SqlCommand command = new SqlCommand("[dbo].sp_UD08_TotPoints1Emp", conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@Company", SqlDbType.VarChar, 15).Value = strCompany;
            command.Parameters.Add("@EmpID", SqlDbType.VarChar, 8).Value = strEmpID;
            conn.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                textBox1.Text = reader["EmpID"].ToString();
                textBox2.Text = reader["firstname"].ToString();
                textBox3.Text = reader["lastname"].ToString();
                textBox4.Text = String.Format("{0:0.#}",reader["TotPoints"]);
            }
            conn.Close();


        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}