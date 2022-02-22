using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Erp.BO;
using ObjectLibrary;

namespace Sandbox
{
    class Program
    {

        static void Main(string[] args)
        {

            string username = "CRDService";
            string password = "gfd723trajsdc97";

            EngWorkbenchInterface workBench = new EngWorkbenchInterface();
            workBench.ClearCheckouts(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), username, password, "serv");

            /*FileStream stream = new FileStream("C:\\customeroutputtest.xml", FileMode.Create);
            XmlWriter writer = XmlWriter.Create(stream);

            writer.WriteStartDocument();

            writer.WriteStartElement("TcBusinessData", "http://teamcenter.com/BusinessModel/TcBusinessData");
            writer.WriteAttributeString("batchXSDVersion", "1.0");
            writer.WriteAttributeString("Date", DateTime.Now.ToString("ddd MMM dd hh:mm:ss tt yyyy"));

            writer.WriteStartElement("Add");
            writer.WriteEndElement();

            writer.WriteStartElement("Change");

            writer.WriteStartElement("TcLOV");
            writer.WriteAttributeString("name", "C4_PR_Cust_Name");
            writer.WriteAttributeString("lovType", "ListOfValuesString");
            writer.WriteAttributeString("usage", "Exhaustive");
            writer.WriteAttributeString("description", "");
            writer.WriteAttributeString("isManagedExternally", "true");

            CustomerInterface ci = new CustomerInterface();
            foreach (Customer customer in ci.GetAllCustomers(ConfigurationManager.AppSettings["EpicorDatabaseServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), ConfigurationManager.AppSettings["EpicorUsername"].ToString(), ConfigurationManager.AppSettings["EpicorPassword"].ToString()))
            {
                writer.WriteStartElement("TcLOVValue");
                writer.WriteAttributeString("value", customer.Name);
                writer.WriteAttributeString("description", "");
                writer.WriteAttributeString("conditionName", "isTrue");
                writer.WriteEndElement();
            }

            writer.WriteEndElement();

            writer.WriteEndElement();
            writer.WriteEndElement();

            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();
            */


        }
    }
}
