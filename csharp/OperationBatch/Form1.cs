using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ObjectLibrary;
using System.Threading;

namespace OperationBatch
{
    public partial class Form1 : Form
    {
        delegate void controlMethod();

        LaborInterface labor;
        JobInterface job;
        string VantageServer = "SARV-APPEPCRP01";
        string database = "EpicorDB";
        string VantageUser = "CRDService";
        string VantagePass = "gfd723trajsdc97";

        public string EmployeeID { get { return tb_EmployeeID.Text; } }
        public string OprCode { get; set; }
        public int Processing { get; set; }
        public string JobNum { get { return tb_JobNum.Text; } }
        public int AsmSeq { get { return String.IsNullOrEmpty(tb_AsmSeq.Text) ? -1 : Int32.Parse(tb_AsmSeq.Text); } }

        public Form1()
        {
            InitializeComponent();
            labor = new LaborInterface();
            job = new JobInterface();
            job.OpenSession(VantageServer, database, VantageUser, VantagePass);
        }

        private void ThreadSafeModify(Control control, controlMethod method)
        {
            if (control.InvokeRequired)
                control.Invoke(new MethodInvoker(method));
            else
                method();
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(JobNum) && AsmSeq != -1)
                {
                    string[] args = new[] { JobNum, AsmSeq.ToString() };
                    ThreadPool.QueueUserWorkItem(new WaitCallback(AddToBatch), args);
                    Processing++;
                    btn_Process.Text = "Adding " + Processing.ToString() + "...";
                    btn_Process.Enabled = false;
                    tb_JobNum.Text = "";
                    tb_AsmSeq.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AddToBatch(object param)
        {
            string[] args = param as string[];
            string jobnum = args[0];
            int asmseq = Int32.Parse(args[1]);

            bool found = false;

            foreach (DataGridViewRow row in dgv_JobBatch.Rows)
            {
                if (row.Cells[0].Value.ToString() == jobnum && row.Cells[1].Value.ToString() == asmseq.ToString())
                    found = true;
            }

            try
            {
                if (!found)
                {
                    decimal qty = job.GetOutstandingQty(jobnum, asmseq, OprCode);

                    ThreadSafeModify(dgv_JobBatch, delegate { dgv_JobBatch.Rows.Add(jobnum, asmseq, qty.ToString()); });
                }
                Processing--;
                if (Processing == 0)
                    ThreadSafeModify(btn_Process, delegate { btn_Process.Text = "Process"; btn_Process.Enabled = true; });
                else
                    ThreadSafeModify(btn_Process, delegate { btn_Process.Text = "Adding " + Processing.ToString() + "..."; btn_Process.Enabled = false; });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_Process_Click(object sender, EventArgs e)
        {
            dgv_JobBatch.EndEdit();
            btn_Process.Text = "Processing...";
            btn_Process.Enabled = false;
            dgv_JobBatch.Enabled = false;
            try
            {
                List<DataGridViewRow> processed = new List<DataGridViewRow>();
                foreach (DataGridViewRow row in dgv_JobBatch.Rows)
                {
                    string jobnum = row.Cells[0].Value.ToString();
                    int asmseq = Int32.Parse(row.Cells[1].Value.ToString());
                    decimal qty = Decimal.Parse(row.Cells[2].Value.ToString());
                    try
                    {
                        Dictionary<string, int> jobops = job.GetJobOperations(jobnum, asmseq);

                        if (jobops.ContainsKey(OprCode))
                        {
                            labor.StartActivity(VantageServer, database, VantageUser, VantagePass, EmployeeID, jobnum, asmseq, jobops[OprCode]);
                            labor.EndActivity(VantageServer, database, VantageUser, VantagePass, EmployeeID, jobnum, asmseq, jobops[OprCode], qty, 0, "", "");
                            processed.Add(row);
                        }
                        else
                            throw new Exception("Job #" + jobnum + ", AsmSeq " + asmseq.ToString() + " does not contain " + OprCode + " Operation");

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Job # " + jobnum + ", AsmSeq " + asmseq.ToString() + " - " + ex.Message);
                    }
                }
                foreach (DataGridViewRow row in processed)
                    dgv_JobBatch.Rows.Remove(row);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            btn_Process.Text = "Process";
            btn_Process.Enabled = true;
            dgv_JobBatch.Enabled = true;
        }

        private void dd_Operation_SelectedIndexChanged(object sender, EventArgs e)
        {
            OprCode = dd_Operation.SelectedItem.ToString();
            dgv_JobBatch.Rows.Clear();
            tb_JobNum.Focus();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            job.Close();
        }

        private void tb_JobNum_TextChanged(object sender, EventArgs e)
        {
            string[] split = JobNum.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            if (split.Count() == 4)
            {
                string[] args = new[] { split[0] + '-' + split[1] + '-' + split[2], split[3].ToString() };
                ThreadPool.QueueUserWorkItem(new WaitCallback(AddToBatch), args);
                tb_JobNum.Text = "";
                tb_AsmSeq.Text = "";
                Processing++;
                btn_Process.Text = "Adding " + Processing.ToString() + "...";
                btn_Process.Enabled = false;
            }
        }
    }
}
