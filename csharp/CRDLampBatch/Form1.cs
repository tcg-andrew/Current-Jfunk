using ObjectLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRDLampBatch
{
    public partial class Form1 : Form
    {
        private string company = "CRD";

        string server = "SARV-SQLPROD01";
        string database = "EpicorDB";
        string username = "RailsAppUserP";
        string password = "wA7tA1FaBS1MpLaU";

        private string vantage_server = "SARV-APPEPCRP01";
        private string vantage_database = "EpicorDB";
        private string vantage_user = "CRDService";
        private string vantage_pass = "gfd723trajsdc97";


        /*string server = "SARV-SQLDEV01";
        string database = "EpicorDB_PG";
        string username = "RailsAppUser";
        string password = "2fe8wJcH";
             
        private string vantage_server = "SARV-APPEPCRD01";
        private string vantage_database = "EpicorDB_PG";
        private string vantage_user = "CRDService";
        private string vantage_pass = "gfd723trajsdc97";*/

        private DataSet result;
        private BindingSource source = new BindingSource();
        bool initial;

        Form2 working;

        public Form1()
        {
            InitializeComponent();
            working = new Form2();
            initial = true;
            date_Start.Value = DateTime.Now.Date;
            date_End.Value = DateTime.Now.AddDays(1).Date;
            dgv_Data.DataSource = source;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            working.Show(this);
            ThreadPool.QueueUserWorkItem(InitialLoad);
        }

        private void InitialLoad(object data)
        {
            GetActiveLabor(true);
            initial = false;
        }

        delegate void SetMainDataCallback(object data);

        private void SetMainData(object data)
        {
            if (this.dgv_Data.InvokeRequired)
            {
                SetMainDataCallback d = new SetMainDataCallback(SetMainData);
                this.Invoke(d, data);
            }
            else
            {
                SqlCommand sqlCommand = new SqlCommand("exec [dbo].sp_Report_LampBatch_Unstarted @Company, @Start, @End");
                sqlCommand.Parameters.AddWithValue("Company", company);
                sqlCommand.Parameters.AddWithValue("Start", date_Start.Value.Date);
                sqlCommand.Parameters.AddWithValue("End", date_End.Value.Date);

                DataSet r = SQLAccess.GetDataSet(server, database, username, password, sqlCommand);
                dgv_Data.DataSource = null;
                dgv_Data.DataSource = r.Tables[0];
                for (int i = 1; i < dgv_Data.Columns.Count; i++)
                {
                    dgv_Data.Columns[i].ReadOnly = true;
                }
                HideWorking();
            }
        }

        delegate void GetActiveLaborCallback(object data);

        private void GetActiveLabor(object data)
        {
            if (this.dgv_Active.InvokeRequired)
            {
                GetActiveLaborCallback d = new GetActiveLaborCallback(GetActiveLabor);
                this.Invoke(d, data);
            }
            else
            {
                SqlCommand sqlCommand = new SqlCommand("exec [dbo].sp_Report_CRDLamp_Started @Company");
                sqlCommand.Parameters.AddWithValue("Company", company);

                dgv_Active.DataSource = SQLAccess.GetDataSet(server, database, username, password, sqlCommand).Tables[0];
                for (int i = 1; i < dgv_Active.Columns.Count; i++)
                {
                    dgv_Active.Columns[i].ReadOnly = true;
                }

                if ((Boolean)data == true)
                    HideWorking();
            }
        }

        delegate void HideWorkingCallback();

        private void HideWorking()
        {
            if (this.working.InvokeRequired)
            {
                HideWorkingCallback d = new HideWorkingCallback(HideWorking);
                this.Invoke(d);
            }
            else
            {
                working.Hide();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            working.Show(this);
            ThreadPool.QueueUserWorkItem(SetMainData);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int d;
            if (txt_EmpID.Text.Length == 0)
                MessageBox.Show("Enter Employee ID before starting batch");
            else
            {
                working.Show();
                ThreadPool.QueueUserWorkItem(StartActivity);
            }
        }

        private void StartActivity(object data)
        {
            EndEdit();
            LaborInterface li = new LaborInterface();
            foreach (DataGridViewRow row in dgv_Data.Rows)
            {
                if (row.Cells[0].Value != null && (bool)row.Cells[0].Value)
                {
                    string jobnum = row.Cells[2].Value.ToString();
                    string asm = row.Cells[3].Value.ToString();
                    string opr = row.Cells[4].Value.ToString();
                    try
                    {
                        SqlConnection transaction = SQLAccess.BeginTransaction(server, database, username, password);
                        try
                        {
                            SqlCommand sqlCommand = new SqlCommand("exec [dbo].sp_Report_CRDLamp_StartBatch @Company, @Jobnum, @AsmSeq, @OprSeq, @EmpID");
                            sqlCommand.Parameters.AddWithValue("Company", company);
                            sqlCommand.Parameters.AddWithValue("Jobnum", jobnum);
                            sqlCommand.Parameters.AddWithValue("AsmSeq", Int32.Parse(asm));
                            sqlCommand.Parameters.AddWithValue("OprSeq", Int32.Parse(opr));
                            sqlCommand.Parameters.AddWithValue("EmpID", txt_EmpID.Text);
                            SQLAccess.NonQuery(sqlCommand, transaction);

                            if (!li.EmployeeHasActiveSequencyLabor(vantage_server, vantage_database, vantage_user, vantage_pass, txt_EmpID.Text, jobnum, asm, opr))
                                li.StartActivity(vantage_server, vantage_database, vantage_user, vantage_pass, txt_EmpID.Text, jobnum, Int32.Parse(asm), Int32.Parse(opr));
                            SQLAccess.CommitTransaction(transaction);
                        }
                        catch (Exception ex)
                        {
                            SQLAccess.RollbackTransaction(transaction);
                            MessageBox.Show(ex.Message);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            ClearEmpID();
            SetMainData(data);
            GetActiveLabor(false);
            HideWorking();
        }

        delegate void ClearEmpIDCallback();

        private void ClearEmpID()
        {
            if (this.txt_EmpID.InvokeRequired)
            {
                ClearEmpIDCallback d = new ClearEmpIDCallback(ClearEmpID);
                this.Invoke(d);
            }
            else
            {
                txt_EmpID.Text = "";
            }
        }

        delegate void EndEditCallback();

        private void EndEdit()
        {
            if (this.dgv_Data.InvokeRequired)
            {
                EndEditCallback d = new EndEditCallback(EndEdit);
                this.Invoke(d);
            }
            else
            {
                dgv_Data.EndEdit();
            }
        }

        delegate void EndActivityEditCallback();

        private void EndActivityEdit()
        {
            if (this.dgv_Active.InvokeRequired)
            {
                EndActivityEditCallback d = new EndActivityEditCallback(EndActivityEdit);
                this.Invoke(d);
            }
            else
            {
                dgv_Active.EndEdit();
            }
        }

        private void EndActivity(object data)
        {
            EndActivityEdit();
            LaborInterface li = new LaborInterface();
            try
            {

                foreach (DataGridViewRow row in dgv_Active.Rows)
                {
                    if (row.Cells[0].Value != null && (bool)row.Cells[0].Value)
                    {
                        string empid = row.Cells[1].Value.ToString();
                        string jobnum = row.Cells[2].Value.ToString();
                        string asmseq = row.Cells[3].Value.ToString();
                        string oprseq = row.Cells[4].Value.ToString();

                        SqlCommand sqlCommand = new SqlCommand("exec [dbo].sp_Report_CRDLamp_CompleteBatch @Company, @Jobnum, @AsmSeq, @OprSeq, @EmpID");
                        sqlCommand.Parameters.AddWithValue("Company", company);
                        sqlCommand.Parameters.AddWithValue("Jobnum", jobnum);
                        sqlCommand.Parameters.AddWithValue("AsmSeq", asmseq);
                        sqlCommand.Parameters.AddWithValue("OprSeq", oprseq);
                        sqlCommand.Parameters.AddWithValue("EmpID", empid);

                        DataSet ds = SQLAccess.GetDataSet(server, database, username, password, sqlCommand);
                        string oprqty = ds.Tables[0].Rows[0][0].ToString();

                        li.EndActivity(vantage_server, vantage_database, vantage_user, vantage_pass, empid, jobnum, Int32.Parse(asmseq), Int32.Parse(oprseq), Decimal.Parse(oprqty), 0, "", "");

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            GetActiveLabor(true);
        }


        private void button2_Click(object sender, EventArgs e)
        {
            working.Show(this);
            ThreadPool.QueueUserWorkItem(EndActivity);
        }
    }
}
