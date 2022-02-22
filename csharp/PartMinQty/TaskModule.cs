using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ModuleBase;
using Epicor.Mfg.Core;

namespace PartMinQty
{
    class TaskModule: ModuleBase.Module, IDropDownFilter
    {
        public Dictionary<string, Dictionary<string, string>> _filters;

        public TaskModule(Session session, string server, string database)
            : base(session, server, database)
        {
            _actions.Add(ModuleAction.Favorite);
            _actions.Add(ModuleAction.Refresh);
            //_actions.Add(ModuleAction.Save);

            {
                GridColumn gc = new GridColumn("Part");
                gc.Frozen = true;
                Columns.Add(gc);
            }
            Columns.Add(new GridColumn("Description"));
            Columns.Add(new GridColumn("Plant"));
            {
                GridColumn gc = new GridColumn("Avg Cost", typeof(Decimal));
                gc.Visible = false;
                Columns.Add(gc);
            }
            Columns.Add(new GridColumn("Min Qty", typeof(Decimal)));
            {
                GridColumn gc = new GridColumn("Difference", typeof(Decimal));
                gc.Format = "0%";
                Columns.Add(gc);
            }
            Columns.Add(new GridColumn("Monthly Usage", typeof(Decimal)));
            {
                GridColumn gc = new GridColumn("Months To Stock", typeof(Int32));
                gc.ReadOnly = false;
                Columns.Add(gc);
            }
            {
                GridColumn gc = new GridColumn("New Minimum", typeof(Decimal));
                gc.ReadOnly = false;
                //gc.UpdateTriggerColumns = new List<string>() { "Months To Stock" };
                //gc.Function = new DataFunction("Months To Stock", "Monthly Usage");
                Columns.Add(gc);
            }
            {
                GridColumn gc = new GridColumn("Total Cost", typeof(Decimal));
                gc.Format = "c";
                //gc.UpdateTriggerColumns = new List<string>() { "Months To Stock", "New Minimum" };
                //gc.Function = new DataFunction("Avg Cost", "New Minimum");
                Columns.Add(gc);
            }
            {
                GridColumn gc = new GridColumn("Process");
                gc.ReadOnly = false;
                //gc.ValidValues.Add("Yes");
                //gc.ValidValues.Add("No");
                Columns.Add(gc);
            }

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

                SqlCommand command = new SqlCommand("exec sp_PartTran_Find_Min_To_Change @Company");
                command.Parameters.AddWithValue("Company", Session.CompanyID);
                DataSet ds = SQLAccess.SQLAccess.GetDataSetSSPI(Server, Database, command);

                #endregion

                System.Data.DataTable dt = GenerateGridTable();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DataRow newRow = dt.NewRow();
                    newRow["Part"] = row[0];
                    newRow["Description"] = row[1];
                    newRow["Plant"] = row[2];
                    newRow["Min Qty"] = row[3];
                    newRow["Monthly Usage"] = row[4];
                    newRow["Difference"] = Decimal.Parse(row[5].ToString()) / 100.0m;
                    newRow["Avg Cost"] = row[6];
                    newRow["Months To Stock"] = 1;
                    newRow["New Minimum"] = row[4];
                    newRow["Total Cost"] = Decimal.Parse(row[6].ToString()) * Decimal.Parse(row[4].ToString());
                    newRow["Process"] = "No";
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
            List<DataGridViewRow> rows = state as List<DataGridViewRow>;

            try
            {
                Part partBusObj = new Part(session.ConnectionPool);

                foreach (DataGridViewRow row in rows)
                {
                    PartDataSet ds = partBusObj.GetByID(row.Cells["Part"].Value.ToString());

                    foreach (PartDataSet.PartPlantRow pp in ds.PartPlant.Rows)
                    {
                        if (pp.Plant == row.Cells["Plant"].Value.ToString())
                            pp.MinimumQty = Decimal.Parse(row.Cells["New Minimum"].Value.ToString());
                    }

                    partBusObj.Update(ds);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Modules["Part Min Qty Alerts"].State = Module.ModuleState.Saved;
            RefreshTab(new string[] { "Part Min Qty Alerts" });
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
