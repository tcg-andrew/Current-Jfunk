#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ObjectLibrary;
using System.Configuration;
using System.IO;
using System.Diagnostics;

#endregion

namespace ServiceLibrary
{
    [ServiceBehavior(Namespace = "http://services.it.tcg/epicor/quoteservice")]
    public class QuoteService : IQuoteService
    {
        #region Service Behaviors

        public quotegetresult getquoteinfo(string username, string password, string systemid, int numdoors, string doorsizeid, string constructionid, string colorid, string lightid, string handleid, string shelvingid, string shelfcolorid, string shelfpostcolorid,
            string door1hinge, string door1lock, string door2hinge, string door2lock, string door3hinge, string door3lock, string door4hinge, string door4lock, string door5hinge, string door5lock, string door6hinge, string door6lock, string door7hinge, string door7lock,
            string door8hinge, string door8lock, string door9hinge, string door9lock, string door10hinge, string door10lock, string door11hinge, string door11lock, string door12hinge, string door12lock, string door13hinge, string door13lock, string door14hinge,
            string door14lock, string door15hinge, string door15lock, string door16hinge, string door16lock, string door17hinge, string door17lock, string door18hinge, string door18lock, string door19hinge, string door19lock, string door20hinge, string door20lock,
            string frame1shelftypeid, string frame1shelfcolorid, string frame1postcolorid, int frame1shelfqty, int frame1postqty, int frame1lanedivqty, int frame1perimguardqty, int frame1glidesheetqty, int frame1ptmqty, int frame1baseqty, int frame1extbracketqty,
            string frame2shelftypeid, string frame2shelfcolorid, string frame2postcolorid, int frame2shelfqty, int frame2postqty, int frame2lanedivqty, int frame2perimguardqty, int frame2glidesheetqty, int frame2ptmqty, int frame2baseqty, int frame2extbracketqty,
            string frame3shelftypeid, string frame3shelfcolorid, string frame3postcolorid, int frame3shelfqty, int frame3postqty, int frame3lanedivqty, int frame3perimguardqty, int frame3glidesheetqty, int frame3ptmqty, int frame3baseqty, int frame3extbracketqty,
            string frame4shelftypeid, string frame4shelfcolorid, string frame4postcolorid, int frame4shelfqty, int frame4postqty, int frame4lanedivqty, int frame4perimguardqty, int frame4glidesheetqty, int frame4ptmqty, int frame4baseqty, int frame4extbracketqty,
            decimal price, decimal prices, decimal discount, decimal adjamt, string adjcode, decimal adjamts, string adjcodes, string inlay, string pushbar, string silkscreen, string frontkick, string backkick, string fbgqty, string bbgqty, string singleel, string led)
        {
            quotegetresult result = new quotegetresult();

            try
            {
                List<QuoteLine> lines = new List<QuoteLine>();

                QuoteLine line1 = new QuoteLine("SYSTEM", 1, "", discount, "", "", price, adjamt, adjcode, "", "", DateTime.Now, ""/*, false*/, "", 0, 0,"");
//                line1.PopulateSystemConfigValues(systemid, numdoors, doorsizeid, constructionid, colorid, lightid, led, handleid, inlay, shelvingid, shelfcolorid, shelfpostcolorid, door1hinge, door2hinge, door3hinge, door4hinge, door5hinge, door6hinge, door7hinge, door8hinge, door9hinge,
//                    door10hinge, door11hinge, door12hinge, door13hinge, door14hinge, door15hinge, door16hinge, door17hinge, door18hinge, door19hinge, door20hinge, door1lock, door2lock, door3lock, door4lock, door5lock, door6lock, door7lock, door8lock, door9lock, door10lock, door11lock,
//                    door12lock, door13lock, door14lock, door15lock, door16lock, door17lock, door18lock, door19lock, door20lock, pushbar, singleel, silkscreen, frontkick, backkick, fbgqty, bbgqty, "", "", "", "");

                lines.Add(line1);
                if (shelvingid != "89")
                {
                    QuoteLine line2 = new QuoteLine("SHELVING", 1, "", discount, "", "", prices, adjamts, adjcodes, "", "", DateTime.Now, ""/*, false*/, "", 0, 0, "");
                    line2.PopulateShelfConfigValues(line1, numdoors, frame1shelftypeid, frame1shelfcolorid, frame1postcolorid, frame1shelfqty, frame1postqty, frame1lanedivqty, frame1perimguardqty, frame1glidesheetqty, frame1ptmqty, frame1baseqty, frame1extbracketqty,
                        frame2shelftypeid, frame2shelfcolorid, frame2postcolorid, frame2shelfqty, frame2postqty, frame2lanedivqty, frame2perimguardqty, frame2glidesheetqty, frame2ptmqty, frame2baseqty, frame2extbracketqty,
                        frame3shelftypeid, frame3shelfcolorid, frame3postcolorid, frame3shelfqty, frame3postqty, frame3lanedivqty, frame3perimguardqty, frame3glidesheetqty, frame3ptmqty, frame3baseqty, frame3extbracketqty,
                        frame4shelftypeid, frame4shelfcolorid, frame4postcolorid, frame4shelfqty, frame4postqty, frame4lanedivqty, frame4perimguardqty, frame4glidesheetqty, frame4ptmqty, frame4baseqty, frame4extbracketqty);
                    lines.Add(line2);
                }

                CustomerInterface customerInterface = new CustomerInterface();
                QuoteInterface quoteInterface = new QuoteInterface();
                //ConfigurationInterface configurationInterface = new ConfigurationInterface();
                QuoteAssemblyInterface quoteAssemblyInterface = new QuoteAssemblyInterface();
                PartInterface partInterface = new PartInterface();
                int quotenum = quoteInterface.CreateQuote(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorPort"].ToString(), username, password, "railsid", "1001", "STOCK", "", "", "", 0, "", "");

                foreach (QuoteLine line in lines)
                {
                    NewQuoteLine lineinfo = quoteInterface.CreateLineItem(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorPort"].ToString(), username, password, "railsid", quotenum, line.PartNum, line.Description, line.Discount, line.QuoteComment, line.JobComment, 0);
                    line.lineNum = lineinfo.linenum;
                    line.UpdateShelfLinenumValue();

                    //if (line.IsConfig)
                        //configurationInterface.UpdateConfiguration(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorPort"].ToString(), username, password, quotenum, lineinfo.linenum, line.PartNum, lineinfo.revision, line.ConfigValues);

                    quoteInterface.UpdateLineQuote(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorPort"].ToString(), username, password, quotenum, lineinfo.linenum);

                    if (line.PartNum == "SHIP" || !String.IsNullOrEmpty(line.PriceAdj_Code))
                    {
                        if (line.PriceAdj_Code == "MATRIX" || line.PartNum == "SHIP")
                            quoteInterface.UpdateLinePrice(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorPort"].ToString(), username, password, quotenum, lineinfo.linenum, line.PriceAdj);
                        else
                            quoteInterface.CreateMiscCharge(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorPort"].ToString(), username, password, quotenum, lineinfo.linenum, line.PriceAdj, line.PriceAdj_Code, line.PriceAdj_Desc);
                    }

                    if (partInterface.PartIsSalesKit(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorPort"].ToString(), username, password, line.PartNum))
                        quoteInterface.UpdateKitComponents(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorPort"].ToString(), username, password, line.PartNum, quotenum, lineinfo);

                    try
                    {
                        if (partInterface.PartGetsDetails(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), username, password, line.ActualPartNum) && line.ActualPartNum != "SHELVING" && !line.custom_mom)
                            quoteAssemblyInterface.CreateQuoteAsm(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorPort"].ToString(), username, password, quotenum, lineinfo.linenum, line.PartNum, lineinfo.revision);
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message != "A valid Source Method is required.")
                            throw ex;
                    }
                    quoteInterface.UpdateLineQty(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorPort"].ToString(), username, password, quotenum, lineinfo.linenum, line.Quantity);
                }

                result.epicor.Add(new quoteinfo(quoteInterface.GetQuote(ConfigurationManager.AppSettings["EpicorServer"].ToString(), ConfigurationManager.AppSettings["EpicorPort"].ToString(), username, password, ConfigurationManager.AppSettings["EpicorDatabase"].ToString(), ConfigurationManager.AppSettings["EpicorUsername"].ToString(), ConfigurationManager.AppSettings["EpicorPassword"].ToString(), quotenum)));
            }
            catch (Exception ex)
            {
                result.exception = ex.Message;
            }
            return result;
        }

        #endregion

        #region Private Methods

        private string TranslateShelfType(int id)
        {
            switch (id.ToString())
            {
                case "0": return "";
                case "89": return "NONE";
                case "55": return "27 IN DEEP STANDARD";
                case "56": return "36 IN DEEP SLIDE-TRAC";
                case "57": return "36 IN DEEP SUPER SLIDE-TRAC";
                case "58": return "43 IN DEEP SUPER SLIDE-TRAC";
                default: throw new Exception("Undefined");
            }
        }

        private string TranslateShelfColor(int id)
        {
            switch (id.ToString())
            {
                case "0": return "";
                case "45": return "WHITE";
                case "46": return "BLACK";
                case "87": return "NONE";
                default: throw new Exception("Undefined");
            }
        }

        private string TranslateShelfPostColor(int id)
        {
            switch (id.ToString())
            {
                case "0": return "";
                case "88": return "NONE";
                case "42": return "GALVANIZED";
                case "43": return "WHITE";
                case "44": return "BLACK";
                default: throw new Exception("Undefined");
            }
        }

        #endregion

    }
}
