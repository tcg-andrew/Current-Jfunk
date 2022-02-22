using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModuleBase;
using Epicor.Mfg.Core;
using System.Data.SqlClient;
using System.Data;

namespace AssemblyNoAutoRec
{
    class TaskModule : Module
    {
        public TaskModule(Session session, string server, string database)
            : base(session, server, database)
        {
            _actions.Add(ModuleAction.Favorite);
            _actions.Add(ModuleAction.Refresh);

            _columns.Add(new GridColumn("Job #"));
            _columns.Add(new GridColumn("Assembly Seq"));
            _columns.Add(new GridColumn("Auto Receive Opr"));
            _columns.Add(new GridColumn("Final Opr"));
            _columns.Add(new GridColumn("Highest Opr Seq"));
        }


        protected override System.Data.DataTable DataMethod(Dictionary<string, object> args)
        {
            try
            {
                #region Get Data Set

                SqlCommand command = new SqlCommand("exec sp_GetJobAssembliesNoAutoRec @Company");
                command.Parameters.AddWithValue("Company", Session.CompanyID);
                DataSet ds = SQLAccess.SQLAccess.GetDataSetSSPI(Server, Database, command);

                #endregion

                System.Data.DataTable dt = GenerateGridTable();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DataRow newRow = dt.NewRow();
                    newRow["Job #"] = row[0];
                    newRow["Assembly Seq"] = row[1];
                    newRow["Auto Receive Opr"] = row[2];
                    newRow["Final Opr"] = row[3];
                    newRow["Highest Opr Seq"] = row[4];
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
    }
}
