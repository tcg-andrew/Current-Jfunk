#region Usings

using System;
using System.Configuration;
using System.Web.Services;
using System.Xml;
using ObjectLibrary;

#endregion

namespace WebServiceHost
{
    [WebService(Namespace = "http://services.it.tcg/epicor/PartService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class PartService : System.Web.Services.WebService
    {
        #region Web Methods

        [WebMethod(Description = "Get Complete Part List",
        CacheDuration = 0, MessageName = "getpartlist")]
        public XmlDataDocument getpartlist()
        {
            PartInterface partInterface = new PartInterface();

            XmlResponse response = new XmlResponse();
            try
            {
                foreach (Part part in partInterface.GetAllParts(ConfigurationManager.AppSettings["Server"].ToString(), ConfigurationManager.AppSettings["Database"].ToString(), ConfigurationManager.AppSettings["Username"].ToString(),
                    ConfigurationManager.AppSettings["Password"].ToString(), 0))
                {
                    response.Data.AppendChild(new part(part).AsXmlNode(response));
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

        public class part
        {
            #region Properties

            public string partnum { get; set; }
            public string desc { get; set; }
            public string type { get; set; }
            public bool nonstock { get; set; }
            public string group { get; set; }
            public string partclass { get; set; }
            public string unit { get; set; }
            public decimal price { get; set; }

            #endregion

            #region Constructors

            public part()
            {
                partnum = "";
                desc = "";
                type = "";
                nonstock = false;
                group = "";
                partclass = "";
                unit = "";
                price = 0;
            }

            public part(ObjectLibrary.Part part)
            {
                partnum = part.PartNum;
                desc = part.Desc;
                type = part.Type;
                nonstock = part.NonStock;
                group = part.Group;
                partclass = part.Class;
                unit = part.Unit;
                price = part.Price;
            }

            #endregion

            #region Public Methods

            public XmlNode AsXmlNode(XmlResponse doc)
            {
                XmlNode root = doc.CreateElement("part");
                
                root.AppendChild(doc.CreateNode("partnum", partnum));
                root.AppendChild(doc.CreateNode("desc", desc));
                root.AppendChild(doc.CreateNode("type", type));
                root.AppendChild(doc.CreateNode("nonstock", nonstock));
                root.AppendChild(doc.CreateNode("group", group));
                root.AppendChild(doc.CreateNode("partclass", partclass));
                root.AppendChild(doc.CreateNode("unit", unit));
                root.AppendChild(doc.CreateNode("price", price));

                return root;
            }

            #endregion
        }

        #endregion
    }
}
