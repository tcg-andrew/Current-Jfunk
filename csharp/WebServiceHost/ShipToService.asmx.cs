#region Usings

using System;
using System.Configuration;
using System.Web.Services;
using System.Xml;
using ObjectLibrary;

#endregion

namespace WebServiceHost
{
    [WebService(Namespace = "http://services.it.tcg/epicor/ShipToService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class ShipToService : System.Web.Services.WebService
    {
        #region Web Methods

        [WebMethod(Description = "Get Complete Ship To Info for One Customer",
        CacheDuration = 0, MessageName = "getshiptobycustnum")]
        public XmlDataDocument getshiptobycustnum(int custnum)
        {
            ShipToInterface shipToInterface = new ShipToInterface();

            XmlResponse response = new XmlResponse();

            try
            {
                foreach (ShipToAddress address in shipToInterface.GetShipToByCustNum(ConfigurationManager.AppSettings["Server"].ToString(), ConfigurationManager.AppSettings["Database"].ToString(), ConfigurationManager.AppSettings["Username"].ToString(),
                    ConfigurationManager.AppSettings["Password"].ToString(), custnum))
                {
                    response.Data.AppendChild(new shipto(address).AsXmlNode(response));
                }
            }
            catch (Exception ex)
            {
                response.Exception = ex.Message;
            }

            return response.Output;
        }

        [WebMethod(Description = "Get Info for One ShipTo for One Customer",
        CacheDuration = 0, MessageName = "getsingleshipto")]
        public XmlDataDocument getsingleshipto(string shiptonum, int custnum)
        {
            ShipToInterface shipToInterface = new ShipToInterface();

            XmlResponse response = new XmlResponse();

            try
            {
                foreach (ShipToAddress address in shipToInterface.GetSingleShipTo(ConfigurationManager.AppSettings["Server"].ToString(), ConfigurationManager.AppSettings["Database"].ToString(), ConfigurationManager.AppSettings["Username"].ToString(),
                    ConfigurationManager.AppSettings["Password"].ToString(), shiptonum, custnum))
                {
                    response.Data.AppendChild(new shipto(address).AsXmlNode(response));
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

        public class shipto
        {
            #region Properties

            public string shiptonum { get; set; }
            public string address1 { get; set; }
            public string address2 { get; set; }
            public string address3 { get; set; }
            public string city { get; set; }
            public string state { get; set; }
            public string zip { get; set; }
            public string country { get; set; }

            #endregion

            #region Constructors

            public shipto()
            {
                shiptonum = "";
                address1 = "";
                address2 = "";
                address3 = "";
                city = "";
                state = "";
                zip = "";
                country = "";
            }

            public shipto(ShipToAddress shipToAddress)
            {
                shiptonum = shipToAddress.ShipToNum;
                address1 = shipToAddress.Address.Address1;
                address2 = shipToAddress.Address.Address2;
                address3 = shipToAddress.Address.Address3;
                city = shipToAddress.Address.City;
                state = shipToAddress.Address.State;
                zip = shipToAddress.Address.Zip;
                country = shipToAddress.Address.Country;
            }

            #endregion

            #region Public Methods

            public XmlNode AsXmlNode(XmlResponse doc)
            {
                XmlNode root = doc.CreateElement("shipto");
                root.AppendChild(doc.CreateNode("shiptonum", shiptonum));
                root.AppendChild(doc.CreateNode("address1", address1));
                root.AppendChild(doc.CreateNode("address2", address2));
                root.AppendChild(doc.CreateNode("address3", address3));
                root.AppendChild(doc.CreateNode("city", city));
                root.AppendChild(doc.CreateNode("state", state));
                root.AppendChild(doc.CreateNode("zip", zip));
                root.AppendChild(doc.CreateNode("country", country));

                return root;
            }

            #endregion
        }

        #endregion
    }
}
