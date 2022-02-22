using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using ObjectLibrary;
using System.Configuration;
using System.IO;

namespace OrderDtlDateMaintenanceServiceHost
{
    partial class OrderDtlDateMaintenanceService : ServiceBase
    {
        #region Values

        System.Timers.Timer timer;
        FileStream stream;
        StreamWriter writer;

        #endregion

        public OrderDtlDateMaintenanceService()
        {
            InitializeComponent();
            timer = new System.Timers.Timer(3600000);
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            stream = new FileStream("D:\\Installpoint\\Logs\\OrderDtlDateMaintenanceServiceLog.txt", FileMode.OpenOrCreate);
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
            try
            {
                OrderInterface oi = new OrderInterface();
                DataSet ds = oi.GetOrderLinesWithWrongDate(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), ConfigurationManager.AppSettings["EpicorUsername"].ToString(), ConfigurationManager.AppSettings["EpicorPassword"].ToString());
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    string ordernum = row[1].ToString();
                    string orderline = row[2].ToString();
                    string origReq = String.IsNullOrEmpty(row[5].ToString()) ? "MISSING" : DateTime.Parse(row[5].ToString()).ToShortDateString();
                    string origNeed = String.IsNullOrEmpty(row[6].ToString()) ? "MISSING" : DateTime.Parse(row[6].ToString()).ToShortDateString();
                    DateTime newReq = DateTime.Parse(row[3].ToString());
                    DateTime newNeed = DateTime.Parse(row[4].ToString());
                    oi.UpdateOrderLineDates(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), "CRDService", "gfd723trajsdc97", ordernum, orderline, newNeed, newReq);
                    writer.WriteLine(DateTime.Now.ToString() + " - Order # " + ordernum + ", Line # " + orderline + ", Req changed from " + origReq + " to " + newReq.ToShortDateString() + ", NeedBy changed from " + origNeed + " to " + newNeed.ToShortDateString());
                }
                writer.Flush();

            }
            catch (Exception ex)
            {
                lock (writer)
                {
                    writer.WriteLine(DateTime.Now.ToString() + " - Error reading quote database: " + ex.Message);
                    writer.Flush();
                }
                throw ex;
            }
            finally
            {
                timer.Start();
            }
        }

    }
}
