#region Usings

using System;
using System.Data;
using Epicor.Mfg.BO;
using Epicor.Mfg.Core;
using ModuleBase;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

#endregion

namespace OrderJobDateMismatch
{
    class TaskModule : ModuleBase.Module
    {
        public TaskModule(Session session, string server, string database)
            : base(session, server, database)
        {
            _actions.Add(ModuleAction.Favorite);
            _actions.Add(ModuleAction.Refresh);
            _actions.Add(ModuleAction.Save);

            {
                GridColumn gc = new GridColumn("Order Shop Cap");
                gc.ReadOnly = false;
                _columns.Add(gc);
            }
            {
                GridColumn gc = new GridColumn("Job Req Due Date");
                gc.ReadOnly = false;
                _columns.Add(gc);
            }
            _columns.Add(new GridColumn("Order #"));
            _columns.Add(new GridColumn("Order Line #"));
            _columns.Add(new GridColumn("Order Release #"));
            _columns.Add(new GridColumn("Job #"));
        }

        protected override System.Data.DataTable DataMethod(Dictionary<string, object> args)
        {
            try
            {
                #region Get Data Set

                DynamicQuery dqBusObj = new DynamicQuery(Session.ConnectionPool);
                string jobName = Session.CompanyID + "-OTFJobEditDates";
                try
                {
                    dqBusObj.DeleteByID(jobName);
                    dqBusObj.GetQueryByID(jobName);
                }
                catch (System.Exception ex)
                {
                    if (ex.Message == "The Query does not exist.")
                        BuildJobMismatchedOrderDatesQuery(dqBusObj, jobName);
                }
                DataSet ds = dqBusObj.ExecuteByID(jobName);
                dqBusObj.DeleteByID(jobName);

                #endregion

                System.Data.DataTable dt = GenerateGridTable();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DataRow newRow = dt.NewRow();
                    newRow["Order Shop Cap"] = DateTime.Parse(row[0].ToString()).ToShortDateString();
                    newRow["Job Req Due Date"] = DateTime.Parse(row[1].ToString()).ToShortDateString();
                    newRow["Order #"] = row[2];
                    newRow["Order Line #"] = row[3];
                    newRow["Order Release #"] = row[4];
                    newRow["Job #"] = row[5];
                    dt.Rows.Add(newRow);
                }

                return dt;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        protected override bool SaveMethod()
        {
            try
            {
                lock (_data)
                {
                    JobEntry jeBusObj = new JobEntry(Session.ConnectionPool);
                    SalesOrder soBusObj = new SalesOrder(Session.ConnectionPool);

                    foreach (DataRow row in _data.Rows)
                    {
                        JobEntryDataSet jeds = jeBusObj.GetByID(row[5].ToString());
                        SalesOrderDataSet sods = soBusObj.GetByID(Int32.Parse(row[2].ToString()));

                        foreach (SalesOrderDataSet.OrderRelRow rel in sods.OrderRel.Rows)
                        {
                            if (rel.OrderLine == Int32.Parse(row[3].ToString()) && rel.OrderRelNum == Int32.Parse(row[4].ToString()))
                                rel.ReqDate = DateTime.Parse(row[0].ToString());
                        }

                        jeds.JobHead[0].ReqDueDate = DateTime.Parse(row[1].ToString());
                        jeds.JobHead[0].StartDate = DateTime.Parse(row[1].ToString());
                        jeds.JobHead[0].DueDate = DateTime.Parse(row[1].ToString());

                        jeBusObj.Update(jeds);
                        soBusObj.Update(sods);
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return true;
        }

        private void BuildJobMismatchedOrderDatesQuery(DynamicQuery dqBusObj, string jobName)
        {
            QueryDesignDataSet qds = new QueryDesignDataSet();
            dqBusObj.GetNewQuery(qds);
            qds.DynamicQuery[0].Description = "Jobs with mismatch dates";
            qds.DynamicQuery[0].ExportID = jobName;
            qds.DynamicQuery[0].IsShared = true;
            qds.DynamicQuery[0].GlobalQuery = true;
            dqBusObj.Update(qds);
            dqBusObj.AddQueryTable(qds, jobName, "JobProd", false);
            dqBusObj.Update(qds);
            dqBusObj.AddQueryTable(qds, jobName, "JobHead", false);
            dqBusObj.Update(qds);
            dqBusObj.AddQueryRelation(qds, jobName, "JobProdJobHead", "JobProd", "JobNum", "JobHead", "each", false);
            dqBusObj.Update(qds);
            dqBusObj.AddQueryTable(qds, jobName, "OrderRel", false);
            dqBusObj.Update(qds);
            dqBusObj.AddQueryRelation(qds, jobName, "JobProdOrderRel", "JobProd", "OrderLineRel", "OrderRel", "each", false);
            dqBusObj.Update(qds);
            dqBusObj.GetNewWhereItem(qds, jobName, "JobHead");
            qds.QueryWhereItem[0].FieldName = "JobComplete";
            qds.QueryWhereItem[0].CompOp = "=";
            qds.QueryWhereItem[0].RValueNumber = "false";
            dqBusObj.Update(qds);
            dqBusObj.GetNewWhereItem(qds, jobName, "OrderRel");
            qds.QueryWhereItem[1].FieldName = "ReqDate";
            qds.QueryWhereItem[1].CompOp = "<>";
            qds.QueryWhereItem[1].IsConst = false;
            qds.QueryWhereItem[1].ToDataTableID = "JobHead";
            qds.QueryWhereItem[1].ToFieldName = "ReqDueDate";
            dqBusObj.Update(qds);
            dqBusObj.AddQueryField(qds, jobName, "OrderRel", "ReqDate");
            dqBusObj.Update(qds);
            dqBusObj.AddQueryField(qds, jobName, "JobHead", "ReqDueDate");
            dqBusObj.Update(qds);
            dqBusObj.AddQueryField(qds, jobName, "OrderRel", "OrderNum");
            dqBusObj.Update(qds);
            dqBusObj.AddQueryField(qds, jobName, "OrderRel", "OrderLine");
            dqBusObj.Update(qds);
            dqBusObj.AddQueryField(qds, jobName, "OrderRel", "OrderRelNum");
            dqBusObj.Update(qds);
            dqBusObj.AddQueryField(qds, jobName, "JobProd", "JobNum");
            dqBusObj.Update(qds);
            qds.SelectedField[0].FieldLabel = "Order Shop Cap";
            qds.SelectedField[1].FieldLabel = "Job Req Due Date";
            qds.SelectedField[2].FieldLabel = "Order #";
            qds.SelectedField[3].FieldLabel = "Line #";
            qds.SelectedField[4].FieldLabel = "Rel #";
            qds.SelectedField[5].FieldLabel = "Job #";
            dqBusObj.Update(qds);
            bool outbool;
            dqBusObj.SaveQuery(jobName, out outbool);
        }
    }
}
