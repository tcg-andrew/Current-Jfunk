using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using ObjectLibrary;
using System.Collections;
using System.Threading;

namespace CRDOprBatch
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
            
        
        Form2 working;
        bool all;
        bool all_labor;

        private DataSet result;
        private BindingSource source = new BindingSource();

        bool initial;
        public Form1()
        {
            InitializeComponent();
            all = false;
            all_labor = false;
            initial = true;
            working = new Form2();
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
            RefreshProfileList(new object[] { false, null });
            GetActiveLabor(true);
            initial = false;
        }

        private void num_DaysAhead_ValueChanged(object sender, EventArgs e)
        {
            working.Show(this);
            ThreadPool.QueueUserWorkItem(RefreshProfileList, new object[] { true, (ddl_Profile.Items.Count > 0 ? ddl_Profile.SelectedItem.ToString(): null) });
        }

        private void ddl_Profile_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!working.Visible)
                working.Show(this);
            ThreadPool.QueueUserWorkItem(LoadSelectedProfile, new object[] { result, ddl_Profile.SelectedItem.ToString() });
        }

        private void RefreshProfileList(object data)
        {
            SqlCommand sqlCommand = new SqlCommand("exec [dbo].sp_Report_CRDOprBatch_Unstarted @Company, @Start, @End");
            sqlCommand.Parameters.AddWithValue("Company", company);
            sqlCommand.Parameters.AddWithValue("Start", date_Start.Value.Date);
            sqlCommand.Parameters.AddWithValue("End", date_End.Value.Date);


            try
            {
                result = SQLAccess.GetDataSet(server, database, username, password, sqlCommand);
                SetProfileData(result.Tables[0], ((object[])data)[1]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if ((Boolean)((object[])data)[0] == true)
                HideWorking();
        }

        delegate void SetProfileDataCallback(DataTable table, object selected);

        private void SetProfileData(DataTable table, object selected)
        {
            if (this.ddl_Profile.InvokeRequired)
            {
                SetProfileDataCallback d = new SetProfileDataCallback(SetProfileData);
                this.Invoke(d, table, selected);
            }
            else
            {
                ddl_Profile.Items.Clear();
                ddl_Profile.Items.Add("");
                foreach (DataRow row in table.Rows)
                {
                    if (!ddl_Profile.Items.Contains(row[4].ToString()))
                        ddl_Profile.Items.Add(row[4].ToString());
                }
                if (selected == null)
                {
                    if (ddl_Profile.Items.Count > 0)
                        ddl_Profile.SelectedIndex = 0;
                }
                else
                {
                    if (ddl_Profile.Items.IndexOf(selected.ToString()) >= 0)
                        ddl_Profile.SelectedItem = selected.ToString();
                    else
                        ddl_Profile.SelectedIndex = 0;
                }
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
                SqlCommand sqlCommand = new SqlCommand("exec [dbo].sp_Report_CRDOprBatch_Started @Company");
                sqlCommand.Parameters.AddWithValue("Company", company);

                    dgv_Active.DataSource = SQLAccess.GetDataSet(server, database, username, password, sqlCommand).Tables[0];
                dgv_Active.ClearSelection();
                    for (int i = 0; i < dgv_Active.Columns.Count; i++)
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

        delegate void SetMainDataCallback(DataSet data, string selected);

        private void SetMainData(DataSet data, string selected)
        {
            if (this.dgv_Data.InvokeRequired)
            {
                SetMainDataCallback d = new SetMainDataCallback(SetMainData);
                this.Invoke(d, data, selected);
            }
            else
            {
                DataSet r = data.Clone();
                foreach (DataRow row in result.Tables[0].Rows)
                {
                    if (row[4].ToString() == selected)
                        r.Tables[0].Rows.Add(row.ItemArray);
                }
                dgv_Data.DataSource = null;
                dgv_Data.DataSource = r.Tables[0];
                dgv_Data.ClearSelection();
                for (int i = 0; i < dgv_Data.Columns.Count; i++)
                {
                    dgv_Data.Columns[i].ReadOnly = true;
                }
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

        private void LoadSelectedProfile(object data)
        {
            SetMainData(((object[])data)[0] as DataSet, ((object[])data)[1] as string);
            HideWorking();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int d;
            if (txt_EmpID.Text.Length == 0)
                MessageBox.Show("Enter Employee ID before starting batch");
            else
            {
                ThreadPool.QueueUserWorkItem(StartActivity, ddl_Profile.SelectedItem as String);
            }
        }

        bool processingstart = false;
        private void StartActivity(object data)
        {
            EndEdit();
            if (!processingstart)
            {
                working.Show();
                processingstart = true;
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
                                SqlCommand sqlCommand = new SqlCommand("exec [dbo].sp_Report_CRDOprBatch_StartBatch @Company, @Jobnum, @AsmSeq, @OprSeq, @EmpID");
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
                RefreshProfileList(new object[] { false, data });
                LoadSelectedProfile(new object[] { result, data });
                GetActiveLabor(false);
                HideWorking();
                processingstart = false;
            }
        }

        bool processingend = false;
        private void EndActivity(object data)
        {
            EndActivityEdit();
            bool endqty = ((Boolean)data);
            if (!processingend)
            {
                working.Show(this);
                processingend = true;
                LaborInterface li = new LaborInterface();
                try
                {

                    foreach (DataGridViewRow row in dgv_Active.Rows)
                    {
                        if (row.Cells[0].Value != null && (bool)row.Cells[0].Value)
                        {
                            SqlConnection transaction = SQLAccess.BeginTransaction(server, database, username, password);
                            try
                            {

                                string empid = row.Cells[1].Value.ToString();
                                string jobnum = row.Cells[2].Value.ToString();
                                string asmseq = row.Cells[3].Value.ToString();
                                string oprseq = row.Cells[4].Value.ToString();

                                SqlCommand sqlCommand = new SqlCommand("exec [dbo].sp_Report_CRDOprBatch_CompleteBatch @Company, @Jobnum, @AsmSeq, @OprSeq, @EmpID");
                                sqlCommand.Parameters.AddWithValue("Company", company);
                                sqlCommand.Parameters.AddWithValue("Jobnum", jobnum);
                                sqlCommand.Parameters.AddWithValue("AsmSeq", asmseq);
                                sqlCommand.Parameters.AddWithValue("OprSeq", oprseq);
                                sqlCommand.Parameters.AddWithValue("EmpID", empid);

                                DataSet ds = SQLAccess.GetDataSet(server, database, username, password, sqlCommand);
                                string oprqty = ds.Tables[0].Rows[0][0].ToString();

                                li.EndActivity(vantage_server, vantage_database, vantage_user, vantage_pass, empid, jobnum, Int32.Parse(asmseq), Int32.Parse(oprseq), (endqty ? Decimal.Parse(oprqty) : 0), 0, "", "");
                                SQLAccess.CommitTransaction(transaction);
                            }
                            catch (Exception ex)
                            {
                                SQLAccess.RollbackTransaction(transaction);
                                MessageBox.Show(ex.Message);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                RefreshProfileList(new object[] { true, (ddl_Profile.Items.Count > 1 ? ddl_Profile.SelectedItem.ToString() : null) });
                GetActiveLabor(true);
                processingend = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(EndActivity, true);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(EndActivity, false);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            working.Show(this);
            ThreadPool.QueueUserWorkItem(new WaitCallback(CheckAllData));

        }

        delegate void CheckAllDataCallback(object data);

        private void CheckAllData(object data)
        {
            if (this.dgv_Data.InvokeRequired)
            {
                CheckAllDataCallback d = new CheckAllDataCallback(CheckAllData);
                this.Invoke(d, data);
            }
            else
            {
                foreach (DataGridViewRow row in dgv_Data.Rows)
                {
                    row.Cells[0].Value = !all;
                }
                all = !all;
                HideWorking();
            }
        }

        delegate void CheckAllLaborDataCallback(object data);

        private void CheckAllLaborData(object data)
        {
            if (this.dgv_Data.InvokeRequired)
            {
                CheckAllLaborDataCallback d = new CheckAllLaborDataCallback(CheckAllLaborData);
                this.Invoke(d, data);
            }
            else
            {
                foreach (DataGridViewRow row in dgv_Active.Rows)
                {
                    row.Cells[0].Value = !all_labor;
                }
                all_labor = !all_labor;
                HideWorking();
            }
        }

        private void date_Start_ValueChanged(object sender, EventArgs e)
        {

        }

        private void date_End_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            working.Show(this);
            ThreadPool.QueueUserWorkItem(RefreshProfileList, new object[] { true, (ddl_Profile.Items.Count > 1 ? ddl_Profile.SelectedItem.ToString() : null) });
        }

        private void button5_Click(object sender, EventArgs e)
        {
            working.Show(this);
            ThreadPool.QueueUserWorkItem(new WaitCallback(CheckAllLaborData));
        }

        private void dgv_Data_SelectionChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgv_Data.Rows)
                row.Cells[0].Value = false;
            foreach (DataGridViewRow row in dgv_Data.SelectedRows)
                row.Cells[0].Value = true;
        }

        private void dgv_Active_SelectionChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgv_Active.Rows)
                row.Cells[0].Value = false;
            foreach (DataGridViewRow row in dgv_Active.SelectedRows)
                row.Cells[0].Value = true;
        }


    }
}
