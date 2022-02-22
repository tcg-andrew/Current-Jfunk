#region Usings

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.ServiceProcess;
using System.Threading;
using System.Timers;
using ObjectLibrary;
using System.Text.RegularExpressions;
using System.Net;
using System.Web.Script.Serialization;
using System.Xml;
using System.Web;
using static ObjectLibrary.TCBridgeInterface;
using System.Data.SqlClient;

#endregion

namespace TCBridgeServiceHost
{
    public partial class TCBridgeService : ServiceBase
    {
        #region Values

        string env;
        System.Timers.Timer timer;


        TCBridgeInterface bridge;
        PartInterface partInterface;
        EngWorkbenchInterface workBench;
        UOMInterface uomInterface;
        UOMClassInterface uomClassInterface;

        Queue<string> toBeProcessed;
        Queue<string> priorityToBeProcessed;
        Queue<string> partOnlyToBeProcessed;
        Queue<string> partOnlyPriorityToBeProcessed;
        int currThreadCount = 0;
        int maxThreadCount = 2;
        FileStream stream;
        StreamWriter writer;
        bool threadone = false;
        bool threadtwo = false;
        bool threadthree = false;
        bool threadfour = false;
        bool threadfive = false;
        bool threadsix = false;

        string username = "CRDService";
        string password = "gfd723trajsdc97";

        DateTime lastDaily;

        #endregion

        #region Constructors

        public TCBridgeService()
        {
            InitializeComponent();
            env = ConfigurationManager.AppSettings["TCEnvironment"].ToString();

            timer = new System.Timers.Timer(30000);
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);

            bridge = new TCBridgeInterface();
            partInterface = new PartInterface();
            workBench = new EngWorkbenchInterface();
            uomInterface = new UOMInterface();
            uomClassInterface = new UOMClassInterface();

            toBeProcessed = new Queue<string>();
            priorityToBeProcessed = new Queue<string>();
            partOnlyToBeProcessed = new Queue<string>();
            partOnlyPriorityToBeProcessed = new Queue<string>();
            stream = new FileStream("D:\\Installpoint\\Logs\\TCBridgeServiceLog.txt", FileMode.OpenOrCreate);
            stream.Position = stream.Length;
            writer = new StreamWriter(stream);
        }

        #endregion

        #region Protected Methods

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

        #endregion

        #region Private Methods

        private void ProcessAsAsmChanges()
        {
            string folder = ConfigurationManager.AppSettings["TCAsm"].ToString();
            EngWorkbenchInterface workBench = new EngWorkbenchInterface();
            PartInterface pi = new PartInterface();
            List<string> success = new List<string>();
            List<string> to_delete = new List<string>();
            foreach (string file in Directory.GetFiles(folder))
            {
                List<string> errors = new List<string>();
                string[] lines = System.IO.File.ReadAllLines(file);

                foreach (string line in lines)
                {
                    try
                    {
                        if (!line.StartsWith("Child Part Number"))
                        {
                            string[] split = line.Split('|');
                            string child = split[0].Split('/')[0];
                            bool pull = split[1].ToLower() == "1";
                            bool view = split[2].ToLower() == "1";
                            string parent = split[3].Split('/')[0];
                            string rev = split[3].Split('/')[1];

                                bool approved = pi.GetRevStatus(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), username, password, parent, rev);


                                workBench.ReviseMaterial(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), username, password, parent, rev, child, pull, view);

                                if (!approved)
                                    pi.UnapproveRevision(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), username, password, parent, rev);

                        }
                        success.Add(line);
                    }
                    catch (Exception ex)
                    {
                        errors.Add(line + " : " + ex.Message);
                    }
                }

                if (errors.Count == 0)
                {
                    string body = "<p>File: <b>" + file + "</b></p>";
                    foreach (string s in success)
                    {
                        body += s + "<br/>";
                    }
                    to_delete.Add(file);
                    SQLAccess.SQLAccess.SendMail(ConfigurationManager.AppSettings["EmailServer"].ToString()
                            , ConfigurationManager.AppSettings["EmailDatabase"].ToString()
                            , ConfigurationManager.AppSettings["EmailUsername"].ToString()
                            , ConfigurationManager.AppSettings["EmailPassword"].ToString()
                            , ConfigurationManager.AppSettings["TCEmailDistribution"].ToString()
                            , env + " Success Processing As Asm Change File"
                            , body);
                }
                else
                {
                    string body = "<p>File: <b>" + file + "</b></p>";
                    foreach (string error in errors)
                    {
                        body += "<p>Error: " + error + "</p>";
                    }
                    to_delete.Add(file);
                    SQLAccess.SQLAccess.SendMail(ConfigurationManager.AppSettings["EmailServer"].ToString()
                            , ConfigurationManager.AppSettings["EmailDatabase"].ToString()
                            , ConfigurationManager.AppSettings["EmailUsername"].ToString()
                            , ConfigurationManager.AppSettings["EmailPassword"].ToString()
                            , ConfigurationManager.AppSettings["TCEmailDistribution"].ToString()
                            , env + " Error Processing As Asm Change File"
                            , body);
                }

            }
            foreach (string file in to_delete)
            {
                try
                {
                    File.Delete(file);
                }
                catch (Exception ex)
                {
                    string body = "Error deleting processed file " + file;
                    body += ex.Message;
                    SQLAccess.SQLAccess.SendMail(ConfigurationManager.AppSettings["EmailServer"].ToString()
                        , ConfigurationManager.AppSettings["EmailDatabase"].ToString()
                        , ConfigurationManager.AppSettings["EmailUsername"].ToString()
                        , ConfigurationManager.AppSettings["EmailPassword"].ToString()
                        , ConfigurationManager.AppSettings["TCEmailDistribution"].ToString()
                        , env + " Error Processing As Asm Change File"
                        , body);
                }
            }
        }

        private void SendDailyErrorRecords(object state)
        {
            try
            {
                if (DateTime.Today.DayOfWeek >= DayOfWeek.Monday && DateTime.Today.DayOfWeek <= DayOfWeek.Friday)
                {
                    SqlCommand command = new SqlCommand(@"select p.partnum, p.revnum, pe.error, convert(varchar, pe.creationdate, 101) as [date]
                                                            from teamcenter.publishevent (nolock) as pe
                                                            inner join teamcenter.partinfo(nolock) as p on pe.publishid = p.publishid
                                                            inner join
                                                            (
                                                                select max(pe.publishid) as publishid, p.partnum
                                                                from teamcenter.publishevent (nolock) as pe
                                                                inner join teamcenter.partinfo(nolock) as p on pe.publishid = p.publishid
                                                                group by p.partnum
                                                            ) m on pe.publishid = m.publishid
                                                        where pe.status = 3
                                                        order by pe.creationdate desc");
                    DataSet ds = SQLAccess.SQLAccess.GetDataSet(ConfigurationManager.AppSettings["TCStageServer"].ToString(), ConfigurationManager.AppSettings["TCStageDatabase"].ToString(), ConfigurationManager.AppSettings["TCStageUsername"].ToString(), ConfigurationManager.AppSettings["TCStagePassword"].ToString(), command);

                    string table = "<table border =\"1\"><tr><th>Part #</th><th>Rev #</th><th>Error</th><th>Date</th></tr>";

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        table += "<tr><td>" + row[0].ToString() + "</td><td>" + row[1].ToString() + "</td><td>" + row[2].ToString() + "</td><td>" + row[3].ToString() + "</td></tr>";
                    }
                    table += "</table>";
                    
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        SQLAccess.SQLAccess.SendMail(ConfigurationManager.AppSettings["EmailServer"].ToString()
                            , ConfigurationManager.AppSettings["EmailDatabase"].ToString()
                            , ConfigurationManager.AppSettings["EmailUsername"].ToString()
                            , ConfigurationManager.AppSettings["EmailPassword"].ToString()
                            , ConfigurationManager.AppSettings["TCEmailDistribution"].ToString()
                            , env + " Daily TC Part Error State Alert"
                            , table);
                    }
                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        { 
            timer.Stop();

            #region Daily Notice Email
            try
            {
                if (lastDaily == null || (DateTime.Today > lastDaily))
                {
                    lastDaily = DateTime.Today;
                    ThreadPool.QueueUserWorkItem(new WaitCallback(SendDailyErrorRecords));
                }
            }
            catch(Exception ex)
            {
                lock (writer)
                {
                    writer.WriteLine(DateTime.Now.ToString() + " - Daily Notice Unhandled error: " + ex.Message);
                    writer.Flush();
                }

                try
                {
                    SQLAccess.SQLAccess.SendMail(ConfigurationManager.AppSettings["EmailServer"].ToString(), ConfigurationManager.AppSettings["EmailDatabase"].ToString(), ConfigurationManager.AppSettings["EmailUser"].ToString(), ConfigurationManager.AppSettings["EmailPassword"].ToString(),
                        ConfigurationManager.AppSettings["TCEmailDistribution"].ToString(), env + "Daily TC Notice Unhandled Error on " + ConfigurationManager.AppSettings["HostName"].ToString(), ex.Message);
                }
                catch (Exception subex)
                {

                }
            }
            #endregion

            #region AsAsm part changes

            try
            {
                ProcessAsAsmChanges();
            }
            catch (Exception ex)
            {
                lock (writer)
                {
                    writer.WriteLine(DateTime.Now.ToString() + " - ProcessAsAsmChanges Unhandled error: " + ex.Message);
                    writer.Flush();
                }

                try
                {
                    SQLAccess.SQLAccess.SendMail(ConfigurationManager.AppSettings["EmailServer"].ToString(), ConfigurationManager.AppSettings["EmailDatabase"].ToString(), ConfigurationManager.AppSettings["EmailUser"].ToString(), ConfigurationManager.AppSettings["EmailPassword"].ToString(),
                        ConfigurationManager.AppSettings["TCEmailDistribution"].ToString(), env + "Process AsAsm Changes Unhandled Error on " + ConfigurationManager.AppSettings["HostName"].ToString(), ex.Message);
                }
                catch (Exception subex)
                {

                }
            }

            #endregion


            DateTime start = DateTime.Now;

            List<PublishEvent> events = new List<PublishEvent>();
            List<PublishEvent> error = new List<PublishEvent>();
            List<PublishEvent> complete = new List<PublishEvent>();
            List<PartInfo> activations = new List<PartInfo>();

            string errors = "";

            try
            {

                events = bridge.GetPendingEvents(ConfigurationManager.AppSettings["TCStageServer"].ToString(), ConfigurationManager.AppSettings["TCStageDatabase"].ToString(), ConfigurationManager.AppSettings["TCStageUsername"].ToString(), ConfigurationManager.AppSettings["TCStagePassword"].ToString(), out errors);


                List<PublishEvent> incomplete = new List<PublishEvent>();


                foreach (PublishEvent ev in events)
                {
                    try
                    {
                        string str_error = "";
                        ev.SetPart(bridge.GetPartInfo(ConfigurationManager.AppSettings["TCStageServer"].ToString(), ConfigurationManager.AppSettings["TCStageDatabase"].ToString(), ConfigurationManager.AppSettings["TCStageUsername"].ToString(), ConfigurationManager.AppSettings["TCStagePassword"].ToString(), ev.PublishID, out str_error));
                        if (str_error.Length > 0)
                            throw new Exception(str_error);

                        uomInterface.CheckCreate(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), username, password, ev.Part.InvUOM);
                        uomClassInterface.Create(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), username, password, ev.Part.InvUOM);

                        if (partInterface.PartExists(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), username, password, ev.Part.PartNum))
                        {
                            partInterface.UpdateTCPart(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), username, password, ev.Part);
                        }
                        else
                        {
                            partInterface.CreateTCPart(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), username, password, ev.Part);
                            if (ev.Part.TypeCode == "P")
                            {
                                activations.Add(ev.Part);
                            }
                        }

                        try
                        {
                            if (String.IsNullOrEmpty(ev.Part.MakeFrom))
                                bridge.GetBOMInfo(ConfigurationManager.AppSettings["TCStageServer"].ToString(), ConfigurationManager.AppSettings["TCStageDatabase"].ToString(), ConfigurationManager.AppSettings["TCStageUsername"].ToString(), ConfigurationManager.AppSettings["TCStagePassword"].ToString(), ev.Part);
                        }
                        catch (Exception ex)
                        {
                            partInterface.UnapproveRevision(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), username, password, ev.Part.PartNum, ev.Part.RevNum);
                            throw ex;
                        }

                        if (ev.Part.TypeCode == "M" && ev.Part.BOM.Count == 0 && String.IsNullOrEmpty(ev.Part.MakeFrom))
                            throw new Exception("Manufactured part must have BOMINFO records or a MakeFrom");

                        if (ev.Part.BOM.Count > 0 || !String.IsNullOrEmpty(ev.Part.MakeFrom))
                        {
                            workBench.CreateTCRev(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), username, password, ev.Part);
                        }
                        if (ev.Part.ReleaseStatus == "PRODUCTION")
                            partInterface.UnapproveOtherRevision(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), username, password, ev.Part);

                        try
                        {
                            if (ev.Part.TypeCode == "M")
                            {
                                partInterface.ActivatePart(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), username, password, ev.Part.PartNum);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Error activating manufactured part: " + ex.Message);
                        }
                        ev.Complete();
                        complete.Add(ev);
                    }
                    catch (MissingPartException ex)
                    {
                        ev.SetError(ex.Message);
                        incomplete.Add(ev);
                    }
                    catch (Exception ex)
                    {
                        ev.SetError(ex.Message);
                        error.Add(ev);
                    }
                }

                List<PublishEvent> reprocess = new List<PublishEvent>();
                do
                {
                    reprocess.Clear();
                    foreach (PublishEvent ev in incomplete)
                        reprocess.Add(ev);
                    incomplete.Clear();
                    foreach (PublishEvent ev in reprocess)
                    {
                        try
                        {
                            workBench.CreateTCRev(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), username, password, ev.Part);
                            ev.Complete();
                            complete.Add(ev);
                        }
                        catch (MissingPartException ex)
                        {
                            ev.SetError(ex.Message);
                            incomplete.Add(ev);
                        }
                        catch (Exception ex)
                        {
                            ev.SetError(ex.Message);
                            error.Add(ev);
                        }
                    }
                } while (incomplete.Count < reprocess.Count);

                foreach (PublishEvent ev in incomplete)
                {
                    error.Add(ev);
                }
            }
            catch (Exception ex)
            {
                lock (writer)
                {
                    writer.WriteLine(DateTime.Now.ToString() + " - Unhandled error: " + ex.Message);
                    writer.Flush();
                }

                try
                {
                    SQLAccess.SQLAccess.SendMail(ConfigurationManager.AppSettings["EmailServer"].ToString(), ConfigurationManager.AppSettings["EmailDatabase"].ToString(), ConfigurationManager.AppSettings["EmailUser"].ToString(), ConfigurationManager.AppSettings["EmailPassword"].ToString(),
                        ConfigurationManager.AppSettings["TCEmailDistribution"].ToString(), env + " TC Bridge Unhandled Error on " + ConfigurationManager.AppSettings["HostName"].ToString(), ex.Message);
                }
                catch (Exception subex)
                {

                }
            }

            Dictionary<PublishEvent, string> complete_errors = new Dictionary<PublishEvent, string>();
            foreach (PublishEvent ev in complete)
            {
                try
                {
                    ev.WriteComplete(ConfigurationManager.AppSettings["TCStageServer"].ToString(), ConfigurationManager.AppSettings["TCStageDatabase"].ToString(), ConfigurationManager.AppSettings["TCStageUsername"].ToString(), ConfigurationManager.AppSettings["TCStagePassword"].ToString());
                }
                catch (Exception ex)
                {
                    lock (writer)
                    {
                        writer.WriteLine(DateTime.Now.ToString() + " - error writing Complete status: " + ex.Message);
                        writer.Flush();
                    }
                    complete_errors.Add(ev, ex.Message);
                }
            }

            Dictionary<PublishEvent, string> error_errors = new Dictionary<PublishEvent, string>();
            foreach (PublishEvent ev in error)
            {
                try
                {
                    ev.WriteError(ConfigurationManager.AppSettings["TCStageServer"].ToString(), ConfigurationManager.AppSettings["TCStageDatabase"].ToString(), ConfigurationManager.AppSettings["TCStageUsername"].ToString(), ConfigurationManager.AppSettings["TCStagePassword"].ToString());
                }
                catch (Exception ex)
                {
                    lock (writer)
                    {
                        writer.WriteLine(DateTime.Now.ToString() + " - error writing Error status: " + ex.Message);
                        writer.Flush();
                    }
                    error_errors.Add(ev, ex.Message);
                }
            }

            string body = "";

            try
            {
                if (!String.IsNullOrEmpty(errors))
                {
                    body += "<br/>Staging Table Errors: " + errors;
                }
                if (complete.Count > 0 && complete.Count != complete_errors.Count)
                {
                    body += "<br/><b>Complete Records</b><br/><table border=\"1\"><tr><th>Event ID</th><th>Part #</th><th>Rev</th><th>Release</th></tr>";
                    foreach (PublishEvent ev in complete)
                    {
                        if (!complete_errors.ContainsKey(ev))
                            body += "<tr><td>" + ev.EventID.ToString() + "</td><td>" + ev.Part.PartNum + "</td><td>" + ev.Part.RevNum + "</td><td>" + ev.Part.ReleaseStatus + "</td></tr>";
                    }
                    body += "</table>";
                }
                if (error.Count > 0 && error.Count != error_errors.Count)
                {
                    body += "<br/><b>Error Records</b><br/><table border=\"1\"><tr><th>Event ID</th><th>Part #</th><th>Rev</th><th>Release</th><th>Error</th></tr>";
                    foreach (PublishEvent ev in error)
                    {
                        if (!error_errors.ContainsKey(ev))
                        {
                            if (ev.Part == null)
                                body += "<tr><td>" + ev.EventID.ToString() + "</td><td>N/A</td><td>N/A</td><td>N/A</td><td>" + ev.Error + "</td></tr>";
                            else
                                body += "<tr><td>" + ev.EventID.ToString() + "</td><td>" + ev.Part.PartNum + "</td><td>" + ev.Part.RevNum + "</td><td>" + ev.Part.ReleaseStatus + "</td><td>" + ev.Error + "</td></tr>";
                        }
                    }
                    body += "</table>";

                }

                if (!String.IsNullOrEmpty(body))
                {
                    TimeSpan elapsed = (DateTime.Now - start);
                    body += "<br/>Elapsed time: " + elapsed.ToString(@"hh\:mm\:ss") + "<br/>";

                    SQLAccess.SQLAccess.SendMail(ConfigurationManager.AppSettings["EmailServer"].ToString(), ConfigurationManager.AppSettings["EmailDatabase"].ToString(), ConfigurationManager.AppSettings["EmailUsername"].ToString(), ConfigurationManager.AppSettings["EmailPassword"].ToString(),
                        ConfigurationManager.AppSettings["TCEmailDistribution"].ToString(), env + " TC Bridge Results", body);
                }

                body = "";
                if (activations.Count > 0)
                {
                    body += "<b>New E10 TC Purchased Parts Needing Activation</b>";
                    body += "<table border=\"1\"><tr><th>Part #</th><th>Description</th></tr>";
                    foreach (PartInfo part in activations)
                    {
                        body += "<tr><td>" + part.PartNum + "</td><td>" + part.PartDesc + "</td></tr>";
                    }
                    body += "</table>";

                    SQLAccess.SQLAccess.SendMail(ConfigurationManager.AppSettings["EmailServer"].ToString(), ConfigurationManager.AppSettings["EmailDatabase"].ToString(), ConfigurationManager.AppSettings["EmailUsername"].ToString(), ConfigurationManager.AppSettings["EmailPassword"].ToString(),
                        ConfigurationManager.AppSettings["TCActivationEmailDistribution"].ToString(), env + " New E10 TC Purchased Parts Needing Activation", body);
                }
            }
            catch (Exception ex)
            {
                lock (writer)
                {
                    writer.WriteLine(DateTime.Now.ToString() + " - error sending email: " + ex.Message + ":" + ex.StackTrace);
                    writer.Flush();
                }
            }
            foreach (PublishEvent ev in complete_errors.Keys)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(TryWriteComplete), new object[] { ev, complete_errors[ev] });
            }
            foreach (PublishEvent ev in error_errors.Keys)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(TryWriteComplete), new object[] { ev, complete_errors[ev] });
            }


            timer.Start();


        }

        private void TryWriteComplete(object state)
        {
            object[] data = state as object[];
            PublishEvent ev = data[0] as PublishEvent;
            string error = data[1] as string;

            SQLAccess.SQLAccess.SendMail(ConfigurationManager.AppSettings["EmailServer"].ToString(), ConfigurationManager.AppSettings["EmailDatabase"].ToString(), ConfigurationManager.AppSettings["EmailUser"].ToString(), ConfigurationManager.AppSettings["EmailPassword"].ToString(),
    ConfigurationManager.AppSettings["TCEmailDistribution"].ToString(), env + "TC Bridge " + ConfigurationManager.AppSettings["HostName"].ToString() + " Error Writing Complete State", "Event ID: " + ev.EventID.ToString() + ", error: " + error + ".  Auto-retry in 5 minutes.");
            Thread.Sleep(30000);
            try
            {
                ev.WriteComplete(ConfigurationManager.AppSettings["TCStageServer"].ToString(), ConfigurationManager.AppSettings["TCStageDatabase"].ToString(), ConfigurationManager.AppSettings["TCStageUsername"].ToString(), ConfigurationManager.AppSettings["TCStagePassword"].ToString());
            }
            catch (Exception ex)
            {
                lock (writer)
                {
                    writer.WriteLine(DateTime.Now.ToString() + " - error writing Complete status: " + ex.Message);
                    writer.Flush();
                }
                TryWriteComplete(new object[] { ev, ex.Message });
            }

        }

        private void TryWriteError(object state)
        {
            object[] data = state as object[];
            PublishEvent ev = data[0] as PublishEvent;
            string error = data[1] as string;

            SQLAccess.SQLAccess.SendMail(ConfigurationManager.AppSettings["EmailServer"].ToString(), ConfigurationManager.AppSettings["EmailDatabase"].ToString(), ConfigurationManager.AppSettings["EmailUser"].ToString(), ConfigurationManager.AppSettings["EmailPassword"].ToString(),
    ConfigurationManager.AppSettings["TCEmailDistribution"].ToString(), env + "TC Bridge " + ConfigurationManager.AppSettings["HostName"].ToString() + " Error Writing Error State", "Event ID: " + ev.EventID.ToString() + ", error: " + error + ".  Auto-retry in 5 minutes.");
            Thread.Sleep(30000);
            try
            {
                ev.WriteError(ConfigurationManager.AppSettings["TCStageServer"].ToString(), ConfigurationManager.AppSettings["TCStageDatabase"].ToString(), ConfigurationManager.AppSettings["TCStageUsername"].ToString(), ConfigurationManager.AppSettings["TCStagePassword"].ToString());
            }
            catch (Exception ex)
            {
                lock (writer)
                {
                    writer.WriteLine(DateTime.Now.ToString() + " - error writing Error status: " + ex.Message);
                    writer.Flush();
                }
                TryWriteError(new object[] { ev, ex.Message });
            }

        }

        #endregion
    }
}
