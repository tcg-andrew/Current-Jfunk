#region Usings

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.ServiceProcess;
using MySql.Data.MySqlClient;
using ObjectLibrary;

#endregion

namespace SOBasedScheduleServiceHost
{
    public partial class SOBasedScheduleService : ServiceBase
    {
        #region Values

        System.Timers.Timer timer;
        FileStream stream;
        StreamWriter writer;

        #endregion

        #region Constructors

        public SOBasedScheduleService()
        {
            InitializeComponent();
            timer = new System.Timers.Timer(3000);
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            stream = new FileStream("D:\\Installpoint\\Logs\\SOBasedScheduleServiceLog.txt", FileMode.OpenOrCreate);
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

        private void AppendUpdateString(ref string orig, string toadd)
        {
            if (!String.IsNullOrEmpty(orig))
                orig += ", ";
            orig += toadd;
        }

        private void CompareSOFields(DataRow row, SOBasedSchedule so, string company)
        {
            try
            {
                string updatestring = "";
                string tracestring = "";

                MySqlCommand command = new MySqlCommand();

                if (row["character01"].ToString() != so.Character01)
                {
                    AppendUpdateString(ref updatestring, "character01 = @Character01");
                    AppendUpdateString(ref tracestring, "character01 = '" + so.Character01.Replace("'", "") + "'");
                    command.Parameters.AddWithValue("Character01", so.Character01.Replace("'", ""));
                }
                if (row["character05"].ToString() != so.Character05)
                {
                    AppendUpdateString(ref updatestring, "character05 = @Character05");
                    AppendUpdateString(ref tracestring, "character05 = '" + so.Character05.Replace("'", "") + "'");
                    command.Parameters.AddWithValue("Character05", so.Character05.Replace("'", ""));
                }
                if (row["character07"].ToString() != so.Character07)
                {
                    AppendUpdateString(ref updatestring, "character07 = @Character07");
                    AppendUpdateString(ref tracestring, "character07 = '" + so.Character07.Replace("'", "") + "'");
                    command.Parameters.AddWithValue("Character07", so.Character07.Replace("'", ""));
                }
                if (row["crddate"].ToString() != so.CRDDate)
                {
                    AppendUpdateString(ref updatestring, "crddate = @CRDDate");
                    AppendUpdateString(ref tracestring, "crddate = '" + so.CRDDate + "'");
                    command.Parameters.AddWithValue("CRDDate", so.CRDDate);
                }
                if (row["custid"].ToString() != so.CustID)
                {
                    AppendUpdateString(ref updatestring, "custid = @CustID");
                    AppendUpdateString(ref tracestring, "custid = '" + so.CustID + "'");
                    command.Parameters.AddWithValue("CustID", so.CustID);
                }
                if (row["drawing"].ToString() != so.Drawing)
                {
                    AppendUpdateString(ref updatestring, "drawing = @Drawing");
                    AppendUpdateString(ref tracestring, "drawing = '" + so.Drawing + "'");
                    command.Parameters.AddWithValue("Drawing", so.Drawing);
                }
                if (row["drawnum"].ToString() != so.DrawNum)
                {
                    AppendUpdateString(ref updatestring, "drawnum = @Drawnum");
                    AppendUpdateString(ref tracestring, "drawnum = '" + so.DrawNum + "'");
                    command.Parameters.AddWithValue("Drawnum", so.DrawNum);
                }
                if (row["jobasmcommenttext"].ToString() != so.JobAsmCommentText)
                {
                    AppendUpdateString(ref updatestring, "jobasmcommenttext = @JobAsmComment");
                    AppendUpdateString(ref tracestring, "jobasmcommenttext = '" + so.JobAsmCommentText.Replace("'", "") + "'");
                    command.Parameters.AddWithValue("JobAsmComment", so.JobAsmCommentText.Replace("'", ""));
                }
                if (row["jobheadcommenttext"].ToString() != so.JobHeadCommentText)
                {
                    AppendUpdateString(ref updatestring, "jobheadcommenttext = @JobHeadComment");
                    AppendUpdateString(ref tracestring, "jobheadcommenttext = '" + so.JobHeadCommentText.Replace("'", "") + "'");
                    command.Parameters.AddWithValue("JobHeadComment", so.JobHeadCommentText.Replace("'", ""));
                }
                if (row["linedesc"].ToString() != so.LineDesc)
                {
                    AppendUpdateString(ref updatestring, "linedesc = @LineDesc");
                    AppendUpdateString(ref tracestring, "linedesc = '" + so.LineDesc.Replace("'", "") + "'");
                    command.Parameters.AddWithValue("LineDesc", so.LineDesc.Replace("'", ""));
                }
                if (row["name"].ToString() != so.Name)
                {
                    AppendUpdateString(ref updatestring, "name = @Name");
                    AppendUpdateString(ref tracestring, "name = '" + so.Name.Replace("'", "") + "'");
                    command.Parameters.AddWithValue("Name", so.Name.Replace("'", ""));
                }
                if (row["needbydate"].ToString() != so.NeedByDate)
                {
                    AppendUpdateString(ref updatestring, "needbydate = @NeedByDate");
                    AppendUpdateString(ref tracestring, "needbydate = '" + so.NeedByDate + "'");
                    command.Parameters.AddWithValue("NeedByDate", so.NeedByDate);
                }
                if (row["number01"].ToString() != so.Number01)
                {
                    AppendUpdateString(ref updatestring, "number01 = @Number01");
                    AppendUpdateString(ref tracestring, "number01 = '" + so.Number01 + "'");
                    command.Parameters.AddWithValue("Number01", so.Number01);
                }
                if (row["opcode"].ToString() != so.OpCode)
                {
                    AppendUpdateString(ref updatestring, "opcode = @OpCode");
                    AppendUpdateString(ref tracestring, "opcode = '" + so.OpCode + "'");
                    command.Parameters.AddWithValue("OpCode", so.OpCode);
                }
                if (row["openrelease"].ToString() != so.OpenRelease)
                {
                    AppendUpdateString(ref updatestring, "openrelease = @OpenRelease");
                    AppendUpdateString(ref tracestring, "openrelease = '" + so.OpenRelease + "'");
                    command.Parameters.AddWithValue("OpenRelease", so.OpenRelease);
                }
                if (row["orderline"].ToString() != so.OrderLine)
                {
                    AppendUpdateString(ref updatestring, "orderline = @OrderLine");
                    AppendUpdateString(ref tracestring, "orderline = '" + so.OrderLine + "'");
                    command.Parameters.AddWithValue("OrderLine", so.OrderLine);
                }
                if (row["ordernum"].ToString() != so.OrderNum)
                {
                    AppendUpdateString(ref updatestring, "ordernum = @OrderNum");
                    AppendUpdateString(ref tracestring, "ordernum = '" + so.OrderNum + "'");
                    command.Parameters.AddWithValue("OrderNum", so.OrderNum);
                }
                if (row["orderrelnum"].ToString() != so.OrderRelNum)
                {
                    AppendUpdateString(ref updatestring, "orderrelnum = @OrderRelNum");
                    AppendUpdateString(ref tracestring, "orderrelnum = '" + so.OrderRelNum + "'");
                    command.Parameters.AddWithValue("OrderRelNum", so.OrderRelNum);
                }
                if (row["ourjobshippedqty"].ToString() != so.OurJobShippedQty)
                {
                    AppendUpdateString(ref updatestring, "ourjobshippedqty = @OurJobShippedQty");
                    AppendUpdateString(ref tracestring, "ourjobshippedqty = '" + so.OurJobShippedQty + "'");
                    command.Parameters.AddWithValue("OurJobShippedQty", so.OurJobShippedQty);
                }
                if (row["ourreqqty"].ToString() != so.OurReqQty)
                {
                    AppendUpdateString(ref updatestring, "ourreqqty = @OurReqQty");
                    AppendUpdateString(ref tracestring, "ourreqqty = '" + so.OurReqQty + "'");
                    command.Parameters.AddWithValue("OurReqQty", so.OurReqQty);
                }
                if (row["ourstockshippedqty"].ToString() != so.OurStockShippedQty)
                {
                    AppendUpdateString(ref updatestring, "ourstockshippedqty = @OurStockShippedQty");
                    AppendUpdateString(ref tracestring, "ourstockshippedqty = '" + so.OurStockShippedQty + "'");
                    command.Parameters.AddWithValue("OurStockShippedQty", so.OurStockShippedQty);
                }
                if (row["outstandingqty"].ToString() != so.OutstandingQty)
                {
                    AppendUpdateString(ref updatestring, "outstandingqty = @OutstandingQty");
                    AppendUpdateString(ref tracestring, "outstandingqty = '" + so.OutstandingQty + "'");
                    command.Parameters.AddWithValue("OutstandingQty", so.OutstandingQty);
                }
                if (row["partnum"].ToString() != so.PartNum)
                {
                    AppendUpdateString(ref updatestring, "partnum = @Partnum");
                    AppendUpdateString(ref tracestring, "partnum = '" + so.PartNum + "'");
                    command.Parameters.AddWithValue("Partnum", so.PartNum);
                }
                if (row["picurl"].ToString() != so.PicUrl)
                {
                    AppendUpdateString(ref updatestring, "picurl = @Picurl");
                    AppendUpdateString(ref tracestring, "picurl = '" + so.PicUrl + "'");
                    command.Parameters.AddWithValue("Picurl", so.PicUrl);
                }
                if (row["ponum"].ToString() != so.PONum)
                {
                    AppendUpdateString(ref updatestring, "ponum = @PoNum");
                    AppendUpdateString(ref tracestring, "ponum = '" + so.PONum.Replace("'", "") + "'");
                    command.Parameters.AddWithValue("PoNum", so.PONum.Replace("'", ""));
                }
                if (row["qtycompleted"].ToString() != so.QtyCompleted)
                {
                    AppendUpdateString(ref updatestring, "qtycompleted = @QtyCompleted");
                    AppendUpdateString(ref tracestring, "qtycompleted = '" + so.QtyCompleted + "'");
                    command.Parameters.AddWithValue("QtyCompleted", so.QtyCompleted);
                }
                if (row["reqdate"].ToString() != so.ReqDate)
                {
                    AppendUpdateString(ref updatestring, "reqdate = @ReqDate");
                    AppendUpdateString(ref tracestring, "reqdate = '" + so.ReqDate + "'");
                    command.Parameters.AddWithValue("ReqDate", so.ReqDate);
                }
                if (row["revisionnum"].ToString() != so.RevisionNum)
                {
                    AppendUpdateString(ref updatestring, "revisionnum = @RevisionNum");
                    AppendUpdateString(ref tracestring, "revisionnum = '" + so.RevisionNum + "'");
                    command.Parameters.AddWithValue("RevisionNum", so.RevisionNum);
                }
                if (row["runqty"].ToString() != so.RunQty)
                {
                    AppendUpdateString(ref updatestring, "runqty = @RunQty");
                    AppendUpdateString(ref tracestring, "runqty = '" + so.RunQty + "'");
                    command.Parameters.AddWithValue("RunQty", so.RunQty);
                }
                if (row["shipviacode"].ToString() != so.ShipViaCode)
                {
                    AppendUpdateString(ref updatestring, "shipviacode = @ShipViaCode");
                    AppendUpdateString(ref tracestring, "shipviacode = '" + so.ShipViaCode + "'");
                    command.Parameters.AddWithValue("ShipViaCode", so.ShipViaCode);
                }
                if (row["shortchar01"].ToString() != so.ShortChar01)
                {
                    AppendUpdateString(ref updatestring, "shortchar01 = @Shortchar01");
                    AppendUpdateString(ref tracestring, "shortchar01 = '" + so.ShortChar01.Replace("'", "") + "'");
                    command.Parameters.AddWithValue("Shortchar01", so.ShortChar01.Replace("'", ""));
                }
                if (row["plant"].ToString() != so.Plant)
                {
                    AppendUpdateString(ref updatestring, "plant = @Plant");
                    AppendUpdateString(ref tracestring, "plant = '" + so.Plant + "'");
                    command.Parameters.AddWithValue("Plant", so.Plant);
                }
                if (row["csr"].ToString() != so.CSR)
                {
                    AppendUpdateString(ref updatestring, "csr = @CSR");
                    AppendUpdateString(ref tracestring, "csr = '" + so.CSR + "'");
                    command.Parameters.AddWithValue("CSR", so.CSR);
                }


                if (!String.IsNullOrEmpty(updatestring))
                {
                    using (MySqlConnection sqlConnection = new MySqlConnection(String.Format("Server = {0}; Database = {1}; uid = {2}; password = {3};", ConfigurationManager.AppSettings["WebProductionServer"].ToString(), ConfigurationManager.AppSettings["WebProductionDatabase"].ToString(), ConfigurationManager.AppSettings["WebProductionUsername"].ToString(), ConfigurationManager.AppSettings["WebProductionPassword"].ToString())))
                    {
                        command.Connection = sqlConnection;
                        string commtext = "update so_based_schedule_locals set " + updatestring + " where id = " + row["id"].ToString();
                        command.CommandText = commtext;
                        string tracetext = "update so_based_schedule_locals set " + tracestring + " where id = " + row["id"].ToString();

                        sqlConnection.Open();

                        command.ExecuteNonQuery();

                        sqlConnection.Close();

                        /*                        lock (writer)
                                                {
                                                    writer.WriteLine(DateTime.Now.ToString() + " - Updated MySql Row - " + tracetext);
                                                    writer.Flush();
                                                }*/
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CompareSOFields - " + ex.Message);
            }
        }

        private void DeleteSORow(DataRow row, string company)
        {
            try
            {
                using (MySqlConnection sqlConnection = new MySqlConnection(String.Format("Server = {0}; Database = {1}; uid = {2}; password = {3};", ConfigurationManager.AppSettings["WebProductionServer"].ToString(), ConfigurationManager.AppSettings["WebProductionDatabase"].ToString(), ConfigurationManager.AppSettings["WebProductionUsername"].ToString(), ConfigurationManager.AppSettings["WebProductionPassword"].ToString())))
                {
                    MySqlCommand command = sqlConnection.CreateCommand();
                    command.CommandText = "delete from so_based_schedule_locals where id = " + row["id"].ToString() + " and company = '" + company + "'";

                    sqlConnection.Open();

                    command.ExecuteNonQuery();

                    sqlConnection.Close();

                    /*                lock (writer)
                                    {
                                        writer.WriteLine(DateTime.Now.ToString() + " - Deleted MySql Row - " + row["id"].ToString() + " - Company: " + company);
                                        writer.Flush();
                                    }
                    */
                }
            }
            catch (Exception ex)
            {
                throw new Exception("DeleteSORow - " + ex.Message);
            }
        }

        private void AddSORow(SOBasedSchedule so, string company, DateTime starttime)
        {
            try
            {
                using (MySqlConnection sqlConnection = new MySqlConnection(String.Format("Server = {0}; Database = {1}; uid = {2}; password = {3};", ConfigurationManager.AppSettings["WebProductionServer"].ToString(), ConfigurationManager.AppSettings["WebProductionDatabase"].ToString(), ConfigurationManager.AppSettings["WebProductionUsername"].ToString(), ConfigurationManager.AppSettings["WebProductionPassword"].ToString())))
                {
                    MySqlCommand command = sqlConnection.CreateCommand();
                    string commtext = "insert into so_based_schedule_locals (assemblyseq, character01, character05, character07, crddate, custid, drawnum, jobasmcommenttext, jobheadcommenttext, jobnum, linedesc, name, needbydate, number01, opcode, openrelease, oprseq, orderline, ordernum, orderrelnum, ourjobshippedqty, ourreqqty, ourstockshippedqty, outstandingqty, partnum, picurl, ponum, qtycompleted, reqdate, revisionnum, runqty, shipviacode, shortchar01, company, progress_recid, drawing, csr, created_at) VALUES ";
                    commtext += "(@AssemblySeq, @Character01, @Character05, @Character07, @CRDDate, @CustID, @DrawNum, @JobAsmComment, @JobHeadComment, @JobNum, @LineDesc, @Name, @NeedByDate, @Number01, @OpCode, @OpenRelease, @OprSeq, @OrderLine, @Ordernum, @OrderRelNum, @OurJobShippedQty, @OurReqQty, @OurStockShippedQty, @OutstandingQty, @Partnum, @PicUrl, @PONum, @QtyCompleted, @ReqDate, @RevisionNum, @RunQty, @ShipViaCode, @ShortChar01, @Company, @Progress_Recid, @Drawing, @CSR, NOW())";
                    command.Parameters.AddWithValue("AssemblySeq", so.AssemblySeq);
                    command.Parameters.AddWithValue("Character01", so.Character01);
                    command.Parameters.AddWithValue("Character05", so.Character05);
                    command.Parameters.AddWithValue("Character07", so.Character07);
                    command.Parameters.AddWithValue("CRDDate", so.CRDDate);
                    command.Parameters.AddWithValue("CustID", so.CustID);
                    command.Parameters.AddWithValue("DrawNum", so.DrawNum);
                    command.Parameters.AddWithValue("JobAsmComment", so.JobAsmCommentText);
                    command.Parameters.AddWithValue("JobHeadComment", so.JobHeadCommentText);
                    command.Parameters.AddWithValue("JobNum", so.JobNum);
                    command.Parameters.AddWithValue("LineDesc", so.LineDesc);
                    command.Parameters.AddWithValue("Name", so.Name);
                    command.Parameters.AddWithValue("NeedByDate", so.NeedByDate);
                    command.Parameters.AddWithValue("Number01", so.Number01);
                    command.Parameters.AddWithValue("OpCode", so.OpCode);
                    command.Parameters.AddWithValue("OpenRelease", so.OpenRelease);
                    command.Parameters.AddWithValue("OprSeq", so.OprSeq);
                    command.Parameters.AddWithValue("OrderLine", so.OrderLine);
                    command.Parameters.AddWithValue("Ordernum", so.OrderNum);
                    command.Parameters.AddWithValue("OrderRelNum", so.OrderRelNum);
                    command.Parameters.AddWithValue("OurJobShippedQty", so.OurJobShippedQty);
                    command.Parameters.AddWithValue("OurReqQty", so.OurReqQty);
                    command.Parameters.AddWithValue("OurStockShippedQty", so.OurStockShippedQty);
                    command.Parameters.AddWithValue("OutstandingQty", so.OutstandingQty);
                    command.Parameters.AddWithValue("Partnum", so.PartNum);
                    command.Parameters.AddWithValue("PicUrl", so.PicUrl);
                    command.Parameters.AddWithValue("PONum", so.PONum);
                    command.Parameters.AddWithValue("QtyCompleted", so.QtyCompleted);
                    command.Parameters.AddWithValue("ReqDate", so.ReqDate);
                    command.Parameters.AddWithValue("RevisionNum", so.RevisionNum);
                    command.Parameters.AddWithValue("RunQty", so.RunQty);
                    command.Parameters.AddWithValue("ShipViaCode", so.ShipViaCode);
                    command.Parameters.AddWithValue("ShortChar01", so.ShortChar01);
                    command.Parameters.AddWithValue("Company", company);
                    command.Parameters.AddWithValue("Progress_recid", so.Progress_recid);
                    command.Parameters.AddWithValue("Drawing", so.Drawing);
                    command.Parameters.AddWithValue("CSR", so.CSR);
                    command.CommandText = commtext;

                    sqlConnection.Open();

                    command.ExecuteNonQuery();

                    sqlConnection.Close();

                    /*                lock (writer)
                                    {
                                        writer.WriteLine(DateTime.Now.ToString() + " - Inserted MySql Row - " + commtext);
                                        writer.Flush();
                                    }*/
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AddSORow - " + ex.Message);
            }
        }

        private void SyncSO(string company)
        {
            ProductionInterface pi = new ProductionInterface();
            DataSet ds = new DataSet();
            DateTime starttime = DateTime.UtcNow;
            try
            {

                using (MySqlConnection sqlConnection = new MySqlConnection(String.Format("Server = {0}; Database = {1}; uid = {2}; password = {3};", ConfigurationManager.AppSettings["WebProductionServer"].ToString(), ConfigurationManager.AppSettings["WebProductionDatabase"].ToString(), ConfigurationManager.AppSettings["WebProductionUsername"].ToString(), ConfigurationManager.AppSettings["WebProductionPassword"].ToString())))
                {
                    MySqlCommand command = sqlConnection.CreateCommand();
                    command.CommandText = "select * from so_based_schedule_locals where company = @Company";
                    command.Parameters.AddWithValue("Company", company);
                    command.CommandTimeout = 300;

                    sqlConnection.Open();

                    // Get Rails side list of SO Sched
                    MySqlDataAdapter sda = new MySqlDataAdapter(command);
                    sda.Fill(ds);

                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("WBP Query - " + ex.Message);
            }

            // Get SQL side list of SO Sched
            List<SOBasedSchedule> so = pi.GetSOBasedSchedule(ConfigurationManager.AppSettings["EpicorDatabaseServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), ConfigurationManager.AppSettings["EpicorUsername"].ToString(), ConfigurationManager.AppSettings["EpicorPassword"].ToString(), company);

            // Check if each Rails side record is still active
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                bool found = false;
                foreach (SOBasedSchedule sched in so)
                {
                    if (row["progress_recid"].ToString() == sched.Progress_recid)
                    {
                        found = true;

                        if (String.IsNullOrEmpty(row["updated_at"].ToString()) || (starttime - DateTime.Parse(row["updated_at"].ToString())).TotalSeconds > 180)
                        {
                            // if it was found, sync all the fields
                            CompareSOFields(row, sched, company);
                        }
                        else
                        {
/*                            if (!String.IsNullOrEmpty(row["updated_at"].ToString()))
                            {
                            lock (writer)
                            {
                                writer.WriteLine(DateTime.Now.ToString() + " - Skipping compare, Record update check failed: Rails updated time = " + row["updated_at"].ToString() + ", process start time = " + starttime.ToString() + ", difference = " + (starttime - DateTime.Parse(row["updated_at"].ToString())).TotalSeconds.ToString() + " seconds");
                                writer.Flush();
                            }
                            }*/
                        }
                        break;
                    }
                }
                if (!found)
                    DeleteSORow(row, company);
            }

            // Check if each SQL side record is in the Rails side
            foreach (SOBasedSchedule sched in so)
            {
                bool found = false;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (row["progress_recid"].ToString() == sched.Progress_recid)
                    {
                        found = true;
                        // We already updated any matching rows, so no need to do that again
                        break;
                    }
                }
                if (!found)
                    AddSORow(sched, company, starttime);
            }
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                timer.Stop();
                SyncSO("CRD");
                SyncSO("CIG");
                SyncSO("NEM");
                SyncSO("PRC1");
                timer.Start();
            }
            catch (Exception ex)
            {
                lock (writer)
                {
                    writer.WriteLine(DateTime.Now.ToString() + " - Error processing update: " + ex.Message);
                    writer.Flush();
                }
                throw ex;
            }
            finally
            {
                timer.Start();
            }
        }

        #endregion
    }
}
