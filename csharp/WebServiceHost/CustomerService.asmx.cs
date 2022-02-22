#region Usings

using System;
using System.Configuration;
using System.Web.Services;
using System.Xml;
using ObjectLibrary;

#endregion

namespace WebServiceHost
{
    [WebService(Namespace = "http://services.it.tcg/epicor/CustomerService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class CustomerService : System.Web.Services.WebService
    {
        #region Web Methods

        [WebMethod(Description = "Get Complete Customer List",
        CacheDuration = 0, MessageName = "getcustomerlist")]
        public XmlDataDocument getcustomerlist()
        {
            CustomerInterface customerInterface = new CustomerInterface();

            XmlResponse response = new XmlResponse();

            try
            {
                foreach (ObjectLibrary.Customer customer in customerInterface.GetAllCustomers(ConfigurationManager.AppSettings["Server"].ToString(), ConfigurationManager.AppSettings["Database"].ToString(), ConfigurationManager.AppSettings["Username"].ToString(),
                    ConfigurationManager.AppSettings["Password"].ToString()))
                {
                    response.Data.AppendChild(new customer(customer).AsXmlNode(response, false));
                }
            }
            catch (Exception ex)
            {
                response.Exception = ex.Message;
            }

            return response.Output;
        }

        [WebMethod(Description = "Get Complete Customer Info for One Customer",
        CacheDuration = 0, MessageName = "getcustomerbycustid")]
        public XmlDataDocument getcustomerbycustid(string custid)
        {
            CustomerInterface customerInterface = new CustomerInterface();

            XmlResponse response = new XmlResponse();

            try
            {
                foreach (ObjectLibrary.Customer customer in customerInterface.GetCustomerByCustID(ConfigurationManager.AppSettings["Server"].ToString(), ConfigurationManager.AppSettings["Database"].ToString(), ConfigurationManager.AppSettings["Username"].ToString(),
                    ConfigurationManager.AppSettings["Password"].ToString(), custid))
                {
                    response.Data.AppendChild(new customer(customer).AsXmlNode(response, true));
                }
            }
            catch (Exception ex)
            {
                response.Exception = ex.Message;
            }

            return response.Output;
        }

        #endregion

        #region Data Formats

        public class customer
        {
            #region Properties

            public string custid { get; set; }
            public string custnum { get; set; }
            public string name { get; set; }
            public string phonenum { get; set; }
            public string faxnum { get; set; }
            public string discountpercent { get; set; }
            public string emailaddress { get; set; }
            public string address1 { get; set; }
            public string address2 { get; set; }
            public string address3 { get; set; }
            public string city { get; set; }
            public string state { get; set; }
            public string zip { get; set; }
            public string country { get; set; }
            public string staddress1 { get; set; }
            public string staddress2 { get; set; }
            public string staddress3 { get; set; }
            public string stcity { get; set; }
            public string ststate { get; set; }
            public string stzip { get; set; }
            public string stcountry { get; set; }

            #endregion

            #region Constructors

            public customer()
            {
                custid = "";
                custnum = "";
                name = "";
                phonenum = "";
                faxnum = "";
                discountpercent = "";
                emailaddress = "";
                address1 = "";
                address2 = "";
                address3 = "";
                city = "";
                state = "";
                zip = "";
                country = "";
                staddress1 = "";
                staddress2 = "";
                staddress3 = "";
                stcity = "";
                ststate = "";
                stzip = "";
                stcountry = "";
            }

            public customer(ObjectLibrary.Customer customer)
            {
                custid = customer.CustID;
                custnum = customer.CustNum;
                name = customer.Name;
                phonenum = customer.PhoneNumber;
                faxnum = customer.FaxNumber;
                discountpercent = customer.DiscountPercent;
                emailaddress = customer.Email;
                address1 = customer.Address.Address1;
                address2 = customer.Address.Address2;
                address3 = customer.Address.Address3;
                city = customer.Address.City;
                state = customer.Address.State;
                zip = customer.Address.Zip;
                country = customer.Address.Country;
                staddress1 = customer.ShipToAddress.Address1;
                staddress2 = customer.ShipToAddress.Address2;
                staddress3 = customer.ShipToAddress.Address3;
                stcity = customer.ShipToAddress.City;
                stcountry = customer.ShipToAddress.Country;
                ststate = customer.ShipToAddress.State;
                stzip = customer.ShipToAddress.Zip;
            }

            #endregion

            #region Public Methods

            public XmlNode AsXmlNode(XmlResponse doc, bool full)
            {
                XmlNode root = doc.CreateElement("customer");
                root.AppendChild(doc.CreateNode("custid", custid));
                root.AppendChild(doc.CreateNode("custnum", custnum));
                root.AppendChild(doc.CreateNode("name", name));
                root.AppendChild(doc.CreateNode("phonenum", phonenum));
                if (full)
                {
                    root.AppendChild(doc.CreateNode("faxnum", faxnum));
                    root.AppendChild(doc.CreateNode("discountpercent", discountpercent));
                    root.AppendChild(doc.CreateNode("email", emailaddress));
                    root.AppendChild(doc.CreateNode("address1", address1));
                    root.AppendChild(doc.CreateNode("address2", address2));
                    root.AppendChild(doc.CreateNode("address3", address3));
                    root.AppendChild(doc.CreateNode("city", city));
                }
                root.AppendChild(doc.CreateNode("state", state));
                root.AppendChild(doc.CreateNode("zip", zip));

                if (full)
                {
                    root.AppendChild(doc.CreateNode("country", country));
                    root.AppendChild(doc.CreateNode("staddress1", staddress1));
                    root.AppendChild(doc.CreateNode("staddress2", staddress2));
                    root.AppendChild(doc.CreateNode("staddress3", staddress3));
                    root.AppendChild(doc.CreateNode("stcity", stcity));
                    root.AppendChild(doc.CreateNode("ststate", ststate));
                    root.AppendChild(doc.CreateNode("stzip", stzip));
                    root.AppendChild(doc.CreateNode("stcountry", stcountry));
                }

                return root;
            }

            #endregion
        }

        #endregion
    }
}
