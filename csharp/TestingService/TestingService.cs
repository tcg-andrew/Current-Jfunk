using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.IO;
using System.Timers;
using ObjectLibrary;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace TestingService
{
    public partial class TestingService : ServiceBase
    {
        System.Timers.Timer timer;
        FileStream stream;
        StreamWriter writer;
        
        public TestingService()
        {
            InitializeComponent();
            timer = new System.Timers.Timer(1000);
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            stream = new FileStream("C:\\TestLog.txt", FileMode.OpenOrCreate);
            stream.Position = stream.Length;
            writer = new StreamWriter(stream);
        }


        protected override void OnStart(string[] args)
        {
            try
            {
                timer.Start();
                writer.WriteLine(DateTime.Now.ToString() + " - Started timer");
                writer.Flush();
            }
            catch (Exception ex)
            {
                    writer.WriteLine(DateTime.Now.ToString() + " - error starting timer: " + ex.Message);
                    writer.Flush();
            }
        }

        protected override void OnStop()
        {
            try
            {
                timer.Stop();
                writer.WriteLine(DateTime.Now.ToString() + " - Stopped Timer");
                writer.Flush();
            }
            catch (Exception ex)
            {
                    writer.WriteLine(DateTime.Now.ToString() + " - error stopping timer: " + ex.Message);
                    writer.Flush();
            }
            finally
            {
                writer.Close();
            }
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
                writer.WriteLine(DateTime.Now.ToString() + " - Elapsed");
            writer.Flush();
            DataSet ds = MySQLAccess.GetDataSet(ConfigurationManager.AppSettings["WebQuoteServer"].ToString(), ConfigurationManager.AppSettings["WebQuoteDatabase"].ToString(), ConfigurationManager.AppSettings["WebQuoteUsername"].ToString(), ConfigurationManager.AppSettings["WebQuotePassword"].ToString(), new MySqlCommand("SELECT id, vantage_status FROM quote_heads WHERE vantage_status = 'PENDING' or vantage_status = 'RESUBMIT'"));
            writer.WriteLine(ds.Tables[0].Rows.Count);
            writer.Flush();
        }
    }
}
