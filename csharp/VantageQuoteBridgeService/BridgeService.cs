using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using System.Data.SqlClient;
using System.IO;
using MySql.Data.MySqlClient;
using ObjectLibrary;
using System.Threading;


namespace VantageQuoteBridgeService
{
    public partial class BridgeService : ServiceBase
    {
        System.Timers.Timer timer;
        Queue<string> toBeProcessed;
        Queue<string> priorityToBeProcessed;
        int currThreadCount = 0;
        int maxThreadCount = 2;

        public BridgeService()
        {
            InitializeComponent();
            timer = new System.Timers.Timer(1000);
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            toBeProcessed = new Queue<string>();
            priorityToBeProcessed = new Queue<string>();
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                DataSet ds = MySQLAccess.GetDataSet("172.16.6.51", "wbq_development", "ror", "SomePW23", new MySqlCommand("SELECT id, vantage_status FROM Quote_Heads WHERE vantage_status = 'PENDING' or vantage_status = 'RESUBMIT'"));

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    string status = row["vantage_status"].ToString();
                    string id = row["id"].ToString();

                    if (status == "PENDING")
                    {
                        lock (toBeProcessed)
                        {
                            toBeProcessed.Enqueue(id);
                        }
                    }
                    else if (status == "RESUBMIT")
                    {
                        lock (priorityToBeProcessed)
                        {
                            priorityToBeProcessed.Enqueue(id);
                        }
                    }
                }

                for (int i = currThreadCount; i < maxThreadCount; i++)
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessRecord));
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                timer.Start();
            }

        }

        public void ProcessRecord(Object state)
        {
            string id = "";

            lock (priorityToBeProcessed)
            {
                if (priorityToBeProcessed.Count > 0)
                    id = priorityToBeProcessed.Dequeue();
            }

            if (id == "")
            {
                lock (toBeProcessed)
                {
                    if (toBeProcessed.Count > 0)
                        id = toBeProcessed.Dequeue();
                }
            }

            if (id != "")
            {
                try
                {
                    WriteVantageProcessing(id);

                    List<QuoteLine> lines = new List<QuoteLine>();

                    #region Read Quote Head Values (custid, ship to information, existing vantage quote num)

                    MySqlCommand command = new MySqlCommand("SELECT customer_custid, ship_to_num, ship_to_name, ship_to_address1, ship_to_address2, ship_to_address3, ship_to_city, ship_to_state, ship_to_zip, vantage_quotenum FROM quote_heads WHERE id = @ID");
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("ID", id);

                    DataSet custds = MySQLAccess.GetDataSet("172.16.6.51", "wbq_development", "ror", "SomePW23", command);
                    string custid = custds.Tables[0].Rows[0]["customer_custid"].ToString();
                    string shiptonum = custds.Tables[0].Rows[0]["ship_to_num"].ToString();
                    string stname = custds.Tables[0].Rows[0]["ship_to_name"].ToString();
                    string stadd1 = custds.Tables[0].Rows[0]["ship_to_address1"].ToString();
                    string stadd2 = custds.Tables[0].Rows[0]["ship_to_address2"].ToString();
                    string stadd3 = custds.Tables[0].Rows[0]["ship_to_address3"].ToString();
                    string stcity = custds.Tables[0].Rows[0]["ship_to_city"].ToString();
                    string ststate = custds.Tables[0].Rows[0]["ship_to_state"].ToString();
                    string stzip = custds.Tables[0].Rows[0]["ship_to_zip"].ToString();
                    int quotenum = Int32.Parse(custds.Tables[0].Rows[0]["vantage_quotenum"].ToString() == "" ? "0" : custds.Tables[0].Rows[0]["vantage_quotenum"].ToString());

                    #endregion

                    #region Read Quote Lines data

                    command = new MySqlCommand("SELECT id, discount_percent, part_number, price, adj_amt, adj_code, adj_comment, quantity, comments, int_comment from quote_lines where quote_head_id = @ID");
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("ID", id);

                    DataSet ds = MySQLAccess.GetDataSet("172.16.6.51", "wbq_development", "ror", "SomePW23", command);

                    int linenum = 0;

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        linenum++;
                        QuoteLine newline = new QuoteLine(row["part_number"].ToString(), Int32.Parse(row["quantity"].ToString()), Decimal.Parse(row["discount_percent"].ToString()), row["comments"].ToString(), row["int_comment"].ToString(), Decimal.Parse(row["price"].ToString()),
                            row["adj_amt"].ToString() == "" ? 0 : Decimal.Parse(row["adj_amt"].ToString()), row["adj_code"].ToString(), row["adj_comment"].ToString());

                        int lineid = Int32.Parse(row["id"].ToString());

                        int lineupid = 0;
                        DataSet lineupds;
                        DataSet lockhingeds;
                        DataSet hdconfigds;
                        if (newline.PartNum == "SYSTEM")
                        {
                            #region Read Line Up, Lock_Hinge, and HD_Door_Config data for configured part and build Configured Values

                            command = new MySqlCommand("SELECT id, type_id, size_id, number_of_doors, construction_id, finish_id, handle_id, light_id, shelf_type_id, shelf_color_id, post_id, single_endlight FROM line_ups WHERE quote_line_id = @ID");
                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("ID", lineid);

                            lineupds = MySQLAccess.GetDataSet("172.16.6.51", "wbq_development", "ror", "SomePW23", command);

                            lineupid = Int32.Parse(lineupds.Tables[0].Rows[0]["id"].ToString());

                            command = new MySqlCommand("SELECT hinge01, lock01, hinge02, lock02, hinge03, lock03, hinge04, lock04, hinge05, lock05, hinge06, lock06, hinge07, lock07, hinge08, lock08, hinge09, lock09, hinge10, lock10, hinge11, lock11, hinge12, lock12, hinge13, lock13, hinge14, lock14, hinge15, lock15, hinge16, lock16, hinge17, lock17, hinge18, lock18, hinge19, lock19, hinge20, lock20 FROM lock_hinges WHERE line_up_id = @ID");
                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("ID", lineupid);

                            lockhingeds = MySQLAccess.GetDataSet("172.16.6.51", "wbq_development", "ror", "SomePW23", command);

                            command = new MySqlCommand("SELECT remove_pushbar, full_silkscreen_color, kickplate_front, kickplate_back, bumper_guard_qty_front, bumper_guard_qty_back, bumper_guard_location_front, bumper_guard_location_back FROM hd_door_configs WHERE line_up_id = @ID");
                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("ID", lineupid);

                            hdconfigds = MySQLAccess.GetDataSet("172.16.6.51", "wbq_development", "ror", "SomePW23", command);

                            DataRow lurow = lineupds.Tables[0].Rows[0];

                            DataRow lhrow = null;
                            if (lockhingeds.Tables[0].Rows.Count > 0)
                                lhrow = lockhingeds.Tables[0].Rows[0];

                            DataRow hdrow = null;
                            if (hdconfigds.Tables[0].Rows.Count > 0)
                                hdrow = hdconfigds.Tables[0].Rows[0];

               /*             newline.PopulateSystemConfigValues(lurow["type_id"].ToString(), Int32.Parse(lurow["number_of_doors"].ToString()), lurow["size_id"].ToString(), lurow["construction_id"].ToString(), lurow["finish_id"].ToString(), lurow["light_id"].ToString(),
                                lurow["handle_id"].ToString(), lurow["shelf_type_id"].ToString(), lurow["shelf_color_id"].ToString(), lurow["post_id"].ToString(), lhrow == null ? "" : lhrow["hinge01"].ToString(), lhrow == null ? "" : lhrow["hinge02"].ToString(), lhrow == null ? "" : lhrow["hinge03"].ToString(), lhrow == null ? "" : lhrow["hinge04"].ToString(),
                                lhrow == null ? "" : lhrow["hinge05"].ToString(), lhrow == null ? "" : lhrow["hinge06"].ToString(), lhrow == null ? "" : lhrow["hinge07"].ToString(), lhrow == null ? "" : lhrow["hinge08"].ToString(), lhrow == null ? "" : lhrow["hinge09"].ToString(), lhrow == null ? "" : lhrow["hinge10"].ToString(), lhrow == null ? "" : lhrow["hinge11"].ToString(), lhrow == null ? "" : lhrow["hinge12"].ToString(),
                                lhrow == null ? "" : lhrow["hinge13"].ToString(), lhrow == null ? "" : lhrow["hinge14"].ToString(), lhrow == null ? "" : lhrow["hinge15"].ToString(), lhrow == null ? "" : lhrow["hinge16"].ToString(), lhrow == null ? "" : lhrow["hinge17"].ToString(), lhrow == null ? "" : lhrow["hinge18"].ToString(), lhrow == null ? "" : lhrow["hinge19"].ToString(), lhrow == null ? "" : lhrow["hinge20"].ToString(),
                                lhrow == null ? "" : lhrow["lock01"].ToString(), lhrow == null ? "" : lhrow["lock02"].ToString(), lhrow == null ? "" : lhrow["lock03"].ToString(), lhrow == null ? "" : lhrow["lock04"].ToString(), lhrow == null ? "" : lhrow["lock05"].ToString(), lhrow == null ? "" : lhrow["lock06"].ToString(), lhrow == null ? "" : lhrow["lock07"].ToString(), lhrow == null ? "" : lhrow["lock08"].ToString(),
                                lhrow == null ? "" : lhrow["lock09"].ToString(), lhrow == null ? "" : lhrow["lock10"].ToString(), lhrow == null ? "" : lhrow["lock11"].ToString(), lhrow == null ? "" : lhrow["lock12"].ToString(), lhrow == null ? "" : lhrow["lock13"].ToString(), lhrow == null ? "" : lhrow["lock14"].ToString(), lhrow == null ? "" : lhrow["lock15"].ToString(), lhrow == null ? "" : lhrow["lock16"].ToString(),
                                lhrow == null ? "" : lhrow["lock17"].ToString(), lhrow == null ? "" : lhrow["lock18"].ToString(), lhrow == null ? "" : lhrow["lock19"].ToString(), lhrow == null ? "" : lhrow["lock20"].ToString(), hdrow == null ? "" : hdrow["remove_pushbar"].ToString(), lurow["single_endlight"].ToString(), hdrow == null ? "" : hdrow["full_silkscreen_color"].ToString(),
                                hdrow == null ? "" : hdrow["kickplate_front"].ToString(), hdrow == null ? "" : hdrow["kickplate_back"].ToString(), hdrow == null ? "" : hdrow["bumper_guard_qty_front"].ToString(), hdrow == null ? "" : hdrow["bumper_guard_qty_back"].ToString(),
                                hdrow == null ? "" : hdrow["bumper_guard_location_front"].ToString(), hdrow == null ? "" : hdrow["bumper_guard_location_back"].ToString());
                            */
                            #endregion
                        }
                        lines.Add(newline);

                        DataSet shelfds;
                        if (newline.PartNum == "SYSTEM")
                        {
                            #region Read Shelf_Counts for system to, build Shelving Configured Values, and add additional line item

                            QuoteLine shelfline = new QuoteLine("SHELVING", Int32.Parse(row["quantity"].ToString()), Decimal.Parse(row["discount_percent"].ToString()), "", "", 0, 0, "", "");

                            command = new MySqlCommand("SELECT type, shelf_type_id, shelf_color_id, post_color_id, shelf_qty, post_qty, lane_div_qty, perim_grd_qty, glide_sht_qty, price_tag_mld_qty, base_qty, ext_bracket_qty FROM shelf_counts WHERE line_up_id = @ID");
                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("ID", lineupid);

                            shelfds = MySQLAccess.GetDataSet("172.16.6.51", "wbq_development", "ror", "SomePW23", command);

                            if (shelfds.Tables[0].Rows.Count > 0)
                            {
                                #region Build Shelf Config Values

                                string frame1shelftypeid = "0";
                                string frame1shelfcolorid = "0";
                                string frame1postcolorid = "0";
                                int frame1shelfqty = 0;
                                int frame1postqty = 0;
                                int frame1lanedivqty = 0;
                                int frame1perimgrdqty = 0;
                                int frame1glideshtqty = 0;
                                int frame1pricetagmldqty = 0;
                                int frame1baseqty = 0;
                                int frame1extbracketqty = 0;
                                string frame2shelftypeid = "0";
                                string frame2shelfcolorid = "0";
                                string frame2postcolorid = "0";
                                int frame2shelfqty = 0;
                                int frame2postqty = 0;
                                int frame2lanedivqty = 0;
                                int frame2perimgrdqty = 0;
                                int frame2glideshtqty = 0;
                                int frame2pricetagmldqty = 0;
                                int frame2baseqty = 0;
                                int frame2extbracketqty = 0;
                                string frame3shelftypeid = "0";
                                string frame3shelfcolorid = "0";
                                string frame3postcolorid = "0";
                                int frame3shelfqty = 0;
                                int frame3postqty = 0;
                                int frame3lanedivqty = 0;
                                int frame3perimgrdqty = 0;
                                int frame3glideshtqty = 0;
                                int frame3pricetagmldqty = 0;
                                int frame3baseqty = 0;
                                int frame3extbracketqty = 0;
                                string frame4shelftypeid = "0";
                                string frame4shelfcolorid = "0";
                                string frame4postcolorid = "0";
                                int frame4shelfqty = 0;
                                int frame4postqty = 0;
                                int frame4lanedivqty = 0;
                                int frame4perimgrdqty = 0;
                                int frame4glideshtqty = 0;
                                int frame4pricetagmldqty = 0;
                                int frame4baseqty = 0;
                                int frame4extbracketqty = 0;

                                foreach (DataRow srow in shelfds.Tables[0].Rows)
                                {
                                    switch (srow["type"].ToString())
                                    {
                                        case "FirstFrame":
                                            frame1shelftypeid = srow["shelf_type_id"].ToString() == "" ? "0" : srow["shelf_type_id"].ToString();
                                            frame1shelfcolorid = srow["shelf_color_id"].ToString() == "" ? "0" : srow["shelf_color_id"].ToString();
                                            frame1postcolorid = srow["post_color_id"].ToString() == "" ? "0" : srow["post_color_id"].ToString();
                                            frame1shelfqty = srow["shelf_qty"].ToString() == "" ? 0 : Int32.Parse(srow["shelf_qty"].ToString());
                                            frame1postqty = srow["post_qty"].ToString() == "" ? 0 : Int32.Parse(srow["post_qty"].ToString());
                                            frame1lanedivqty = srow["lane_div_qty"].ToString() == "" ? 0 : Int32.Parse(srow["lane_div_qty"].ToString());
                                            frame1perimgrdqty = srow["perim_grd_qty"].ToString() == "" ? 0 : Int32.Parse(srow["perim_grd_qty"].ToString());
                                            frame1glideshtqty = srow["glide_sht_qty"].ToString() == "" ? 0 : Int32.Parse(srow["glide_sht_qty"].ToString());
                                            frame1pricetagmldqty = srow["price_tag_mld_qty"].ToString() == "" ? 0 : Int32.Parse(srow["price_tag_mld_qty"].ToString());
                                            frame1baseqty = srow["base_qty"].ToString() == "" ? 0 : Int32.Parse(srow["base_qty"].ToString());
                                            frame1extbracketqty = srow["ext_bracket_qty"].ToString() == "" ? 0 : Int32.Parse(srow["ext_bracket_qty"].ToString());
                                            break;
                                        case "SecondFrame":
                                            frame2shelftypeid = srow["shelf_type_id"].ToString() == "" ? "0" : srow["shelf_type_id"].ToString();
                                            frame2shelfcolorid = srow["shelf_color_id"].ToString() == "" ? "0" : srow["shelf_color_id"].ToString();
                                            frame2postcolorid = srow["post_color_id"].ToString() == "" ? "0" : srow["post_color_id"].ToString();
                                            frame2shelfqty = srow["shelf_qty"].ToString() == "" ? 0 : Int32.Parse(srow["shelf_qty"].ToString());
                                            frame2postqty = srow["post_qty"].ToString() == "" ? 0 : Int32.Parse(srow["post_qty"].ToString());
                                            frame2lanedivqty = srow["lane_div_qty"].ToString() == "" ? 0 : Int32.Parse(srow["lane_div_qty"].ToString());
                                            frame2perimgrdqty = srow["perim_grd_qty"].ToString() == "" ? 0 : Int32.Parse(srow["perim_grd_qty"].ToString());
                                            frame2glideshtqty = srow["glide_sht_qty"].ToString() == "" ? 0 : Int32.Parse(srow["glide_sht_qty"].ToString());
                                            frame2pricetagmldqty = srow["price_tag_mld_qty"].ToString() == "" ? 0 : Int32.Parse(srow["price_tag_mld_qty"].ToString());
                                            frame2baseqty = srow["base_qty"].ToString() == "" ? 0 : Int32.Parse(srow["base_qty"].ToString());
                                            frame2extbracketqty = srow["ext_bracket_qty"].ToString() == "" ? 0 : Int32.Parse(srow["ext_bracket_qty"].ToString());
                                            break;
                                        case "ThirdFrame":
                                            frame3shelftypeid = srow["shelf_type_id"].ToString() == "" ? "0" : srow["shelf_type_id"].ToString();
                                            frame3shelfcolorid = srow["shelf_color_id"].ToString() == "" ? "0" : srow["shelf_color_id"].ToString();
                                            frame3postcolorid = srow["post_color_id"].ToString() == "" ? "0" : srow["post_color_id"].ToString();
                                            frame3shelfqty = srow["shelf_qty"].ToString() == "" ? 0 : Int32.Parse(srow["shelf_qty"].ToString());
                                            frame3postqty = srow["post_qty"].ToString() == "" ? 0 : Int32.Parse(srow["post_qty"].ToString());
                                            frame3lanedivqty = srow["lane_div_qty"].ToString() == "" ? 0 : Int32.Parse(srow["lane_div_qty"].ToString());
                                            frame3perimgrdqty = srow["perim_grd_qty"].ToString() == "" ? 0 : Int32.Parse(srow["perim_grd_qty"].ToString());
                                            frame3glideshtqty = srow["glide_sht_qty"].ToString() == "" ? 0 : Int32.Parse(srow["glide_sht_qty"].ToString());
                                            frame3pricetagmldqty = srow["price_tag_mld_qty"].ToString() == "" ? 0 : Int32.Parse(srow["price_tag_mld_qty"].ToString());
                                            frame3baseqty = srow["base_qty"].ToString() == "" ? 0 : Int32.Parse(srow["base_qty"].ToString());
                                            frame3extbracketqty = srow["ext_bracket_qty"].ToString() == "" ? 0 : Int32.Parse(srow["ext_bracket_qty"].ToString());
                                            break;
                                        case "FourthFrame":
                                            frame4shelftypeid = srow["shelf_type_id"].ToString() == "" ? "0" : srow["shelf_type_id"].ToString();
                                            frame4shelfcolorid = srow["shelf_color_id"].ToString() == "" ? "0" : srow["shelf_color_id"].ToString();
                                            frame4postcolorid = srow["post_color_id"].ToString() == "" ? "0" : srow["post_color_id"].ToString();
                                            frame4shelfqty = srow["shelf_qty"].ToString() == "" ? 0 : Int32.Parse(srow["shelf_qty"].ToString());
                                            frame4postqty = srow["post_qty"].ToString() == "" ? 0 : Int32.Parse(srow["post_qty"].ToString());
                                            frame4lanedivqty = srow["lane_div_qty"].ToString() == "" ? 0 : Int32.Parse(srow["lane_div_qty"].ToString());
                                            frame4perimgrdqty = srow["perim_grd_qty"].ToString() == "" ? 0 : Int32.Parse(srow["perim_grd_qty"].ToString());
                                            frame4glideshtqty = srow["glide_sht_qty"].ToString() == "" ? 0 : Int32.Parse(srow["glide_sht_qty"].ToString());
                                            frame4pricetagmldqty = srow["price_tag_mld_qty"].ToString() == "" ? 0 : Int32.Parse(srow["price_tag_mld_qty"].ToString());
                                            frame4baseqty = srow["base_qty"].ToString() == "" ? 0 : Int32.Parse(srow["base_qty"].ToString());
                                            frame4extbracketqty = srow["ext_bracket_qty"].ToString() == "" ? 0 : Int32.Parse(srow["ext_bracket_qty"].ToString());
                                            break;
                                    }
                                }

                                #endregion

                                shelfline.PopulateShelfConfigValues(linenum, frame1shelftypeid, frame1shelfcolorid, frame1postcolorid, frame1shelfqty, frame1postqty, frame1lanedivqty, frame1perimgrdqty, frame1glideshtqty, frame1pricetagmldqty, frame1baseqty,
                                    frame1extbracketqty, frame2shelftypeid, frame2shelfcolorid, frame2postcolorid, frame2shelfqty, frame2postqty, frame2lanedivqty, frame2perimgrdqty, frame2glideshtqty, frame2pricetagmldqty, frame2baseqty, frame2extbracketqty,
                                    frame3shelftypeid, frame3shelfcolorid, frame3postcolorid, frame3shelfqty, frame3postqty, frame3lanedivqty, frame3perimgrdqty, frame3glideshtqty, frame3pricetagmldqty, frame3baseqty, frame3extbracketqty, frame4shelftypeid,
                                    frame4shelfcolorid, frame4postcolorid, frame4shelfqty, frame4postqty, frame4lanedivqty, frame4perimgrdqty, frame4glideshtqty, frame4pricetagmldqty, frame4baseqty, frame4extbracketqty);


                                lines.Add(shelfline);
                                linenum++;
                            }

                            #endregion
                        }
                    }

                    #endregion

                    VantageBridgeInterface vantageBridgeInterface = new VantageBridgeInterface();
                 //   int processed_quotenum = vantageBridgeInterface.CreateQuote("jfunk", "vantage", quotenum, custid, shiptonum, stname, stadd1, stadd2, stadd3, stcity, ststate, stzip, lines);
        //            WriteVantageSuccess(id, processed_quotenum);
                }
                catch (Exception ex)
                {
                    WriteVantageError(id, ex.Message);
                }
            }
        }

        private void WriteVantageProcessing(string id)
        {
            MySqlCommand command = new MySqlCommand("UPDATE quote_heads SET vantage_status = 'PROCESSING', vantage_lastupdate = @TimeStamp WHERE id = @ID");
            command.Parameters.AddWithValue("TimeStamp", DateTime.Now.ToString());
            command.Parameters.AddWithValue("ID", id);

            MySQLAccess.NonQuery("172.16.6.51", "wbq_development", "ror", "SomePW23", command);
        }

        private void WriteVantageError(string id, string message)
        {
            MySqlCommand command = new MySqlCommand("UPDATE quote_heads SET vantage_status = 'ERROR', vantage_lastupdate = @TimeStamp, vantage_message = @Message WHERE id = @ID");
            command.Parameters.AddWithValue("TimeStamp", DateTime.Now.ToString());
            command.Parameters.AddWithValue("ID", id);
            command.Parameters.AddWithValue("Message", message);

            MySQLAccess.NonQuery("172.16.6.51", "wbq_development", "ror", "SomePW23", command);
        }

        private void WriteVantageSuccess(string id, int quotenum)
        {
            MySqlCommand command = new MySqlCommand("UPDATE quote_heads SET vantage_status = 'COMPLETE', vantage_lastupdate = @TimeStamp, @vantage_quotenum = @QuoteNum WHERE id = @ID");
            command.Parameters.AddWithValue("TimeStamp", DateTime.Now.ToString());
            command.Parameters.AddWithValue("ID", id);
            command.Parameters.AddWithValue("QuoteNum", quotenum);

            MySQLAccess.NonQuery("172.16.6.51", "wbq_development", "ror", "SomePW23", command);
        }

        protected override void OnStart(string[] args)
        {
            timer.Start();
        }

        protected override void OnStop()
        {
            timer.Stop();
        }
    }
}
