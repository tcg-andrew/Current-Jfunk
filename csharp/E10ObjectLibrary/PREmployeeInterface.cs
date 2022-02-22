using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Erp.Proxy.BO;
using Erp.BO;
using Ice.Core;
using Erp.Contracts;

namespace ObjectLibrary
{
    public class PREmployeeInterface : EpicorExtension<PREmployeeImpl, PREmployeeSvcContract>
    {
        public string GetVacationTimeRemaining(string server, string database, string username, string password, string company, string id)
        {
            string result = "Employee Not Found";

            SqlCommand sqlCommand = new SqlCommand("exec [dbo].sp_GetEmpInfo @Company, @EmpID");
            sqlCommand.Parameters.AddWithValue("Company", company);
            sqlCommand.Parameters.AddWithValue("EmpID", id);

            try
            {
                DataSet ds = SQLAccess.GetDataSet(server, database, username, password, sqlCommand);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    result = ds.Tables[0].Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }

        public string GetName(string server, string database, string username, string password, string company, string id)
        {
            string result = "Employee Not Found";

            SqlCommand sqlCommand = new SqlCommand("exec [dbo].sp_GetEmpInfo @Company, @EmpID");
            sqlCommand.Parameters.AddWithValue("Company", company);
            sqlCommand.Parameters.AddWithValue("EmpID", id);

            try
            {
                DataSet ds = SQLAccess.GetDataSet(server, database, username, password, sqlCommand);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    result = ds.Tables[0].Rows[0][1].ToString();
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }
    }
}
