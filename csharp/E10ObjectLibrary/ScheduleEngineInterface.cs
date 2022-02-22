using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Erp.Proxy.BO;
using Erp.BO;
using Ice.Core;
using Ice.Lib.Framework;
using Erp.Contracts;

namespace ObjectLibrary
{
    public class ScheduleEngineInterface : EpicorExtension<ScheduleEngineImpl, ScheduleEngineSvcContract>
    {
        #region Update Methods

        public string UpdateJobDate(string server, string port, string username, string password, string jobnum, DateTime date)
        {
            string result = "";
            try
            {
                OpenSession(server, port, username, password, Session.LicenseType.Default);
                JobEntryImpl je = WCFServiceSupport.CreateImpl<JobEntryImpl>(objSess, Erp.Proxy.BO.CustomerImpl.UriPath);
                JobEntryDataSet ds = je.GetByID(jobnum);

                JobEntryDataSet.JobHeadRow job = ds.JobHead.Rows[0] as JobEntryDataSet.JobHeadRow;
                bool finished;
                string log;

                ScheduleEngineDataSet sds = new ScheduleEngineDataSet();
                ScheduleEngineDataSet.ScheduleEngineRow row1 = sds.ScheduleEngine.NewScheduleEngineRow();
                row1.Company = job.Company;
                row1.JobNum = job.JobNum;
                row1.AssemblySeq = 0;
                row1.OprSeq = 0;
                row1.OpDtlSeq = 0;
                row1.StartDate = date;
                row1.StartTime = 0;
                row1.EndDate = job.ReqDueDate;
                row1.EndTime = 0;
                row1.WhatIf = false;
                row1.Finite = false;
                row1.SchedTypeCode = "ja";
                row1.ScheduleDirection = "Start";
                row1.SetupComplete = false;
                row1.ProductionComplete = false;
                row1.OverrideMtlCon = true;
                row1.RecalcExpProdYld = false;
                sds.ScheduleEngine.Rows.Add(row1);
                sds.AcceptChanges();
                BusinessObject.MoveJobItem(sds, out finished, out log);
                result += log;

                sds = new ScheduleEngineDataSet();
                ScheduleEngineDataSet.ScheduleEngineRow row2 = sds.ScheduleEngine.NewScheduleEngineRow();
                row2.Company = job.Company;
                row2.JobNum = job.JobNum;
                row2.AssemblySeq = 0;
                row2.OprSeq = 0;
                row2.OpDtlSeq = 0;
                row2.StartDate = date;
                row2.StartTime = 0;
                row2.EndDate = job.ReqDueDate;
                row2.EndTime = 0;
                row2.WhatIf = false;
                row2.Finite = false;
                row2.SchedTypeCode = "ja";
                row2.ScheduleDirection = "End";
                row2.SetupComplete = false;
                row2.ProductionComplete = false;
                row2.OverrideMtlCon = true;
                row2.RecalcExpProdYld = false;
                sds.ScheduleEngine.Rows.Add(row2);
                sds.AcceptChanges();
                BusinessObject.MoveJobItem(sds, out finished, out log);
                result += log;

            }
            catch (Exception ex)
            {
                throw new Exception("Exception in UpdateJobDate: " + ex);
            }
            finally
            {
                CloseSession();
            }
            return result;
        }

        #endregion
    }
}
