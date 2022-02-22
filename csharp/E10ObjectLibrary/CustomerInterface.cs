#region Usings

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System;
using Erp.Proxy.BO;
using Erp.BO;
using Ice.Core;
using Erp.Contracts;

#endregion

namespace ObjectLibrary
{
    #region Class Customer

    public class Customer
    {
        #region Properties

        public string CustID { get; set; }
        public string CustNum { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string DiscountPercent { get; set; }
        public string Email { get; set; }
        public string ContactName { get; set; }
        public Address Address { get; set; }
        public string ShipToNum { get; set; }
        public string ShipToName { get; set; }
        public Address ShipToAddress { get; set; }
        public string SpecialInstructions { get; set; }
        public Boolean TaxExempt { get; set; }
        public Boolean ISEPackage { get; set; }
        public string SalesRepCode { get; set; }
        public string ShipToEmail { get; set; }

        #endregion

        #region Constructors

        public Customer()
        {
            CustID = "";
            CustNum = "";
            Name = "";
            PhoneNumber = "";
            FaxNumber = "";
            DiscountPercent = "";
            Email = "";
            ContactName = "";
            Address = new Address();
            ShipToNum = "";
            ShipToName = "";
            ShipToAddress = new Address();
            SpecialInstructions = "";
            TaxExempt = false;
            ISEPackage = false;
            SalesRepCode = "";
            ShipToEmail = "";
        }

        public Customer(string custid, string custnum, string name, string phonenumber, string faxnumber, string discountpercent, string email, string contactname, string address1, string address2, string address3, string city, string state, string zip,
            string country, string shiptonum, string shiptoname, string staddress1, string staddress2, string staddress3, string stcity, string ststate, string stzip, string stcountry, string specialinstructions, Boolean taxexempt, Boolean isepackage, string salesrepcode, string stemail)
        {
            CustID = custid;
            CustNum = custnum;
            Name = name;
            PhoneNumber = phonenumber;
            FaxNumber = faxnumber;
            DiscountPercent = discountpercent;
            Email = email;
            ContactName = contactname;
            Address = new Address(address1, address2, address3, city, state, zip, country);
            ShipToNum = shiptonum;
            ShipToName = shiptoname;
            ShipToAddress = new Address(staddress1, staddress2, staddress3, stcity, ststate, stzip, stcountry);
            SpecialInstructions = specialinstructions;
            TaxExempt = taxexempt;
            ISEPackage = isepackage;
            SalesRepCode = salesrepcode;
            ShipToEmail = stemail;
        }

        #endregion
    }

    #endregion
    // TODO:5 Add unit tests
    public class CustomerInterface: EpicorExtension<CustomerImpl, CustomerSvcContract>
    {
        public void UpdateSalesRepCode(string server, string database, string username, string password, string custid, string salesrepcode)
        {
            OpenSession(server, database, username, password, Session.LicenseType.Default);
            try
            {
                CustomerDataSet ds = BusinessObject.GetByCustID(custid, true);
                foreach (CustomerDataSet.CustomerRow row in ds.Customer.Rows)
                {
                    if (row.CustID == custid)
                    {
                        if (row.SalesRepCode != salesrepcode)
                            row.SalesRepCode = salesrepcode;
                        foreach (CustomerDataSet.ShipToRow strow in ds.ShipTo.Rows)
                        {
                            if (!String.IsNullOrEmpty(strow.ShipToNum) && strow.SalesRepCode != salesrepcode)
                                strow.SalesRepCode = salesrepcode;
                        }
                    }
                }
                BusinessObject.Update(ds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseSession();
            }

        }

        public bool UpdateDiscountPercent(string server, string database, string username, string password, string custid, decimal discount)
        {
            bool result = true;
            OpenSession(server, database, username, password, Session.LicenseType.Default);
            try
            {
                CustomerDataSet ds = BusinessObject.GetByCustID(custid, true);
                foreach (CustomerDataSet.CustomerRow row in ds.Customer.Rows)
                {
                    if (row.CustID == custid)
                    {
                        if (row.DiscountPercent != discount)
                            row.DiscountPercent = discount;
                    }
                }
                BusinessObject.Update(ds);
            }
            catch (Exception ex)
            {
                result = false;
                throw ex;
            }
            finally
            {
                CloseSession();
            }
            return result;

        }


        public void UpdateBillTo(string server, string database, string username, string password, string custid, string btaddress1, string btaddress2, string btphone, string btfax)
        {
            OpenSession(server, database, username, password, Session.LicenseType.Default);
            try
            {
                CustomerDataSet ds = BusinessObject.GetByCustID(custid, true);
                foreach (CustomerDataSet.CustomerRow row in ds.Customer.Rows)
                {
                    if (row.CustID == custid)
                    {
                        row.BTAddress1 = btaddress1;
                        row.BTAddress2 = btaddress2;
                        row.BTPhoneNum = btphone;
                        row.BTFaxNum = btfax;
                    }
                }
                BusinessObject.Update(ds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseSession();
            }

        }

        #region Public Methods

        public bool IsShipToValid(string server, string database, string username, string password, string custid, string shiptonum, string name, string address1, string address2, string address3, string city, string state, string zip, string stemail, string stemailcc)
        {
            bool result = true;
            try
            {
                OpenSession(server, database, username, password, Session.LicenseType.Default);
                CustomerDataSet ds = BusinessObject.GetByCustID(custid, true);
                
                bool found = false;
                if (ds.Customer.Rows.Count == 0)
                    throw new Exception("IsShipToValid - No customer found matching Customer ID " + custid);
                else
                {
                    foreach (CustomerDataSet.ShipToRow row in ds.ShipTo.Rows)
                    {
                        
                        if (row.ShipToNum == shiptonum)
                        {
                            found = true;
                            if (row.Name != name || row.Address1 != address1 || row.Address2 != address2 || row.Address3 != address3 || row.City != city || row.State != state || row.ZIP != zip)
                                result = false;
                            else
                            {
                                if (row.EMailAddress != stemail)
                                {
                                    row.EMailAddress = stemail;
                                    row.RowMod = "U";
                                    BusinessObject.Update(ds);
                                }
                                if (row["character01"].ToString() != stemailcc)
                                {
                                    row["character01"] = stemailcc;
                                    row.RowMod = "U";
                                    BusinessObject.Update(ds);
                                }
                            }
                        }
                    }
                    if (result && !found)
                    {
                        int custnum = ds.Customer[0].CustNum;
                        BusinessObject.GetNewShipTo(ds, custnum);
                        CustomerDataSet.ShipToRow newShipTo = ds.ShipTo[ds.ShipTo.Count - 1];
                        newShipTo.ShipToNum = shiptonum;
                        newShipTo.Name = name;
                        newShipTo.EMailAddress = stemail;
                        newShipTo["character01"] = stemailcc;
                        if (address1.Length > 0)
                            BusinessObject.OnChangeofShipToAddr(ds, custnum, shiptonum, address1, "Address1");
                        if (address2.Length > 0)
                            BusinessObject.OnChangeofShipToAddr(ds, custnum, shiptonum, address2, "Address2");
                        if (address3.Length > 0)
                            BusinessObject.OnChangeofShipToAddr(ds, custnum, shiptonum, address3, "Address3");
                        BusinessObject.OnChangeofShipToAddr(ds, custnum, shiptonum, city, "City");
                        BusinessObject.OnChangeofShipToAddr(ds, custnum, shiptonum, state, "State");
                        newShipTo.RowMod = "U";
                        newShipTo.ZIP = zip;
                        BusinessObject.OnChangeofShipToAddr(ds, custnum, shiptonum, zip, "ZIP");
                        if (Char.IsLetter(zip[0]))
                            BusinessObject.OnChangeofShipToAddr(ds, custnum, shiptonum, "5", "CountryNum");
                        BusinessObject.Update(ds);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("IsShipToValid - " + ex.Message);
            }
            finally
            {
                CloseSession();
            }
            return result;

        }

        #region Create Methods

        #endregion

        #region Retrieve Methods

        public List<Customer> GetAllCustomers(string server, string database, string username, string password)
        {
            List<Customer> result = new List<Customer>();

            SqlCommand sqlCommand = new SqlCommand("exec [dbo].sp_CustomerLookup_2");

            try
            {
                DataSet ds = SQLAccess.GetDataSet(server, database, username, password, sqlCommand);

                foreach (DataRow row in ds.Tables[0].Rows)
                    result.Add(new Customer(row["custid"].ToString(), row["custnum"].ToString(), row["name"].ToString(), row["phonenum"].ToString(), "", "", "", "", 
                        "", "", "", row["city"].ToString(), row["state"].ToString(), row["zip"].ToString(), "",
                        "", "", "", "", "", "", "", "", "", 
                        "", false, false, "", ""));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public Customer GetCustomerByCustID(string server, string database, string username, string password, string custid)
        {
            SqlCommand sqlCommand = new SqlCommand("exec [dbo].sp_CustomerInfo_2 @CustID");
            sqlCommand.Parameters.AddWithValue("CustID", custid);

            try
            {
                DataSet ds = SQLAccess.GetDataSet(server, database, username, password, sqlCommand);

                if (ds.Tables[0].Rows.Count > 0)
                    return new Customer(ds.Tables[0].Rows[0]["custid"].ToString(), ds.Tables[0].Rows[0]["custnum"].ToString(), ds.Tables[0].Rows[0]["name"].ToString(), ds.Tables[0].Rows[0]["phonenum"].ToString(), ds.Tables[0].Rows[0]["faxnum"].ToString(),
                        ds.Tables[0].Rows[0]["discountpercent"].ToString(), ds.Tables[0].Rows[0]["emailaddress"].ToString(), ds.Tables[0].Rows[0]["contactname"].ToString(), ds.Tables[0].Rows[0]["address1"].ToString(), ds.Tables[0].Rows[0]["address2"].ToString(),
                        ds.Tables[0].Rows[0]["address3"].ToString(), ds.Tables[0].Rows[0]["city"].ToString(), ds.Tables[0].Rows[0]["state"].ToString(), ds.Tables[0].Rows[0]["zip"].ToString(), ds.Tables[0].Rows[0]["country"].ToString(),
                        ds.Tables[0].Rows[0]["shiptonum"].ToString(), ds.Tables[0].Rows[0]["stname"].ToString(), ds.Tables[0].Rows[0]["staddress1"].ToString(), ds.Tables[0].Rows[0]["staddress2"].ToString(), ds.Tables[0].Rows[0]["staddress3"].ToString(), ds.Tables[0].Rows[0]["stcity"].ToString(), ds.Tables[0].Rows[0]["ststate"].ToString(),
                        ds.Tables[0].Rows[0]["stzip"].ToString(), ds.Tables[0].Rows[0]["stcountry"].ToString(), ds.Tables[0].Rows[0]["specialinst"].ToString(), ds.Tables[0].Rows[0]["taxexempt"].ToString() == "CRT", ds.Tables[0].Rows[0]["isepackage"].ToString() == "1", ds.Tables[0].Rows[0]["salesrepcode"].ToString(), ds.Tables[0].Rows[0]["stemail"].ToString());
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetCustomerSpecialInstructions(string server, string database, string username, string password, string custid)
        {
            SqlCommand sqlCommand = new SqlCommand("exec [dbo].sp_GetCustomerSpecialInstructions @CustID");
            sqlCommand.Parameters.AddWithValue("CustID", custid);

            try
            {
                return SQLAccess.GetScalar(server, database, username, password, sqlCommand);
            }
            catch (NullReferenceException)
            {
                throw new Exception(String.Format("Customer {0} not found", custid));
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

