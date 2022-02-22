#region Usings

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Epicor.Mfg.Core;
using ModuleBase;

#endregion

namespace JobMtlHighIssuedQty
{
    class TaskModule : Module, IDateFilter, IDropDownFilter
    {
        public Dictionary<string, Dictionary<string, string>> _filters;

        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public TaskModule(Session session, string server, string database)
            : base(session, server, database)
        {
            _actions.Add(ModuleAction.Favorite);
            _actions.Add(ModuleAction.Refresh);

            _columns.Add(new GridColumn("Job Status"));
            _columns.Add(new GridColumn("Created Date"));
            _columns.Add(new GridColumn("Req Due Date"));
            _columns.Add(new GridColumn("Plant"));
            _columns.Add(new GridColumn("Job #"));
            _columns.Add(new GridColumn("Job Part #"));
            _columns.Add(new GridColumn("Assembly"));
            _columns.Add(new GridColumn("Operation"));
            _columns.Add(new GridColumn("Mtl Part #"));
            _columns.Add(new GridColumn("Req Qty", typeof(Decimal)));
            _columns.Add(new GridColumn("Issued Qty", typeof(Decimal)));

            {
                GridColumn gc = new GridColumn("Part Class");
          //      gc.Visible = false;
                _columns.Add(gc);
            }

            From = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            To = DateTime.Now;

            _filters = new Dictionary<string, Dictionary<string, string>>();

            _filters.Add("Plant", GetPlants());
            _filters.Add("Part Class", GetPartClasses());
            SelectedFilter = new Dictionary<string, string>();
            SelectedFilter.Add("Plant", Session.PlantID);
            SelectedFilter.Add("Part Class", "All");

        }

        protected override System.Data.DataTable DataMethod(Dictionary<string, object> args)
        {
            try
            {
                #region Get Data Set

                SqlCommand command = new SqlCommand("exec dbo.sp_GetJobMtlsWithIssuedQtyGTReqQty2 @Company, @FromDate, @ToDate");
                command.Parameters.AddWithValue("Company", Session.CompanyID);
                command.Parameters.AddWithValue("FromDate", From);
                command.Parameters.AddWithValue("ToDate", To.AddDays(1).AddSeconds(-1));

                DataSet ds = SQLAccess.SQLAccess.GetDataSetSSPI(Server, Database, command);

                #endregion

                System.Data.DataTable dt = GenerateGridTable();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DataRow newRow = dt.NewRow();
                    newRow["Job Status"] = row[0].ToString() == "0" ? "Open" : "Closed";
                    newRow["Created Date"] = row[1];
                    newRow["Req Due Date"] = row[2];
                    newRow["Plant"] = row[3];
                    newRow["Job #"] = row[4];
                    newRow["Job Part #"] = row[5];
                    newRow["Assembly"] = row[6];
                    newRow["Operation"] = row[7];
                    newRow["Mtl Part #"] = row[8];
                    newRow["Req Qty"] = Decimal.Parse(row[9].ToString());
                    newRow["Issued Qty"] = Decimal.Parse(row[10].ToString());
                    newRow["Part Class"] = row[11];
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
            throw new NotImplementedException();
        }

        public Dictionary<string, Dictionary<string, string>> Filters
        {
            get 
            {
                if (_filters == null)
                {
                    _filters = new Dictionary<string, Dictionary<string, string>>();

                    _filters.Add("Plant", GetPlants());
                    _filters.Add("Part Class", GetPartClasses());
                    SelectedFilter = new Dictionary<string, string>();
                    SelectedFilter.Add("Plant", Session.PlantID);
                    SelectedFilter.Add("Part Class", "All");
                }
                return _filters;
            }
        }

        public Dictionary<string, string> SelectedFilter { get; set; }

        private Dictionary<string, string> GetPlants()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            SqlCommand sqlCommand = new SqlCommand("exec [dbo].sp_GetPlantsForCompany @Company");
            sqlCommand.Parameters.AddWithValue("Company", Session.CompanyID);

            try
            {
                DataSet ds = SQLAccess.SQLAccess.GetDataSetSSPI(Server, Database, sqlCommand);

                foreach (DataRow row in ds.Tables[0].Rows)
                    result.Add(row[0].ToString(), row[0].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        private Dictionary<string, string> GetPartClasses()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            SqlCommand sqlCommand = new SqlCommand("exec [dbo].sp_GetPartClassesForCompany @Company");
            sqlCommand.Parameters.AddWithValue("Company", Session.CompanyID);

            try
            {
                DataSet ds = SQLAccess.SQLAccess.GetDataSetSSPI(Server, Database, sqlCommand);

                result.Add("All", "%");
                foreach (DataRow row in ds.Tables[0].Rows)
                    result.Add(row[1].ToString().Trim(), row[0].ToString().Trim());
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
    }
}
