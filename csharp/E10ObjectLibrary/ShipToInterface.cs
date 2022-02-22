#region Usings

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System;

#endregion

namespace ObjectLibrary
{
    #region Class SalesRep

    public class SalesRep
    {
        #region Properties

        public string Name { get; set; }
        public string RepCode { get; set; }

        #endregion

        #region Constructors

        public SalesRep()
        {
            Name = "";
            RepCode = "";
        }

        public SalesRep(string n, string c)
        {
            Name = n;
            RepCode = c;
        }

        #endregion
    }

    #endregion

    #region Class ShipToAddress

    public class ShipToAddress
    {
        #region Properties

        public string Name { get; set; }
        public string ShipToNum { get; set; }
        public Address Address { get; set; }
        public string PhoneNum { get; set; }
        public string Email { get; set; }
        public string EmailCC { get; set; }

        #endregion

        #region Constructors

        public ShipToAddress()
        {
            ShipToNum = "";
            Address = new Address();
            Name = "";
            PhoneNum = "";
            Email = "";
            EmailCC = "";
        }

        public ShipToAddress(string shiptonum, string name, string address1, string address2, string address3, string city, string state, string zip, string country, string phone, string email, string emailcc)
        {
            Name = name;
            ShipToNum = shiptonum;
            Address = new Address(address1, address2, address3, city, state, zip, country);
            PhoneNum = phone;
            Email = email;
            EmailCC = emailcc;
        }

        #endregion
    }

    #endregion
    // TODO:5 Add unit tests
    public class ShipToInterface
    {
        #region Public Methods

        #region Retrieve Methods

        public List<ShipToAddress> GetShipToByCustID(string server, string database, string username, string password, string custid)
        {
            List<ShipToAddress> result = new List<ShipToAddress>();
            SqlCommand sqlCommand = new SqlCommand("exec [dbo].sp_GetShipToByCustID @CustID");
            sqlCommand.Parameters.AddWithValue("CustID", custid);

            try
            {
                DataSet ds = SQLAccess.GetDataSet(server, database, username, password, sqlCommand);

                foreach (DataRow row in ds.Tables[0].Rows)
                    result.Add(new ShipToAddress(row["shiptonum"].ToString(), row["name"].ToString(), row["staddress1"].ToString(), row["staddress2"].ToString(), row["staddress3"].ToString(), row["stcity"].ToString(), row["ststate"].ToString(), row["stzip"].ToString(), row["stcountry"].ToString(), row["phone"].ToString(), row["email"].ToString(), row["emailcc"].ToString()));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public List<SalesRep> GetSalesRepList(string server, string database, string username, string password, string company)
        {
            List<SalesRep> result = new List<SalesRep>();
            SqlCommand sqlCommand = new SqlCommand("exec [dbo].sp_GetSalesRepList @Company");
            sqlCommand.Parameters.AddWithValue("Company", company);

            try
            {
                DataSet ds = SQLAccess.GetDataSet(server, database, username, password, sqlCommand);

                foreach (DataRow row in ds.Tables[0].Rows)
                    result.Add(new SalesRep(row["name"].ToString(), row["salesrepcode"].ToString()));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

/*        public static ShipToAddress GetSingleShipTo(string server, string database, string username, string password, string shiptonum, int custnum)
        {
            SqlCommand sqlCommand = new SqlCommand("exec [dbo].sp_GetSingleShipTo @ShipToNum, @CustNum");
            sqlCommand.Parameters.AddWithValue("ShipToNum", shiptonum);
            sqlCommand.Parameters.AddWithValue("CustNum", custnum);

            try
            {
                DataSet ds = SQLAccess.GetDataSet(server, database, username, password, sqlCommand);

                if (ds.Tables[0].Rows.Count > 0)
                    return new ShipToAddress(ds.Tables[0].Rows[0]["shiptonum"].ToString(), ds.Tables[0].Rows[0]["staddress1"].ToString(), ds.Tables[0].Rows[0]["staddress2"].ToString(), ds.Tables[0].Rows[0]["staddress3"].ToString(), 
                        ds.Tables[0].Rows[0]["stcity"].ToString(), ds.Tables[0].Rows[0]["ststate"].ToString(), ds.Tables[0].Rows[0]["stzip"].ToString(), ds.Tables[0].Rows[0]["stcountry"].ToString());
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        */
        #endregion

        #endregion
    }
}