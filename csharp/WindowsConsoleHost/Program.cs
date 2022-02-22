using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using ServiceLibrary;
using System.ServiceModel.Description;
using FlatWSDL;
using ObjectLibrary;
using MySql.Data.MySqlClient;
using System.Data;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using System.Xml;
using System.Web;
using System.Data.SqlClient;

namespace WindowsConsoleHost
{
    class Program
    {
        static FileStream stream;
        static StreamWriter writer;

        static string username = "CRDService";
        static string password = "gfd723trajsdc97";

        static void Main(string[] args)
        {
            stream = new FileStream("C:\\VantageBridgeServiceTestLog.txt", FileMode.OpenOrCreate);
            stream.Position = stream.Length;
            writer = new StreamWriter(stream);

            //     FlatWSDLServiceHost abchost = new FlatWSDLServiceHost(typeof(ABCCodeService));
         //   FlatWSDLServiceHost parthost = new FlatWSDLServiceHost(typeof(PartService));
        //    FlatWSDLServiceHost freighthost = new FlatWSDLServiceHost(typeof(FreightRateService));
     //       FlatWSDLServiceHost quotehost = new FlatWSDLServiceHost(typeof(QuoteService));
//FlatWSDLServiceHost laborhost = new FlatWSDLServiceHost(typeof(LaborService));
//            FlatWSDLServiceHost customerservicehost = new FlatWSDLServiceHost(typeof(CustomerService));

//            FlatWSDLServiceHost foxprohost = new FlatWSDLServiceHost(typeof(FoxProService));
            //         FlatWSDLServiceHost productionhost = new FlatWSDLServiceHost(typeof(ProductionService));
//            FlatWSDLServiceHost pahost = new FlatWSDLServiceHost(typeof(PowerAnalyzerService));

       //     abchost.Open();
       //     parthost.Open();
        //    freighthost.Open();
            //quotehost.Open();
  //          laborhost.Open();
//            customerservicehost.Open();
            //foxprohost.Open();
            //productionhost.Open();
      //      pahost.Open();

    //        Console.WriteLine("Service is running....");

            Console.WriteLine("Start - " + DateTime.Now.ToShortTimeString());
            TestBridge("75511");
            Console.WriteLine("Stop - " + DateTime.Now.ToShortTimeString());
            //      Console.ReadLine();

//            abchost.Close();
  //          parthost.Close();
    //        freighthost.Close();
            //quotehost.Close();
        //    laborhost.Close();
//            customerservicehost.Close();
//            foxprohost.Close();
            //productionhost.Close();
            //pahost.Close();
        }

        private static bool AddNewMissingPart(int quotenum, int line, string part, string desc, DateTime batch)
        {
            bool result = false;
            try
            {
                SqlCommand command = new SqlCommand("exec dbo.QuoteMissingParts_Insert @QuoteNum, @QuoteLine, @Part, @Desc, @Created");
                command.Parameters.AddWithValue("QuoteNum", quotenum);
                command.Parameters.AddWithValue("QuoteLine", line);
                command.Parameters.AddWithValue("Part", part);
                if (String.IsNullOrEmpty(desc))
                    command.Parameters.AddWithValue("Desc", DBNull.Value);
                else
                    command.Parameters.AddWithValue("Desc", desc);
                command.Parameters.AddWithValue("Created", batch);

                result = SQLAccess.GetScalar(ConfigurationManager.AppSettings["EpicorDatabaseServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), ConfigurationManager.AppSettings["EpicorUsername"].ToString(), ConfigurationManager.AppSettings["EpicorPassword"].ToString(), command) == "1";
            }
            catch (Exception ex)
            {
                lock (writer)
                {
                    writer.WriteLine(DateTime.Now.ToString() + " - Error adding new missing part entry: " + ex.Message + ":" + ex.StackTrace);
                    writer.Flush();
                }
            }
            return result;
        }

        private static void AddNewAsmRequest(string missing, XmlQuoteAsm asm, XmlQuoteParse line)
        {
            try
            {
                string filename = "prrequest" + missing + ".txt.";

                string[] lines = {
                "Customer Name|CRD Internal Use House Account",
                "Customer Info|CRD Internal Use House Account",
                "Requested Date|" + DateTime.Now.ToString("dd-MMM-yyyy HH:mm"),
                "System|" + line.System,
                "Product Type|" + line.ProdCode,
                "Temperature|" + line.Temperature,
                "Color|" + line.Color,
                "Number of Doors|" + asm.NumDoors,
                "Swing Configuration|" + asm.Swing,
                "Texture|" + line.Texture,
                "Lights|" + line.Lights,
                "Locks|" + line.Locks,
                "Handle|" + line.Handle,
                "PR Name|Missing Assembly:" + missing,
                "PR Description|" + asm.Description.Split(new string[] { "[*]" }, StringSplitOptions.None)[0]
                };
                /*System.IO.File.WriteAllLines("\\\\stlv-apptcd17\\tc12data\\crd_utilities\\crd_pr_auto_generate\\source\\" + filename, lines);

                SqlCommand command = new SqlCommand("exec dbo.QuoteRequestedParts_Insert @Part");
                command.Parameters.AddWithValue("Part", missing);
                SQLAccess.NonQuery(ConfigurationManager.AppSettings["EpicorDatabaseServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), ConfigurationManager.AppSettings["EpicorUsername"].ToString(), ConfigurationManager.AppSettings["EpicorPassword"].ToString(), command);*/
            }
            catch (Exception ex)
            {
                lock (writer)
                {
                    writer.WriteLine(DateTime.Now.ToString() + " - Error adding new missing part request: " + ex.Message + ":" + ex.StackTrace);
                    writer.Flush();
                }
            }

        }

        static private void SendBatchMissingPartNotice(DateTime batch)
        {
            try
            {
                SqlCommand command = new SqlCommand("exec dbo.QuoteMissingParts_Notify @Batch, @Distro");
                command.Parameters.AddWithValue("Batch", batch);
                command.Parameters.AddWithValue("Distro", ConfigurationManager.AppSettings["QuoteMissingPartsDistribution"]);

                SQLAccess.NonQuery(ConfigurationManager.AppSettings["EpicorDatabaseServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), ConfigurationManager.AppSettings["EpicorUsername"].ToString(), ConfigurationManager.AppSettings["EpicorPassword"].ToString(), command);
            }
            catch (Exception ex)
            {
                lock (writer)
                {
                    writer.WriteLine(DateTime.Now.ToString() + " - Error sending batch missing part notice: " + ex.Message);
                    writer.Flush();
                }
            }
        }

        static private void RemoveMissingPartNotice(int quotenum)
        {
            try
            {
                SqlCommand command = new SqlCommand("exec QuoteMissingParts_Remove @QuoteNum");
                command.Parameters.AddWithValue("QuoteNum", quotenum);

                SQLAccess.NonQuery(ConfigurationManager.AppSettings["EpicorDatabaseServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), ConfigurationManager.AppSettings["EpicorUsername"].ToString(), ConfigurationManager.AppSettings["EpicorPassword"].ToString(), command);
            }
            catch (Exception ex)
            {
                lock (writer)
                {
                    writer.WriteLine(DateTime.Now.ToString() + " - Error removing missing part notice for quote # " + quotenum.ToString() + ": " + ex.Message);
                    writer.Flush();
                }
            }

        }

        static void TestBridge(string id)
        {
            DateTime batch = DateTime.Now;

            if (id != "")
            {
                try
                {
                    WriteVantageProcessing(id);

                    bool skip_price_validation = false;
                    List<QuoteLine> lines = new List<QuoteLine>();

                    #region Read Quote Head Values (custid, ship to information, existing vantage quote num)

                    MySqlCommand command = new MySqlCommand("SELECT customer_custid, comments, ship_to_num, ship_to_name, ship_to_address1, ship_to_address2, ship_to_address3, ship_to_city, ship_to_state, ship_to_zip, vantage_quotenum, vantage_message, csrs.name, freight_carrier, freight_transit, job_reference, quote_heads.user_id, quote_heads.ship_to_email, quote_heads.ship_to_email_cc, quote_heads.salesrepcode FROM quote_heads left join users on quote_heads.user_id = users.id left join csrs on users.email = csrs.user_name WHERE quote_heads.id = @ID");
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("ID", id);

                    DataSet custds = MySQLAccess.GetDataSet(ConfigurationManager.AppSettings["WebQuoteServer"].ToString(), ConfigurationManager.AppSettings["WebQuoteDatabase"].ToString(), ConfigurationManager.AppSettings["WebQuoteUsername"].ToString(), ConfigurationManager.AppSettings["WebQuotePassword"].ToString(), command);
                    string custid = custds.Tables[0].Rows[0]["customer_custid"].ToString();
                    string shiptonum = custds.Tables[0].Rows[0]["ship_to_num"].ToString();
                    string stname = custds.Tables[0].Rows[0]["ship_to_name"].ToString();
                    string stadd1 = custds.Tables[0].Rows[0]["ship_to_address1"].ToString();
                    string stadd2 = custds.Tables[0].Rows[0]["ship_to_address2"].ToString();
                    string stadd3 = custds.Tables[0].Rows[0]["ship_to_address3"].ToString();
                    string stcity = custds.Tables[0].Rows[0]["ship_to_city"].ToString();
                    string ststate = custds.Tables[0].Rows[0]["ship_to_state"].ToString();
                    string stzip = custds.Tables[0].Rows[0]["ship_to_zip"].ToString();
                    string comments = custds.Tables[0].Rows[0]["comments"].ToString();
                    string csr = custds.Tables[0].Rows[0]["name"].ToString();
                    int quotenum = Int32.Parse(custds.Tables[0].Rows[0]["vantage_quotenum"].ToString() == "" ? "0" : custds.Tables[0].Rows[0]["vantage_quotenum"].ToString());
                    string preverror = custds.Tables[0].Rows[0]["vantage_message"].ToString();
                    string carrier = TranslateCarrier(custds.Tables[0].Rows[0]["freight_carrier"].ToString());
                    string jobref = custds.Tables[0].Rows[0]["job_reference"].ToString();
                    string userid = custds.Tables[0].Rows[0]["user_id"].ToString();
                    string stemail = custds.Tables[0].Rows[0]["ship_to_email"].ToString();
                    string stemailcc = custds.Tables[0].Rows[0]["ship_to_email_cc"].ToString();
                    string salesrepcode = custds.Tables[0].Rows[0]["salesrepcode"].ToString();
                    int transitdays = 0;
                    try
                    {
                        transitdays = Int32.Parse(custds.Tables[0].Rows[0]["freight_transit"].ToString());
                    }
                    catch (Exception ex)
                    {
                    }

                    Regex quotenum_reg = new Regex(@"\(QUOTENUM (\d*)\)");
                    Match quotenum_match = quotenum_reg.Match(preverror);

                    if (quotenum_match.Groups.Count > 1)
                        quotenum = Int32.Parse(quotenum_match.Groups[1].Value);

                    #endregion

                    #region Read Quote Lines data

                    command = new MySqlCommand("SELECT id, description, discount_percent, part_number, price, list_price, adj_amt, adj_code, adj_comment, adj_amt_shelving, adj_code_shelving, adj_comment_shelving, quantity, comments, int_comment, inlay_color, nco, created_at, modline_part_number, wbc_ref_id, system_weight, vantage_line_num, vantage_shelving_line_num from quote_lines where quote_head_id = @ID and quantity > 0 ");
                    //                    command = new MySqlCommand("SELECT id, description, discount_percent, part_number, price, list_price, adj_amt, adj_code, adj_comment, adj_amt_shelving, adj_code_shelving, adj_comment_shelving, quantity, comments, int_comment, inlay_color, nco, created_at, modline_part_number, system_weight, vantage_line_num, vantage_shelving_line_num from quote_lines where quote_head_id = @ID and quantity > 0 ");
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("ID", id);

                    DataSet ds = MySQLAccess.GetDataSet(ConfigurationManager.AppSettings["WebQuoteServer"].ToString(), ConfigurationManager.AppSettings["WebQuoteDatabase"].ToString(), ConfigurationManager.AppSettings["WebQuoteUsername"].ToString(), ConfigurationManager.AppSettings["WebQuotePassword"].ToString(), command);

                    decimal total_price = 0;
                    int linenum = 0;
                    bool process = true;
                    bool missing = false;

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        linenum++;
                        if (!String.IsNullOrEmpty(row["wbc_ref_id"].ToString()))
                        {
                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["WBCServer"].ToString() + "/instances/output.xml?ref_id=" + HttpUtility.HtmlEncode(row["wbc_ref_id"].ToString()) + "&password=" + userid);
                            request.ContentType = "application/json";
                            request.ContentLength = 0;
                            request.Timeout = Int32.MaxValue;
                            request.Method = "Get";
                            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.Load(response.GetResponseStream());
                            XmlQuoteParse quoteparse = new XmlQuoteParse(xmlDoc);

                            PartInterface partInterface = new PartInterface();
                            foreach (XmlQuoteAsm asm in quoteparse.Assemblies)
                            {
                                if (!asm.Name.StartsWith("SLDOOR") && !asm.Name.StartsWith("SLFRAME") && !partInterface.PartExists(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), username, password, asm.Name))
                                {
                                    if (AddNewMissingPart(Int32.Parse(id), linenum, asm.Name, asm.Description, batch))
                                        AddNewAsmRequest(asm.Name, asm, quoteparse);
                                    missing = true;
                                }
                                foreach (XmlQuoteMtl mtl in asm.SubAssemblies)
                                {
                                    if (!mtl.Name.StartsWith("SLDOOR") && !mtl.Name.StartsWith("SLFRAME") && !partInterface.PartExists(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), username, password, mtl.Name))
                                    {
                                        if (AddNewMissingPart(Int32.Parse(id), linenum, mtl.Name, "", batch))
                                            AddNewAsmRequest(mtl.Name, asm, quoteparse);
                                        missing = true;
                                    }
                                }
                            }

                            QuoteLine newline = new QuoteLine(row["part_number"].ToString(), Int32.Parse(row["quantity"].ToString()), quoteparse.Description/*row["description"].ToString()*/, row["discount_percent"].ToString() == "" ? 0 : Decimal.Parse(row["discount_percent"].ToString()), row["comments"].ToString(), row["int_comment"].ToString(), Decimal.Parse(row["list_price"].ToString()),
                                row["adj_amt"].ToString() == "" ? 0 : Decimal.Parse(row["adj_amt"].ToString()), row["adj_code"].ToString(), row["adj_comment"].ToString(), row["nco"].ToString(), DateTime.Parse(row["created_at"].ToString()), row["modline_part_number"].ToString(), row["system_weight"].ToString(),
                                row["vantage_line_num"].ToString() == "" ? 0 : Int32.Parse(row["vantage_line_num"].ToString()), Int32.Parse(row["id"].ToString()), "", false, true, quoteparse);
                            lines.Add(newline);

                            total_price += Decimal.Parse(row["price"].ToString());
                            int shelfline = 0;
                            /*foreach (string part in quoteparse.Lines.Keys)*/
                            foreach (XmlQuoteLine qline in quoteparse._Lines)
                            {
                                QuoteLine newlooseline = null;
                                if (qline.Template == "SHELF")
                                {
                                    if (shelfline == 0)
                                    {
                                        newlooseline = new QuoteLine(qline.PartNum, qline.Qty * Int32.Parse(row["quantity"].ToString()), "", row["discount_percent"].ToString() == "" ? 0 : Decimal.Parse(row["discount_percent"].ToString()), "", "", quoteparse.ShelfPrice / qline.Qty, row["adj_amt_shelving"].ToString() == "" ? 0 : Decimal.Parse(row["adj_amt_shelving"].ToString()), row["adj_code_shelving"].ToString(), row["adj_comment_shelving"].ToString(), "", DateTime.Parse(row["created_at"].ToString()), "", "", row["vantage_line_num"].ToString() == "" ? 0 : Int32.Parse(row["vantage_line_num"].ToString()), Int32.Parse(row["id"].ToString()), "", false, false, null, true, true);
                                        total_price += Decimal.Parse(row["price"].ToString());
                                    }
                                    else
                                        newlooseline = new QuoteLine(qline.PartNum, qline.Qty * Int32.Parse(row["quantity"].ToString()), "", 0, "", "", 0, 0, "", "", "", DateTime.Parse(row["created_at"].ToString()), "", "", row["vantage_line_num"].ToString() == "" ? 0 : Int32.Parse(row["vantage_line_num"].ToString()), Int32.Parse(row["id"].ToString()), "", false, false, null, true);
                                    lines.Add(newlooseline);
                                    shelfline++;
                                }
                                else
                                {
                                    newlooseline = new QuoteLine(qline.PartNum, qline.Qty * Int32.Parse(row["quantity"].ToString()), qline.Description, 0, "", "", 0, 0, "", "", "", DateTime.Parse(row["created_at"].ToString()), "", "", row["vantage_line_num"].ToString() == "" ? 0 : Int32.Parse(row["vantage_line_num"].ToString()), Int32.Parse(row["id"].ToString()), "FX", false, false, quoteparse, true);
                                    lines.Add(newlooseline);
                                }
                            }
                        }
                        else if (row["part_number"].ToString() == "MISCPART" && !String.IsNullOrEmpty(row["modline_part_number"].ToString()))
                        {
                            string modline_hash = row["modline_part_number"].ToString();
                            modline_hash = modline_hash.Substring(1, modline_hash.Length - 3);
                            string[] hash_parts = modline_hash.Split(new string[] { "}, " }, StringSplitOptions.None);
                            bool first = true;
                            foreach (string hash_part in hash_parts)
                            {
                                if (hash_part.IndexOf("{") < hash_part.Length - 1)
                                {
                                    string clean_hash_part = hash_part.Substring(hash_part.IndexOf("{") + 1);
                                    clean_hash_part = clean_hash_part.Replace("\"", "");
                                    string[] modline_parts = clean_hash_part.Split(new string[] { ", " }, StringSplitOptions.None);
                                    foreach (string modline_part in modline_parts)
                                    {
                                        string part = modline_part.Split(new string[] { "=>" }, StringSplitOptions.None)[0];
                                        int qty = Int32.Parse(modline_part.Split(new string[] { "=>" }, StringSplitOptions.None)[1]);

                                        QuoteLine newline = new QuoteLine(row["part_number"].ToString(), qty, "", row["discount_percent"].ToString() == "" ? 0 : Decimal.Parse(row["discount_percent"].ToString()), (first ? row["description"].ToString() + "\n\n" : "") + row["comments"].ToString(), row["int_comment"].ToString(), Decimal.Parse(row["list_price"].ToString()),
                                        row["adj_amt"].ToString() == "" ? 0 : Decimal.Parse(row["adj_amt"].ToString()), row["adj_code"].ToString(), row["adj_comment"].ToString(), row["nco"].ToString(), DateTime.Parse(row["created_at"].ToString()), part, row["system_weight"].ToString(),
                                        row["vantage_line_num"].ToString() == "" ? 0 : Int32.Parse(row["vantage_line_num"].ToString()), Int32.Parse(row["id"].ToString()), "", first/*, Boolean.Parse(row["matrix_price"].ToString())*/);
                                        first = false;
                                        lines.Add(newline);
                                    }
                                }
                            }
                        }
                        else
                        {

                            QuoteLine newline = new QuoteLine(row["part_number"].ToString(), Int32.Parse(row["quantity"].ToString()), row["description"].ToString(), row["discount_percent"].ToString() == "" ? 0 : Decimal.Parse(row["discount_percent"].ToString()), row["comments"].ToString(), row["int_comment"].ToString(), Decimal.Parse(row["list_price"].ToString()),
                                row["adj_amt"].ToString() == "" ? 0 : Decimal.Parse(row["adj_amt"].ToString()), row["adj_code"].ToString(), row["adj_comment"].ToString(), row["nco"].ToString(), DateTime.Parse(row["created_at"].ToString()), row["modline_part_number"].ToString(), row["system_weight"].ToString(),
                                row["vantage_line_num"].ToString() == "" ? 0 : Int32.Parse(row["vantage_line_num"].ToString()), Int32.Parse(row["id"].ToString()), ""/*, Boolean.Parse(row["matrix_price"].ToString())*/);

                            lines.Add(newline);

                            if (newline.ActualPartNum == "MISCPART")
                                throw new Exception("Cannot bridge MISCPART.  Please submit Hard MOM instead");

                            int lineid = Int32.Parse(row["id"].ToString());

                            int lineupid = 0;
                            DataSet lineupds;
                            DataSet lockhingeds;
                            DataSet hdconfigds;
                            DataSet shelfds;

                            if (newline.ActualPartNum == "SYSTEM")
                            {
                                #region Read Line Up, Lock_Hinge, and HD_Door_Config data for configured part and build Configured Values

                                command = new MySqlCommand("SELECT id, type_id, size_id, number_of_doors, construction_id, finish_id, handle_id, light_id, led_id, shelf_type_id, shelf_color_id, post_id, single_endlight, shelving_price, flangeless, bracket_color_id, single_lock FROM line_ups WHERE quote_line_id = @ID");
                                command.Parameters.Clear();
                                command.Parameters.AddWithValue("ID", lineid);

                                lineupds = MySQLAccess.GetDataSet(ConfigurationManager.AppSettings["WebQuoteServer"].ToString(), ConfigurationManager.AppSettings["WebQuoteDatabase"].ToString(), ConfigurationManager.AppSettings["WebQuoteUsername"].ToString(), ConfigurationManager.AppSettings["WebQuotePassword"].ToString(), command);

                                if (lineupds.Tables[0].Rows[0]["finish_id"].ToString() == "90")
                                    throw new Exception("Cannot bridge Unknown Standard Finish.  Please select a valid finish instead");

                                lineupid = Int32.Parse(lineupds.Tables[0].Rows[0]["id"].ToString());

                                command = new MySqlCommand("SELECT hinge01, lock01, hinge02, lock02, hinge03, lock03, hinge04, lock04, hinge05, lock05, hinge06, lock06, hinge07, lock07, hinge08, lock08, hinge09, lock09, hinge10, lock10, hinge11, lock11, hinge12, lock12, hinge13, lock13, hinge14, lock14, hinge15, lock15, hinge16, lock16, hinge17, lock17, hinge18, lock18, hinge19, lock19, hinge20, lock20 FROM lock_hinges WHERE line_up_id = @ID");
                                command.Parameters.Clear();
                                command.Parameters.AddWithValue("ID", lineupid);

                                lockhingeds = MySQLAccess.GetDataSet(ConfigurationManager.AppSettings["WebQuoteServer"].ToString(), ConfigurationManager.AppSettings["WebQuoteDatabase"].ToString(), ConfigurationManager.AppSettings["WebQuoteUsername"].ToString(), ConfigurationManager.AppSettings["WebQuotePassword"].ToString(), command);

                                command = new MySqlCommand("SELECT remove_pushbar, full_silkscreen_color, kickplate_front, kickplate_back, bumper_guard_qty_front, bumper_guard_qty_back, bumper_guard_location_front, bumper_guard_location_back FROM hd_door_configs WHERE line_up_id = @ID");
                                command.Parameters.Clear();
                                command.Parameters.AddWithValue("ID", lineupid);

                                hdconfigds = MySQLAccess.GetDataSet(ConfigurationManager.AppSettings["WebQuoteServer"].ToString(), ConfigurationManager.AppSettings["WebQuoteDatabase"].ToString(), ConfigurationManager.AppSettings["WebQuoteUsername"].ToString(), ConfigurationManager.AppSettings["WebQuotePassword"].ToString(), command);

                                DataRow lurow = lineupds.Tables[0].Rows[0];

                                DataRow lhrow = null;
                                if (lockhingeds.Tables[0].Rows.Count > 0)
                                    lhrow = lockhingeds.Tables[0].Rows[0];

                                DataRow hdrow = null;
                                if (hdconfigds.Tables[0].Rows.Count > 0)
                                    hdrow = hdconfigds.Tables[0].Rows[0];

                                newline.PopulateSystemConfigValues(lurow["type_id"].ToString(), Int32.Parse(lurow["number_of_doors"].ToString()), lurow["size_id"].ToString(), lurow["construction_id"].ToString(), lurow["finish_id"].ToString(), lurow["light_id"].ToString(), lurow["led_id"].ToString(),
                                    lurow["handle_id"].ToString(), row["inlay_color"].ToString(), lurow["shelf_type_id"].ToString(), lurow["shelf_color_id"].ToString(), lurow["post_id"].ToString(), lhrow == null ? "" : lhrow["hinge01"].ToString(), lhrow == null ? "" : lhrow["hinge02"].ToString(), lhrow == null ? "" : lhrow["hinge03"].ToString(), lhrow == null ? "" : lhrow["hinge04"].ToString(),
                                    lhrow == null ? "" : lhrow["hinge05"].ToString(), lhrow == null ? "" : lhrow["hinge06"].ToString(), lhrow == null ? "" : lhrow["hinge07"].ToString(), lhrow == null ? "" : lhrow["hinge08"].ToString(), lhrow == null ? "" : lhrow["hinge09"].ToString(), lhrow == null ? "" : lhrow["hinge10"].ToString(), lhrow == null ? "" : lhrow["hinge11"].ToString(), lhrow == null ? "" : lhrow["hinge12"].ToString(),
                                    lhrow == null ? "" : lhrow["hinge13"].ToString(), lhrow == null ? "" : lhrow["hinge14"].ToString(), lhrow == null ? "" : lhrow["hinge15"].ToString(), lhrow == null ? "" : lhrow["hinge16"].ToString(), lhrow == null ? "" : lhrow["hinge17"].ToString(), lhrow == null ? "" : lhrow["hinge18"].ToString(), lhrow == null ? "" : lhrow["hinge19"].ToString(), lhrow == null ? "" : lhrow["hinge20"].ToString(),
                                    lhrow == null ? "" : lhrow["lock01"].ToString(), lhrow == null ? "" : lhrow["lock02"].ToString(), lhrow == null ? "" : lhrow["lock03"].ToString(), lhrow == null ? "" : lhrow["lock04"].ToString(), lhrow == null ? "" : lhrow["lock05"].ToString(), lhrow == null ? "" : lhrow["lock06"].ToString(), lhrow == null ? "" : lhrow["lock07"].ToString(), lhrow == null ? "" : lhrow["lock08"].ToString(),
                                    lhrow == null ? "" : lhrow["lock09"].ToString(), lhrow == null ? "" : lhrow["lock10"].ToString(), lhrow == null ? "" : lhrow["lock11"].ToString(), lhrow == null ? "" : lhrow["lock12"].ToString(), lhrow == null ? "" : lhrow["lock13"].ToString(), lhrow == null ? "" : lhrow["lock14"].ToString(), lhrow == null ? "" : lhrow["lock15"].ToString(), lhrow == null ? "" : lhrow["lock16"].ToString(),
                                    lhrow == null ? "" : lhrow["lock17"].ToString(), lhrow == null ? "" : lhrow["lock18"].ToString(), lhrow == null ? "" : lhrow["lock19"].ToString(), lhrow == null ? "" : lhrow["lock20"].ToString(), hdrow == null ? "" : hdrow["remove_pushbar"].ToString(), lurow["single_endlight"].ToString(), hdrow == null ? "" : hdrow["full_silkscreen_color"].ToString(),
                                    hdrow == null ? "" : hdrow["kickplate_front"].ToString(), hdrow == null ? "" : hdrow["kickplate_back"].ToString(), hdrow == null ? "" : hdrow["bumper_guard_qty_front"].ToString(), hdrow == null ? "" : hdrow["bumper_guard_qty_back"].ToString(),
                                    hdrow == null ? "" : hdrow["bumper_guard_location_front"].ToString(), hdrow == null ? "" : hdrow["bumper_guard_location_back"].ToString(), lurow["flangeless"].ToString(), lurow["bracket_color_id"].ToString(), lurow["single_lock"].ToString());

                                #endregion

                                // Rails does not delete the shelf_count line if a configuration is changed to be no shelving, so we need to check for that
                                if (lurow["shelf_type_id"].ToString() != "89")
                                {
                                    #region Read Shelf_Counts for system to, build Shelving Configured Values, and add additional line item

                                    QuoteLine shelfline = new QuoteLine("SHELVING", Int32.Parse(row["quantity"].ToString()), "", Decimal.Parse(row["discount_percent"].ToString()), "", "", Decimal.Parse(lurow["shelving_price"].ToString() == "" ? "0" : lurow["shelving_price"].ToString()), row["adj_amt_shelving"].ToString() == "" ? 0 : Decimal.Parse(row["adj_amt_shelving"].ToString()), row["adj_code_shelving"].ToString(), row["adj_comment_shelving"].ToString(), "", DateTime.Parse(row["created_at"].ToString()), "", "", row["vantage_shelving_line_num"].ToString() == "" ? 0 : Int32.Parse(row["vantage_shelving_line_num"].ToString()), Int32.Parse(row["id"].ToString()), ""/*, false*/);

                                    command = new MySqlCommand("SELECT type, shelf_type_id, shelf_color_id, post_color_id, shelf_qty, post_qty, lane_div_qty, perim_grd_qty, glide_sht_qty, price_tag_mld_qty, base_qty, ext_bracket_qty FROM shelf_counts WHERE line_up_id = @ID");
                                    command.Parameters.Clear();
                                    command.Parameters.AddWithValue("ID", lineupid);

                                    shelfds = MySQLAccess.GetDataSet(ConfigurationManager.AppSettings["WebQuoteServer"].ToString(), ConfigurationManager.AppSettings["WebQuoteDatabase"].ToString(), ConfigurationManager.AppSettings["WebQuoteUsername"].ToString(), ConfigurationManager.AppSettings["WebQuotePassword"].ToString(), command);

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

                                        shelfline.PopulateShelfConfigValues(newline, Int32.Parse(lurow["number_of_doors"].ToString()), frame1shelftypeid, frame1shelfcolorid, frame1postcolorid, frame1shelfqty, frame1postqty, frame1lanedivqty, frame1perimgrdqty, frame1glideshtqty, frame1pricetagmldqty, frame1baseqty,
                                            frame1extbracketqty, frame2shelftypeid, frame2shelfcolorid, frame2postcolorid, frame2shelfqty, frame2postqty, frame2lanedivqty, frame2perimgrdqty, frame2glideshtqty, frame2pricetagmldqty, frame2baseqty, frame2extbracketqty,
                                            frame3shelftypeid, frame3shelfcolorid, frame3postcolorid, frame3shelfqty, frame3postqty, frame3lanedivqty, frame3perimgrdqty, frame3glideshtqty, frame3pricetagmldqty, frame3baseqty, frame3extbracketqty, frame4shelftypeid,
                                            frame4shelfcolorid, frame4postcolorid, frame4shelfqty, frame4postqty, frame4lanedivqty, frame4perimgrdqty, frame4glideshtqty, frame4pricetagmldqty, frame4baseqty, frame4extbracketqty);


                                        lines.Add(shelfline);
                                    }

                                    #endregion
                                }
                            }
                        }
                        total_price += Decimal.Parse(row["price"].ToString());

                    }

                    #endregion

                    if (missing)
                    {
                        WriteVantageError(id, "Missing assembly parts.  Refer to notice email.");
                        //SendBatchMissingPartNotice(batch);
                        process = false;
                    }

                    VantageBridgeInterface vantageBridgeInterface = new VantageBridgeInterface();

                    while (process)
                    {
                        process = false;
                        DateTime start = DateTime.Now;
                        try
                        {
                            int processed_quotenum = vantageBridgeInterface.CreateQuote(username, password, id, quotenum, custid, shiptonum, stname, stadd1, stadd2, stadd3, stcity, ststate, stzip, csr, comments, lines, total_price, carrier, transitdays, stemail, stemailcc, salesrepcode, jobref, skip_price_validation);
                            lock (writer)
                            {
                                DateTime end = DateTime.Now;
                                //writer.WriteLine(DateTime.Now.ToString() + " - Completed quote creation for ID # " + id + " - Duration : " + String.Format("{0:00}:{1:00}:{2:00}", (end - start).Minutes, (end - start).Seconds, (end - start).Milliseconds));
                                //writer.Flush();
                            }
                            foreach (QuoteLine line in lines)
                            {
                                WriteVantageLineNum(line);
                            }
                            //RemoveMissingPartNotice(Int32.Parse(id));
                            WriteVantageSuccess(id, processed_quotenum);
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message.StartsWith("CreateQuote - Deadloop has been resolved"))
                            {
                                process = true;
                                lock (writer)
                                {
                                    writer.WriteLine(DateTime.Now.ToString() + " - Deadloop encountered for quote_head id " + id + ". Retrying");
                                    writer.Flush();
                                }
                            }
                            else if (ex.Message.StartsWith("CreateQuote - This is a duplicate entry of an existing record"))
                            {
                                process = true;
                                lock (writer)
                                {
                                    writer.WriteLine(DateTime.Now.ToString() + " - Duplicate entry encountered for quote_head id " + id + ". Retrying");
                                    writer.Flush();
                                }
                            }
                            else
                                throw (ex);
                        }
                    }

                }
                                catch (Exception ex)
                                {
                                    WriteVantageError(id, ex.Message);
                                    lock (writer)
                                    {
                                        writer.WriteLine(DateTime.Now.ToString() + " - Error processing of quote_head id " + id + " error = " + ex.Message + ", trace = " + ex.StackTrace);
                                        writer.Flush();
                                    }
                                }
                                finally
                                {
                                }
            }
        }

        private static void WriteVantageProcessing(string id)
        {
            MySqlCommand command = new MySqlCommand("UPDATE quote_heads SET vantage_status = 'PROCESSING', vantage_lastupdate = @TimeStamp, vantage_message = null WHERE id = @ID");
            command.Parameters.AddWithValue("TimeStamp", DateTime.Now);
            command.Parameters.AddWithValue("ID", id);

            MySQLAccess.NonQuery(ConfigurationManager.AppSettings["WebQuoteServer"].ToString(), ConfigurationManager.AppSettings["WebQuoteDatabase"].ToString(), ConfigurationManager.AppSettings["WebQuoteUsername"].ToString(), ConfigurationManager.AppSettings["WebQuotePassword"].ToString(), command);
        }

        private static void WriteVantageError(string id, string message)
        {
            MySqlCommand command = new MySqlCommand("UPDATE quote_heads SET vantage_status = 'ERROR', vantage_lastupdate = @TimeStamp, vantage_message = @Message WHERE id = @ID");
            command.Parameters.AddWithValue("TimeStamp", DateTime.Now);
            command.Parameters.AddWithValue("ID", id);
            command.Parameters.AddWithValue("Message", message);

            MySQLAccess.NonQuery(ConfigurationManager.AppSettings["WebQuoteServer"].ToString(), ConfigurationManager.AppSettings["WebQuoteDatabase"].ToString(), ConfigurationManager.AppSettings["WebQuoteUsername"].ToString(), ConfigurationManager.AppSettings["WebQuotePassword"].ToString(), command);
        }

        private static void WriteVantageSuccess(string id, int quotenum)
        {
            MySqlCommand command = new MySqlCommand("UPDATE quote_heads SET vantage_status = 'COMPLETE', vantage_lastupdate = @TimeStamp, vantage_quotenum = @QuoteNum, vantage_message = null WHERE id = @ID");
            command.Parameters.AddWithValue("TimeStamp", DateTime.Now);
            command.Parameters.AddWithValue("ID", id);
            command.Parameters.AddWithValue("QuoteNum", quotenum);

            MySQLAccess.NonQuery(ConfigurationManager.AppSettings["WebQuoteServer"].ToString(), ConfigurationManager.AppSettings["WebQuoteDatabase"].ToString(), ConfigurationManager.AppSettings["WebQuoteUsername"].ToString(), ConfigurationManager.AppSettings["WebQuotePassword"].ToString(), command);
        }

        private static void WriteVantageLineNum(QuoteLine line)
        {
            MySqlCommand command = null;
            if (line.PartNum == "SHELVING")
                command = new MySqlCommand("UPDATE quote_lines SET vantage_shelving_line_num = @LineNum WHERE id = @ID");
            else
                command = new MySqlCommand("UPDATE quote_lines SET vantage_line_num = @LineNum WHERE id = @ID");
            command.Parameters.AddWithValue("ID", line.RailsID);
            command.Parameters.AddWithValue("LineNum", line.lineNum);

            MySQLAccess.NonQuery(ConfigurationManager.AppSettings["WebQuoteServer"].ToString(), ConfigurationManager.AppSettings["WebQuoteDatabase"].ToString(), ConfigurationManager.AppSettings["WebQuoteUsername"].ToString(), ConfigurationManager.AppSettings["WebQuotePassword"].ToString(), command);
        }

        private static string TranslateCarrier(string carrier)
        {
            switch (carrier)
            {
                case "Wilson Trucking Corporation": return "WTVA";
                case "Con-Way Express": return "CNWY";
                case "ABF Freight Systems": return "ABFS";
                default: return "";
            }
        }

    }
}
