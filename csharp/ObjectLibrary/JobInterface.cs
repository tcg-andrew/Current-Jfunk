using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epicor.Mfg.BO;
using System.Data.SqlClient;
using System.Data;
using Epicor.Mfg.Core;

namespace ObjectLibrary
{
    #region Class Job

    public class Job
    {
        #region Properties

        public string JobNum { get; set; }
        public string OrderNum { get; set; }
        public int OrderLine { get; set; }
        public int OrderRel { get; set; }
        public DateTime ReqDate { get; set; }
        public DateTime OrderDate { get; set; }

        #endregion

        #region Constructors

        public Job()
        {
            JobNum = "";
            OrderNum = "";
            OrderLine = 0;
            OrderRel = 0;
            ReqDate = DateTime.Now;
            OrderDate = DateTime.Now;
        }

        public Job(string jobnum, string ordernum, int orderline, int orderrel, DateTime reqdate, DateTime orderdate)
        {
            JobNum = jobnum;
            OrderNum = ordernum;
            OrderLine = orderline;
            OrderRel = orderrel;
            ReqDate = reqdate;
            OrderDate = orderdate;
        }

        #endregion
    }

    #endregion

    public class JobInterface: EpicorExtension<JobEntry>
    {
        #region public methods

        #region Retrieve Methods

        public List<Job> GetJobsWithMismatchedDates(string server, string database, string username, string password)
        {
            List<Job> result = new List<Job>();
            SqlCommand sqlCommand = new SqlCommand("exec [dbo].sp_GetJobsWithMismatchedDates");

            try
            {
                DataSet ds = SQLAccess.GetDataSet(server, database, username, password, sqlCommand);

                foreach (DataRow row in ds.Tables[0].Rows)
                    result.Add(new Job(row["Job #"].ToString(), row["Order #"].ToString(), Int32.Parse(row["Order Line #"].ToString()), Int32.Parse(row["Order Release #"].ToString()), DateTime.Parse(row["Job Date"].ToString()), DateTime.Parse(row["Order Date"].ToString())));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public DataSet GetJobsWithMismatchedMaterialDates(string server, string database, string username, string password)
        {
            DataSet result = new DataSet();
            SqlCommand sqlCommand = new SqlCommand("exec [dbo].sp_GetJobMtls_WrongDate");

            try
            {
                result = SQLAccess.GetDataSet(server, database, username, password, sqlCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public void OpenSession(string server, string port, string username, string password)
        {
            OpenSession(server, port, username, password, Session.LicenseType.Default);
        }

        public void Close()
        {
            CloseSession();
        }

        public decimal GetOutstandingQty(string jobnum, int assembly, string opr)
        {
            decimal result = 0;

            try
            {
                JobEntryDataSet ds = BusinessObject.GetByID(jobnum);
                foreach (JobEntryDataSet.JobOpDtlRow row in ds.JobOpDtl.Rows)
                {
                    if (row.AssemblySeq == assembly && row.OpDtlDesc == opr)
                    {
                        result = row.JobOperRowParent.RunQty - row.JobOperRowParent.QtyCompleted;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public Dictionary<string, int> GetJobOperations(string jobnum, int asmseq)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();

            try
            {
                JobEntryDataSet ds = BusinessObject.GetByID(jobnum);
                foreach (JobEntryDataSet.JobOpDtlRow row in ds.JobOpDtl.Rows)
                {
                    if (row.AssemblySeq == asmseq)
                        result.Add(row.OpDtlDesc, row.OprSeq);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        #endregion

        #region Update Methods

        public void UpdateJobDate(string server, string port, string username, string password, Job job)
        {
            try
            {
                OpenSession(server, port, username, password, Session.LicenseType.Default);
                JobEntryDataSet ds = BusinessObject.GetByID(job.JobNum);

                bool released = ds.JobHead[0].JobReleased;
                bool engineered = ds.JobHead[0].JobEngineered;

                if (released)
                    ds.JobHead[0].JobReleased = false;
                if (engineered)
                    ds.JobHead[0].JobEngineered = false;

                BusinessObject.Update(ds);

                OrderInterface orderInterface = new OrderInterface();
                orderInterface.UpdateOrderReqDate(server, port, username, password, job);

                ds.JobHead[0].WIStartDate = job.ReqDate;
                ds.JobHead[0].WIDueDate = job.ReqDate;
                ds.JobHead[0].DueDate = job.ReqDate;
                ds.JobHead[0].ReqDueDate = job.ReqDate;
                ds.JobHead[0].StartDate = job.ReqDate;

                if (engineered)
                    ds.JobHead[0].JobEngineered = true;
                if (released)
                    ds.JobHead[0].JobReleased = true;

                BusinessObject.Update(ds);
                CloseSession();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #endregion
    }
}