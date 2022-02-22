using ObjectLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Xml;

namespace TCCustomerServiceHost
{
    public partial class TCCustomerService : ServiceBase
    {
        System.Timers.Timer timer;
        FileStream stream;
        StreamWriter writer;
        DateTime _last;

        public TCCustomerService()
        {
            InitializeComponent();

            timer = new System.Timers.Timer(6000);
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);

            stream = new FileStream("D:\\Installpoint\\Logs\\TCCustomerServiceLog.txt", FileMode.OpenOrCreate);
            stream.Position = stream.Length;
            writer = new StreamWriter(stream);
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                timer.Start();
            }
            catch (Exception ex)
            {
                lock (writer)
                {
                    writer.WriteLine(DateTime.Now.ToString() + " - error starting timer: " + ex.Message);
                    writer.Flush();
                }
                throw ex;
            }
        }

        protected override void OnStop()
        {
            try
            {
                timer.Stop();
            }
            catch (Exception ex)
            {
                lock (writer)
                {
                    writer.WriteLine(DateTime.Now.ToString() + " - error stopping timer: " + ex.Message);
                    writer.Flush();
                }
                throw ex;
            }
            finally
            {
                writer.Close();
            }
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer.Stop();

            if (_last == null || (_last != null && _last < DateTime.Today))
            {
                _last = DateTime.Today;

                FileStream stream = new FileStream("C:\\customeroutputtest.xml", FileMode.Create);
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

            }
            timer.Start();

        }
    }
}
