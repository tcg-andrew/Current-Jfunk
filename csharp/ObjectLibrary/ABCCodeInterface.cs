#region Usings

using System;
using System.Collections.Generic;
using Epicor.Mfg.BO;
using Epicor.Mfg.Core;
using System.Data;
using System.Data.SqlClient;
using ObjectLibrary;

#endregion

namespace ObjectLibrary
{
    #region Class ABCCode

    public class ABCCode
    {
        #region Properties

        public string Code { get; set; }
        public decimal MinVol { get; set; }
        public decimal MinCost { get; set; }
        public int Frequency { get; set; }

        #endregion

        #region Constructors

        public ABCCode()
        {
            this.Code = "";
            this.MinVol = 0;
            this.MinCost = 0;
            this.Frequency = 0;
        }

        public ABCCode(string code, decimal minvol, decimal mincost, int freq)
        {
            this.Code = code;
            this.MinVol = minvol;
            this.MinCost = mincost;
            this.Frequency = freq;
        }

        #endregion
    }

    #endregion
    // TODO:5 Add unit tests
    public class ABCCodeInterface: EpicorExtension<AbcCode>
    {
        #region Public Methods

        #region Retrieve Methods

        public List<ABCCode> GetCodes(string server, string database, string username, string password)
        {
            List<ABCCode> results = new List<ABCCode>();
            SqlCommand sqlCommand = new SqlCommand("exec [dbo].sp_ABCCodeLookup");

            try
            {
                DataSet ds = SQLAccess.GetDataSet(server, database, username, password, sqlCommand);

                foreach (DataRow row in ds.Tables[0].Rows)
                    results.Add(new ABCCode(row["code"].ToString(), System.Convert.ToDecimal(row["minvol"].ToString()), System.Convert.ToDecimal(row["mincost"].ToString()), System.Convert.ToInt32(row["freq"].ToString())));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return results;
        }

        #endregion

        #region Update Methods

        public void UpdateCode(string server, string port, string username, string password, ABCCode code)
        {
            try
            {
                OpenSession(server, port, username, password, Session.LicenseType.Default);
                AbcCodeDataSet ds = BusinessObject.GetByID(code.Code);

                if (ds.AbcCode.Rows.Count > 0)
                {
                    AbcCodeDataSet.AbcCodeRow row = ds.AbcCode.Rows[0] as AbcCodeDataSet.AbcCodeRow;

                    row.MinDollarVolume = code.MinVol;
                    row.MinUnitCost = code.MinCost;
                    row.CountFreq = code.Frequency;

                    BusinessObject.Update(ds);
                }

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
