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

namespace JobMtlDateMaintenanceServiceHost
{
    partial class JobMtlDateMaintenanceService : ServiceBase
    {
        #region Values

        private string vantage_user = "CRDService";
        private string vantage_pass = "gfd723trajsdc97";
        System.Timers.Timer timer;
        FileStream stream;
        StreamWriter writer;

        #endregion

        public JobMtlDateMaintenanceService()
        {
            InitializeComponent();
            timer = new System.Timers.Timer(3600000);
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            stream = new FileStream("C:\\JobMtlDateMaintenanceServiceLog.txt", FileMode.OpenOrCreate);
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
                JobInterface ji = new JobInterface();
                ScheduleEngineInterface si = new ScheduleEngineInterface();
                DataSet ds = ji.GetJobsWithMismatchedMaterialDates(ConfigurationManager.AppSettings["EpicorDatabaseServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), ConfigurationManager.AppSettings["EpicorUsername"].ToString(), ConfigurationManager.AppSettings["EpicorPassword"].ToString());
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    try
                    {
                        string jobnum = row[0].ToString();
                        string origReq = String.IsNullOrEmpty(row[2].ToString()) ? "MISSING" : DateTime.Parse(row[2].ToString()).ToShortDateString();
                        DateTime newReq = DateTime.Parse(row[1].ToString());
                        string log = si.UpdateJobDate(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), vantage_user, vantage_pass, jobnum, newReq);
                        writer.WriteLine(DateTime.Now.ToString() + " - Job # " + jobnum + ", Req changed from " + origReq + " to " + newReq.ToShortDateString() + ". Log: " + log);
                        SQLAccess.SendMail(ConfigurationManager.AppSettings["EpicorDatabaseServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), ConfigurationManager.AppSettings["EpicorUsername"].ToString(), ConfigurationManager.AppSettings["EpicorPassword"].ToString(), "lpearson@styleline.com; jfunk@styleline.com;", "Job Material Dates Updated", DateTime.Now.ToString() + " - Job # " + jobnum + ", Req changed from " + origReq + " to " + newReq.ToShortDateString() + ". Log: " + log);
                    }
                    catch (Exception ex)
                    {
                        writer.WriteLine(DateTime.Now.ToString() + " - Error processing record : " + ex.Message);
                    }
                }
                writer.Flush();

            }
            catch (Exception ex)
            {
                lock (writer)
                {
                    writer.WriteLine(DateTime.Now.ToString() + " - Error in Elapsed " + ex.Message);
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
